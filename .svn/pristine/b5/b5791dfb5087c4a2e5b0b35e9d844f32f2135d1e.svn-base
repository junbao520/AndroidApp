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
    /// 右转, 减速项目
    /// 路口右转向灯评判
    /// 特殊：打错转向灯
    /// </summary>
    public class TurnRight : SlowSpeed
    {
   

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.TurnRightVoice;
            MaxDistance = Settings.TurnRightDistance;
            SlowSpeedLimit = Settings.TurnRightSpeedLimit;
            OverSpeedMustBrake = Settings.TurnRightBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.TurnRightLightCheck;
            CheckBrakeRequired = Settings.TurnRightBrakeRequire;
            CheckLoudSpeakerInDay = Settings.TurnRightLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.TurnRightLoudSpeakerNightCheck;
        }


        protected override void StopCore()
        {
            if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) >= Constants.ErrorSignalCount)
            {
                //打错转向灯
                BreakRule(DeductionRuleCodes.RC30205);
            }
            else
            {
                //检测转向灯
                //if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) < Constants.ErrorSignalCount)
                {
                    //是否打转向灯
                    BreakRule(DeductionRuleCodes.RC30205);
                }
                else
                {
                    //检测时间
                    var isOk = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
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
            get { return DeductionRuleCodes.RC40901; }
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.TurnRight; }
        }
    }
}
