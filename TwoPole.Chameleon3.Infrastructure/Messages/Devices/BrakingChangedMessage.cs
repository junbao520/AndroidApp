using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class BrakingChangedMessage : StateChangedMessage
    {
        public BrakingChangedMessage(bool newValue) : base(newValue)
        {
        }
    }
}
