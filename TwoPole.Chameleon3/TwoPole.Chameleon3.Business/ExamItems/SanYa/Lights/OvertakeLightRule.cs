﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.Rules;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
{
    /// <summary>
    /// 检测左右转向灯，和远近光交替使用
    /// </summary>
    public class OvertakeLightRule : LightRule
    {
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam", "LeftIndicatorLight", "RightIndicatorLight" };

        private bool isOpenLeftIndicatorLight = false;
        private bool isOpenRightIndicatorLight = false;
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight)
                isOpenLeftIndicatorLight = true;
            if (sensor.RightIndicatorLight)
                isOpenRightIndicatorLight = true;
            //远近光交替
            var result = AdvancedSignal.CheckHighBeam(LightTimeout, 1);
            if (result && isOpenLeftIndicatorLight && isOpenRightIndicatorLight)
                return true;

            return false;
        }
    }
}
