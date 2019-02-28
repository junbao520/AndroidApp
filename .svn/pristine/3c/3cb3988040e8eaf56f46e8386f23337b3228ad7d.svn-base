using System;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class GearChangedMessage : StateChangedMessage<Gear>
    {
        public GearChangedMessage(Gear currentGear, Gear lastGear)
            :base(currentGear, lastGear)
        {
        }

        public Gear LastGear { get { return OldValue; } }
        public Gear Gear { get { return NewValue; } }
    }
}
