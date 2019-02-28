using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
{
    /// <summary>
    /// 远光（左转向、右转向、雾灯都不能开启）
    /// </summary>
    public class HighBeamLightRule : LightRule
    {

        private readonly string[] _validPropertyNames = { "HighBeam", "OutlineLight", "LowBeam" };

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

            if (sensor.HighBeam &&
                sensor.LowBeam)
                return true;
            return false;
        }
    }
}
