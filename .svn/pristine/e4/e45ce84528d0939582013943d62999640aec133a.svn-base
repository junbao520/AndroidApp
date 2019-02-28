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
    [Activity(Label = "VehicleStartingActivity")]
    public class VehicleStartingActivity : BaseSettingActivity
    {

        #region 变量定义
        //


        #region 起步
        EditText edtTxtStartVoice;
        //
        EditText edtTxtStartEndVoice;
        // 起步项目距离，单位：米
        EditText edtTxtStartStopCheckForwardDistance;
        // 起步时间（单位：秒）
        EditText edtTxtStartTimeout;
        //起步喇叭检测
        CheckBox chkStartLoudSpeakerCheck;
        // 起步检测白考左转向灯
        CheckBox chkIsCheckStartLight;
        // 起步夜间检测远近光交替
        CheckBox chkStartLowAndHighBeamInNight;
        // 起步闯动检测
        CheckBox chkStartShockEnable;

        //起步一档
        CheckBox chkStartIsMustGearOne;
        // 不松手刹最小时间（单位：秒）
        EditText edtTxtStartReleaseHandbrakeTimeout;
        // 起步时发动机最高转速（配置0，不评判）
        EditText edtTxtStartEngineRpm;
        // 起步闯动值
        EditText edtTxtStartShockValue;

        EditText edtTxtStartShockCount;
        #endregion


        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.start);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.Start;
            ActivityName = this.GetString(Resource.String.VehicleStartingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }



        public override void InitSetting()
        {
            #region 项目语音
           

            edtTxtStartVoice.Text =ItemVoice;
            edtTxtStartEndVoice.Text = ItemEndVoice;
            
            #endregion

          

            #region 起步

            edtTxtStartStopCheckForwardDistance.Text = Settings.StartStopCheckForwardDistance.ToString();
            edtTxtStartTimeout.Text = Settings.StartTimeout.ToString();
            chkIsCheckStartLight.Checked = Settings.IsCheckStartLight;
            chkStartLowAndHighBeamInNight.Checked = Settings.StartLowAndHighBeamInNight;
            chkStartShockEnable.Checked = Settings.StartShockEnable;
            edtTxtStartReleaseHandbrakeTimeout.Text = Settings.StartReleaseHandbrakeTimeout.ToString();
            edtTxtStartEngineRpm.Text = Settings.StartEngineRpm.ToString();
            edtTxtStartShockValue.Text = Settings.StartShockValue.ToString();
            edtTxtStartShockCount.Text = Settings.StartShockCount.ToString();
            chkStartLoudSpeakerCheck.Checked = Settings.VehicleStartingLoudSpeakerDayCheck;
            chkStartIsMustGearOne.Checked= Settings.StartIsMustGearOne;
            #endregion



        }

        public void InitControl()
        {
         
            #region 起步
            edtTxtStartVoice = FindViewById<EditText>(Resource.Id.edtTxtStartVoice);
            edtTxtStartEndVoice = FindViewById<EditText>(Resource.Id.edtTxtStartEndVoice);

            edtTxtStartStopCheckForwardDistance = FindViewById<EditText>(Resource.Id.edtTxtStartStopCheckForwardDistance);
            edtTxtStartTimeout = FindViewById<EditText>(Resource.Id.edtTxtStartTimeout);
            chkIsCheckStartLight = FindViewById<CheckBox>(Resource.Id.chkIsCheckStartLight);
            chkStartLoudSpeakerCheck = FindViewById<CheckBox>(Resource.Id.chkStartLoudSpeakerCheck);
            chkStartLowAndHighBeamInNight = FindViewById<CheckBox>(Resource.Id.chkStartLowAndHighBeamInNight);
            chkStartShockEnable = FindViewById<CheckBox>(Resource.Id.chkStartShockEnable);
            chkStartIsMustGearOne= FindViewById<CheckBox>(Resource.Id.chkStartIsMustGearOne);
            edtTxtStartReleaseHandbrakeTimeout = FindViewById<EditText>(Resource.Id.edtTxtStartReleaseHandbrakeTimeout);
            edtTxtStartEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtStartEngineRpm);
            edtTxtStartShockValue = FindViewById<EditText>(Resource.Id.edtTxtStartShockValue);
            edtTxtStartShockCount = FindViewById<EditText>(Resource.Id.edtTxtStartShockCount);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
             
                ItemVoice= edtTxtStartVoice.Text;
                ItemEndVoice= edtTxtStartEndVoice.Text;
                #region 起步
                Settings.StartStopCheckForwardDistance = Convert.ToDouble(edtTxtStartStopCheckForwardDistance.Text);
                Settings.StartTimeout = Convert.ToInt32(edtTxtStartTimeout.Text);
                Settings.IsCheckStartLight = chkIsCheckStartLight.Checked;
                //白考核夜考是一样的
                Settings.VehicleStartingLoudSpeakerDayCheck = chkStartLoudSpeakerCheck.Checked;
                Settings.VehicleStartingLoudSpeakerNightCheck = chkStartLoudSpeakerCheck.Checked;

                Settings.StartLowAndHighBeamInNight = chkStartLowAndHighBeamInNight.Checked;
                Settings.StartShockEnable = chkStartShockEnable.Checked;
                Settings.StartReleaseHandbrakeTimeout = Convert.ToInt32(edtTxtStartReleaseHandbrakeTimeout.Text);
                Settings.StartEngineRpm = Convert.ToInt32(edtTxtStartEngineRpm.Text);
                Settings.StartShockValue = Convert.ToDouble(edtTxtStartShockValue.Text);
                Settings.StartShockCount = Convert.ToDouble(edtTxtStartShockCount.Text);

                Settings.StartIsMustGearOne = chkStartIsMustGearOne.Checked;
                #endregion
                #region listSetting


                List <Setting> lstSetting = new List<Setting>
                {

new Setting { Key ="StartStopCheckForwardDistance", Value = Settings.StartStopCheckForwardDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartTimeout", Value = Settings.StartTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IsCheckStartLight", Value = Settings.IsCheckStartLight.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="VehicleStartingLoudSpeakerDayCheck", Value = Settings.VehicleStartingLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="VehicleStartingLoudSpeakerNightCheck", Value = Settings.VehicleStartingLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartLowAndHighBeamInNight", Value = Settings.StartLowAndHighBeamInNight.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartShockEnable", Value = Settings.StartShockEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartReleaseHandbrakeTimeout", Value = Settings.StartReleaseHandbrakeTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartEngineRpm", Value = Settings.StartEngineRpm.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartShockValue", Value = Settings.StartShockValue.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartShockCount", Value = Settings.StartShockCount.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartIsMustGearOne", Value = Settings.StartIsMustGearOne.ToString(), GroupName = "GlobalSettings" },
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