using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("车辆运动状态")]
    public enum CarState : byte
    {
        [Description("停车")]
        Stop = 0,

        [Description("运动")]
        Moving = 1,
    }
    public  enum OpenLightResult :byte
    {
        Default=0,
        [Description("远光")]
        HighBeam = 1,

        [Description("近光")]
        LowBeam = 2,
    }
}
