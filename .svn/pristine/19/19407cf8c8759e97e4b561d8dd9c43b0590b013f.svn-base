using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 状态改变的消息
    /// </summary>
    public class StateChangedMessage<T> : MessageBase
    {
        public StateChangedMessage(T newValue, T oldValue)
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
            this.ChangedTime = DateTime.Now;
        }

        public DateTime ChangedTime { get; private set; }

        public T NewValue { get; private set; }
        public T OldValue { get; private set; }

        public CarSignalInfo SignalInfo { get; set; }
    }

    public class StateChangedMessage : StateChangedMessage<bool>
    {
        public StateChangedMessage(bool newValue)
            : base(newValue, !newValue)
        {
        }
    }
}
