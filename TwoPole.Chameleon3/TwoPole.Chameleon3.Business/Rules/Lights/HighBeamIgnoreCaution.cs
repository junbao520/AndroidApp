using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 远光（左转向、右转向、雾灯都不能开启）(忽略应急灯，即开不开都可以)
    /// </summary>
    public class HighBeamIgnoreCaution : LightRule
    {

        private readonly string[] _validPropertyNames = { "HighBeam", "OutlineLight", "LowBeam", "CautionLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight ||
             sensor.RightIndicatorLight ||
             sensor.FogLight)
                return false;

            if (sensor.HighBeam && 
                sensor.LowBeam && 
                sensor.OutlineLight)
                return true;

            return false;
        }
    }
}
