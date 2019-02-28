using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 远光（左转向、右转向、雾灯都不能开启） 
    /// </summary>
    /// //TwoPole.Chameleon3.Business.Rules.HighBeamLightRule,TwoPole.Chameleon3.Business
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
                sensor.LowBeam && 
                sensor.OutlineLight)
                return true;

           

            return false;
        }
    }
}
