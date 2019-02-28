using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    /// <summary>
    /// 接收到GPS信号触发
    /// </summary>
    public abstract class CarSignalTrigger : TriggerBase, ICarSignalDependency
    {
        public override int Order
        {
            get { return Orders.CarSignalTriggerMinOrder + 10; }
        }

        public virtual void Execute(CarSignalInfo signalInfo)
        {
            if(IsRunning)
                Run(signalInfo);
        }

        public abstract void Run(CarSignalInfo signalInfo);
    }
}
