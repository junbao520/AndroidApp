using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class CarStateChangedMessage : StateChangedMessage<CarState>
    {
        public CarStateChangedMessage(CarState newValue, CarState oldValue)
            : base(newValue, oldValue)
        {
        }

        public bool IsMoving { get { return NewValue == CarState.Moving; } }
    }
}
