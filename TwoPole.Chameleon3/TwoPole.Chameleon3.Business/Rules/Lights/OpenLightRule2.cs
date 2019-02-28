using System.Collections.Generic;
using System.Linq;
using TwoPole.Chameleon3.Business.Rules;
using TwoPole.Chameleon3.Infrastructure;


namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 开启灯光（近光）
    /// 不能开启雾灯和报警灯
    /// 最后再检测灯光是否打错
    /// </summary>
    public class OpenLightRule2 : LightRule
    {
        //private bool _firstCheck = false;
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        public override void Reset()
        {
            LightTimeout = Settings.SimulationLightTimeout;
            
            base.Reset();
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return false;
           

        }

        protected override bool TimeoutRuleCheck(CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight ||
                sensor.RightIndicatorLight ||
                sensor.CautionLight ||
                sensor.FogLight ||
                sensor.HighBeam)
                return false;

            return sensor.LowBeam && sensor.OutlineLight;
        }
    }
}
