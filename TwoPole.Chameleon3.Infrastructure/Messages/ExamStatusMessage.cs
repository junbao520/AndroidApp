using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class ExamStatusMessage : MessageBase
    {
        public ExamStatusMessage(string message, CarSensorInfo sensorInfo)
        {
            Message = message;
            SensorInfo = sensorInfo;
        }

        public string Message { get; private set; }
        public CarSensorInfo SensorInfo { get; private set; }
    }
}
