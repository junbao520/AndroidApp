using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IExamItem : IDisposable, ICarSignalDependency
    {
        #region 考试项目信息


        IList<IRule> Rules { get; set; }
        /// <summary>
        /// 考试项目名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 考试项目编码（唯一）
        /// </summary>
        string ItemCode { get; set; }

        /// <summary>
        /// 开始考试时的语音提示
        /// </summary>
        string VoiceText { get; set; }

       /// <summary>
       /// 语音文件
       /// </summary>
        string VoiceFile { get; set; }

        /// <summary>
        /// 项目结束语音文件
        /// </summary>
        string EndVoiceFile { get; set; }

        /// <summary>
        /// 当前考试状态
        /// </summary>
        ExamItemState State { get; }

        MapPoint TriggerPoint { get; }
        
        /// <summary>
        /// 设置考试规则
        /// </summary>
        /// <param name="rules">当前项目的考试规则</param>
        void SetRules(IEnumerable<IRule> rules);
        #endregion

       /// <summary>
       /// 异步执行考试规则
       /// </summary>
       /// <param name="context">规则检测的上下文</param>
       /// <param name="token">取消任务的Token</param>
       /// <returns></returns>
        Task StartAsync(ExamItemExecutionContext context, CancellationToken token);

        Task StopAsync();

        bool CheckRule(Func<bool> checker, string ruleCode, string subRuleCode = null);

        bool CheckRule(bool isBroken, string ruleCode, string subRuleCode = null);
    }
}
