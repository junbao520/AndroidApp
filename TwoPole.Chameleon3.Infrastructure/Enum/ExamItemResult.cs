using System.ComponentModel;

namespace TwoPole.Chameleon3.Infrastructure
{
    [Description("考试结果")]
    public enum ExamItemResult
    {
        [Description("未考试")]
        None = 0,

        [Description("未通过")]
        Failed,

        /// <summary>
        /// 表示有扣分
        /// </summary>
        [Description("通过")]
        Passed,

        /// <summary>
        /// 满分通过
        /// </summary>
        [Description("满分")]
        Perfect,
    }
}