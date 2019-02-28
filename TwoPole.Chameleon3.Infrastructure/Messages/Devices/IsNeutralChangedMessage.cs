using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class IsNeutralChangedMessage : StateChangedMessage
    {
        public IsNeutralChangedMessage(bool newValue)
            : base(newValue)
        {
        }
    }
}
