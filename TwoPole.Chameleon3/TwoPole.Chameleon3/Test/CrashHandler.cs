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
using Java.Lang;
using static Java.Lang.Thread;
using Java.IO;

namespace TwoPole.Chameleon3
{
    /// <summary>
    ///  UncaughtException处理类,当程序发生Uncaught异常的时候,有该类来接管程序,并记录发送错误报告. 
    /// </summary>
   public class CrashHandler : IUncaughtExceptionHandler
    {

        private static CrashHandler instance= new CrashHandler();
        private Thread.IUncaughtExceptionHandler mDefaultHandler;
        //程序的Context对象  
        private Context mContext;

        //保证只有一个实力
        public  CrashHandler()
        {
        }
        public IntPtr Handle
        {
            get
            {
                return new IntPtr(2121);
            }
        }
        public static CrashHandler getInstance()
        {
            return instance;
        }
        public void init(Context context)
        {
            mContext = context;
            //获取系统默认的UncaughtException处理器  
            mDefaultHandler = Thread.DefaultUncaughtExceptionHandler;
            //设置该CrashHandler为程序的默认处理器      
            Thread.DefaultUncaughtExceptionHandler = this;
        }

        public void Dispose()
        {
           
        }
        

        public void UncaughtException(Thread thread, Throwable ex)
        {
           
            File file = new File(Android.OS.Environment.ExternalStorageDirectory, "SpecialError.txt");
            //第二个参数意义是说是否以append方式添加内容  
            BufferedWriter bw = new BufferedWriter(new FileWriter(file, true));
            //记录日志时间
            string info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "---" + ex.Message;
            //自动换行
            bw.Write(info);
            bw.Write("\r\n");//
            bw.Flush();

           // showToast(mContext, "很抱歉，程序遭遇异常，即将退出！");
            Toast.MakeText(mContext, "很抱歉，程序遭遇异常，即将退出！", ToastLength.Long);
            Thread.Sleep(2000);
        }
    }
}