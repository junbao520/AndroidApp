using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;


namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    ///1.直接通过十字路口
    ///1.不按规定减速慢行，检测速度是否超过限速
    //2.项目语音
    //3 项目距离 
    //4.夜考不交替使用远近光灯示意
    /// </summary>
    public class StraightThroughIntersection : SlowSpeed
    {
 

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.StraightThroughIntersectionVoice;
            MaxDistance = Settings.StraightThroughIntersectionDistance;
            SlowSpeedLimit = Settings.StraightThroughIntersectionSpeedLimit;
            OverSpeedMustBrake = Settings.StraightThroughIntersectionBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.StraightThroughIntersectionLightCheck;
            CheckBrakeRequired = Settings.StraightThroughIntersectionBrakeRequire;
            CheckLoudSpeakerInDay = Settings.StraightThroughIntersectionLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.StraightThroughIntersectionLoudSpeakerNightCheck;
            //路口直行准备距离
            PrepareDistance = Settings.ThroughStraightPrepareD;
        }

       

        public override string ItemCode
        {
            get { return ExamItemCodes.StraightThrough; }
        }
    }
}
