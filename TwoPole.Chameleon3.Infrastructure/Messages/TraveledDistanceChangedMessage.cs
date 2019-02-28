using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class TraveledDistanceChangedMessage : MessageBase
    {
        public TraveledDistanceChangedMessage(object sender, double newDistance)
            :base(sender)
        {
            TraveledDistance = newDistance;
        }

        public double TraveledDistance { get; private set; }
    }
}
