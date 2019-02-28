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
    /// 通过学校区域, 减速项目
    /// 禁止鸣笛
    /// </summary>
    public class ThroughSchoolArea : SlowSpeed
    {
  
        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.SchoolAreaVoice;
            MaxDistance = Settings.SchoolAreaDistance;
            SlowSpeedLimit = Settings.SchoolAreaSpeedLimit;
            OverSpeedMustBrake = Settings.SchoolAreaBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.SchoolAreaLightCheck;
            CheckBrakeRequired = Settings.SchoolAreaBrakeRequire;
            CheckLoudSpeakerInDay = Settings.SchoolAreaLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.SchoolAreaLoudSpeakerNightCheck;
            PrepareDistance = Settings.SchoolAreaPrepareD;
        }

        protected override void StopCore()
        {
            //禁止鸣笛
            if (Settings.SchoolAreaForbidLoudSpeakerCheck)
            {
                var hasLoudspeaker = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Loudspeaker);
                if(hasLoudspeaker)
                    BreakRule(DeductionRuleCodes.RC30214);
            }

            base.StopCore();
        }

        protected override string SpeedLimitRuleCode { get { return DeductionRuleCodes.RC41101; } }
        
        public override string ItemCode
        {
            get { return ExamItemCodes.SchoolArea; }
        }
    }
}
