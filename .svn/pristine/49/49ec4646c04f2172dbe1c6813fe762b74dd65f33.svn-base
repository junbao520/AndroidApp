using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class ExamItemStateChangedMessage : MessageBase
    {
        public ExamItemStateChangedMessage(ExamItemState currentState, ExamItemState oldState)
        {
            this.OldState = oldState;
            this.NewState = currentState;
        }

        public ExamItemStateChangedMessage(ExamItemState currentState, ExamItemState oldState, object sender)
            : base(sender)
        {
            this.OldState = oldState;
            this.NewState = currentState;
        }

        public IExamItem ExamItem { get { return Sender as IExamItem; } }

        public ExamItemState OldState { get; private set; }
        public ExamItemState NewState { get; private set; }
    }
}
