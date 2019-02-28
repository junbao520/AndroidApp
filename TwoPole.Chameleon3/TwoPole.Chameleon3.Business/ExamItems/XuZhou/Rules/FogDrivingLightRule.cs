using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;
namespace TwoPole.Chameleon3.Business.Areas.JiangSu.Xuzhou.Rules
{
    /// <summary>
    /// 雾天行驶（雾灯和报警灯,不需要开近光） 
    /// 客户要求雾天行驶不判远光灯
    /// </summary>
    public class FogDrivingLightRule : LightRule
    {

        private readonly string[] _validPropertyNames = { "FogLight", "CautionLight", "LowBeam","HighBeam","OutlineLight", "RightIndicatorLight", "LeftIndicatorLight" };

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            //if (sensor.HighBeam)
            //    return false;

            if (sensor.FogLight && 
                sensor.CautionLight && 
                sensor.OutlineLight)
                return true;

            return false;
        }
    }
}
