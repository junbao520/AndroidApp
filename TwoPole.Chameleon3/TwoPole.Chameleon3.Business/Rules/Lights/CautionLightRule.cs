﻿using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 报警灯；
    /// 近光、远光、左转向、右转向和小灯和雾灯不能开启
    /// </summary>
    public class CautionLightRule : LightRule
    {
        private readonly string[] _validPropertyNames = { "CautionLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
       
            if (!sensor.CautionLight)
                 return false;
            if (!sensor.OutlineLight)
                return false;
            if (sensor.HighBeam)
                 return false;
            //开不开近光都合格//沪州要求开近光不合格
            if (sensor.LowBeam)
                return false;
            if (sensor.FogLight)
                 return false;
            if (sensor.LeftIndicatorLight)
                 return false;
            if (sensor.RightIndicatorLight)
                 return false;

             return true;
        }
    }
}
