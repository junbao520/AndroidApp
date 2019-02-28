using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;

namespace TwoPole.Chameleon3.Business.ExamItems.LuXian
{
    /// <summary>
    /// 起步除了左转灯都不能开
    /// </summary>
    public class VehicleStarting : TwoPole.Chameleon3.Business.ExamItems.VehicleStarting
    {
     

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!_hasChecked && ((DateTime.Now - StartTime).TotalSeconds > 11 || signalInfo.SpeedInKmh > 5))
            {
                _hasChecked = true;
                CheckAllLight(signalInfo);
            }
            base.ExecuteCore(signalInfo);
        }

        private bool _hasChecked = false;
        /// <summary>
        /// 检测除左转外的其他灯
        /// </summary>
        private void CheckAllLight(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.OutlineLight || signalInfo.Sensor.LowBeam || signalInfo.Sensor.HighBeam ||
                signalInfo.Sensor.FogLight || signalInfo.Sensor.CautionLight|| signalInfo.Sensor.RightIndicatorLight)
            {
                CheckRule(true, DeductionRuleCodes.RC41609);
            }
        }
    }
}
