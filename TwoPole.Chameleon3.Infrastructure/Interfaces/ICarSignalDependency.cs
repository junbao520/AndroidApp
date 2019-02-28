using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface ICarSignalDependency
    {
        int Order { get; }

        void Execute(CarSignalInfo signalInfo);
    }
}
