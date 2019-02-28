using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class EngineRuleMessage : MessageBase
    {
        public EngineRuleMessage(bool enable)
        {
            this.Enable = enable;
        }

        public bool Enable { get; private set; }
    }
}
