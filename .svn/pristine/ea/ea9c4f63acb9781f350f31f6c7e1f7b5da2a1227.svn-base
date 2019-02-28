using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 急弯坡路
    ///  1.夜考不交替使用远近光灯示意
    /// </summary>
    public class SharpTurn : SlowSpeed
    {

        //public SharpTurn(IAdvancedCarSignal advancedSignal)
        //    : base(advancedSignal)
        //{
        //}

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
        protected override void StopCore()
        {

            if (Settings.SharpTurnLeftLightCheck&&!Settings.SharpTurnRightLightCheck)
            {
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
            //检测转向灯
            if (Settings.SharpTurnRightLightCheck&&!Settings.SharpTurnLeftLightCheck)
            {
                if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) < Constants.ErrorSignalCount)
                {
                    //是否打转向灯
                    BreakRule(DeductionRuleCodes.RC30205);
                    //是否
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
            //这样左转和右转只要随便开了一个就可以
            if (Settings.SharpTurnLeftLightCheck&&Settings.SharpTurnRightLightCheck)
            {
                var isOkRight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                var isOkLeft = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                if (!isOkLeft&&!isOkRight)
                {
                    BreakRule(DeductionRuleCodes.RC30206);
                }
            }


            base.StopCore();
        }

        public override string ItemCode { get { return ExamItemCodes.SharpTurn; } }
    }
}
