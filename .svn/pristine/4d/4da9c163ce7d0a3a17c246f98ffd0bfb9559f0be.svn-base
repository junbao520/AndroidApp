using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 进行压线检测，是否进行压线检测
    /// </summary>
    public sealed class RuleCheckRollingSolidLinesMessage : MessageBase
    {
        public RuleCheckRollingSolidLinesMessage(bool enable)
        {
            this.Enable = enable;
        }

        /// <summary>
        /// 是否进行压线检测
        /// </summary>
        public bool Enable { get; private set; }
    }
}
