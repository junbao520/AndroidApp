using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("上车准备结束标志")]
    public enum PrepareDrivingEndFlag : byte
    {
        [Description("安全带")]
        SafeBelt = 1,

        [Description("开关车门")]
        Door = 2,

        [Description("安全带和发动机打火")]
        EngineAndSafeBelt = 3,
       [Description("手动触发")]
        ManualTrigger = 4,
        [Description("踩刹车结束（泸州）")]
        Brake = 5,

    }


}
