using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 小灯（处理爱丽舍近光会自动闪烁）
    /// </summary>
    public class OutlineLightRuleIgnoreLowBeam : LightRule
    {
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (!sensor.OutlineLight)
                 return false;
            if (sensor.HighBeam)
                 return false;
            //if (sensor.LowBeam)
            //    return false;
            if (sensor.FogLight)
                 return false;
            if (sensor.LeftIndicatorLight)
                 return false;
            if (sensor.RightIndicatorLight)
                 return false;

             return true;
        }
    }
}
