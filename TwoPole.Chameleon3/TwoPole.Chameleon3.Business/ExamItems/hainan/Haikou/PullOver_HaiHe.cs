using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.HaiKou.ExamItems
{
    /// <summary>
    /// <summary>
    /// 1,考试员发出靠边停车指令后，未能在规定的距离内停车
    /// 2,拉紧驻车制动器前放松行车制动踏板
    /// 3,下车后不关闭车门
    /// 4,停车后，未拉紧驻车制动器
    /// 5,停车前不使用或错误使用转向灯(方向判定)
    /// 6,下车不关灯
    /// 7,下车前不关闭发动机
    /// 8, 需要摸外后视镜不然触犯规则
    /// 特殊要求：在停车的时候右转向灯必须亮
    /// 15秒关闭车门
    /// 下车后不关右转向灯不用扣分（张定辉教练反馈，王涛，20151202）
    /// </summary>
    public class PullOver_HaiHe : ExamItemBase
    {
        protected const int PullOverCloseDoorTimeout = 15;

        #region  私有变量

        //初始化靠边停车状态
        private PullOverStep PullOverStepState = PullOverStep.None;

        protected bool CheckedPulloverHandbrake = false;

        /// <summary>
        /// 停车时间
        /// </summary>
        private DateTime? StopCarTime { get; set; }
        /// <summary>
        /// 开车门时间
        /// </summary>
        private DateTime? OpenDoorTime { get; set; }

        private bool _isCheckedPulloverStop = false;

        //是否摸了外后视镜
        private bool _isTouchExteriorMirror = false;

        /// <summary>
        /// 是否超过规则时间没有关闭车门
        /// </summary>
        private bool CloseDoorTimeOut()
        {
            if (!OpenDoorTime.HasValue)
                return false;

            return (DateTime.Now - OpenDoorTime.Value).TotalSeconds > PullOverCloseDoorTimeout;
        }

        private bool IsPullHandBrakeTimeOut()
        {
            if (!StopCarTime.HasValue || Settings.PullOverHandbrakeTimeout <= 0)
                return false;

            return (DateTime.Now - StopCarTime.Value).TotalSeconds > Settings.PullOverHandbrakeTimeout;
        }
        #endregion


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            MaxDistance = Settings.PullOverMaxDrivingDistance;
            VoiceExamItem = Settings.PullOverVoice;
            Messenger.Register<CloseDoorMessage>(this, OnCloseDoorMessage);
            //初始化靠边停车的距离评判规则
            //DistanceRule = Resolve<ParkingDistanceRule>();
            //base.Rules.Add(distanceRule);
        }
        int CloseDoorCount = 0;
        private void OnCloseDoorMessage(CloseDoorMessage message)
        {
            CloseDoorCount += 1;
        }

        protected override void OnDrivingOverDistance()
        {
            BreakRule(DeductionRuleCodes.RC40602);
            base.OnDrivingOverDistance();
        }

        /// <summary>
        /// 检测手刹是否超时
        /// </summary>
        /// <param name="signalInfo"></param>
        protected void CheckHandbrake(CarSignalInfo signalInfo)
        {
            //配置0，不检测手刹
            if (Settings.PullOverHandbrakeTimeout <= 0)
            {
                return;
            }
            if (signalInfo.Sensor.Handbrake)
            {

            }
            else
            {
                //检测手刹拉起时，是否脚刹有松开
                if (!CheckedPulloverHandbrake)
                {
                    if (!signalInfo.Sensor.Brake)
                    {
                        CheckedPulloverHandbrake = true;
                        BreakRule(DeductionRuleCodes.RC40608);
                    }
                }
            }

            if (IsPullHandBrakeTimeOut())
            {

                BreakRule(DeductionRuleCodes.RC40607);
            }
        }

        /// <summary>
        /// 检查结束标记
        /// </summary>
        /// <param name="signalInfo"></param>
        protected void CheckEndMark(CarSignalInfo signalInfo)
        {
            switch (Settings.PullOverEndMark)
            {
                case PullOverEndMark.CautionLightCheck:
                    if (signalInfo.Sensor.CautionLight)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                case PullOverEndMark.LowBeamCheck:
                    if (!signalInfo.Sensor.LowBeam)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                case PullOverEndMark.EngineExtinctionCheck:
                    if (!signalInfo.Sensor.Engine)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                case PullOverEndMark.SafetyBeltCheck:
                    if (!signalInfo.Sensor.SafetyBelt)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                case PullOverEndMark.OpenCloseDoorCheck:
                    if (signalInfo.Sensor.Door)
                    {
                        OpenDoorTime = DateTime.Now;
                        PullOverStepState = PullOverStep.CheckStop;
                    }
                    break;
                case PullOverEndMark.Handbrake:
                    if (signalInfo.Sensor.Handbrake)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                default:
                    PullOverStepState = PullOverStep.CheckStop;
                    break;
            }
        }
        /// <summary>
        /// 摸外后视镜时间
        /// </summary>
        private DateTime? _ExteriorMirrorTime { get; set; }

        private bool _isCheckExterior = false;

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!_isCheckExterior && _ExteriorMirrorTime.HasValue)
            {
                //摸后视镜后，不足3秒开门，扣10分
                //z做成配置 
                //如果配置成0 永远false
                if ((DateTime.Now - _ExteriorMirrorTime.Value).TotalSeconds <Settings.PullOverOpenDoorTime)
                {
                    if (signalInfo.Sensor.Door)
                    {
                        _isCheckExterior = true;
                        BreakRule(DeductionRuleCodes.RC41705);
                    }
                }
                else
                {
                    _isCheckExterior = true;
                }
            }

            //如果车停了又继续走了，并且状态在结束 标识之前 充值状态 //要求是下车前
            //if ((int)PullOverStepState >= (int)PullOverStep.StopCar && signalInfo.CarState != CarState.Stop && PullOverStepState != PullOverStep.CloseDoor && PullOverStepState != PullOverStep.CheckStop)
            //{
            //    PullOverStepState = PullOverStep.None;
            //    _isTouchExteriorMirror = false;
            //}
            if ((int)PullOverStepState >= (int)PullOverStep.StopCar &&
                !_isTouchExteriorMirror)
            {
                if (signalInfo.Sensor.ExteriorMirror)
                {
                    _ExteriorMirrorTime = DateTime.Now;
                    _isTouchExteriorMirror = true;
                    //TODO:特殊修改 触摸观望镜 播报语音
                    Speaker.PlayAudioAsync("观望");
                    //Task.Run(() =>
                    //{
                    //    //Thread.Sleep(2 * 1000);
                    //    //播放刹车语音 
                    //    Speaker.PlayAudioAsync("观望镜确认");
                    //    //Speaker.SpeakBreakeVoice();
                    //});

                }
            }

            if (PullOverStepState == PullOverStep.None)
            {
                if (signalInfo.Sensor.Handbrake)
                {
                    PullOverStepState = PullOverStep.StopCar;
                    StopCarTime = DateTime.Now;
                    Messenger.Send(new EngineRuleMessage(false));
                    Logger.DebugFormat("{0}-关闭发动机熄火评判规则", Name);
                }
                //if (signalInfo.CarState == CarState.Stop)
                //{
                //    PullOverStepState = PullOverStep.StopCar;
                //    StopCarTime = DateTime.Now;
                //    Messenger.Send(new EngineRuleMessage(false));
                //    Logger.DebugFormat("{0}-关闭发动机熄火评判规则", Name);
                //}

            }
            else if (PullOverStepState == PullOverStep.StopCar)
            {
                //停车转向灯检查 停车前不使用或错误使用转向灯
                PullOverStepState = PullOverStep.OpenPullOverLight;
                if (CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight) || !signalInfo.Sensor.RightIndicatorLight)
                {
                    BreakRule(DeductionRuleCodes.RC40610);
                }
                else
                {
                    //写死检测3秒前的灯
                    var lastSignal = CarSignalSet.QueryCachedSeconds(Settings.TurnLightAheadOfTime).LastOrDefault();

                    if (lastSignal == null || !lastSignal.Sensor.RightIndicatorLight)
                    {
                        BreakRule(DeductionRuleCodes.RC40611);
                    }
                    //var advancedSignal = Resolve<IAdvancedCarSignal>();
                    //var rightIndicator = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                    //if (!rightIndicator)
                    //{
                    //    BreakRule(DeductionRuleCodes.RC40611);
                    //}
                }
            }
            //开光车门之后再检查手刹
            else if (PullOverStepState == PullOverStep.OpenPullOverLight)
            {
                CheckHandbrake(signalInfo);
                PullOverStepState = PullOverStep.PullHandbrake;
            }
            else if (PullOverStepState == PullOverStep.PullHandbrake)
            {
                //判断靠边停车是否结束
                CheckEndMark(signalInfo);



            }
            else if (PullOverStepState == PullOverStep.CheckStop)
            {
                //if (signalInfo.Sensor.ExteriorMirror)
                //{
                //    _isTouchExteriorMirror = true;
                //    Speaker.PlayAudioAsync("sanya/ExteriorMirror.wav", Infrastructure.Speech.SpeechPriority.Normal);
                //}
                //检测手刹
                // 


                if (!_isCheckedPulloverStop)
                {
                    _isCheckedPulloverStop = true;
                    CheckPullOverStop(signalInfo);
                }

                //如果结束标识 不是开关车门
                if (!(Settings.PullOverEndMark == PullOverEndMark.None ||
                    Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck))
                {
                    StopCore();
                    return;
                }

                if (!OpenDoorTime.HasValue)
                    OpenDoorTime = DateTime.Now;

                if (!signalInfo.Sensor.Door)
                {
                    //客户要求结束考试开关车门改为一次
                        if (CloseDoorCount >= 1)
                        {
                            PullOverStepState = PullOverStep.CloseDoor;
                            //关车门
                            Messenger.Send(new DoorChangedMessage(signalInfo.Sensor.Door));
                            StopCore();
                            return;
                        }
                }

                //在规定的时间内没有关闭车门
                if (CloseDoorTimeOut())
                {
                    BreakRule(DeductionRuleCodes.RC40605);
                    StopCore();
                }
            }

        }

        /// <summary>
        /// 是否已经操作完毕（由于海口需要开关2次车门，第二次开门才播报被扣分项目）
        /// </summary>
        private bool IsSuccessed = false;
        /// <summary>
        /// 停车后信息检查
        /// </summary>
        /// <param name="signalInfo"></param>
        protected void CheckPullOverStop(CarSignalInfo signalInfo)
        {
            //检测发动机是否熄火
            if (Settings.PullOverStopEngineBeforeGetOff && signalInfo.Sensor.Engine)
            {
                BreakRule(DeductionRuleCodes.RC40609);
            }
            //检查安全带是否松开
            if (Settings.PullOverOpenSafetyBeltBeforeGetOff && signalInfo.Sensor.SafetyBelt)
            {
                //根据数据库改的
                BreakRule(DeductionRuleCodes.RC40900);
            }
            //检测灯光是否关闭
            if (Settings.PullOverCloseLowBeamBeforeGetOff)
            {
                //近光灯
                if (signalInfo.Sensor.LowBeam)
                {
                    BreakRule(DeductionRuleCodes.RC40612);
                }
                //转向灯
                //if (signalInfo.Sensor.RightIndicatorLight)
                //{
                //    BreakRule(DeductionRuleCodes.RC40613);
                //}
            }

            //自动档不检测 
            if (Settings.PullOverNeutralCheck)
            {
                if (!signalInfo.Sensor.IsNeutral)
                {
                    BreakRule(DeductionRuleCodes.RC40617);
                }
            }

            //夜间打开报警灯
            if (Settings.PullOverOpenCautionBeforeGetOff)
            //if (Settings.PullOverOpenCautionBeforeGetOff && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                if (!signalInfo.Sensor.CautionLight)
                {
                    BreakRule(DeductionRuleCodes.RC40614);
                }
            }
        }

        protected override void StartCore(ExamItemExecutionContext context, CancellationToken token)
        {
            Logger.InfoFormat("靠边停车开始");
            base.StartCore(context, token);
        }

        protected override void StopCore()
        {
            //如果靠边停车结束后触发靠边整个项目结束
            if (Settings.PulloverEndAutoTriggerStopExam)
            {
                Messenger.Send(new ExamFinishingMessage(false));
            }
            //如果没有摸外后视镜则触犯规则

           // StopCore()
            if (!_isTouchExteriorMirror)
                BreakRule(DeductionRuleCodes.RC40601);
            Logger.InfoFormat("靠边停车结束");
            Messenger.Send(new EngineRuleMessage(true));
            Logger.DebugFormat("{0}-启用发动机熄火评判规则", Name);
            base.StopCore();
        }

        /// <summary>
        /// 开始执行项目对数据进行初始化
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        /// 
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            PullOverStepState = PullOverStep.None;
            return base.InitExamParms(signalInfo);
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PullOver; }
        }
    }
}
