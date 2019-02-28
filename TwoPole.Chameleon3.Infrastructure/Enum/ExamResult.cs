using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public enum ExamResult
    {
        [Description("判定中")]
        None,
        [Description("考试合格")]
        Pass,
        [Description("考试不合格")]
        Fail
    }
}
