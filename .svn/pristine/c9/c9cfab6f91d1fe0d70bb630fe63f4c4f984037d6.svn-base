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

namespace TwoPole.Chameleon3
{

    public class SettingYunFu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingYunFu);
        }


        protected void InitControl()
        {

            var mgViewDataConnection = (ImageView)FindViewById(Resource.Id.mgViewDataConnection);
            var mgViewGear = (ImageView)FindViewById(Resource.Id.mgViewGear);
            var mgViewChecking = (ImageView)FindViewById(Resource.Id.mgViewChecking);
            var mgViewInfo = (ImageView)FindViewById(Resource.Id.mgViewInfo);
            var mgViewDevice = (ImageView)FindViewById(Resource.Id.mgViewDevice);

            mgViewDataConnection.Click += Development_Click;
            mgViewGear.Click += Development_Click;
            mgViewChecking.Click += Development_Click;
            mgViewDevice.Click += Development_Click;
            mgViewInfo.Click += MgViewInfo_Click;
        }

        private void MgViewInfo_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(CarSensor));
            StartActivity(intent);
        }

        private void Development_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "正在开发中", ToastLength.Long);
        }
    }
}