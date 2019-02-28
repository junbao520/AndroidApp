using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace TwoPole.Chameleon3.Foundation.Gps
{
    //=======================================================================
    /// <summary>
    /// Represents a GpsReading. Useful for parsing and writing NMEA data.
    /// </summary>
    /// <remarks>
    /// see http://aprs.gids.nl/nmea/ for NMEA specification
    /// </remarks>
    public class GpsReading
    {
       // private static readonly ILog Logger = LogManager.GetLogger<GpsReading>();
        //=======================================================================

        #region -= properties =-

        //public HdtData HdtData
        //{
        //    get { return this._hdtData; }
        //    set { this._hdtData = value; }
        //}

        //protected HdtData _hdtData = new HdtData();

        ///// <summary>
        ///// DOP and Active Satellite Data
        ///// </summary>
        //public GsaData DopActiveSatellites
        //{
        //    get { return this._dopActiveSatellites; }
        //    set { this._dopActiveSatellites = value; }
        //}

        //protected GsaData _dopActiveSatellites = new GsaData();        
        
        ///// <summary>
        ///// Satellites in View Data
        /////  because the gps gives you several messages, each with 4 satellites, we should aggregate them
        ///// </summary>
        //public List<GsvData> SatellitesInView
        //{
        //    get { return this._satellitesInView; }
        //    set { this._satellitesInView = value; }
        //}

        //protected List<GsvData> _satellitesInView = new List<GsvData>();

        ///// <summary>
        ///// Signal Strength Data
        ///// </summary>
        //public MssData SignalStrength
        //{
        //    get { return this._signalStrength; }
        //    set { this._signalStrength = value; }
        //}

        //protected MssData _signalStrength = new MssData();
        /// <summary>
        /// Fixed GPS Data
        /// </summary>
        public GgaData FixedGpsData
        {
            get { return this._fixedGpsData; }
            set { this._fixedGpsData = value; }
        }

        protected GgaData _fixedGpsData = new GgaData();
        
        /// <summary>
        /// Recommended Minimum GPS Data
        /// </summary>
        public RmcData Summary
        {
            get { return this._summary; }
            set { this._summary = value; }
        }

        protected RmcData _summary = new RmcData();

        /// <summary>
        /// Groundspeed and Heading Data
        /// </summary>
        public VtgData GroundVector
        {
            get { return this._groundVector; }
            set { this._groundVector = value; }
        }

        protected HgaData _heading;//= new HgaData();

        /// <summary>
        /// NovAtel Headingga
        /// </summary>
        public HgaData Heading
        {
            get { return this._heading; }
            set { this._heading = value; }
        }

        /// <summary>
        /// NovAtel BestUtm
        /// </summary>
        protected UtmData _Utm = new UtmData();
        public UtmData Utm
        {
            get { return this._Utm; }
            set { this._Utm = value; }
        }

        protected VtgData _groundVector = new VtgData();

        public AvrData AvrData
        {
            get { return this._avrData; }
            set { this._avrData = value; }
        }

        protected AvrData _avrData = new AvrData();

        public VgkData VgkData
        {
            get { return this._vgkData; }
            set { this._vgkData = value; }
        }

        public NtrData NtrData { get; private set; }

        protected VgkData _vgkData = new VgkData();

        #endregion

        //=======================================================================

        //=======================================================================

        #region -= constructors =-

        #endregion

        //=======================================================================

        //=======================================================================

        #region -= public methods =-

        #endregion

        //=======================================================================

        //=======================================================================

        #region -= static methods =-

        //=======================================================================
        public static GpsReading Parse(List<string> inputStrings)
        {
            //---- declare vars
            GpsReading gpsReading;
            string[] sentences = new string[inputStrings.Count];

            //---- copy our strings into an array	
            inputStrings.CopyTo(sentences);

            //---- parse 'em
            gpsReading = ParseSentences(sentences);

            //---- return
            return gpsReading;
        }

        public static GpsReading Parse(string[] inputStrings)
        {
            if (inputStrings == null || inputStrings.Length == 0)
                return null;

            var gpsReading = ParseSentences(inputStrings);

            return gpsReading;
        }

        //=======================================================================

        //=======================================================================
        public static GpsReading Parse(string inputString)
        {
            //---- declare vars
            GpsReading gpsReading;
            string[] sentences = inputString.Split(new char[] { '\r', '\n' });

            //---- parse 'em
            gpsReading = ParseSentences(sentences);

            //---- return
            return gpsReading;
        }
        //=======================================================================

        //=======================================================================
        private static GpsReading ParseSentences(string[] sentences)
        {
            //---- declare vars
            GpsReading gpsReading = new GpsReading();
            //---- loop through each sentence
            foreach (var sentence in sentences.SelectMany(SpiltSentence))
            {
                //---- if the sentence has a header and data
                if (sentence != null && sentence.Length > 6)
                {
                    try
                    {
                        ParseSentence(gpsReading, sentence);
                    }
                    catch (Exception exp)
                    {
                        //Logger.ErrorFormat("解析Gps信号 {0} 发生异常", sentence, exp);
                    }
                }
            }

            //---- return our parsed gps reading
            return gpsReading;
        }

        private static IEnumerable<string> SpiltSentence(string sentence)
        {
            if (string.IsNullOrEmpty(sentence))
                yield break;

            var s = sentence.First().ToString();
            foreach (var c in sentence.Skip(1))
            {
                if (c == '$')
                {
                    yield return s;
                    s = c.ToString();
                }
                else
                {
                    s += c;
                }
            }
            if (s.Length > 0)
                yield return s;
        }

        private static void ParseSentence(GpsReading gpsReading, string sentence)
        {
            var cmd = ParseCommand(sentence);
            if (string.IsNullOrEmpty(cmd))
                return;

            //GPNTR
            switch (cmd.ToUpper())
            {
                case "$GPGGA":
                case "$GNGGA":
                case "$BDGGA":
                    gpsReading.FixedGpsData = GgaData.Parse(sentence);
                    break;
                case "$GPVTG":
                case "$GNVTG":
                case "$BDVTG": 
                    gpsReading.GroundVector = VtgData.Parser(sentence);
                    break;
                case "$GPRMC":
                case "$GNRMC":
                case "$BDRMC":
                    gpsReading.Summary = RmcData.Parse(sentence);
                    break;
                case "#HEADINGA":
                    gpsReading.Heading = HgaData.Parse(sentence);
                    break;
                case "#BESTUTMA":
                    gpsReading.Utm = UtmData.Parse(sentence);
                    break;
                case "$GPNTR":
                    gpsReading.NtrData = NtrData.Parse(sentence);
                    break;
                //case "$GPGSA":
                //    gpsReading.DopActiveSatellites = GsaData.Parse(sentence);
                //    break;
                //case "$GPGSV":
                //    gpsReading.SatellitesInView.Add(GsvData.Parse(sentence));
                //    break;
                //case "$GPHDT":
                //    gpsReading.HdtData = HdtData.Parse(sentence);
                //    break;
                case "$PTNL":
                    if (sentence.IndexOf("AVR", System.StringComparison.Ordinal) > 0)
                    {
                        gpsReading.AvrData = AvrData.Parse(sentence);
                    }
                    else if (sentence.IndexOf("VGK", System.StringComparison.Ordinal) > 0)
                    {
                        gpsReading.VgkData = VgkData.Parse(sentence);
                    }
                    break;
            }
        }

        private static string ParseCommand(string sentence)
        {
            var index = sentence.IndexOf(',');
            if (index > 0)
                return sentence.Substring(0, index);
            return null;
        }
        //=======================================================================

        #endregion
        //=======================================================================
    }
    //=======================================================================
}
