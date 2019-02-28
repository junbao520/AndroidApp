using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;


namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 通过人行横道
    /// </summary>
    public class ThroughPedestrianCrossing : SlowSpeed
    {
  

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.PedestrianCrossingVoice;
            MaxDistance = Settings.PedestrianCrossingDistance;
            SlowSpeedLimit = Settings.PedestrianCrossingSpeedLimit;
            OverSpeedMustBrake = Settings.PedestrianCrossingBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.PedestrianCrossingLightCheck;
            CheckBrakeRequired = Settings.PedestrianCrossingBrakeRequire;
            CheckLoudSpeakerInDay = Settings.PedestrianCrossingLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.PedestrianCrossingLoudSpeakerNightCheck;

            PrepareDistance = Settings.PedestrainCrossingPrepareD;
        }

        protected override string SpeedLimitRuleCode
        {
            get { return DeductionRuleCodes.RC41603; }
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PedestrianCrossing; }
        }
    }
}
