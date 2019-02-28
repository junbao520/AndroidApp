using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 开启近光,右转
    /// </summary>
    public class RightLowBeamLightRule : LightRule
    {

        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "RightIndicatorLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        private bool Right = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.RightIndicatorLight)
                Right = true;

            if (sensor.HighBeam)
                return false;

            return sensor.LowBeam && sensor.OutlineLight && Right;
        }
    }
}
