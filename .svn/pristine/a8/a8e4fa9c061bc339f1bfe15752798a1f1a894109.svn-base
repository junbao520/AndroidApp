using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;


namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.ExamItems
{
    /// <summary>
    /// 直接通过十字路口
    /// 1.不按规定减速慢行，检测速度是否超过限速
    //2.项目语音
    //3 项目距离 
    //4.夜考不交替使用远近光灯示意
    //特殊：不能乱打灯扣10分
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
        }

        //是否已经播报乱打转向灯
        private bool isSpeakIndicator = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor == null)
                return;

            //检测刹车保持时间
            if (Settings.BrakeKeepTime > 0 && !isBrakeRuleSpeaked)
            {
                if (signalInfo.Sensor.Brake)
                {
                    if (!startBrakeTime.HasValue)
                    {
                        startBrakeTime = DateTime.Now;
                    }
                }
                else
                {
                    if (!startBrakeTime.HasValue)
                        return;

                    isBrakeRuleSpeaked = true;

                    if ((DateTime.Now - startBrakeTime.Value).TotalSeconds < Settings.BrakeKeepTime)
                    {
                        BreakRule(DeductionRuleCodes.RC30221);
                    }

                }

            }

            //乱打转向灯扣10分
            if (!isSpeakIndicator && (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) >= Constants.ErrorSignalCount ||
                CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) >= Constants.ErrorSignalCount))
            {
                isSpeakIndicator = true;
                BreakRule(DeductionRuleCodes.RC40212);
            }


        }
        public override string ItemCode
        {
            get { return ExamItemCodes.StraightThrough; }
        }
    }
}
