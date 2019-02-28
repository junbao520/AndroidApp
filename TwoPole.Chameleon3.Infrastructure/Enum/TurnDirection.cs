using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("转向方向")]
    public enum TurnDirection : byte
    {
        [Description("左转")]
        Left,
        [Description("右转")]
        Right
    }
}
