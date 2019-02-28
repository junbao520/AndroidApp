﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public sealed class OpenDoorMessage : MessageBase { }
    public sealed class CloseDoorMessage : MessageBase { }
    public class EngineStartMessage : MessageBase { }

    public class EngineStopMessage
        : MessageBase { }

}