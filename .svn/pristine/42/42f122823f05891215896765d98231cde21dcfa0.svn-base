using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using TwoPole.Chameleon3.Business.ExamItems;

namespace TwoPole.Chameleon3.Business.Areas.Chongqin.Fuling.ExamItems
{
    /// <summary>
    /// 1,考试员发出靠边停车指令后，未能在规定的距离内停车
    /// 2,拉紧驻车制动器前放松行车制动踏板
    /// 3,下车后不关闭车门
    /// 4,停车后，未拉紧驻车制动器
    /// 5,停车前不使用或错误使用转向灯(方向判定)
    /// 6,下车不关灯
    /// 7,下车前不关闭发动机w
    /// </summary>
    public class PullOver : ExamItemBase
    {
        protected const int PullOverCloseDoorTimeout = 10;

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

            //todo:
            //初始化靠边停车的距离评判规则
            //DistanceRule = Resolve<ParkingDistanceRule>();
            //base.Rules.Add(distanceRule);
        }

        //protected ParkingDistanceRule DistanceRule { get; private set; }

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
                PullOverStepState = PullOverStep.PullHandbrake;
                return;
            }

            if (signalInfo.Sensor.Handbrake)
            {
                PullOverStepState = PullOverStep.PullHandbrake;
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
                PullOverStepState = PullOverStep.PullHandbrake;
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

        private bool isSpeakHandBrake = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if ((int)PullOverStepState >= (int)PullOverStep.StopCar)
            {
                if (!isSpeakHandBrake && signalInfo.Sensor.Handbrake)
                {
                    isSpeakHandBrake = true;
                    //请操作完成后下车
                    Speaker.PlayAudioAsync("请操作完成后下车",SpeechPriority.Normal);
                }
            }

            if (PullOverStepState == PullOverStep.None)
            {
                ///拉手刹结束启动靠边停车检测
                if (signalInfo.Sensor.Handbrake)
                {
                    PullOverStepState = PullOverStep.StopCar;
                    StopCarTime = DateTime.Now;
                    Messenger.Send(new EngineRuleMessage(false));
                    Logger.DebugFormat("{0}-关闭发动机熄火评判规则", Name);
                }
            }
            else if (PullOverStepState == PullOverStep.StopCar)
            {
                //停车转向灯检查
                PullOverStepState = PullOverStep.OpenPullOverLight;
                if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC40610);
                }
                else
                {
          
                    var rightIndicator = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                    if (!rightIndicator)
                    {
                        BreakRule(DeductionRuleCodes.RC40611);
                    }
                }
            }
            else if (PullOverStepState == PullOverStep.OpenPullOverLight)
            {
                CheckHandbrake(signalInfo);
            }
            else if (PullOverStepState == PullOverStep.PullHandbrake)
            {
                //判断靠边停车是否结束
                CheckEndMark(signalInfo);
            }
            else if (PullOverStepState == PullOverStep.CheckStop)
            {
                if (!_isCheckedPulloverStop)
                {
                    _isCheckedPulloverStop = true;
                    CheckPullOverStop(signalInfo);
                }

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
                    PullOverStepState = PullOverStep.CloseDoor;
                    Messenger.Send(new DoorChangedMessage(signalInfo.Sensor.Door));
                    StopCore();
                    return;
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
                if (signalInfo.Sensor.RightIndicatorLight)
                {
                    BreakRule(DeductionRuleCodes.RC40613);
                }
            }
            //夜间打开报警灯
            if (Settings.PullOverOpenCautionBeforeGetOff && Context.ExamTimeMode == ExamTimeMode.Night)
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
