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
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "ParameterSettings")]
    public class SystemSettings : Activity
    {

        Infrastructure.ISpeaker speaker = Singleton.GetSpeaker;
        string ActionText = string.Empty;
        LinearLayout linearLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SystemSettings);
            InitControl();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            //ImageView imageView = new ImageView(this);
            //TODO:可能存在Bug
            //if (GetIntentParameterIsShowAds())
            //{
            //    //背景图显示公司Logo
            //    imageView.SetBackgroundResource(Resource.Drawable.voice_main_bg_new);
            //}
            //else
            //{
            //    //背景图不显示公司Logo
            //    imageView.SetBackgroundResource(Resource.Drawable.bg);
            //}


            //背景图不显示公司Logo
            // imageView.SetBackgroundResource(Resource.Drawable.bg);
            // Button btnStraightDriving = FindViewById<Button>(Resource.Id.btnParameterSettings);
            //btnStraightDriving.LayoutParameters= new LinearLayout.LayoutParams(0, 170);

            try
            {
                if (DataBase.ProgramType != null)
                    if (DataBase.ProgramType == 1)
                    {
                        linearLayout.SetBackgroundResource(Resource.Drawable.hzx_bg3);
                    }
                    else
                    {
                        linearLayout.SetBackgroundResource(Resource.Drawable.bg);
                    }
            }
            catch (Exception ex)
            {
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage(ex.ToString());
                dlgAlert.SetTitle("提示");
                dlgAlert.Show();
            }




        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            LogManager.WriteSystemLog("SystemSettings  AndroidEnvironment_UnhandledExceptionRaise:" + e.Exception.Message);
        }

        //public bool GetIntentParameterIsShowAds()
        //{
        //    return Intent.GetBooleanExtra("IsShowAds",true);
        //}
        protected void InitControl()
        {

            Button btnParameterSettings = FindViewById<Button>(Resource.Id.btnParameterSettings);
            btnParameterSettings.Click += BtnParameterSettings_Click;

            Button btnCarStatus = FindViewById<Button>(Resource.Id.btncarStatus);
            btnCarStatus.Click += BtnCarStatus_Click;

            Button btnLinPlan = FindViewById<Button>(Resource.Id.btnLinePlan);
            btnLinPlan.Click += BtnLinPlan_Click;


            Button btnlightingSimulation = FindViewById<Button>(Resource.Id.btnLightingSimulation);
            btnlightingSimulation.Click += BtnlightingSimulation_Click;

            linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

        }



        private void BtnlightingSimulation_Click(object sender, EventArgs e)
        {
            ActionText = ((Button)sender).Text;
            speaker.SpeakActionVoice(ActionText);
            Intent intent = new Intent();
            intent.SetClass(this, typeof(LightSimulationNew));
            StartActivity(intent);
        }

        private void BtnLinPlan_Click(object sender, EventArgs e)
        {
            ActionText = ((Button)sender).Text;
            speaker.SpeakActionVoice(ActionText);
            Intent intent = new Intent();
            intent.SetClass(this, typeof(Road_List));
            StartActivity(intent);
        }

        private void BtnCarStatus_Click(object sender, EventArgs e)
        {
            ActionText = ((Button)sender).Text;
            speaker.SpeakActionVoice(ActionText);
            Intent intent = new Intent();
            intent.SetClass(this, typeof(CarSensor));
            StartActivity(intent);
        }

        private void BtnParameterSettings_Click(object sender, EventArgs e)
        {
            ActionText = ((Button)sender).Text;
            speaker.SpeakActionVoice(ActionText);
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ParameterSetting));
            StartActivity(intent);
        }
    }
}