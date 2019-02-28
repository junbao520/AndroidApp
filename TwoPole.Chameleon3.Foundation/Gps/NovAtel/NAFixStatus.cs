using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    [Description("定位状态")]
    public enum NAFixStatus
    {
        [Description("未解算")]
        None,
        [Description("已设置固定坐标")]
        FixedPos,
        [Description("已设置固定高度")]
        FixedHeight,
        [Description("使用瞬时多普勒计算的速度解")]
        DOPPLER_VELOCITY,
        [Description("单点定位解")]
        Single,
        [Description("伪距差分定位解")]
        Psrdiff,
        [Description("星际增强系统解")]
        Sbas,
        [Description("卡尔曼滤波在没有新的观测量下给出的解算")]
        Propagated,
        [Description("OMNISTAR_VBS位置解")]
        Omnistar,
        [Description("L1浮点解")]
        L1_FLOAT,
        [Description("浮点电离层延迟模糊度解算")]
        IONOFREE_FLOAT,
        [Description("窄带浮点模糊度解算")]
        Narrow_FLOAT,
        [Description("L1固定解")]
        L1_INT,
        [Description("窄带整周模糊度解算")]
        Narrow_INT,
        [Description("OMNISTAR_HP位置解")]
        Omnistar_HP,
        [Description("OMNISTAR_XP位置解")]
        Omnistar_XP,
    }
}
