using System;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class GearChangedState
    {
        public Gear Gear { get; private set; }
        public double PeriodMilliseconds { get; private set; }
        public DateTime LastTime { get; private set; }
        public DateTime FirstTime { get; private set; }

        public GearChangedState(Gear gear, DateTime firstTime)
            : this(gear, firstTime, firstTime)
        {
        }

        public GearChangedState(Gear gear, DateTime firstTime, DateTime lastTime)
        {
            FirstTime = firstTime;
            LastTime = lastTime;
            Gear = gear;
            PeriodMilliseconds = (LastTime - FirstTime).TotalMilliseconds;
        }
    }
}