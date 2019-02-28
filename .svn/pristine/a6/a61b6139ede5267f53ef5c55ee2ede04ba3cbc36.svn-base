using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class SwitchGearMessage : StateChangedMessage<Gear>
    {
        public SwitchGearMessage(Gear currentGear, Gear lastGear)
            : base(currentGear, lastGear)
        {
            LastGear = lastGear;
            Gear = currentGear;
        }

        public Gear LastGear { get; private set; }
        public Gear Gear { get; private set; }
    }
}
