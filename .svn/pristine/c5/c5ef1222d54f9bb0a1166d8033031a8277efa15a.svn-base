using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 只开启双闪
    /// </summary>
    public class OpenCautionLightRuleOnly : LightRule
    {
        private readonly string[] _validPropertyNames = { "CautionLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            // return !propertyNames.All(x => _validPropertyNames.Contains(x));
            return false;
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
              
             return true;
        }
        protected override bool TimeoutRuleCheck(CarSensorInfo signalInfo)
        {
            if (!signalInfo.CautionLight)
            {
                return false;
            }
            return true;
        }
    }
}
