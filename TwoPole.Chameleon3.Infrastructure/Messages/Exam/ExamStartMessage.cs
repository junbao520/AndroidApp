using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class ExamStartMessage : GenericMessage<ExamContext>
    {
        public ExamStartMessage(ExamContext context) : base(context) { }

        public ExamContext ExamContext { get { return base.Content; } }
    }
}
