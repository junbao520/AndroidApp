using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    public abstract class MessageTrigger : TriggerBase
    {
        public override int Order
        {
            get { return Orders.MessageTriggerOrder + 10; }
        }

        protected IMessenger Messenger { get; private set; }

        protected MessageTrigger(IMessenger messenger)
        {
            Messenger = messenger;
        }

        public override void Start(ExamContext context)
        {
            if (ValidParameters())
            {
                base.Start(context);
                RegisterMessages(Messenger);
            }
            else
            {
                //Logger.WarnFormat("触发器：{0}-验证参数失败，停止执行；", Name);
            }
        }

        protected virtual void RegisterMessages(IMessenger messenger)
        {
        }

        public override void Stop()
        {
            base.Stop();
            Messenger.Unregister(this);
        }

        protected virtual bool ValidParameters()
        {
            return true;
        }

        protected override void Free(bool disposing)
        {
            base.Free(disposing);
            Stop();
        }
    }
}
