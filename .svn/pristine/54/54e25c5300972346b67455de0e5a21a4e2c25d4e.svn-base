using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    public class DefaultTriggerHandler : DisposableBase, ITriggerHandler
    {
        public ITrigger[] Triggers { get; private set; }
        public IMessenger Messenger { get; private set; }

        public DefaultTriggerHandler(IProviderFactory providerFactory,
            IMessenger messenger)
        {
            Triggers = providerFactory.CreateTriggers();
            Messenger = messenger;

        }

        public void Start(ExamContext context)
        {
            foreach (var trigger in Triggers)
            {
                try
                {
                    trigger.Start(context);
                }
                catch (Exception exp)
                {
                    //Logger.ErrorFormat("启动触发器{0}发生异常，原因：{1}", trigger.Name, exp, exp);
                }
            }
            Messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
        }

        protected virtual void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            foreach (var trigger in Triggers.Where(x => x.IsRunning).OfType<CarSignalTrigger>())
            {
                try
                {
                    trigger.Execute(message.CarSignal);
                }
                catch (Exception exp)
                {
                   // Logger.ErrorFormat("执行触发器{0}发生异常，原因：{1}", trigger.Name, exp, exp);
                }
            }
        }

        public void Stop()
        {
            Messenger.Unregister(this);
            foreach (var trigger in Triggers)
            {
                try
                {
                    trigger.Stop();
                }
                catch (Exception exp)
                {
                   // Logger.ErrorFormat("结束触发器{0}发生异常，原因：{1}", trigger.Name, exp, exp);
                }
            }
        }

        protected override void Free(bool disposing)
        {
            Messenger.Unregister(this);
            foreach (var trigger in Triggers.OfType<IDisposable>())
            {
                trigger.Dispose();
            }
            Triggers = null;
        }
    }
}
