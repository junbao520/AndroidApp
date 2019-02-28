using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;

namespace TwoPole.Chameleon3.Business.Areas.ChongQin.WanZhou.Rules
{
    /// <summary>
    /// 路口右 左转近光
    /// </summary>
    public class TurnRightBeamLightRule : LightRule
    {
        private bool isOpenRightIndicatorLight = false;
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "RightIndicatorLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.HighBeam || sensor.LeftIndicatorLight || sensor.FogLight || sensor.CautionLight)
                return false;

            if (sensor.RightIndicatorLight)
                isOpenRightIndicatorLight = true;

            return sensor.LowBeam && sensor.OutlineLight && isOpenRightIndicatorLight;
        }
    }
}
