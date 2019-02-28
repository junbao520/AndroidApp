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
    [Activity(Label = "ThroughPedestrianCrossingActivity")]
    public class ThroughPedestrianCrossingActivity : BaseSettingActivity
    {

        #region 变量定义
        
 
         #region 人行横道
         // 人行横道项目语音
         EditText edtTxtPedestrianCrossingVoice;
         // 人行横道项目结束语音
         EditText edtTxtPedestrianCrossingEndVoice;
         // 人行横道项目距离
         EditText edtTxtPedestrianCrossingDistance;

        // 人行横道准备距离
        EditText edtTxtPedestrianCrossingPrepareD;
        // 人行横道速度限制
        EditText edtTxtPedestrianCrossingSpeedLimit;
         // 人行横道要求踩刹车速度限制
         EditText edtTxtPedestrianCrossingBrakeSpeedUp;
         // 人行横道必须踩刹车
         CheckBox chkPedestrianCrossingBrakeRequire;
         // 人行横道远近光交替灯光检测
         CheckBox chkPedestrianCrossingLightCheck;
         // 人行横道白考喇叭检测
         CheckBox chkPedestrianCrossingLoudSpeakerDayCheck;
        // 人行横道夜考喇叭检测
        CheckBox chkPedestrianCrossingLoudSpeakerNightCheck;
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.pedestriancrossingvoice);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.PedestrianCrossing;
            ActivityName = this.GetString(Resource.String.ThroughPedestrianCrossingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }

        public void InitControl()
        {
    


            #region 人行横道
            edtTxtPedestrianCrossingVoice = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingVoice);
            edtTxtPedestrianCrossingEndVoice = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingEndVoice);
            edtTxtPedestrianCrossingDistance = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingDistance);
            edtTxtPedestrianCrossingPrepareD = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingPrepareD);
            edtTxtPedestrianCrossingSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingSpeedLimit);
            edtTxtPedestrianCrossingBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingBrakeSpeedUp);
            chkPedestrianCrossingBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingBrakeRequire);
            chkPedestrianCrossingLightCheck = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingLightCheck);
            chkPedestrianCrossingLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingLoudSpeakerDayCheck);
            chkPedestrianCrossingLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingLoudSpeakerNightCheck);
            #endregion

        }
   

        public override void InitSetting()
        {
           

            edtTxtPedestrianCrossingVoice.Text = ItemVoice;
            edtTxtPedestrianCrossingEndVoice.Text = ItemEndVoice;

            #region 人行横道
            edtTxtPedestrianCrossingDistance.Text = Settings.PedestrianCrossingDistance.ToString();
            edtTxtPedestrianCrossingPrepareD.Text = Settings.PedestrainCrossingPrepareD.ToString();
            edtTxtPedestrianCrossingSpeedLimit.Text = Settings.PedestrianCrossingSpeedLimit.ToString();
            edtTxtPedestrianCrossingBrakeSpeedUp.Text = Settings.PedestrianCrossingBrakeSpeedUp.ToString();
            chkPedestrianCrossingBrakeRequire.Checked = Settings.PedestrianCrossingBrakeRequire;
            chkPedestrianCrossingLightCheck.Checked = Settings.PedestrianCrossingLightCheck;
            chkPedestrianCrossingLoudSpeakerDayCheck.Checked = Settings.PedestrianCrossingLoudSpeakerDayCheck;
            chkPedestrianCrossingLoudSpeakerNightCheck.Checked = Settings.PedestrianCrossingLoudSpeakerNightCheck;
            #endregion

     
        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
   
                ItemVoice = edtTxtPedestrianCrossingVoice.Text;
                ItemEndVoice = edtTxtPedestrianCrossingEndVoice.Text;
                
                #region 人行横道
                Settings.PedestrianCrossingDistance = Convert.ToInt32(edtTxtPedestrianCrossingDistance.Text);
                Settings.PedestrainCrossingPrepareD = Convert.ToInt32(edtTxtPedestrianCrossingPrepareD.Text);
       
                Settings.PedestrianCrossingSpeedLimit = Convert.ToInt32(edtTxtPedestrianCrossingSpeedLimit.Text);
                Settings.PedestrianCrossingBrakeSpeedUp = Convert.ToInt32(edtTxtPedestrianCrossingBrakeSpeedUp.Text);
                Settings.PedestrianCrossingBrakeRequire = chkPedestrianCrossingBrakeRequire.Checked;
                Settings.PedestrianCrossingLightCheck = chkPedestrianCrossingLightCheck.Checked;
                Settings.PedestrianCrossingLoudSpeakerDayCheck = chkPedestrianCrossingLoudSpeakerDayCheck.Checked;
                Settings.PedestrianCrossingLoudSpeakerNightCheck = chkPedestrianCrossingLoudSpeakerNightCheck.Checked;
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 人行横道
new Setting { Key ="PedestrianCrossingDistance", Value = Settings.PedestrianCrossingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrainCrossingPrepareD", Value = Settings.PedestrainCrossingPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingSpeedLimit", Value = Settings.PedestrianCrossingSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingBrakeSpeedUp", Value = Settings.PedestrianCrossingBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingBrakeRequire", Value = Settings.PedestrianCrossingBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingLightCheck", Value = Settings.PedestrianCrossingLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingLoudSpeakerDayCheck", Value = Settings.PedestrianCrossingLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingLoudSpeakerNightCheck", Value = Settings.PedestrianCrossingLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
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