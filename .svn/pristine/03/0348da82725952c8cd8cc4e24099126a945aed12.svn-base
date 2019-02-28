using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class SlowSpeedMessage: MessageBase
    {
        public SlowSpeedMessage(SlowSpeedPlace place)
        {
            this.Place = place;
        }

        public SlowSpeedPlace Place { get; set; }
    }

    public enum SlowSpeedPlace
    {
        [Description("通过人行横道线")]
        PedestrianCrossing = 0,
        [Description("通过学校区域")]
        ThroughSchoolArea = 1,
        [Description("通过公共汽车区域")]
        ThroughBusArea = 2
    }
}
