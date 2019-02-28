using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 减速让行项目 TwoPole.Chameleon3.Business.ExamItems.SlowSpeed,TwoPole.Chameleon3.Business
    /// </summary>
    public  class SlowSpeed : ExamItemBase
    {
        #region 参数设置
        /// <summary>
        /// 速度限制
        /// </summary>
        protected double SlowSpeedLimit { get; set; }
        /// <summary>
        /// 速度以上必须踩刹车
        /// </summary>
        protected double OverSpeedMustBrake { get; set; }
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        protected bool CheckBrakeRequired { get; set; }
        /// <summary>
        /// 白天喇叭
        /// </summary>
        protected bool CheckLoudSpeakerInDay { get; set; }
        /// <summary>
        /// 夜间喇叭
        /// </summary>
        protected bool CheckLoudSpeakerInNight { get; set; }
        /// <summary>
        /// 远近光交替
        /// </summary>
        protected bool CheckLowAndHighBeam { get; set; }
        #endregion

  
        protected DateTime? startBrakeTime { get; set; }
        //是否播报过刹车持续时间规则
        protected bool isBrakeRuleSpeaked = false;
        protected double PrepareDistance = 0;

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            MaxDistance = Settings.PedestrianCrossingDistance;
            VoiceExamItem = Settings.PedestrianCrossingVoice;
            //MaxElapsedTime =new TimeSpan(10);
            startBrakeTime = null;
            isBrakeRuleSpeaked = false;
        }
     
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            startBrakeTime = null;
            isBrakeRuleSpeaked = false;
            return base.InitExamParms(signalInfo);
        }

        protected override void OnDrivingOverDistance()
        { 
            base.OnDrivingOverDistance();
        }
        //进入项目必须松一次刹车再踩
        protected bool _hasLoosenBrake = false;
        protected virtual bool CheckSpeedLimit()
        {
            //进入项目后没得松开刹车动作再踩，则不算踩刹车
            if (Settings.CheckBrakeMustInitem)
            {
                if (!_hasLoosenBrake)
                    return false;
            }
            //判断下如果是泸州
           

            //必踩刹车
            if (CheckBrakeRequired && hasBraked==false)
            {
                var hasBrake = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Brake);
                Logger.InfoFormat("{0}-{1}-{2}-必踩刹车-{3}", Name, StartDistance, StartTime, hasBrake);
                if (!hasBrake)
                    return false;
            }
           // 速度限制
            if (SlowSpeedLimit > 0)
            {
                var isOverspeed = CarSignalSet.Current.SpeedInKmh > SlowSpeedLimit;
                Logger.InfoFormat("{0}-{1}-{2}-速度限制-{3}:{4}", Name, StartDistance, StartTime, isOverspeed, SlowSpeedLimit);
                if (isOverspeed)
                    return false;
            }
           // 该速度以上必踩刹车
            if (OverSpeedMustBrake > 0)
            {
                var isOverspeed = CarSignalSet.Current.SpeedInKmh > OverSpeedMustBrake;
                Logger.InfoFormat("{0}-{1}-{2}-该速度以上必踩刹车-{3}:{4}", Name, StartDistance, StartTime, isOverspeed, OverSpeedMustBrake);
                if (isOverspeed)
                {
                    var hasBrake = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Brake);
                    if (!hasBrake)
                        return false;
                }
            }
            return true;
        }

        protected override void CheckBrakeVoice(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.Brake)
                hasBraked = true;
            base.CheckBrakeVoice(signalInfo);
        }

        /// <summary>
        /// 当前角度
        /// </summary>
        protected double CurrentAngle { get; set; }

        protected double? stopDelayDistance { get; set; }
        //检测是否踩过刹车
        protected bool hasBraked = false;
        protected virtual void CheckDelayStop(CarSignalInfo signalInfo)
        { }
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor == null)
                return;
            //有准备距离时,准备距离期间不进行任何检测
            if (PrepareDistance > 2)
            {
                if (signalInfo.Distance - StartDistance < PrepareDistance)
                    return;

            }
            CurrentAngle = signalInfo.BearingAngle;
            //是否有松开刹车动作
            //检测是否有松开刹车的动作
            if (!signalInfo.Sensor.Brake)
                _hasLoosenBrake = true;

            LeftIndicatorCheck(IndicatorCheckState.MidCheckAngle);

            CheckDelayStop(signalInfo);


            //检测刹车保持时间
            if (Settings.BrakeKeepTime > 0 && !isBrakeRuleSpeaked)
            {
                if (signalInfo.Sensor.Brake)
                {
                    if (!startBrakeTime.HasValue)
                    {
                        startBrakeTime = DateTime.Now;
                    }
                }
                else
                {
                    if (!startBrakeTime.HasValue)
                        return;

                    isBrakeRuleSpeaked = true;

                    //if ((DateTime.Now - startBrakeTime.Value).TotalSeconds < Settings.BrakeKeepTime)
                    //{
                    //    BreakRule(DeductionRuleCodes.RC30221);
                    //}

                }

            }

        }
        /// <summary>
        /// 是否检测过转向灯
        /// </summary>

        protected bool indicatorChecked = false;

        /// <summary>
        /// 检测转向灯
        /// </summary>
        protected virtual void LeftIndicatorCheck(IndicatorCheckState checkState)
        {
        }
        /// <summary>
        /// 直接结束（不进行任何判定）
        /// </summary>
        protected void StopImmediately()
        {
            base.StopCore();

        }

        protected override void StopCore()
        {
     
            var isOk = CheckSpeedLimit();
            if (!isOk)
            {
                CheckRule(true,DeductionRuleCodes.RC41001);
            }

            //喇叭检测
            if ((CheckLoudSpeakerInDay && Context.ExamTimeMode == ExamTimeMode.Day) ||
                (CheckLoudSpeakerInNight && Context.ExamTimeMode == ExamTimeMode.Night))
            {
                var hasLoudSpeaker = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Loudspeaker);
                if (!hasLoudSpeaker)
                {
                    CheckRule(true, DeductionRuleCodes.RC30212);
                }
            }
            //夜间远近光交替
            if (CheckLowAndHighBeam && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                var hasLowAndHighBeam = AdvancedSignal.CheckHighBeam(StartTime);
                if (!hasLowAndHighBeam)
                {
                    CheckRule(true, DeductionRuleCodes.RC41603);
                }
            }

            base.StopCore();
        }

        protected virtual string SpeedLimitRuleCode { get { return DeductionRuleCodes.RC40701; } }

        public override string ItemCode
        {
            get { return ExamItemCodes.SlowSpeed; }
        }
    }
    public enum IndicatorCheckState
    {
        None,
        /// <summary>
        /// 角度达到检测转向灯
        /// </summary>
        MidCheckAngle,
        /// <summary>
        /// 结束时检测转向灯
        /// </summary>
        StopCheck,
    }
}
