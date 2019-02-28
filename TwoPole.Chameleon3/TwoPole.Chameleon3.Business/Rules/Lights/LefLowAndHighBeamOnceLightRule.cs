using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 开启近光,左转
    /// </summary>
    public class LeftLowAndHighBeamOnceLightRule : LightRule
    {

        private readonly string[] _validPropertyNames = { "OutlineLight", "LowBeam", "HighBeam", "LeftIndicatorLight" };
        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return !propertyNames.All(x => _validPropertyNames.Contains(x));
        }

        private bool Left = false;
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (sensor.LeftIndicatorLight)
                Left = true;
            if (sensor.RightIndicatorLight ||
                sensor.CautionLight ||
                sensor.FogLight)
                return false;


            //由于程序检测语音播完时，可能第一次闪光都已经操作过了，所以时间往前推1秒,20160801,李
            double LightTimeout_new = LightTimeout + 1;
            var result = AdvancedSignal.CheckHighBeam(LightTimeout_new, 2) && Left;
            return result;
        }
    }

}