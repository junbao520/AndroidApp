using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    public interface ITrigger
    {
        string Name { get; set; }

        int Order { get; set; }

        bool IsRunning { get; }

        void Start(ExamContext context);

        void Stop();
    }
}
