using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IModule : IDisposable
    {
        Task InitAsync(ExamInitializationContext context);

        Task StartAsync(ExamContext context);

        Task StopAsync(bool close=false);
    }
}
