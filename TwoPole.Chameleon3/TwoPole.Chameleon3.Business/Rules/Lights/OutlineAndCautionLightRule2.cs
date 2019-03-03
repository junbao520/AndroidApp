using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 小灯和报警灯；
    /// 近光、远光雾灯不能开启（不管左右转向灯，处理左右转不同步问题）
    /// </summary>
    public class OutlineAndCautionLightRule2 : LightRule
    {
        private readonly string[] _validPropertyNames = { "CautionLight", "OutlineLight", "RightIndicatorLight", "LeftIndicatorLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }
        /// <summary>
        /// 记录是否已经检测到左转和右转
        /// </summary>
        private bool _hasLeft = false;
        private bool _hasRight = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            //if (!sensor.OutlineLight)
            //    return false;

            if (sensor.HighBeam)
                return false;
            //开近光扣分
            if (sensor.LowBeam)
                return false;
            if (sensor.FogLight)
                return false;
            if (_hasLeft && _hasRight)
                return true;

            if (sensor.LeftIndicatorLight)
                _hasLeft = true;
            if (sensor.RightIndicatorLight)
                _hasRight = true;
            if (sensor.CautionLight)
                _hasLeft = _hasRight = true;

            return false;
        }
    }
}
