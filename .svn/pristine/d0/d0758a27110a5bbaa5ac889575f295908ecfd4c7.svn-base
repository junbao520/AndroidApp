using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    /// <summary>
    /// 距离改变触发
    /// </summary>
    public abstract class DistanceTrigger : MessageTrigger
    {
        public double Distance { get; protected set; }

        protected DistanceTrigger(IMessenger messenger)
            :base(messenger)
        {
        }

        protected override void RegisterMessages(IMessenger messenger)
        {
            base.RegisterMessages(messenger);
            messenger.Register<TraveledDistanceChangedMessage>(this, OnDistanceChanged);
        }

        protected override bool ValidParameters()
        {
            return Distance > 0;
        }

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
           // Distance = settings.GetIntValue("Distance", -1);
        }

        protected virtual void OnDistanceChanged(TraveledDistanceChangedMessage message)
        {
            if (!IsRunning)
                return;

            if (message.TraveledDistance > Distance)
            {
                //Logger.DebugFormat("{0}-行驶里程{0}达到触发值{1}", Name, message.TraveledDistance, Distance);
                Run(message.TraveledDistance);
            }
        }

        public abstract void Run(double currentDistance);

        protected override void Free(bool disposing)
        {
            base.Free(disposing);
            Messenger.Unregister(this);
        }
    }
}
