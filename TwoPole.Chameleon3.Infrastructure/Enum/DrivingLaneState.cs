using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("道路行驶状态")]
    public enum LaneState : byte
    {
        [Description("普通行驶")]
        Normal,

        [Description("压线行驶")]
        Cross,
    }
}
