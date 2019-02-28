using System.Collections.Generic;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 开启灯光（近光或者远光都可以）
    /// 不能开启雾灯和报警灯
    /// </summary>
    public class OpenHighLightRule : LightRule
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

            return sensor.LowBeam && sensor.OutlineLight;
        }
    }
}
