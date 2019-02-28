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
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "ThroughBusAreaActivity")]
    public class ThroughBusAreaActivity : BaseSettingActivity
    {

        #region 变量定义
      
         #region 公交车站

         // 公交车站项目语音
         EditText edtTxtBusAreaVoice;
         // 公交区域结束语音
         EditText edtTxtBusAreaEndVoice;
         // 公交车站项目距离
         EditText edtTxtBusAreaDistance;
        // 公交车站项目准备距离
        EditText edtTxtBusAreaPrepareD;
        // 公交车站速度限制
        EditText edtTxtBusAreaSpeedLimit;
         // 公交车站要求踩刹车速度限制
         EditText edtTxtBusAreaBrakeSpeedUp;
         // 公交车站必须踩刹车
         CheckBox chkBusAreaBrakeRequire;
         // 公交车站远近光交替
         CheckBox chkBusAreaLightCheck;
         // 公交车站白考喇叭检测
         CheckBox chkBusAreaLoudSpeakerDayCheck;
         // 公交车站夜考喇叭检测
         CheckBox chkBusAreaLoudSpeakerNightCheck;
         #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.busarea);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.BusArea;
            ActivityName = this.GetString(Resource.String.ThroughBusAreaStr);
            setMyTitle(ActivityName);
            InitSetting();
        }

        public void InitControl()
        {
            #region 公交汽车
            edtTxtBusAreaVoice = FindViewById<EditText>(Resource.Id.edtTxtBusAreaVoice);
            edtTxtBusAreaEndVoice = FindViewById<EditText>(Resource.Id.edtTxtBusAreaEndVoice);
            edtTxtBusAreaDistance = FindViewById<EditText>(Resource.Id.edtTxtBusAreaDistance);
            edtTxtBusAreaPrepareD = FindViewById<EditText>(Resource.Id.edtTxtBusAreaPrepareD);
            edtTxtBusAreaSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtBusAreaSpeedLimit);
            edtTxtBusAreaBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtBusAreaBrakeSpeedUp);
            chkBusAreaBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkBusAreaBrakeRequire);
            chkBusAreaLightCheck = FindViewById<CheckBox>(Resource.Id.chkBusAreaLightCheck);
            chkBusAreaLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkBusAreaLoudSpeakerDayCheck);
            chkBusAreaLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkBusAreaLoudSpeakerNightCheck);
            #endregion
        }
 

        public override void InitSetting()
        {
            #region 项目语音
           


            edtTxtBusAreaVoice.Text = ItemVoice;
            edtTxtBusAreaEndVoice.Text = ItemEndVoice;
           
            #endregion


             #region 公交汽车
             edtTxtBusAreaDistance.Text = Settings.BusAreaDistance.ToString();
              edtTxtBusAreaPrepareD.Text = Settings.BusAreaPrepareD.ToString();
            edtTxtBusAreaSpeedLimit.Text = Settings.BusAreaSpeedLimit.ToString();
             edtTxtBusAreaBrakeSpeedUp.Text = Settings.BusAreaBrakeSpeedUp.ToString();
             chkBusAreaBrakeRequire.Checked = Settings.BusAreaBrakeRequire;
             chkBusAreaLightCheck.Checked = Settings.BusAreaLightCheck;
             chkBusAreaLoudSpeakerDayCheck.Checked = Settings.BusAreaLoudSpeakerDayCheck;
             chkBusAreaLoudSpeakerNightCheck.Checked = Settings.BusAreaLoudSpeakerNightCheck;
             #endregion

      
        }


        public override void UpdateSettings()
        {
            try
            {
              
                ItemVoice = edtTxtBusAreaVoice.Text;
                ItemEndVoice = edtTxtBusAreaEndVoice.Text;
   



                #region 公交汽车
                Settings.BusAreaDistance = Convert.ToInt32(edtTxtBusAreaDistance.Text);
                Settings.BusAreaPrepareD = Convert.ToInt32(edtTxtBusAreaPrepareD.Text);
                Settings.BusAreaSpeedLimit = Convert.ToInt32(edtTxtBusAreaSpeedLimit.Text);
                Settings.BusAreaBrakeSpeedUp = Convert.ToInt32(edtTxtBusAreaBrakeSpeedUp.Text);
                Settings.BusAreaBrakeRequire = chkBusAreaBrakeRequire.Checked;
                Settings.BusAreaLightCheck = chkBusAreaLightCheck.Checked;
                Settings.BusAreaLoudSpeakerDayCheck = chkBusAreaLoudSpeakerDayCheck.Checked;
                Settings.BusAreaLoudSpeakerNightCheck = chkBusAreaLoudSpeakerNightCheck.Checked;
                #endregion

                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 公交汽车
new Setting { Key ="BusAreaDistance", Value = Settings.BusAreaDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaPrepareD", Value = Settings.BusAreaPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaSpeedLimit", Value = Settings.BusAreaSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaBrakeSpeedUp", Value = Settings.BusAreaBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaBrakeRequire", Value = Settings.BusAreaBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaLightCheck", Value = Settings.BusAreaLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaLoudSpeakerDayCheck", Value = Settings.BusAreaLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaLoudSpeakerNightCheck", Value = Settings.BusAreaLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                };
                #endregion
                UpdateSettings(lstSetting);
                Finish();
            }
            catch (Exception ex)
            {
                string HeaderText = string.Format("{0}  保存失败：{1}", ActivityName, ex.Message);
                setMyTitle(HeaderText);
                Logger.Error(ActivityName, ex.Message);

            }

        }
    }
}