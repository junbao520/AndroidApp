using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    public static class GpsExtensions
    {
        public static Quality ToQuality(this NAFixStatus status)
        {
            switch (status)
            {
                case NAFixStatus.Narrow_INT:
                case NAFixStatus.FixedPos:
                    return Quality.FixedRealTimeKinematic;
                case NAFixStatus.None:
                    return Quality.Unknown;
                //case NovAtelFixStatus.FixedHeight:
                //case NovAtelFixStatus.DOPPLER_VELOCITY:
                //case NovAtelFixStatus.Single:
                //case NovAtelFixStatus.Psrdiff:
                //case NovAtelFixStatus.Sbas:
                //case NovAtelFixStatus.Propagated:
                //case NovAtelFixStatus.Omnistar:
                //case NovAtelFixStatus.L1_FLOAT:
                //case NovAtelFixStatus.IONOFREE_FLOAT:
                //case NovAtelFixStatus.Narrow_FLOAT:
                //case NovAtelFixStatus.L1_INT:
                //case NovAtelFixStatus.Omnistar_HP:
                //case NovAtelFixStatus.Omnistar_XP:
                default:
                    return Quality.NoFix;
            }
        }
    }
}
