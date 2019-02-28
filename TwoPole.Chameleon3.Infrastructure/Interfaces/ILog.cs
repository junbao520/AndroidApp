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

namespace TwoPole.Chameleon3.Infrastructure
{
   public interface ILog
    {
        bool WriteAuthFile(string Msg, Context context);
        string ReadAuthFile(Context context);
        void WriteSensorLog(string Msg);
        void WriteSensorLog(IEnumerable<string> commands);
        void Debug(string Tag, string Msg);

        void Debug(string Msg);

        void DebugFormat(string format, params object[] args);


        void Info(string Tag, string Msg);

        void Info(string Msg);

        void InfoFormat(string format, params object[] args);

        void Error(string Tag, string Msg);
        void Error(Exception exp, string Type);

        void Error(Exception exp, Type Type);
        void Error(string Msg);

        void ErrorFormat(string format, params object[] args);

        void Warn(string Tag, string Msg);

        void Warn(string Msg);

        void WarnFormat(string format, params object[] args);

    }
}