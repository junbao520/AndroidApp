using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Models
{
    public  class DirectionInfo
    {
        /// <summary>
        /// 角度
        /// </summary>
       public double AngleX { get; set; }
       public double AngleY { get; set; }
       public double AngleZ { get; set; }

        /// <summary>
        /// 角速度
        /// </summary>
       public double AngleSpeedX { get; set; }
       public double AngleSpeedY { get; set; }
       public double AngleSpeedZ { get; set; }

        /// <summary>
        /// 角加速度
        /// </summary>
       public double AccelerationX { get; set; }
       public double AccelerationY { get; set; }
       public double AccelerationZ { get; set; }
    }
}
