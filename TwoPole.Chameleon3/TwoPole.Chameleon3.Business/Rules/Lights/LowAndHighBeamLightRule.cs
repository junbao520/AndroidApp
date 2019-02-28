using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 远近光交替
    /// </summary>
    public class LowAndHighBeamLightRule : LightRule
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

            if (!sensor.LowBeam ||
                !sensor.OutlineLight)
                return false;

            //Light
            //
            //
            //LightTimeout = 3;
            var result = AdvancedSignal.CheckHighBeam(LightTimeout, 1);
            return result;
        }
    }
}
