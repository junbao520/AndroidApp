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
    /// <summary>
    /// 所有的基础设置都会继承这个类
    /// </summary>
    [Activity(Label = "BaseOnHeaderActivity")]
    public class AssistanceBaseSettingActivity : Activity
    {
        protected IDataService dataService;
        protected ISpeaker speaker;
        protected IMessenger messager;
        protected GlobalSettings Settings;
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        protected ILog Logger;
       protected TextView m_setting_head_title;
        public delegate void deleMethod();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Header);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Settings = dataService.GetSettings();
            Logger = Singleton.GetLogManager;
            // Create your application here
        }
        public void initHeader()
        {
            var HeaderLeftView = FindViewById(Resource.Id.btn_header_left);
            HeaderLeftView.Click += delegate { ShowConfirmDialog(); };

             m_setting_head_title = (TextView)FindViewById(Resource.Id.setting_head_title);

        }
        public void setMyTitle(string str)
        {
            m_setting_head_title.Text = str;
        }
        public void ShowQuestionDialog(string Title,string msg,deleMethod PositiveMehtod,deleMethod NeturalMehtod,string PositiveButtonText="是",string NeutralButtonText="否")
        {
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle(Title)
            .SetMessage(msg)
            .SetPositiveButton(PositiveButtonText, (s, e) =>
            {
                //进行数据保存
                PositiveMehtod();
            })
            .SetNeutralButton(NeutralButtonText, (s, e) =>
            {
                //不保存数据进行返回
                NeturalMehtod();
            })
            .Create();       //创建alertDialog对象  
            alertDialog.Show();
        }
        public bool ShowQuestionDialog(string Title, string msg, string PositiveButtonText, string NeutralButtonText, string  NegativeButtonText )
        {
            bool Result = false;
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle(Title)
            .SetMessage(msg)
            .SetNegativeButton(NegativeButtonText, (s, e) =>
            {
                //取消则不返回
            })
            .SetPositiveButton(PositiveButtonText, (s, e) =>
            {
                //进行数据保存
                Result = true;
            })
            .SetNeutralButton(NeutralButtonText, (s, e) =>
            {
                //不保存数据进行返回

            })
            .Create();       //创建alertDialog对象  
            alertDialog.Show();
            return Result;
        }
        public virtual void ShowConfirmDialog()
        {
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("提示")
            .SetMessage("您是是否需要保存配置?")
            .SetNegativeButton("取消", (s, e) =>
            {
                //取消则不返回
            })
            .SetPositiveButton("是", (s, e) =>
            {
                //进行数据保存
                UpdateSettings();
            })
            .SetNeutralButton("否", (s, e) =>
            {
                //不保存数据进行返回
                Finish();
            })
            .Create();       //创建alertDialog对象  
            alertDialog.Show();
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            //if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            //{
            //    ShowConfirmDialog();
            //}
            return base.OnKeyDown(keyCode, key);
        }

        public virtual void UpdateSettings()
        {

        }
        public virtual void InitSetting()
        {

        }


    }

    public enum DialogResult
    {
        Yes,
        No,
        Cancel
    }


}