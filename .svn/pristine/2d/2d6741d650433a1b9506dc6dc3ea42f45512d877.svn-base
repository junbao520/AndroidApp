using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.ExamItems
{
    /// <summary>
    /// 通过人行横道，有行人通过；
    /// 在通过人行横道的时候，去评定是否有停车的动作
    /// </summary>
    /// 


    public class ThroughPedestrianCrossingHasPeople : ExamItemBase
    {
        #region 项目检测规则
        //是否在10秒内停车
        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            MaxDistance = Settings.PedestrianCrossingDistance;
            //必须语音播报
            VoiceExamItem = true;


        }


        /// <summary>
        /// 超过规则距离则触犯规则
        /// </summary>
        protected override void OnDrivingOverDistance()
        {
            BreakRule(DeductionRuleCodes.RC41103);
            StopCore();
        }


        /// <summary>
        /// 停车后则语音播报行人已通过，然后结束考试
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (signalInfo.CarState == CarState.Stop)
            {
                Speaker.PlayAudioAsync("行人已通过", SpeechPriority.Normal);
                StopCore();
            }
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PedestrianCrossingHasPeople; }
        }
    }
}
