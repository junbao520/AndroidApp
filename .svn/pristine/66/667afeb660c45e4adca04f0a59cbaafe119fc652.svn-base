using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 近光(处理爱丽舍接线方式，近光会自动闪烁)
    /// </summary>
    public class LowBeamLightRuleIgnoreLowBeam : LightRule
    {

        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }


        private bool hasLowlighted = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.HighBeam)
                return false;
            if (sensor.LowBeam)
                hasLowlighted = true;

            return hasLowlighted && sensor.OutlineLight;
        }
    }
}
