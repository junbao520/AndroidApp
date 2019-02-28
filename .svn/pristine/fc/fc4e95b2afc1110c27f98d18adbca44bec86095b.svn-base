using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public sealed class NeutralGearChangedMessage : GenericMessage<bool>
    {
        public NeutralGearChangedMessage(bool isNeutralGear)
            : base(isNeutralGear) { }

        public bool IsNeutralGear { get { return base.Content; } }
    }
}
