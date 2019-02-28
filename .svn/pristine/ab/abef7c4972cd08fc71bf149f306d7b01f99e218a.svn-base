using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    public class HdtData
    {
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }
        protected double _angle;

        public static HdtData Parse(string inputString)
        {
            //---- declare vars
            HdtData data = new HdtData();
            string dataString = inputString.Substring(0, inputString.IndexOf('*')); // strip off the checksum
            string[] values = dataString.Split(',');
            //$GPHDT,176.304,T*32
            //---- if we don't have 15 (header + 14), it's no good
            if (values.Length < 3)
            { throw new FormatException(); }
            if (values[1] == null || values[1].Length == 0)
                data.Angle = 0.0d;
            else
                data.Angle = double.Parse(values[1]);
            return data;
        }
    }
}
