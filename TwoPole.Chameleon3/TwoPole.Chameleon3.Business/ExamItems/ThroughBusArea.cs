using GalaSoft.MvvmLight.Messaging;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 通过公共汽车区域
    /// </summary>
    public class ThroughBusArea : SlowSpeed
    {
      
        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.BusAreaVoice;
            MaxDistance = Settings.BusAreaDistance;
            SlowSpeedLimit = Settings.BusAreaSpeedLimit;
            OverSpeedMustBrake = Settings.BusAreaBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.BusAreaLightCheck;
            CheckBrakeRequired = Settings.BusAreaBrakeRequire;
            CheckLoudSpeakerInDay = Settings.BusAreaLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.BusAreaLoudSpeakerNightCheck;
            PrepareDistance= Settings.BusAreaPrepareD;

        }

        protected override string SpeedLimitRuleCode
        {
            get { return DeductionRuleCodes.RC41201; }
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.BusArea; }
        }
    }
}
