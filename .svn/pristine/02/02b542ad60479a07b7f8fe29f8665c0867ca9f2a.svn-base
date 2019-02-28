using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Providers;

namespace TwoPole.Chameleon3.Infrastructure
{
    /// <summary>
    /// 考试规则接口
    /// </summary>
    public interface IRule : IProvider
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 规则执行序号
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 规则运行的模式
        /// </summary>
        RuleTimeMode TimeMode { get; }

        /// <summary>
        /// 规则分组
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// 获取当前的ExamItem
        /// </summary>
        IExamItem ExamItem { get; set; }

        RuleExecutionResult Check(CarSignalInfo carSignal);
    }
}
