using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 准备结束整个考试的消息
    /// </summary>
    public class ExamFinishingMessage : MessageBase
    {
        public ExamFinishingMessage(bool manual)
            : base()
        {
            this.Manual = manual;
        }

        /// <summary>
        /// 是否人工强制结束
        /// </summary>
        public bool Manual { get; private set; }
    }
}
