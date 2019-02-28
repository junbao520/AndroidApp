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
    [Activity(Label = "SanLianYunFun")]
    public class SanLianYunFuSetting: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingYunFu);
            InitControl();
        }
        protected void InitControl()
        {

            var mgViewDataConnection = (ImageView)FindViewById(Resource.Id.mgViewDataConnection);
            var mgViewGear = (ImageView)FindViewById(Resource.Id.mgViewGear);
            var mgViewChecking = (ImageView)FindViewById(Resource.Id.mgViewChecking);
            var mgViewInfo = (ImageView)FindViewById(Resource.Id.mgViewInfo);
            var mgViewDevice = (ImageView)FindViewById(Resource.Id.mgViewDevice);
            var mgViewLight= (ImageView)FindViewById(Resource.Id.mgLight);

            mgViewDataConnection.Click += Development_Click;
            mgViewGear.Click += Development_Click;
            mgViewChecking.Click += Development_Click;
            mgViewDevice.Click += Development_Click;
            mgViewInfo.Click += MgViewInfo_Click;
            mgViewLight.Click += MgViewLight_Click;
        }

        private void MgViewLight_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(LightSimulationNew));
            StartActivity(intent);
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