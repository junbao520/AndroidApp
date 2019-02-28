using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class BinaryCarSensorMessage : MessageBase
    {
        public BinaryCarSensorMessage(byte[] CarSensor)
        {

            this.Sensor = CarSensor;
        }
        public byte[] Sensor { get; set; }
    }
}