using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    public class AvrData
    {
        public static AvrData Parse(string inputString)
        {
            AvrData data = new AvrData();
            if (inputString == null || inputString.Length == 0)
                return data;
            string dataString = inputString;
            if (inputString.Contains("*"))
                dataString = inputString.Substring(0, inputString.IndexOf('*')); // strip off the checksum
            else
                return data;
            string[] values = dataString.Split(',');
            if (values.Length < 12)
            { throw new FormatException(); }

            data.YawAngle = double.Parse(values[3]);
            data.TiltAngle = double.Parse(values[5]);
            data.Distance = double.Parse(values[9]);

            return data;
        }
        public double YawAngle;
        public double TiltAngle;
        public double Distance;
        //AVR实例:  $PTNL,AVR,064754.00,+0.3937,Yaw,-3.0095,Tilt,,,1.368,3,1.4,15*3A
        //064754.00 UTC时间
        //+0.3937,Yaw 角度 两个天线的水平角度和正北的夹角，以定位的天线为基础；车辆行驶的方向与正北的夹角；
        //（请参考上图,BD982默认测向天线和位置输出均为车尾的那个天线.即与扩展基站做差分的那个天线.此天线如果放在车尾,则AVR里的航向矢量是由车头天线指向车尾.）
        //-3.0095，Tilt 两个天线的垂直夹角，以测姿的天线为基础，即车辆身与地平面的夹角；
        //1.368：车辆上两个天线的距离
        //3：RTK固定；
        //1.4：定位精度综合因子，数值越小精度越高；
        //15：卫星数15颗卫星；

    }
}
