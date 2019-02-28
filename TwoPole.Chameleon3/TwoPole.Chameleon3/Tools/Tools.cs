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
using Java.IO;
using Android.Net;
using Android.Graphics;
using Java.Net;
using TwoPole.Chameleon3.Infrastructure.Services;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 工具类 主要是写一些通用的函数
    /// </summary>
    public static class Tools
    {


        public static int EncryptKey = 7;

       
            
        public static void WriteAuthFile(string Msg, string FileName)
        {

            try
            {
                File file = new File(Android.OS.Environment.ExternalStorageDirectory, FileName);
                //第二个参数意义是说是否以append方式添加内容  
                BufferedWriter bw = new BufferedWriter(new FileWriter(file, false));
                //记录日志时间
                //自动换行
                bw.Write(Msg);
                bw.Flush();
            }
            catch (Exception ex)
            {
            }
        }

        public static string  ReadFile(string FileName)
        {
            try
            {
                File file = new File(Android.OS.Environment.ExternalStorageDirectory, FileName);
                if (!file.Exists())
                {
                    return string.Empty;
                }
                //第二个参数意义是说是否以append方式添加内容  
                BufferedReader br = new BufferedReader(new FileReader(file));
                var msg=br.ReadLine();
                br.Close();
                return msg;
                //记录日志时间
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string Encrypt(string s)
        {
            Encoding ascii = Encoding.ASCII;//实例化。
            string EncryptString = "";//定义。
            for (int i = 0; i < s.Length; i++)//遍历。
            {
                int j;
                byte[] b = new byte[1];
                j = Convert.ToInt32(ascii.GetBytes(s[i].ToString())[0]);//获取字符的ASCII。
                j = j + EncryptKey;//加密
                b[0] = Convert.ToByte(j);//转换为八位无符号整数。
                EncryptString = EncryptString + ascii.GetString(b);//显示。

            }
            return EncryptString;
        }


        public static string Decryptor(string s)
        {
            Encoding ascii = Encoding.ASCII;//实例化。
            string DecryptorString = "";//定义。
            for (int i = 0; i < s.Length; i++)//遍历。          
            {
                int j;
                byte[] b = new byte[1];
                j = Convert.ToInt32(ascii.GetBytes(s[i].ToString())[0]);//获取字符的ASCII。
                j = j - EncryptKey; DecryptorString = DecryptorString + ascii.GetString(b);//显示。
            }
            return DecryptorString;
        }

        public static bool IsNetworkConnected(Context context)
        {
    
            // 获取手机所有连接管理对象（包括对wi-fi,net等连接的管理）.CONNECTIVITY_SERVICE
            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);

            NetworkInfo[] networkInfo = connectivityManager.GetAllNetworkInfo();

            foreach (NetworkInfo item in networkInfo)
            {
                if (item.GetState() == NetworkInfo.State.Connected)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 通过Ping命令检测外网是否通
        /// </summary>
        /// <returns></returns>
        public static bool Ping()
        {
            try
            {
                String ip = "www.baidu.com";// ping 的地址，可以换成任何一种可靠的外网 
                Java.Lang.Process p = Java.Lang.Runtime.GetRuntime().Exec("ping -c 3 -w 100 " + ip);// ping网址3次 
                // 读取ping的内容，可以不加 
                BufferedReader input = new BufferedReader(new InputStreamReader(p.InputStream));
                //StringBuffer
                //StringBuffer stringBuffer = new StringBuffer();
                string str = string.Empty;

                string content = "";
                while ((content = input.ReadLine()) != null)
                {
                    str += content;
                }
                // ping的状态 
                int status = p.WaitFor();
                if (status == 0)
                {
                    return true;
                }
                else
                {
                }
            }
            catch (IOException e)
            {
            }
            finally
            {
            }
            return false;
        }

        public static int BuyMinute()
        {
            //通过Http 进行查询是否购买学时 如果购买则返回 购买的学时时长 并且开始计算
            return 2;
        }

     


    }
}