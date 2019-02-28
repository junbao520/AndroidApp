using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 远近光交替4次，往前推了1秒的检测时间
    /// </summary>
    public class LowAndHighBeamFourthLightRule : LightRule
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
            //由于程序检测语音播完时，可能第一次闪光都已经操作过了，所以时间往前推1秒,20160801,李
            double LightTimeout_new = LightTimeout + 1;
            var result = AdvancedSignal.CheckHighBeam(LightTimeout_new, 4);
            return result;
        }
    }
}
