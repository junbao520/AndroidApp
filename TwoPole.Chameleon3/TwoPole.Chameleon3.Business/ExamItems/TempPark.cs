using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 临时停车项目
    /// </summary>
    public  class TempPark : ExamItemBase
    {
     
        protected IAdvancedCarSignal AdvancedSignal { get; private set; }

        private TempParkStep tempParkStep = TempParkStep.None;

        private DateTime TempParkTime = DateTime.Now;

        /// <summary>
        /// 是否在停车时间范围内空挡
        /// </summary>
        private bool IsNeutral = false;

        /// <summary>
        /// 是否在停车时间范围内手刹
        /// </summary>
        private bool IsHandbrake = false;

    

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            //获取靠边停车项目距离
            MaxDistance = Settings.PullOverMaxDrivingDistance;
            VoiceExamItem = Settings.PullOverVoice;
          
           
        }

        /// <summary>
        /// 遵义客户要求修改起步的时候判断手刹空挡 应急灯三秒 不要开关车门
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            Logger.Debug(tempParkStep.ToString());
            if (signalInfo.CarState == CarState.Stop && tempParkStep == TempParkStep.None)
            {
                //判断右转向是否有3秒钟
                //第一次进来
                if (CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight) || !signalInfo.Sensor.RightIndicatorLight)
                {
                    BreakRule(DeductionRuleCodes.RC40610);
                }
                else
                {
                    var lastSignal = CarSignalSet.QueryCachedSeconds(Settings.TurnLightAheadOfTime).LastOrDefault();
                    if (lastSignal == null || !lastSignal.Sensor.RightIndicatorLight)
                    {
                        BreakRule(DeductionRuleCodes.RC40611);
                    }
                }
                TempParkTime = DateTime.Now;
                tempParkStep = TempParkStep.Wait;
            }
            else if (tempParkStep==TempParkStep.Wait)
            {
                if (signalInfo.Sensor.Gear== Gear.Neutral && IsNeutral==false)
                {
                    IsNeutral =true;
                }
                if (signalInfo.Sensor.Handbrake&&IsHandbrake==false)
                {
                    IsHandbrake = true;
                }
                if (signalInfo.CarState == CarState.Moving)
                {
                    tempParkStep = TempParkStep.Moving;
                }
            }
            else if (signalInfo.CarState == CarState.Moving && tempParkStep == TempParkStep.Moving)
            {
                if (!IsNeutral)
                {
                    BreakRule(DeductionRuleCodes.RC42801);
                }
                if (!IsHandbrake)
                {
                    BreakRule(DeductionRuleCodes.RC40607, DeductionRuleCodes.SRC4060701);
                }
                //如果小于2秒避免误判
                if (CarSignalSet.Query(TempParkTime).Count(d => d.Sensor.CautionLight) <2)
                {
                    BreakRule(DeductionRuleCodes.RC41601);
                }

                tempParkStep = TempParkStep.VehicleStarting;
                //临时停车完成之后先不要结束这个项目直接去检查左转向和 喇叭！
            }
            else if (signalInfo.CarState == CarState.Moving && tempParkStep == TempParkStep.VehicleStarting)
            {
                //起步应该是有
                if (Settings.IsCheckStartLight)
                {
                    if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) < Constants.ErrorSignalCount)
                    {
                        BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020501);
                    }
                }
                if ((Settings.VehicleStartingLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day) ||
                  (Settings.VehicleStartingLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night))
                {
                    if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.Loudspeaker))
                    {
                        BreakRule(DeductionRuleCodes.RC40208);
                    }
                }
                StopCore();
            }

        }


        public override string ItemCode
        {
            get { return ExamItemCodes.TempPark; }
        }
    }

    public enum TempParkStep
    {
        None,
        Wait,
        Moving,
        VehicleStarting
    }
}
