using System;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    //=======================================================================
    /// <summary>
    /// Fixed GPS Data
    /// </summary>
    public class GgaData
    {

        /// <summary>
        /// Time of Position Reading
        /// </summary>
        public DateTime UtcTime
        {
            get { return this._utcTime; }
            set { this._utcTime = value; }
        }
        protected DateTime _utcTime;

        /// <summary>
        /// Position
        /// </summary>
        public Position Position
        {
            get { return this._position; }
            set { this._position = value; }
        }
        protected Position _position = new Position();

        /// <summary>
        /// Number of satelites in use (not the number visible).
        /// </summary>
        public int NumberOfSatelitesInUse
        {
            get { return this._numberOfSatelitesInUse; }
            set { this._numberOfSatelitesInUse = value; }
        }
        protected int _numberOfSatelitesInUse;

        /// <summary>
        /// Horizontal dilution of precision 
        /// </summary>
        public decimal HorizontalDilutionOfPrecision
        {
            get { return this._horizontalDilutionOfPrecision; }
            set { this._horizontalDilutionOfPrecision = value; }
        }
        protected decimal _horizontalDilutionOfPrecision;

        /// <summary>
        /// Antenna altitude above/below mean sea level (geoid)
        /// </summary>
        public Elevation Elevation
        {
            get { return this._elevation; }
            set { this._elevation = value; }
        }
        protected Elevation _elevation;

        /// <summary>
        /// Geoidal separation (Diff. between WGS-84 earth ellipsoid and mean sea level. '-' = geoid is below WGS-84 ellipsoid) 
        /// </summary>
        public GeoidalSeparation GeoidalSeparation
        {
            get { return this._geoidalSeparation; }
            set { this._geoidalSeparation = value; }
        }
        protected GeoidalSeparation _geoidalSeparation = new GeoidalSeparation();

        /// <summary>
        /// The DGPS difference correction
        /// </summary>
        public DifferenceCorrection DifferenceCorrection
        {
            get { return this._differenceCorrection; }
            set { this._differenceCorrection = value; }
        }
        protected DifferenceCorrection _differenceCorrection = new DifferenceCorrection();


        /// <summary>
        /// GPS Quality
        /// </summary>
        public Quality Quality
        {
            get { return this._quality; }
            set { this._quality = value; }
        }
        protected Quality _quality;


        //=======================================================================
        #region -= static methods =-

        //=======================================================================
        public static GgaData Parse(string inputString)
        {
            if (string.IsNullOrEmpty(inputString) || inputString.IndexOf("*", StringComparison.Ordinal) < 0)
                return null;
            //---- declare vars
            GgaData data = new GgaData();
            string dataString = inputString.Substring(0, inputString.IndexOf('*')); // strip off the checksum
            string[] values = dataString.Split(',');

            //---- if we don't have 15 (header + 14), it's no good
            if (values.Length < 15)
            { throw new FormatException(); }

            //---- if the time is six digits
            if (values[1].Length == 9)
            {
                //---- make sure that they're actually numbers
                //int temp;
                //if (int.TryParse(values[1], out temp))
                //{
                //---- should add more validation here
                int hour = int.Parse(values[1].Substring(0, 2));
                int minute = int.Parse(values[1].Substring(2, 2));
                int second = int.Parse(values[1].Substring(4, 2));
                int millisecond = 0;
                if (values[1].Length >= 9)
                    millisecond = Convert.ToInt32(double.Parse(values[1].Substring(6, 3)) * 1000);
                data.UtcTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second, millisecond);
                //}
                //else { throw new FormatException("Date or time string is invalid"); }
            }
            //由于长春老网口版，上面会抛异常所以注销掉，20160323
            //else { throw new FormatException("Date or time string is invalid"); }

            //---- lat/long position
            data.Position = Position.Parse(values[2] + "," + values[3] + ";" + values[4] + "," + values[5]);

            //---- quality
            data.Quality = (Quality)Enum.Parse(typeof(Quality), values[6]);
            switch (int.Parse(values[6]))
            {
                case 0:
                    data.Quality = Quality.NoFix;
                    break;
                case 1:
                    data.Quality = Quality.GpsFix;
                    break;
                case 2:
                    //data.Quality = Quality.DifferentialGpsFix;
                    //todo:北斗星通，E,2时认为是高精度
                    data.Quality = Quality.FixedRealTimeKinematic;
                    break;
                case 3:
                    data.Quality = Quality.PulsePerSecond;
                    break;
                case 4:
                    data.Quality = Quality.FixedRealTimeKinematic;
                    break;
                case 5:
                    data.Quality = Quality.FloatRealTimeKinematic;
                    break;
                case 6:
                    data.Quality = Quality.Estimated;
                    break;
                case 7:
                    data.Quality = Quality.ManualInput;
                    break;
                case 8:
                    data.Quality = Quality.Simulated;
                    break;
                default:
                    data.Quality = Quality.Unknown;
                    break;
            }

            //---- number of satellites
            data.NumberOfSatelitesInUse = int.Parse(values[7]);

            //---- HDOP
            data.HorizontalDilutionOfPrecision = (string.IsNullOrEmpty(values[8]) ? 0 : decimal.Parse(values[8]));

            //---- elevation
            if (!string.IsNullOrEmpty(values[9]) && !string.IsNullOrEmpty(values[10]))
                data.Elevation = Elevation.Parse(values[9] + "," + values[10]);

            //---- geoidal separation
            if (!string.IsNullOrEmpty(values[11]) && !string.IsNullOrEmpty(values[12]))
                data.GeoidalSeparation = GeoidalSeparation.Parse(values[11] + "," + values[12]);

            //---- age
            data.DifferenceCorrection.Age = (string.IsNullOrEmpty(values[13]) ? 0M : decimal.Parse(values[13]));

            //---- station ID
            if (!string.IsNullOrEmpty(values[14]))
                data.DifferenceCorrection.StationID = values[14];

            //---- return
            return data;

            //eg3. $GPGGA,hhmmss.ss,llll.ll,a,yyyyy.yy,a,x,xx,x.x,x.x,M,x.x,M,x.x,xxxx*hh
            //1    = UTC of Position
            //2    = Latitude
            //3    = N or S
            //4    = Longitude
            //5    = E or W
            //6    = GPS quality indicator (0=invalid; 1=GPS fix; 2=Diff. GPS fix)
            //7    = Number of satellites in use [not those in view]
            //8    = Horizontal dilution of position
            //9    = Antenna altitude above/below mean sea level (geoid)
            //10   = Meters  (Antenna height unit)
            //11   = Geoidal separation (Diff. between WGS-84 earth ellipsoid and
            //       mean sea level.  -=geoid is below WGS-84 ellipsoid)
            //12   = Meters  (Units of geoidal separation)
            //13   = Age in seconds since last update from diff. reference station
            //14   = Diff. reference station ID#
            //15   = Checksum

        }
        //=======================================================================

        //=======================================================================
        public static bool TryParse(string inputString, out GgaData ggaData)
        {
            try
            {
                ggaData = Parse(inputString);
                return true;
            }
            catch
            {
                ggaData = null;
                return false;
            }
        }
        //=======================================================================



        #endregion
        //=======================================================================


    }
    //=======================================================================
}
