using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class CarSignalReceivedMessage : GenericMessage<CarSignalInfo>
    {
        public CarSignalReceivedMessage(CarSignalInfo signalInfo)
            : base(signalInfo)
        {
        }

        public CarSignalInfo CarSignal { get { return this.Content; } }
    }
}
