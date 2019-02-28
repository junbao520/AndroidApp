using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 窄路会车，开启近光(忽略应急灯，即开不开都可以)
    /// 不能开启远光、报警、雾灯
    /// </summary>
    public class LowBeamIgnoreCaution : LightRule
    {

        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "CautionLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.HighBeam)
                return false;

            return sensor.LowBeam && sensor.OutlineLight;
        }
    }
}
