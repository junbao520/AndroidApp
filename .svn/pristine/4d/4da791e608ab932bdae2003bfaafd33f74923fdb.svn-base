using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
{
    /// <summary>
    /// 开启前大灯远近光交替变换
    /// 适用海南三亚
    /// </summary>
    public class OpenLightLowAndHighBeamLightRule : LightRule
    {
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam" };
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        private bool isOpenLowBem = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LowBeam)
            { isOpenLowBem = true; }

            var result = AdvancedSignal.CheckHighBeam(LightTimeout, 1);
            if (result && isOpenLowBem)
                return true;
            return false;
        }
    }
}
