using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 关闭所有灯光，远光、近光、雾灯、右转、报警灯(左转不管)
    /// </summary>
    public class CloseAllLightExceptLeftRule : LightRule
    {
        private readonly string[] _validPropertyNames = { "LeftIndicatorLight" };
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        //所有灯关完就直接结束项目
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (!sensor.HighBeam &&
                !sensor.LowBeam &&
                !sensor.OutlineLight &&
                !sensor.FogLight &&
                !sensor.CautionLight &&
                !sensor.RightIndicatorLight)
                return true;

            return false;
        }
        //protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        //{
        //    return false;
        //}

        protected override bool TimeoutRuleCheck(CarSensorInfo signalInfo)
        {
            if (signalInfo.HighBeam ||
                signalInfo.LowBeam ||
                signalInfo.OutlineLight)
                return false;
         
            return true;
        }
    }
}
