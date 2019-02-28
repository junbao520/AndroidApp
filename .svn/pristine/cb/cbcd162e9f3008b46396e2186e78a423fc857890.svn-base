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
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "BaseOnHeaderActivity")]
    public class BaseOnHeaderActivity : Activity
    {
        protected IDataService dataService;
        protected  ISpeaker speaker;
        protected IMessenger messager;
        protected GlobalSettings Settings;
     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.setTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.setTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //全屏并且无标题栏，必须在OnCreate前面设置。
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
           // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
           
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Header);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Settings = dataService.GetSettings();
            // Create your application here
        }
        //其实我觉得直接保存就算了
        public void initHeader()
        {
            var HeaderLeftView = FindViewById(Resource.Id.btn_header_left);
            HeaderLeftView.Click += delegate { Finish(); };

        }
        public void ShowConfirmDialog()
        {
          
        }
        public void setMyTitle(string str)
        {
            TextView m_setting_head_title = (TextView)FindViewById(Resource.Id.setting_head_title);
            m_setting_head_title.Text = str;
        }
    }
}