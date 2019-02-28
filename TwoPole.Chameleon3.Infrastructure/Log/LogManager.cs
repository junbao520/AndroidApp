using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Java.IO;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class LogManager : ILog
    {
        public string DeubgFileName = "Debug";
        public string WarnFileName = "Warn";
        public string ErrorFileName = "ERROR";
        public string InfoFileName = "Info";
        public string SensorFileName = "Sensor";
        private string FolderName = string.Empty;
        IDataService dataService;
        public bool IsLogEnable
        {
            get
            {
                return dataService.GetSettings().GpsLogEnable;
            }
        }
       

        public LogManager(IDataService dataService)
        {
            //依赖注入
            this.FolderName = dataService.FolderName;
            this.dataService = dataService;
        }

        //其实最好是依赖注入

        public string getSdCardPath()
        {
            string sdpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            return sdpath;
        }
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }
        /// <summary>
        /// 把授权编码写入到文件中
        /// </summary>
        /// <param name="AuthMsg"></param>
        public bool WriteAuthFile(string AuthMsg, Context context)
        {
            try
            {
                System.IO.Stream auth_outStream = context.OpenFileOutput("Auth.ini", FileCreationMode.Private);
                auth_outStream.Write(Encoding.ASCII.GetBytes(AuthMsg), 0, Encoding.ASCII.GetBytes(AuthMsg).Length);
                auth_outStream.Flush();
                auth_outStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("WriteAuthCode", ex.Message);
                return false;
            }
        }
        public string ReadAuthFile(Context context)
        {
            try
            {
                Java.Lang.StringBuffer sb = new Java.Lang.StringBuffer();
                System.IO.Stream fis = context.OpenFileInput("Auth.ini");

                int ch;
                while ((ch = fis.ReadByte()) != -1)
                {
                    sb.Append((char)ch);
                }
                fis.Close();
                string AuthMsg = sb.ToString();
                return AuthMsg;
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
        public void WriteFile(string Msg, string FileName)
        {

            try
            {
                File file = new File(Android.OS.Environment.ExternalStorageDirectory+"/"+this.FolderName, FileName + ".txt");
                //第二个参数意义是说是否以append方式添加内容  
                BufferedWriter bw = new BufferedWriter(new FileWriter(file, true));
                //记录日志时间
                string info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "---" + Msg;
                //自动换行
                bw.Write(info);
                bw.Write("\r\n");//
                bw.Flush();
            }
            catch (Exception ex)
            {
                Error("WriteLog" + ex.Message);
            }

        }
        //写入Info消息进入Speech.txt
        public static void WriteSpeechInfo(string msg)
        {
            msg = "info " + msg;
            WriteSystemLog(msg);
        }
        public static void WriteSystemLog(Exception exp, string Type)
        {
            string msg = GetExceptionMsg(exp, Type);
            WriteSystemLog(msg);
        }
        static string GetExceptionMsg(Exception ex, string Type)
        {

        
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString()+ "【出现类】："+Type);
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            sb.AppendLine("***************************************************************");
            //写入日志
            return sb.ToString();
        }
        public static void WriteSystemLog(string Msg)
        {
            try
            {
                File file = new File(Android.OS.Environment.ExternalStorageDirectory, "SystemLog" + ".txt");
                //第二个参数意义是说是否以append方式添加内容  
                BufferedWriter bw = new BufferedWriter(new FileWriter(file, true));
                //记录日志时间
                string info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "---" + Msg;
                //自动换行
                bw.Write(info);
                bw.Write("\r\n");//
                bw.Flush();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void WriteSensorLog(string Msg)
        {
            WriteFile(Msg, SensorFileName);
        }
        public void WriteSensorLog(IEnumerable<string> commands)
        {

            try
            {
                if (IsLogEnable)
                {
                    File file = new File(Android.OS.Environment.ExternalStorageDirectory, SensorFileName + ".txt");
                    //第二个参数意义是说是否以append方式添加内容  
                    BufferedWriter bw = new BufferedWriter(new FileWriter(file, true));
                    //记录日志时间 //记录时间
                    string info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-";

                    //我需要记录我接收到这条数据的时间
                    for (int i = 0; i < commands.Count(); i++)
                    {
                        if (i == 0)
                        {
                            bw.Write(info + commands.ElementAt(i));
                        }
                        else
                        {
                            bw.Write(commands.ElementAt(i));
                        }

                        bw.Write("\r\n");
                    }
                    bw.Flush();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public void Debug(string Tag, string Msg)
        {
            if (IsLogEnable)
            {
                Log.Debug(Tag, Msg);

                WriteFile(Tag + "-" + Msg, DeubgFileName);
            }

        }
        public void Debug(string Msg)
        {
            if (IsLogEnable)
            {
                //Log.Debug("Chameleon3Debug", Msg);
                WriteFile(Msg, DeubgFileName);
            }

        }
        public void DebugFormat(string format, params object[] args)
        {
            if (IsLogEnable)
            {
                Log.Debug("Chameleon3Debug", format, args);
                string str = string.Format(format, args);
                WriteFile(str, DeubgFileName);
            }

        }

        public void Info(string Tag, string Msg)
        {
            if (IsLogEnable)
            {
                Log.Info(Tag, Msg);
                WriteFile(Tag + "-" + Msg, InfoFileName);
            }

        }
        public void Info(string Msg)
        {
            if (IsLogEnable)
            {
                Log.Info("Chameleon3Info", Msg);
                WriteFile(Msg, InfoFileName);
            }

        }
        public void InfoFormat(string format, params object[] args)
        {
            if (IsLogEnable)
            {
                Log.Info("Chameleon3Info", format, args);
                string str = string.Format(format, args);
                WriteFile(str, InfoFileName);
            }

        }
        public void Error(string Tag, string Msg)
        {
            Log.Error(Tag, Msg);
            WriteFile(Tag + "-" + Msg, ErrorFileName);
        }
        public void Error(string Msg)
        {
            Log.Error("Chameleon3Error", Msg);
            WriteFile(Msg, ErrorFileName);
        }
        public void Error(Exception exp,string Type)
        {
            var msg = GetExceptionMsg(exp, Type);
            WriteFile(msg, ErrorFileName);
        }
        public void Error(Exception exp, Type Type)
        {
            var msg = GetExceptionMsg(exp, Type.ToString());
            WriteFile(msg, ErrorFileName);
        }
        public void ErrorFormat(string format, params object[] args)
        {
            Log.Error("Chameleon3Error", format, args);
            string str = string.Format(format, args);
            WriteFile(str, ErrorFileName);
        }
        public void Warn(string Tag, string Msg)
        {
            Log.Warn(Tag, Msg);
            WriteFile(Tag + "-" + Msg, WarnFileName);
        }
        public void Warn(string Msg)
        {
            Log.Warn("Chameleon3Warn", Msg);
            WriteFile(Msg, WarnFileName);
        }
        public void WarnFormat(string format, params object[] args)
        {
            Log.Warn("Chameleon3Warn", format, args);
            string str = string.Format(format, args);
            WriteFile(str, WarnFileName);
        }

    }
}