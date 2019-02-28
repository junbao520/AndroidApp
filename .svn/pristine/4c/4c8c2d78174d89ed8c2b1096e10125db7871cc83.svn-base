using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure
{
    public sealed class ExamItemExecutionContext
    {
        public string ItemCode { get; set; }

        public ExamItemTriggerSource TriggerSource { get; set; }

        public MapPoint TriggerPoint { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public ExamContext ExamContext { get; private set; }

        public ExamTimeMode ExamTimeMode { get { return ExamContext.ExamTimeMode; } }

        public ExamMode ExamMode { get { return ExamContext.ExamMode; } }

        public string ExamGroup { get; set; }

        public bool HasMap { get { return !ExamContext.Map.IsEmpty(); } }


        public ExamItemExecutionContext(ExamContext context)
        {
            ExamContext = context;
            ExamGroup = ExamContext.ExamGroup;
        }
    }
}
