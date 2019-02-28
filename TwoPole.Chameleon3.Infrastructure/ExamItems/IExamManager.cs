using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IExamManager
    {
        IList<IExamItem> ExamItems { get; }

        /// <summary>
        /// 开始考试
        /// </summary>
        Task StartExamAsync(ExamContext context);

        /// <summary>
        /// 结束考试
        /// </summary>
        Task StopExamAsync(bool close=false);

        /// <summary>
        /// 开始某个项目的考试
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        Task<IExamItem> StartItemAsync(ExamItemExecutionContext context, CancellationToken token, string ItemVoice = "", string ItemEndVoice = "");
    }
}