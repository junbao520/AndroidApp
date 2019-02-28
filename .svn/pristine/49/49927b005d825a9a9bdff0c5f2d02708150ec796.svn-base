using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    //=======================================================================
    /// <summary>
    /// Ground speed and course data
    /// </summary>
    public class VtgData
    {
        /// <summary>
        /// Heading, from true north, in degrees
        /// </summary>
        public decimal TrueHeading
        {
            get { return this._trueHeading; }
            set { this._trueHeading = value; }
        }
        protected decimal _trueHeading;

        /// <summary>
        /// Heading, from magnetic north, in degrees
        /// </summary>
        public decimal MagneticHeading
        {
            get { return this._magneticHeading; }
            set { this._magneticHeading = value; }
        }
        protected decimal _magneticHeading;

        /// <summary>
        /// Velocity of travel over the ground in knots
        /// </summary>
        public decimal GroundSpeedInKnots
        {
            get { return this._groundSpeedInKnots; }
            set { this._groundSpeedInKnots = value; }
        }
        protected decimal _groundSpeedInKnots;

        /// <summary>
        /// Velocity of travel over the ground in Kilometers per Hour (KMH)
        /// </summary>
        public double GroundSpeedInKmh
        {
            get { return this._groundSpeedInKmh; }
            set { this._groundSpeedInKmh = value; }
        }
        protected double _groundSpeedInKmh;

        /// <summary>
        /// 
        /// </summary>
        public GpsMode Mode
        {
            get { return this._mode; }
            set { this._mode = value; }
        }
        protected GpsMode _mode;

        /// <summary>
        /// 
        /// </summary>
        public int CheckSum
        {
            get { return this._checkSum; }
            set { this._checkSum = value; }
        }
        protected int _checkSum;

        public static VtgData Parser(string inputString)
        {
            VtgData data = new VtgData();
            if (inputString == null || inputString.Length == 0)
                return data;
            string dataString = inputString;
            if (inputString.Contains("*"))
                dataString = inputString.Substring(0, inputString.IndexOf('*')); // strip off the checksum
            else
                return data;
            string[] values = dataString.Split(',');
            if (values.Length < 10)
            {
                return data;
                //throw new FormatException(); 

            }

            if (string.Compare(values[9], "a", true) == 0)
                data.Mode = GpsMode.Autonomous;
            else if (string.Compare(values[9], "d", true) == 0)
                data.Mode = GpsMode.DifferentialGPS;
            else
                data.Mode = GpsMode.DR;
            string speed = values[7];
            if (speed == null || speed.Length == 0)
                speed = "0.0";
            data.GroundSpeedInKmh = double.Parse(speed);
            speed = values[5];
            if (speed == null || speed.Length == 0)
                speed = "0.0";
            data.GroundSpeedInKnots = decimal.Parse(speed);

            return data;
        }
        //$GPVTG[0],359.95[1],T[2],[3],M[4],15.15[5],N[6],28.0[7],K[8],A[9]*04
        //$GPVTG,<1>,T,<2>,M,<3>,N,<4>,K,<5>*hh<CR><LF> 
        //<1> 以真北为参考基准的地面航向（000~359度，前面的0也将被传输） 
        //<2> 以磁北为参考基准的地面航向（000~359度，前面的0也将被传输） 
        //<3> 地面速率（000.0~999.9节，前面的0也将被传输） 
        //<4> 地面速率（0000.0~1851.8公里/小时，前面的0也将被传输） 
        //<5> 模式指示（仅NMEA0183 3.00版本输出，A=自主定位，D=差分，E=估算，N=数据无效）
    }
    //=======================================================================
}
