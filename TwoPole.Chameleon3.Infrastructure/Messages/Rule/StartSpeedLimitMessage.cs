using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class StartSpeedLimitMessage : GenericMessage<double>
    {
        public StartSpeedLimitMessage(double speedInKmh)
            :base(speedInKmh)
        {
        }

        public double SpeedInKMH
        {
            get { return this.Content; }
        }
    }
}
