using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.Rules;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
{
     
    /// <summary>
    /// 小灯和报警灯；
    /// 近光、远光、左转向、右转向和雾灯不能开启
    /// </summary>
    public class OutlineAndCautionLightRule : LightRule
    {
        //左右转向不管，忽略车左右信号不同步情况
        private readonly string[] _validPropertyNames = { "CautionLight", "OutlineLight", "HighBeam", "LowBeam", "LeftIndicatorLight", "RightIndicatorLight" };

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
              
            }
            if (sensor.RightIndicatorLight)
            {
                Right = true;
          
            }
            if (sensor.CautionLight)
            {
              
                Left = true;
                Right = true;
            }
            if (sensor.CautionLight||(Left&&Right))
            {
                Logger.Info("难以移动已经合格");
                IsLightFinish = true;
                return true;
            }
                
            return false;
        }
    }
}
