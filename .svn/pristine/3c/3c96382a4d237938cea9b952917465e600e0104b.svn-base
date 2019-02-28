using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 1,考试员发出靠边停车指令后，未能在规定的距离内停车
    /// 2,拉紧驻车制动器前放松行车制动踏板
    /// 3,下车后不关闭车门
    /// 4,停车后，未拉紧驻车制动器
    /// 5,停车前不使用或错误使用转向灯(方向判定)
    /// 6,下车不关灯
    /// 7,下车前不关闭发动机 突然项目结束
    /// </summary>
    public class PullOver : ExamItemBase
    {
        protected  int PullOverCloseDoorTimeout = 10;

        #region  私有变量

        //初始化靠边停车状态
        protected PullOverStep PullOverStepState = PullOverStep.None;

        protected bool CheckedPulloverHandbrake = false;

        /// <summary>
        /// 停车时间
        /// </summary>
        protected DateTime? StopCarTime { get; set; }
        /// <summary>
        /// 开车门时间
        /// </summary>
        protected  DateTime? OpenDoorTime { get; set; }

        protected bool _isCheckedPulloverStop = false;

        /// <summary>
        /// 是否超过规则时间没有关闭车门
        /// </summary>
        protected bool CloseDoorTimeOut()
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
            PullOverCloseDoorTimeout = Settings.PullOverDoorTimeout;

            //注册消息ClosesDoor //
            
            Messenger.Register<CloseDoorMessage>(this, OnCloseDoorMessage);
            //初始化靠边停车的距离评判规则
            //初始化靠边
            Constants.PullOverDistance = 0;
            //
        }


        protected int CloseDoorCount = 0;
        private void OnCloseDoorMessage(CloseDoorMessage message)
        {
            CloseDoorCount+= 1;
        }
        
      
        protected override void OnDrivingOverDistance()
        {
            //超距特殊处理下
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
                //检测手刹拉起时，是否脚刹有松开
                if (!CheckedPulloverHandbrake)
                {
                    CheckedPulloverHandbrake = true;
                    if (!signalInfo.Sensor.Brake)
                    {
                        BreakRule(DeductionRuleCodes.RC40608);
                    }
                }
            }
            else
            {
                if (IsPullHandBrakeTimeOut())
                {
                    PullOverStepState = PullOverStep.PullHandbrake;
                    BreakRule(DeductionRuleCodes.RC40607);
                }
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
                        //todo:客户要求是开光车门两次才报成绩 //感觉这个需求比较奇怪
                        //需要第二次关门报成绩
                        PullOverStepState = PullOverStep.CheckStop;
                    }
                    break;
                case PullOverEndMark.Handbrake:
                    if (signalInfo.Sensor.Handbrake)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                case PullOverEndMark.OpenDoorCheck:
                    if (signalInfo.Sensor.Door)
                        PullOverStepState = PullOverStep.CheckStop;
                    break;
                default:
                    PullOverStepState = PullOverStep.CheckStop;
                    break;
            }
        }

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            //设置了变道角度后
            //设置了变道
            if (Constants.PullOverDistance==0)
            {
                Constants.PullOverDistance = signalInfo.Distance;
            }
     
            if (Settings.PulloverAngle > 0.5 && signalInfo.BearingAngle.IsValidAngle() &&
                StartAngle.IsValidAngle() &&
                !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.PulloverAngle))
            {
                CheckRightIndicatorLight();
            }
            if (Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck &&
               PullOverStepState < PullOverStep.StopCar && signalInfo.Sensor.Door)
            {
                if (!signalInfo.Sensor.Handbrake)
                    CheckRule(true, DeductionRuleCodes.RC40607, DeductionRuleCodes.SRC4060701);

                CheckEndMark(signalInfo);
            }
            if (PullOverStepState == PullOverStep.None)
            {

                if ((signalInfo.CarState == CarState.Stop && Settings.PullOverMark == PullOverMark.CarStop) ||
                    (signalInfo.Sensor.Handbrake && Settings.PullOverMark == PullOverMark.Handbrake))
                {
                    PullOverStepState = PullOverStep.StopCar;
                    StopCarTime = DateTime.Now;
                    Messenger.Send(new EngineRuleMessage(false));
                }
            }
            else if (PullOverStepState == PullOverStep.StopCar)
            {
                //停车转向灯检查
                PullOverStepState = PullOverStep.OpenPullOverLight;

                CheckRightIndicatorLight();
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
                if (Settings.PullOverEndMark == PullOverEndMark.OpenDoorCheck)
                {
                    StopCore();
                    return;
                }
                //true false
                if (!(Settings.PullOverEndMark == PullOverEndMark.None || 
                    Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck))
                {
                    StopCore();
                    return;
                }
                if (!OpenDoorTime.HasValue)
                    OpenDoorTime = DateTime.Now;

                //关车门
                if (!signalInfo.Sensor.Door)
                {

                    //海南版本特殊要求开关车门两次
                    if (DataBase.VersionNumber.Contains("海南"))
                    {

                        if (CloseDoorCount>=2)
                        {
                            PullOverStepState = PullOverStep.CloseDoor;
                            //关车门
                            Messenger.Send(new DoorChangedMessage(signalInfo.Sensor.Door));
                            StopCore();
                            return;
                        }
                   
                    }
                    else
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

        //已经检测过右转
        private bool IsRightIndicatorLightChecked = false;
        /// <summary>
        /// 检测右转（动方向或者车停时检测）
        /// </summary>
        protected void CheckRightIndicatorLight()
        {
            if (IsRightIndicatorLightChecked)
                return;

            //如果扣分了 
            //IsRightIndicatorLightChecked = true;
            if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
            {
                IsRightIndicatorLightChecked = true;
                BreakRule(DeductionRuleCodes.RC40610);
            }
            else
            {
                var advancedSignal = Singleton.GetAdvancedCarSignal;
                var rightIndicator = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                if (!rightIndicator)
                {
                    IsRightIndicatorLightChecked = true;
                    BreakRule(DeductionRuleCodes.RC40611);
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
            else if(Settings.PulloverEndAutoTriggerStart)
            {
                //TODO:不知道为何ExamManager 里面没有能够触发消息，这个暂时不好调试。以后有时间在调试测试
                //Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
               // var message = new MapPoint(Coordinate.Empty, 0, string.Empty, MapPointType.PullOver);
               //TODO:这种方式只适合多伦界面。
                Messenger.Send(new VehicleStartingMessage());
                Logger.InfoFormat("靠边停车结束，起步继续考试");
            }
            Logger.InfoFormat("靠边停车结束 当前流程状态 :{0}", PullOverStepState.ToString());
            Messenger.Send(new EngineRuleMessage(true));
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
            //航向角无效，不记录
            if (!signalInfo.BearingAngle.IsValidAngle())
                return false;

            StartAngle = signalInfo.BearingAngle;

            PullOverStepState = PullOverStep.None;
            return base.InitExamParms(signalInfo);
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PullOver; }
        }
    }

    /// <summary>
    /// 靠边停车步骤状态
    /// </summary>
    public enum PullOverStep
    {
        None = 0,
        //开启停车灯
        OpenPullOverLight = 1,
        //停车
        StopCar = 2,
        //拉手刹检测
        PullHandbrake = 3,
        ///检测项目结束
        CheckStop = 4,
        //关闭车门
        CloseDoor = 5,
    }
}
