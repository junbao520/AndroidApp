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
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Org.Json;
using System.Xml;
using Java.IO;
using Java.Text;
using TwoPole.Chameleon3.Domain;
using Java.Util;
using TwoPole.Chameleon3.Infrastructure.Services;
using Android.Content.PM;
using System.Net;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 启动时广告界面 
    /// </summary>
    [Activity(Label = "Advertisement")]
    public class Advertisement: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

           //启动MainActivity

            Intent intent = new Intent();
            intent.SetClass(this, typeof(MainActivity));
            StartActivity(intent);

           
            Finish();

        }

    }
}