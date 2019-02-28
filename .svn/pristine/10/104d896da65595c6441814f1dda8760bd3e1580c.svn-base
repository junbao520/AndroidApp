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
using System.Threading;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "Loading")]
    public class Loading : Activity
    {
        //打开的时候出现一个正在对话框 
        //it is time
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.loading);
            //延时5秒钟,
            Handler handler = new Handler();
            //延迟4秒
            handler.PostDelayed(PostDelayAction, 4000);
        }
        public void PostDelayAction()
        {
            //跳转到其他页面
            this.Finish();
        }
        
    }

}