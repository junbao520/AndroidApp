using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public sealed class MapPointChangedMessage : MessageBase
    {
        public MapPointChangedMessage(MapPoint currentPoint, MapPoint previousPoint = null)
        {
            CurrentNode = currentPoint;
            PreviousNode = previousPoint;
        }

        public MapPoint PreviousNode { get; private set; }
        public MapPoint CurrentNode { get; private set; }
    }
}
