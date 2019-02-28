using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 雾天行驶（只开雾灯和报警灯）
    /// </summary>
    public class FogDrivingLightRule2 : LightRule
    {

        private readonly string[] _validPropertyNames = { "FogLight", "CautionLight", "OutlineLight", "LowBeam" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        private bool Left = false;
        private bool Right = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight)
                Left = true;
            if (sensor.RightIndicatorLight)
                Right = true;

            if (sensor.HighBeam)
                return false;

            if (sensor.FogLight && sensor.OutlineLight && ( sensor.CautionLight || (Left && Right) ) )
                return true;

            return false;
        }
    }
}
