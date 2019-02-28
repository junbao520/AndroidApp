using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    public class VgkData
    {
        public double DistanceEast;
        public double DistanceNorth;
        public double DistanceHigh;
        public DateTime UtcTime;
        public int FixedSatelliteCount;

        public static VgkData Parse(string inputString)
        {
            VgkData data = new VgkData();
            if (inputString == null || inputString.Length == 0)
                return data;
            string dataString = inputString;
            if (inputString.Contains("*"))
                dataString = inputString.Substring(0, inputString.IndexOf('*')); // strip off the checksum
            else
                return data;
            string[] values = dataString.Split(',');
            if (values.Length < 9)
            { throw new FormatException(); }

            data.DistanceEast = double.Parse(values[4]);
            data.DistanceNorth = double.Parse(values[5]);
            data.DistanceHigh = double.Parse(values[6]);
            data.FixedSatelliteCount = int.Parse(values[8]);
            return data;
        }
        //$PTNL,VGK,025508.00,121712,-0001.708,-0038.987,-0010.642,3,05,3.3,M*1A
        //025508.00：时间 美国时间02点55分08秒00毫秒
        //121712：日期12年12月17日
        //-0001.708：与基准点在东方的距离（平面），单位为 米，负为在扩展基站以西；
        //-0038.987：与基准点在北方的距离（平面），单位为 米；负为在扩展基站以南；
        //-0010.642：与基准点在高度的距离（平面），单位为 米；负为低于基准点；
        //3：差分类型--RTK固定；
        //05：卫星数；
        //3.3：PDOP，定位精度综合因子，数值越小精度越高；

    }
}
