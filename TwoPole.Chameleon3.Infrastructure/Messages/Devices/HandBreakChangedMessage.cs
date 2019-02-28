using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class HandBreakChangedMessage: StateChangedMessage
    {
      
        public HandBreakChangedMessage(bool newValue) : base(newValue)
        {
      
        }
    }
}
