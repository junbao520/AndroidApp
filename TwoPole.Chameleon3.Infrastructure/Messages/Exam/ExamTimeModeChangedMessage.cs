using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class ExamTimeModeChangedMessage : GenericMessage<ExamTimeMode>
    {
        public ExamTimeModeChangedMessage(ExamTimeMode mode)
            : base(mode)
        {
        }

        public ExamTimeMode Mode
        {
            get { return this.Content; }
        }
    }
}
