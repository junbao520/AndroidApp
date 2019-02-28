using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
{
    /// <summary>
    /// 雾天行驶（雾灯和报警灯） 2016/11/10 陶哥去三亚 要求所有灯光项目不管小灯  难以移动需要开双闪 和不能开雾灯   雾天行驶只要开雾灯和双闪就可以了
    /// </summary>
    public class FogDrivingLightRule : LightRule
    {

        private readonly string[] _validPropertyNames = { "FogLight", "CautionLight", "LowBeam", "OutlineLight" , "LeftIndicatorLight", "RightIndicatorLight" };
        bool IsOpenCautionLight = false;
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        private bool Left = false;
        private bool Right = false;
        private bool IsLightFinish = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (IsLightFinish)
            {
                return IsLightFinish;
            }
            if (sensor.LeftIndicatorLight)
            {
                Left = true;
               // LogManager.WriteSystemLog("FogDrivingLightRulesensor.LeftIndicatorLight");
               // Logger.Info("FogDrivingLightRule", "sensor.LeftIndicatorLight");
            }
            if (sensor.RightIndicatorLight)
            {
                Right = true;
               // LogManager.WriteSystemLog("FogDrivingLightRulesensorRightIndicatorLight");
              //  Logger.Info("FogDrivingLightRule","sensor.RightIndicatorLight");
            }
            if (sensor.CautionLight)
            {
                Left = Right = true;
               // LogManager.WriteSystemLog("FogDrivingLightRulesensorCautionLight");
                //Logger.Info("FogDrivingLightRule","sensor.CautionLight");
            }
            //内裹
            if (sensor.FogLight&&Right&&Left)
            {
                IsLightFinish = true;
               // LogManager.WriteSystemLog("雾天行驶已经合格");
                Logger.Info("雾天行驶已经合格");
                return true;
            }

            return false;
           
        }
    }
}
