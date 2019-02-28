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
    /// 左转, 减速项目
    /// 路口右转向灯评判
    /// 特殊：打错转向灯
    /// </summary>
    public class TurnLeft : SlowSpeed
    {
       


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.TurnLeftVoice;
            MaxDistance = Settings.TurnLeftDistance;
            SlowSpeedLimit = Settings.TurnLeftSpeedLimit;
            OverSpeedMustBrake = Settings.TurnLeftBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.TurnLeftLightCheck;
            CheckBrakeRequired = Settings.TurnLeftBrakeRequire;
            CheckLoudSpeakerInDay = Settings.TurnLeftLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.TurnLeftLoudSpeakerNightCheck;
        }


        protected override void StopCore()
        {
            if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) >= Constants.ErrorSignalCount)
            {
                //打错转向灯
                BreakRule(DeductionRuleCodes.RC30205);
            }
            else
            {
                //检测转向灯
                if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) < Constants.ErrorSignalCount)
                {
                    //是否打转向灯
                    BreakRule(DeductionRuleCodes.RC30205);
                }
                else
                {
                    var isOk = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                    if (!isOk)
                    {
                        BreakRule(DeductionRuleCodes.RC30206);
                    }
                }
            }
           

            base.StopCore();
        }

        protected override string SpeedLimitRuleCode
        {
            get { return DeductionRuleCodes.RC40801; }
        }


        public override string ItemCode
        {
            get { return ExamItemCodes.TurnLeft; }
        }
    }
}
