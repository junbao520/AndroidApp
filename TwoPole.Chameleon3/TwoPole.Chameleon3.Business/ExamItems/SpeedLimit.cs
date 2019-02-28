using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Threading;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 限速项目
    /// </summary>
    public class SpeedLimit : SlowSpeed
    {
   
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

        public override string ItemCode { get { return ExamItemCodes.SharpTurn; } }
       
    }
}
