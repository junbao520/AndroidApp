using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("档位显示来源")]
    public  enum GearSource:byte
    {
        [Description("转速比")]
        SpeadRadio=1,
        [Description("档显")]
        GearDisplay=2,
        [Description("OBD")]
        OBD =3,
    }
}
