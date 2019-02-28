using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Collections.Specialized;
using System;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.ExamItems
{
    /// <summary>
    /// 超车
    /// 1,在规定的距离和时间超车
    /// 2,速度限制
    /// 3,超车灯光检测
    /// 4,是否在规则的速度内超车
    /// 5,夜间超车只需要打左右灯光即可
    /// 特殊要求，在距离到达后再进行评判，播下一个项目
    /// 6，修改，不管白天晚上只需要打左右灯光即可
    /// </summary>
    public class Overtake : ExamItemBase
    {
        #region 私有变量
        /// <summary>
        /// 开始变道的角度
        /// </summary>
        private bool IsLoudSpeakerCheck = false;

        #endregion

       

   

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            MaxDistance = Settings.OvertakeMaxDistance;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.OvertakeTimeout);
            VoiceExamItem = Settings.OvertakeVoice;
        }

        protected override void OnDrivingOverDistance()
        {
            base.OnDrivingOverDistance();
        }

        protected override void OnDrivingTimeout()
        {
            base.OnDrivingTimeout();
        }

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!IsLoudSpeakerCheck && signalInfo.Sensor.Loudspeaker)
                IsLoudSpeakerCheck = true;

            base.ExecuteCore(signalInfo);
        }
        
        protected override void StopCore()
        {
            //左转和右转进行了灯光检测
            var isCheckedLeftIndicatorLight = CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight);
            var isCheckedRightIndicatorLight = CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight);

            var isCheckedLeftIndicatorLightEnough = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
            var isCheckedRightIndicatorLightEnough = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
            if (!(isCheckedLeftIndicatorLight && isCheckedRightIndicatorLight))
            {
                BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
            }
            else
            {
                if (!(isCheckedLeftIndicatorLightEnough && isCheckedRightIndicatorLightEnough))
                {
                    BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                }
            }


            //检查喇叭
            if (!IsLoudSpeakerCheck)
            {
                if (Settings.OvertakeLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night)
                {
                    BreakRule(DeductionRuleCodes.RC30212);
                }
                if (Settings.OvertakeLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day)
                {
                    BreakRule(DeductionRuleCodes.RC30212);
                }
            }

            //夜考双闪
            if (Settings.OvertakeLowAndHighBeamCheck && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                if (!AdvancedSignal.CheckHighBeam(StartTime))
                {
                    BreakRule(DeductionRuleCodes.RC41603);
                }
            }

            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.Overtaking; }
        }
    }
}
