using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;
namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
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

            var result = AdvancedSignal.CheckHighBeam(LightTimeout, 1);
            return result;
        }
    }
}
