using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 左转 + 远近光交替1次
    /// </summary>
    public class LowAndHighBeamOnceLeftIndicatorRule : LightRule
    {
        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam", "LeftIndicatorLight" };
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        private bool _isLeft = false;

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight)
                _isLeft = true;
            if (sensor.RightIndicatorLight ||
                sensor.CautionLight ||
                sensor.FogLight)
                return false;

            if (!sensor.LowBeam ||
                !sensor.OutlineLight)
                return false;

            double timeout = LightTimeout;
            ////作对就通过,不能检测7秒前的，不然会检测到上一次操作的远光(中控还没有配置时间和操作)
            //if (Settings.SimulationLightPassway == 1)
            //    timeout = 3;

            var result = AdvancedSignal.CheckHighBeam(timeout, 1) && _isLeft;
            return result;
        }
    }
}
