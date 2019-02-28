using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;


namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 开启前大灯远近光交替变换
    /// 适用海南三亚
    /// </summary>
    public class OpenLightLowAndHighBeamLightRule: LightRule
    {
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam" };
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight ||
                sensor.RightIndicatorLight ||
                sensor.CautionLight ||
                sensor.FogLight)
                return false;


            var result = AdvancedSignal.CheckHighBeam(LightTimeout, 2);
            if (result && sensor.LowBeam)
                return true;
            return false;
        }
    }
}
