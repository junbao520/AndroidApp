using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("方向角来源")]
    public enum AngleSource : byte
    {
        [Description("GPS角度")]
        GPS = 1,

        [Description("陀螺仪角度")]
        Gyroscope = 2,

        [Description("外置陀螺仪角度")]
        ExternalGyroscope=3
    }
    
    
}
