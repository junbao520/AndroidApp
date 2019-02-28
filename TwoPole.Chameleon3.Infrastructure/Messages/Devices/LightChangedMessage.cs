using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 灯光改变的消息类
    /// </summary>
    public sealed class LightChangedMessage : MessageBase
    {
        public LightChangedMessage(IList<string> propertyNames, CarSensorInfo sensor)
        {
            this.PropertyNames = propertyNames;
            this.Sensor = sensor;
        }

        public CarSensorInfo Sensor { get; private set; }

        /// <summary>
        /// 有灯光变化的属性名称
        /// </summary>
        public IList<string> PropertyNames { get; private set; }

        /// <summary>
        /// 灯光变化后的值
        /// </summary>
        public IList<bool> PropertyValues { get; private set; }
    }
}
