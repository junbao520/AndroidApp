using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 雾天行驶（雾灯和报警灯）
    /// </summary>
    public class FogDrivingLightRule: LightRule
    {

        private readonly string[] _validPropertyNames = { "FogLight", "CautionLight", "LowBeam", "OutlineLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.HighBeam ||
             sensor.LeftIndicatorLight ||
             sensor.RightIndicatorLight)
                return false;

            if (sensor.FogLight && 
                sensor.CautionLight && 
                sensor.OutlineLight &&
                sensor.LowBeam)
                return true;

            return false;
        }
    }
}
