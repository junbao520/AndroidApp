using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class ExamItemEndMessage : MessageBase
    {
        public ExamItemEndMessage(string examItemCode, ExamItemStartMode mode)
        {
            this.ExamItemCode = examItemCode;
            this.Source = mode;
        }

        public ExamItemEndMessage(string examItemCode, ExamItemStartMode mode, object sender)
            : base(sender)
        {
            this.ExamItemCode = examItemCode;
            this.Source = mode;
        }

        /// <summary>
        /// 触发结束考试的来源
        /// </summary>
        public ExamItemStartMode Source { get; private set; }

        /// <summary>
        /// 考试项目
        /// </summary>
        public string ExamItemCode { get; private set; }
    }
}
