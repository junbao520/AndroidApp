using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 整个考试结束的消息
    /// </summary>
    public class ExamFinishedMessage : GenericMessage<ExamContext>
    {
        public ExamFinishedMessage(bool manual, bool passed, ExamContext context)
            : base(context)
        {
            this.Manual = manual;
            this.Passed = passed;
        }

        /// <summary>
        /// 是否人工强制结束
        /// </summary>
        public bool Manual { get; private set; }

        /// <summary>
        /// 考试成功还是失败
        /// </summary>
        public bool Passed { get; private set; }

        public ExamContext ExamContext { get { return base.Content; } }
    }
}
