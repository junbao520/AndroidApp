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
    [Activity(Label = "StraightThroughIntersectionActivity")]
    public class StraightThroughIntersectionActivity : BaseSettingActivity
    {

        #region 变量定义

        #region 路口直行
        // 路口直行项目语音
        EditText edtTxtStraightThroughIntersectionVoice;
        // 路口直行项目结束语音
        EditText edtTxtStraightThroughIntersectionEndVoice;
        // 路口直行项目距离
        EditText edtTxtStraightThroughIntersectionDistance;
        // 路口直行准备距离
        EditText edtTxtThroughStraightPrepareD;
        // 路口直行速度限制
        EditText edtTxtStraightThroughIntersectionSpeedLimit;
        // 路口直行要求踩刹车速度限制
        EditText edtTxtStraightThroughIntersectionBrakeSpeedUp;
        // 路口直行必须踩刹车
        CheckBox chkStraightThroughIntersectionBrakeRequire;
        // 路口直行夜考远近光交替
        CheckBox chkStraightThroughIntersectionLightCheck;
        // 路口直行白考喇叭检测
        CheckBox chkStraightThroughIntersectionLoudSpeakerDayCheck;
        // 路口直行夜考喇叭检测
        CheckBox chkStraightThroughIntersectionLoudSpeakerNightCheck;
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.straightthroughintersection);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.StraightThrough;
            ActivityName = this.GetString(Resource.String.StraightThroughIntersectionStr);
            setMyTitle(ActivityName);
            InitSetting();
        }



        public override void InitSetting()
        {
            
            edtTxtStraightThroughIntersectionVoice.Text = ItemVoice;
            edtTxtStraightThroughIntersectionEndVoice.Text = ItemEndVoice;

            #region 路口直行
            edtTxtStraightThroughIntersectionDistance.Text = Settings.StraightThroughIntersectionDistance.ToString();
            edtTxtThroughStraightPrepareD.Text = Settings.ThroughStraightPrepareD.ToString();
            edtTxtStraightThroughIntersectionSpeedLimit.Text = Settings.StraightThroughIntersectionSpeedLimit.ToString();
            edtTxtStraightThroughIntersectionBrakeSpeedUp.Text = Settings.StraightThroughIntersectionBrakeSpeedUp.ToString();
            chkStraightThroughIntersectionBrakeRequire.Checked = Settings.StraightThroughIntersectionBrakeRequire;
            chkStraightThroughIntersectionLightCheck.Checked = Settings.StraightThroughIntersectionLightCheck;
            chkStraightThroughIntersectionLoudSpeakerDayCheck.Checked = Settings.StraightThroughIntersectionLoudSpeakerDayCheck;
            chkStraightThroughIntersectionLoudSpeakerNightCheck.Checked = Settings.StraightThroughIntersectionLoudSpeakerNightCheck;
            #endregion


           

        }

        public void InitControl()
        {
         
            #region 路口直行
            edtTxtStraightThroughIntersectionVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionVoice);
            edtTxtStraightThroughIntersectionEndVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionEndVoice);
            edtTxtStraightThroughIntersectionDistance = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionDistance);
            edtTxtThroughStraightPrepareD = FindViewById<EditText>(Resource.Id.edtTxtThroughStraightPrepareD);
            edtTxtStraightThroughIntersectionSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionSpeedLimit);
            edtTxtStraightThroughIntersectionBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionBrakeSpeedUp);
            chkStraightThroughIntersectionBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionBrakeRequire);
            chkStraightThroughIntersectionLightCheck = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionLightCheck);
            chkStraightThroughIntersectionLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionLoudSpeakerDayCheck);
            chkStraightThroughIntersectionLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionLoudSpeakerNightCheck);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
       
                ItemVoice= edtTxtStraightThroughIntersectionVoice.Text;
                ItemEndVoice = edtTxtStraightThroughIntersectionEndVoice.Text;
      

                #region 路口直行
                Settings.StraightThroughIntersectionDistance = Convert.ToInt32(edtTxtStraightThroughIntersectionDistance.Text);
                Settings.ThroughStraightPrepareD= Convert.ToInt32(edtTxtThroughStraightPrepareD.Text);
                Settings.StraightThroughIntersectionSpeedLimit = Convert.ToInt32(edtTxtStraightThroughIntersectionSpeedLimit.Text);
                Settings.StraightThroughIntersectionBrakeSpeedUp = Convert.ToInt32(edtTxtStraightThroughIntersectionBrakeSpeedUp.Text);
                Settings.StraightThroughIntersectionBrakeRequire = chkStraightThroughIntersectionBrakeRequire.Checked;
                Settings.StraightThroughIntersectionLightCheck = chkStraightThroughIntersectionLightCheck.Checked;
                Settings.StraightThroughIntersectionLoudSpeakerDayCheck = chkStraightThroughIntersectionLoudSpeakerDayCheck.Checked;
                Settings.StraightThroughIntersectionLoudSpeakerNightCheck = chkStraightThroughIntersectionLoudSpeakerNightCheck.Checked;
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                { 
                    #region 路口直行
                    new Setting { Key ="ThroughStraightPrepareD", Value = Settings.ThroughStraightPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionDistance", Value = Settings.StraightThroughIntersectionDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionSpeedLimit", Value = Settings.StraightThroughIntersectionSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionBrakeSpeedUp", Value = Settings.StraightThroughIntersectionBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionBrakeRequire", Value = Settings.StraightThroughIntersectionBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionLightCheck", Value = Settings.StraightThroughIntersectionLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionLoudSpeakerDayCheck", Value = Settings.StraightThroughIntersectionLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionLoudSpeakerNightCheck", Value = Settings.StraightThroughIntersectionLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
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