using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface ICarSignalSeed : IDisposable
    {
        void InitAsync();

        Task StartAsync();

        Task StopAsync();

        Action<CarSignalInfo> CarSignalRecevied { get; set; }
    }
}
