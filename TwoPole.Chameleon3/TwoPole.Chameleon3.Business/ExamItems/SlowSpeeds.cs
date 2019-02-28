using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 通过人行横道，有行人通过；
    /// 在通过人行横道的时候，去评定是否有停车的动作
    /// </summary>
    /// 
    public class SlowSpeeds : SlowSpeed
    {
    

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            MaxDistance = Settings.PedestrianCrossingDistance;
            VoiceExamItem = Settings.PedestrianCrossingVoice;
            startBrakeTime = null;
            isBrakeRuleSpeaked = false;
        }

        protected override string SpeedLimitRuleCode { get { return DeductionRuleCodes.RC40701; } }

        public override string ItemCode
        {
            get { return ExamItemCodes.SlowSpeed; }
        }
    }
}
