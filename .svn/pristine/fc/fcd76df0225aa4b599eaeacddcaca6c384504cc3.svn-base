using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    //=======================================================================
    /// <summary>
    /// GPS Quality Indicator
    /// </summary>
    [Description("Gps精度")]
    public enum Quality : int
    {
        //Invalid = 0,
        //Fix = 1,
        //Differential = 2,
        //Sensitive = 3,

        /// <summary>
        /// Not enough information is available to specify the current fix quality.
        /// </summary>
        [Description("未知")]
        Unknown,
        /// <summary>No fix is currently obtained.</summary>
        [Description("定位")]
        NoFix,
        /// <summary>A fix is currently obtained using GPS satellites only.</summary>
        [Description("GPS定位")]
        GpsFix,
        /// <summary>A fix is obtained using both GPS satellites and DGPS/WAAS ground
        /// stations.  Position error is as low as 0.5-5 meters.</summary>
        [Description("差分精度")]
        DifferentialGpsFix,
        /// <summary>
        /// A PPS or pulse-per-second fix.  PPS signals very accurately indicate the start of a second.
        /// </summary>
        [Description("非高精度")]
        PulsePerSecond,
        /// <summary>
        /// Used for surveying.  A fix is obtained with the assistance of a reference station.  Position error is as low as 1-5 centimeters.
        /// </summary>
        [Description("高精度")]
        FixedRealTimeKinematic,
        /// <summary>
        /// Used for surveying.  A fix is obtained with the assistance of a reference station.  Position error is as low as 20cm to 1 meter.
        /// </summary>
        [Description("浮动精度")]
        FloatRealTimeKinematic,
        /// <summary>
        /// The fix is being estimated.
        /// </summary>
        [Description("非高精度")]
        Estimated,
        /// <summary>
        /// The fix is being input manually.
        /// </summary>
        [Description("非高精度")]
        ManualInput,
        /// <summary>
        /// The fix is being simulated.
        /// </summary>
        [Description("非高精度")]
        Simulated
    }
    //=======================================================================
}
