using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    public abstract class MapPointChangedTrigger : MessageTrigger
    {
        protected MapPointChangedTrigger(IMessenger messenger)
            : base(messenger)
        {
        }

        protected override void RegisterMessages(IMessenger messenger)
        {
            base.RegisterMessages(messenger);
            messenger.Register<MapPointChangedMessage>(this, OnMapPointChanged);
        }

        protected virtual void OnMapPointChanged(MapPointChangedMessage message)
        {
        }
    }
}
