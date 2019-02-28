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
    [Activity(Label = "TurnRoundActivity")]
    public class TurnRoundActivity : BaseSettingActivity
    {

        #region 变量定义
        #region 掉头
        // 掉头项目语音 
        EditText edtTxtTurnRoundVoice;
        // 掉头项目结束语音 
        EditText edtTxtTurnRoundEndVoice;
        // 掉头所用最大距离限制（单位：米）
        EditText edtTxtTurnRoundMaxDistance;
        // 掉头所用准备距离限制（单位：米）
        EditText edtTxtTurnRoundPrepareD;
        // 掉头转向角度差确认开始掉头（单位：度）
        EditText edtTxtTurnRoundStartAngleDiff;
        // 掉头结束掉头转向角度差（单位：度）
        EditText edtTxtTurnRoundEndAngleDiff;
        // 掉头必踩刹车
        CheckBox chkTurnRoundBrakeRequired;
        // 掉头夜间远近光交替检查
        CheckBox chkTurnRoundLightCheck;
        // 掉头白考喇叭检测 
        CheckBox chkTurnRoundLoudSpeakerDayCheck;
        // 掉头夜考喇叭检测 
        CheckBox chkTurnRoundLoudSpeakerNightCheck;

        CheckBox chkTurnRoundErrorLight;
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.turnround);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.TurnRound;
            ActivityName = this.GetString(Resource.String.TurnRoundStr);
            setMyTitle(ActivityName);
            InitSetting();
        }



        public override void InitSetting()
        {
            #region 项目语音
           

            edtTxtTurnRoundVoice.Text = ItemVoice;
            edtTxtTurnRoundEndVoice.Text = ItemEndVoice;

            
            #endregion

            #region 掉头
            edtTxtTurnRoundMaxDistance.Text = Settings.TurnRoundMaxDistance.ToString();
            edtTxtTurnRoundPrepareD.Text = Settings.TurnRoundPrepareD.ToString();
            edtTxtTurnRoundStartAngleDiff.Text = Settings.TurnRoundStartAngleDiff.ToString();
            edtTxtTurnRoundEndAngleDiff.Text = Settings.TurnRoundEndAngleDiff.ToString();
            chkTurnRoundBrakeRequired.Checked = Settings.TurnRoundBrakeRequired;
            chkTurnRoundLightCheck.Checked = Settings.TurnRoundLightCheck;
            chkTurnRoundLoudSpeakerDayCheck.Checked = Settings.TurnRoundLoudSpeakerDayCheck;
            chkTurnRoundLoudSpeakerNightCheck.Checked = Settings.TurnRoundLoudSpeakerNightCheck;


            chkTurnRoundErrorLight.Checked = Settings.TurnRoundErrorLight;
            #endregion
        }

        public void InitControl()
        {
         

            #region 掉头
            edtTxtTurnRoundVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundVoice);
            edtTxtTurnRoundEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundEndVoice);
            edtTxtTurnRoundMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundMaxDistance);
            edtTxtTurnRoundPrepareD = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundPrepareD);
            edtTxtTurnRoundStartAngleDiff = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundStartAngleDiff);
            edtTxtTurnRoundEndAngleDiff = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundEndAngleDiff);
            chkTurnRoundBrakeRequired = FindViewById<CheckBox>(Resource.Id.chkTurnRoundBrakeRequired);
            chkTurnRoundLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLightCheck);
            chkTurnRoundLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLoudSpeakerDayCheck);
            chkTurnRoundLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLoudSpeakerNightCheck);

            chkTurnRoundErrorLight = FindViewById<CheckBox>(Resource.Id.chkTurnRoundErrorLight);
            
            #endregion


        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
               
                ItemVoice= edtTxtTurnRoundVoice.Text;
                ItemEndVoice = edtTxtTurnRoundEndVoice.Text;
            
                #region 掉头


                Settings.TurnRoundMaxDistance = Convert.ToInt32(edtTxtTurnRoundMaxDistance.Text);
                Settings.TurnRoundPrepareD = Convert.ToInt32(edtTxtTurnRoundPrepareD.Text);
                Settings.TurnRoundStartAngleDiff = Convert.ToInt32(edtTxtTurnRoundStartAngleDiff.Text);
                Settings.TurnRoundEndAngleDiff = Convert.ToInt32(edtTxtTurnRoundEndAngleDiff.Text);
                Settings.TurnRoundBrakeRequired = chkTurnRoundBrakeRequired.Checked;
                Settings.TurnRoundLightCheck = chkTurnRoundLightCheck.Checked;
                Settings.TurnRoundLoudSpeakerDayCheck = chkTurnRoundLoudSpeakerDayCheck.Checked;
                Settings.TurnRoundLoudSpeakerNightCheck = chkTurnRoundLoudSpeakerNightCheck.Checked;

                Settings.TurnRoundErrorLight = chkTurnRoundErrorLight.Checked ;
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 掉头
new Setting { Key ="TurnRoundMaxDistance", Value = Settings.TurnRoundMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundPrepareD", Value = Settings.TurnRoundPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundStartAngleDiff", Value = Settings.TurnRoundStartAngleDiff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundEndAngleDiff", Value = Settings.TurnRoundEndAngleDiff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundBrakeRequired", Value = Settings.TurnRoundBrakeRequired.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLightCheck", Value = Settings.TurnRoundLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLoudSpeakerDayCheck", Value = Settings.TurnRoundLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLoudSpeakerNightCheck", Value = Settings.TurnRoundLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundErrorLight", Value = Settings.TurnRoundErrorLight.ToString(), GroupName = "GlobalSettings" },
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