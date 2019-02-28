using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class StartOnIntersectionMessage: MessageBase
    {
        public StartOnIntersectionMessage(ThroughDirection direction)
        {
            this.Direction = direction;
        }

        public ThroughDirection Direction { get; private set; }
    }

    public enum ThroughDirection
    {
        [Description("直行")]
        Straight = 0,
        [Description("左转弯")]
        TurnLeft = 1,
        [Description("右转弯")]
        TurnRight = 2
    }
}
