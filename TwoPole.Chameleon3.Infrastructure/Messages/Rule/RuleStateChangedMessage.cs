using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class RuleStateChangedMessage : MessageBase
    {
        public RuleStateChangedMessage(RuleState currentState, RuleState oldState)
        {
            this.OldState = oldState;
            this.NewState = currentState;
        }

        public RuleStateChangedMessage(RuleState currentState, RuleState oldState, object sender)
            : base(sender)
        {
            this.OldState = oldState;
            this.NewState = currentState;
        }

        public IRule Rule { get { return Sender as IRule; } }

        public RuleState OldState { get; private set; }
        public RuleState NewState { get; private set; }
    }
}
