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
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 所有的基础设置都会继承这个类
    /// </summary>
    [Activity(Label = "BaseOnHeaderActivity")]
    public class BaseSettingActivity : Activity
    {
        protected IDataService dataService;
        protected ISpeaker speaker;
        protected IMessenger messager;
        protected GlobalSettings Settings;
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        protected ILog Logger;
        public delegate void deleMethod();
        protected TextView m_setting_head_title;
        protected bool IsShowConfirmDialog = true;
        //Activity对应的中文名称
        protected string ActivityName = string.Empty;
        protected List<Setting> lstSetting = new List<Setting>();
        protected ExamItem examItem = null;
        protected string ItemCode = string.Empty;
        private string _ItemVoice;
        protected string ItemVoice
        {
            get { return GetItemVoice(ItemCode, true); }
            set { _ItemVoice = value; }
        }
        private string _ItemEndVoice;
        protected string ItemEndVoice {

            get { return GetItemVoice(ItemCode, false); }
            set { _ItemEndVoice = value; }
        }

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
        }
       
        private void BindSpinner(List<string> lstDataSource, Spinner spinner)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstDataSource);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.Visibility = ViewStates.Visible;

        }
        public void initHeader(bool IsShowDialog=true)
        {
            var HeaderLeftView = FindViewById(Resource.Id.btn_header_left);
            if(HeaderLeftView!=null)
                HeaderLeftView.Click += delegate { ShowConfirmDialog(); };
            m_setting_head_title = (TextView)FindViewById(Resource.Id.setting_head_title);
            IsShowConfirmDialog = IsShowDialog;
        }
        public void setMyTitle(string str)
        {
            if(m_setting_head_title!=null)
               m_setting_head_title.Text = str;
        }
        public void ShowQuestionDialog(string Title, string msg, deleMethod PositiveMehtod, deleMethod NeturalMehtod, string PositiveButtonText = "是", string NeutralButtonText = "否")
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
      
        public  bool ShowQuestionDialog(string Title, string msg, string PositiveButtonText, string NeutralButtonText, string NegativeButtonText)
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
            if (!IsShowConfirmDialog)
            {
                Finish();
                return;
            }
            builder = new AlertDialog.Builder(this);
          
            alertDialog = builder
            .SetTitle("温馨提示")
            .SetMessage("您是否需要保存配置?")
             
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

            //Window window = alertDialog.Window;
            //WindowManager.LayoutParams lp = window.getAttributes();
            alertDialog.Show();
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                if (IsShowConfirmDialog)
                {
                    ShowConfirmDialog();
                }
            }
            return base.OnKeyDown(keyCode, key);
        }
        public string GetItemVoice(string ItemCode, bool IsStart = true)
        {

            var ExamItem = dataService.AllExamItems.Where(s => s.ItemCode == ItemCode).FirstOrDefault();

            if (ExamItem == null)
            {
                return string.Empty;
            }
            var VoiceText = IsStart ? ExamItem.VoiceText : ExamItem.EndVoiceText;

            return VoiceText;
        }
        public void UpdateExamItem(ExamItem item)
        {
            dataService.UpdateExamItemsVoice(item);
        }
        public void UpdateExamItem()
        {
            examItem = new ExamItem();
            examItem.ItemCode = ItemCode;
            examItem.VoiceText = _ItemVoice;
            examItem.EndVoiceText = _ItemEndVoice;
            dataService.UpdateExamItemsVoice(examItem);
        }
        public Setting GetSetting(string Key,object value)
        {
            return new Setting { Key = Key, Value = value.ToString(), GroupName = "GlobalSettings" };
        }
      
        public virtual void UpdateSettings()
        {  
            speaker.SpeakActionVoice(ActivityName+"保存成功");
        }
        public virtual void UpdateSettings(IList<Setting> listSetting)
        {
            //如果是考试项目就进行更新项目的开始和结束语音
            if (!string.IsNullOrEmpty(ItemCode))
            {
                UpdateExamItem();
            }
            dataService.SaveUpdateSettings(listSetting);
         
            speaker.SpeakActionVoice(ActivityName + "保存成功");
            //进行一个资源的释放
            listSetting = null;
        }
        public virtual void InitSetting()
        {

        }


    }




}