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
    public class CarSensorMessage : MessageBase
    {
        public CarSensorMessage(string[] CarSensor)
        {

            this.Sensor = CarSensor;
        }
        public string[] Sensor { get; set; }
    }

    public class CarSensorBthMessage : MessageBase
    {
        public CarSensorBthMessage(string CarSensor)
        {
            this.Sensor = CarSensor;
        }
        public string Sensor { get; set; }
    }
    public class USBConnectMessage : MessageBase
    {
        public USBConnectMessage()
        {

        }
    }

}