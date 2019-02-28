using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 起伏弯道项目(使用急弯坡路配置)
    /// </summary>
    public  class WavedCurve : SlowSpeed
    {
        protected IAdvancedCarSignal AdvancedSignal { get; private set; }


 

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.SharpTurnVoice;
            MaxDistance = Settings.SharpTurnDistance;
            SlowSpeedLimit = Settings.SharpTurnSpeedLimit;
            OverSpeedMustBrake = Settings.SharpTurnBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.SharpTurnLightCheck;
            CheckBrakeRequired = Settings.SharpTurnBrake;
            CheckLoudSpeakerInDay = Settings.SharpTurnLoudspeakerInDay;
            CheckLoudSpeakerInNight = Settings.SharpTurnLoudspeakerInNight;
          
           
        }



        public override string ItemCode
        {
            get { return ExamItemCodes.WavedCurve; }
        }
    }
}
