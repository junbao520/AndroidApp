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
namespace TwoPole.Chameleon3
{
 
    // CrashApplication
    public class CrashApplication : Application
    {
        protected CrashApplication(IntPtr javaReference, JniHandleOwnership transfer):base(javaReference,transfer)
        {

        }
        //æÕ”√
        public override void OnCreate()
        {
            base.OnCreate();
            CrashHandler crashHandler = CrashHandler.getInstance();
            //crashHandler.init(getApplicationContext());
            crashHandler.init(ApplicationContext);
        }
    }
}