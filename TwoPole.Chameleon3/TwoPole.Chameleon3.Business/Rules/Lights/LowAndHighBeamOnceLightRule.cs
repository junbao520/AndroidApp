﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 远近光交替1次
    /// </summary>
    public class LowAndHighBeamOnceLightRule : LightRule
    {
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam" };
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

            if (!sensor.LowBeam ||
                !sensor.OutlineLight)
                return false;
            //var result = AdvancedSignal.CheckHighBeam(LightTimeout, 1);
            //由于设定7秒，会检测到上一个远光操作，如果连续几个远近光一次，会出现第2次不操作也会算对的情况，改为3秒
            //TODO：为什么要写死?
            //TODO:
            var result = AdvancedSignal.CheckHighBeam(LightTimeout, 1);
            return result;
        }
    }
}
