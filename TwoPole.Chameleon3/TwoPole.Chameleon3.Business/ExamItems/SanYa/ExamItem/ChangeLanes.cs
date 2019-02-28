using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Collections.Specialized;
using System;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.ExamItems
{
    /// <summary>
    /// 变更车道：左、右转向灯都可以
    /// 只需要打灯就可以了，不需要真的变道
    /// </summary>
    public class ChangeLanes : ExamItemBase
    {
        #region 规则检测
        /// <summary>
        /// 1，变更车道前打转向灯
        /// 2，在规定的距离和时间变道
        /// 3，速度限制
        /// 4,项目距离结束才进行评判
        /// </summary>

        #endregion

   
        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.ChangeLanesVoice;
            MaxDistance = Settings.ChangeLanesMaxDistance;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.ChangeLanesTimeout);
        }

        /// <summary>
        /// 超距结束项目
        /// </summary>
        protected override void OnDrivingOverDistance()
        {
            base.OnDrivingOverDistance();
        }

        /// <summary>
        /// 超时完成项目
        /// </summary>
        protected override void OnDrivingTimeout()
        {
            base.OnDrivingTimeout();
        }



        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            base.ExecuteCore(signalInfo);
        }

        protected override void StopCore()
        {
            //左转和右转进行了灯光检测
            var isCheckedLeftIndicatorLight = CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight);
            var isCheckedRightIndicatorLight = CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight);

            var isCheckedLeftIndicatorLightEnough = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
            var isCheckedRightIndicatorLightEnough = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
            if (!(isCheckedLeftIndicatorLight || isCheckedRightIndicatorLight))
            {
                BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
            }
            else
            {
                if (!(isCheckedLeftIndicatorLightEnough || isCheckedRightIndicatorLightEnough))
                {
                    BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020603);
                }
            }
            //夜考双闪
            if (Settings.ChangeLanesLowAndHighBeamCheck && Context.ExamTimeMode == ExamTimeMode.Night)
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
            get { return ExamItemCodes.ChangeLanes; }
        }

    }
}
