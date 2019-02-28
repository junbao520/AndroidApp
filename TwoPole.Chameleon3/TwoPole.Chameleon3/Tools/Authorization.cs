using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using TwoPole.Chameleon3.Infrastructure;
using Android.Content;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 试用期3个月授权
    /// </summary>
    public class Authorization
    {
        #region 注册表试用期
        protected ILog Logger;
        //开发软件 
        //开发软件
        //visual studio 
        //visual 
        //
        private static bool isFirstOpen = true;
        //车辆试用识别码
        private string SchoolID = "0000";

        //TODO:试用信息写入文件授权 手机SD卡根目录
        //TODO:这样的方式安全等级不够,有空可以进行研究新的安全等级高的授权
        private readonly string AuthFileName = "System.sys";

        private string strSysInfoPath = string.Empty;
        //是否激活



        Context context;

        public bool IsActive { get { return strSysInfoPath == string.Empty; } }
        public string AuthorizationCode { get { return strSysInfoPath.Split('@')[1]; } }
        public DateTime? AuthorizationTime { get { return GetAuthorizationTime(); } }
        public string AuthorizationInfo { get { return IsActive ? string.Empty : string.Format("授权编码:{0}授权日期:{1}", AuthorizationCode, AuthorizationTime.Value.ToString("yyyy/MM/dd")); } }


        public Authorization(Context context)
        {
            this.context = context;
            Logger = Singleton.GetLogManager;
            strSysInfoPath = ReadAuthFile();
            speaker = Singleton.GetSpeaker;
        }
        ISpeaker speaker;
        public bool CheckPeriod(GpsInfo tempGpsInfo)
        {
            //gps无信号时，禁止使用
            if (tempGpsInfo.FixedSatelliteCount < 2)
            {
                speaker.PlayAudioAsync("GPS无信号，请检查");
                return false;
            }

            if (isFirstOpen)
            {
                try
                {
                    DateTime tempTime = tempGpsInfo.LocalTime;
                    Logger.DebugFormat("获取当前gps日期:{0}", tempTime);
                    if (tempTime.Year > 2014)
                    {

                        //isFirstOpen = false;
                        if (!ShowInfo(tempTime))
                        {
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    Logger.ErrorFormat("获取验证码出错：{0}", ex.Message);

                }


            }
            return true;
        }

        /// <summary>
        /// 验证码是否可用
        /// </summary>
        /// <param name="meg"></param>
        /// <returns></returns>
        public bool ISCheckValid(string inputAuthCode, ref string msg)
        {
            try
            {


                if (inputAuthCode.Length > 16 ||
                    !Regex.IsMatch(inputAuthCode, @"^[0-9]\d{15}"))
                {
                    msg = "授权码由16位数字组成！";
                    return false;
                }

                //第1，6，11，16为驾校识别码
                var schoolID = inputAuthCode.Substring(0, 1) + inputAuthCode.Substring(5, 1) +
                               inputAuthCode.Substring(10, 1) + inputAuthCode.Substring(15, 1);


                //1575—7249—5135—1504
                //1495—4248—5635—0554
                //5757 3952  5104
                var expireString = inputAuthCode.Substring(1, 4) + inputAuthCode.Substring(6, 4) +
                                   inputAuthCode.Substring(11, 4);


                byte[] expireBytes = new byte[expireString.Length / 2];

                //取出来  57   
                //5757 3952  5104  //6 //6
                for (int i = 0, j = 0; i < expireString.Length / 2; i = i + 1, j++)
                {
                    var temp = expireString.Substring(2 * i, 2);

                    expireBytes[j] = Convert.ToByte(temp);
                }
                string dateString = Encoding.UTF8.GetString(expireBytes);

                if (!Regex.IsMatch(dateString, @"^[0-9]\d{5}"))
                {
                    msg = "授权码不合法，请检查！";
                    return false;
                }
                string active = getActiveInfo();
                bool isValidSchoolID = false;
                if (active == String.Empty)
                {
                    msg = "软件已激活！";
                    return true;
                }
                else
                {
                    //获取本地试用识别码
                    SchoolID = active.Split('@')[1];
                    if (SchoolID == schoolID)
                    {
                        isValidSchoolID = true;
                    }

                }
                if (!isValidSchoolID)
                {
                    msg = "授权码不合法！";
                    return false;
                }
                string[] original = active.Split('@');

                if (!CheckYear(dateString, original[0], schoolID, ref msg))
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                msg = "授权码错误！" + ex.StackTrace;
                Logger.ErrorFormat("软件激活出错：{0}", ex.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 比对年月日，判断延期还是永久激活
        /// </summary>
        /// <param name="tempdateTime">20151218</param>
        /// <param name="originaldateTime">2015/12/18</param>
        /// <returns></returns>
        private bool CheckYear(string tempdateTime, string originaldateTime, string schoolID, ref string msg)
        {

            int tempYear = Convert.ToInt32(tempdateTime.Substring(0, 2));
            int tempMonth = Convert.ToInt32(tempdateTime.Substring(2, 2));
            int tempDay = Convert.ToInt32(tempdateTime.Substring(4, 2));
            string[] original = originaldateTime.Split('/');
            int originalYear = Convert.ToInt32(original[0].Substring(2, 2));
            int originalMonth = Convert.ToInt32(original[1]);
            int originalDay = Convert.ToInt32(original[2].Substring(0, 2).Trim());
            if (tempYear == 99 && tempMonth == 13 && tempDay == 32)
            {
                //永久激活

                //var rk2 = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                //rk2.DeleteSubKey("2Pole");
                WriteAuthFile("");
                msg = "激活成功！";
                return true;
            }
            if (tempYear - originalYear < 0 || tempMonth > 12 || tempMonth < 1 ||
                tempDay > 31 || tempDay < 1)
            {
                msg = "非法授权码！";
                return false;
            }
            //延期写入
            var writeDatetime = "20" + tempdateTime.Substring(0, 2) + "/" + tempdateTime.Substring(2, 2) + "/" +
                                tempdateTime.Substring(4, 2);
            if (!WriteArthorization(writeDatetime, schoolID))
            {
                msg = "延期激活失败！";
                return false;
            }
            else
            {
                msg = string.Format("延期成功,延期到:{0}", writeDatetime);
            }


            return true;

        }

        /// <summary>
        /// 获取是否激活
        /// </summary>
        /// <returns></returns>
        public string getActiveInfo()
        {
            string strSysInfoPath = string.Empty;
            strSysInfoPath = ReadAuthFile();
            if (strSysInfoPath == string.Empty)
            {
                return string.Empty;
            }
            return strSysInfoPath;
        }

        public string GetAuthorizationCode()
        {
            //Logger.Error("GetAuthorizationCode");
            string strSysInfoPath = String.Empty;
            strSysInfoPath = ReadAuthFile();
            //Logger.Error("GetAuthorizationCode:strSysInfoPath"+ strSysInfoPath);
            if (!string.IsNullOrEmpty(strSysInfoPath))
            {
                return strSysInfoPath.Split('@')[1];
            }
            return string.Empty;
        }
        public DateTime? GetAuthorizationTime()
        {
            try
            {
                //string strSysInfoPath = String.Empty;
                //strSysInfoPath = ReadAuthFile();
                //Logger.Error("GetAuthorizationCode:strSysInfoPath"+ strSysInfoPath);
                if (!IsActive)
                {
                    return Convert.ToDateTime(strSysInfoPath.Split('@')[0]);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("GetAuthorizationTime", ex.Message);
            }
            return null;
        }
        public string GetAuthorizationInfo()
        {
            //string strSysInfoPath = String.Empty;
            //strSysInfoPath = ReadAuthFile();
            if (!IsActive)
            {
              return string.Format("授权编码:{0}授权日期:{1}", AuthorizationCode,AuthorizationTime.Value.ToString("yyyy/MM/dd"));
            }
            return string.Empty;
        }

        /// <summary>
        /// 提取注册表信息,false则到期
        /// </summary>
        /// <param name="tempTime"></param>
        /// <returns></returns>
        private bool ShowInfo(DateTime tempTime)
        {
            //string strSysInfoPath = String.Empty;
            //strSysInfoPath = ReadAuthFile();
            if (strSysInfoPath == String.Empty)
            {
                return true;
            }
            else
            {

                //日期
                var getString = strSysInfoPath.Split('@');
                //测试
                var beginTime = Convert.ToDateTime(getString[0] + " 10:00");

                if (tempTime >= beginTime)
                {
                    Logger.DebugFormat("到期时间{0}，当前日期{1}", beginTime, tempTime);
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// 延期时，重写到期日期
        /// </summary>
        /// <param name="datetimeString"></param>
        /// <param name="schoolID"></param>
        /// <returns></returns>
        private bool WriteArthorization(string datetimeString, string schoolID)
        {
            string textId = schoolID;
            if (textId == String.Empty ||
                textId.Length > 4 ||
                !Regex.IsMatch(textId, @"^[0-9]\d{3}"))
            {
                //MessageBox.Show("请输入试用识别码，识别码为4位数字");
                return false;
            }
            var tempTime = datetimeString;
            var tempTimeString = String.Concat(tempTime, "@", textId);

            WriteAuthFile(tempTimeString);
           
            //var encodeTime = Encode(tempTimeString);
            //var rk2 = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            //var rk = rk2.CreateSubKey("2Pole");
            //rk.SetValue("ExpirationTime", encodeTime);
            return true;
        }

        //需要保证两种授权都要生效
        public bool WriteAuthFileEncrypt(string AuthMsg,string FileName)
        {
            try
            {
                AuthMsg = Tools.Encrypt(AuthMsg);
                Tools.WriteAuthFile(AuthMsg, FileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           

        }
       
        public bool WriteAuthFile(string AuthMsg,string FileName= "Auth.ini")
        {
            try
            {
                System.IO.Stream auth_outStream = context.OpenFileOutput(FileName, FileCreationMode.Private);
                auth_outStream.Write(Encoding.ASCII.GetBytes(AuthMsg), 0, Encoding.ASCII.GetBytes(AuthMsg).Length);
                auth_outStream.Flush();
                auth_outStream.Close();

                Logger.Error("WriteAuthFile OK");
                WriteAuthFileEncrypt(AuthMsg, AuthFileName);
                Logger.Error("WriteAuthFileEncrypt OK");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string ReadAuthFile(string FileName= "Auth.ini")
        {
            try
            {
                Java.Lang.StringBuffer sb = new Java.Lang.StringBuffer();
                //文件不存在 
                if (!System.IO.File.Exists(FileName))
                {
                    return string.Empty;
                }
                System.IO.Stream fis = context.OpenFileInput(FileName);
                int ch;
                while ((ch = fis.ReadByte()) != -1)
                {
                    sb.Append((char)ch);
                }
                fis.Close();
                string AuthMsg = sb.ToString();
                Logger.Debug("AuthMsg:"+AuthMsg);
                if (string.IsNullOrEmpty(AuthMsg))
                {
                    AuthMsg = ReadDecryptorFile(AuthFileName);
                   Logger.Debug("ReadDecryptorFile:" + AuthMsg);
                }
                return AuthMsg;
            }
            catch (Exception exp)
            {
                //记录详细日志
                Logger.Error(exp, GetType().ToString());
                return string.Empty;
            }
      
        }


        public string ReadDecryptorFile(string FileName)
        {
             var msg = Tools.ReadFile(FileName);
             Logger.DebugFormat("ReadDecryptorFile:"+msg+"Decrypt"+ Tools.Decryptor(msg));
             return  Tools.Decryptor(msg);
        }

        /// <summary>
        /// 将字符串转成二进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encode(string s)
        {
            byte[] data = Encoding.Unicode.GetBytes(s);
            StringBuilder result = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }


        /// <summary>
        /// 将二进制转成字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decode(string s)
        {

            System.Text.RegularExpressions.CaptureCollection cs =
                System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 获取授权到期时间 如果没有授权则返回空
        /// </summary>
        /// <returns></returns>
        public string GetExpireDay(string GpsTime)
        {
            string strSysInfoPath = String.Empty;
            strSysInfoPath = ReadAuthFile();
            string str = string.Empty;
            if (strSysInfoPath == String.Empty)
            {
                return String.Empty;
            }
            else
            {
                string AuthDay = strSysInfoPath.Split('@')[0];
                int Day = (Convert.ToDateTime(AuthDay) - Convert.ToDateTime(GpsTime)).Days;

                if (Math.Abs(Day) <= 5)
                {
                    str = string.Format("您的授权到期日:{0},距离今天还剩余:{1}天", AuthDay, Day.ToString());
                }
                else
                {
                    str = string.Empty;
                }
                //如果点开始考试的话需要去获取Gps日期

            }
            return str;
        }
        #endregion
    }
}
