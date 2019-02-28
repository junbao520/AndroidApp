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
    [Activity(Label = "TurnLeftActivity")]
    public class TurnLeftActivity : BaseSettingActivity
    {

        #region 变量定义
        #region 路口左转

        // 路口左转项目语音
        EditText edtTxtTurnLeftVoice;
        // 路口右转项目语音
        EditText edtTxtTurnLeftEndVoice;
        // 路口左转项目距离
        EditText edtTxtTurnLeftDistance;

        // 路口左转项目准备距离
        EditText edtTxtTurnLeftPrepareD;
        //左转变道角度
        EditText edtTxtTurnLeftAngle;
        // 路口左转速度限制
        EditText edtTxtTurnLeftSpeedLimit;
        // 路口左转要求踩刹车速度限制
        EditText edtTxtTurnLeftBrakeSpeedUp;
        // 路口左转必须踩刹车
        CheckBox chkTurnLeftBrakeRequire;
        //路口左转专项灯检测
        CheckBox chkTurnLeftLightCheck;
        // 路口左转白考喇叭检测
        CheckBox chkTurnLeftLoudSpeakerDayCheck;
        // 路口左转夜考喇叭检测
        CheckBox chkTurnLeftLoudSpeakerNightCheck;
        //角度达到就完成
        CheckBox chkTurnLeftEndFlag;

        CheckBox chkTurnLeftErrorLight;
        #endregion
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.turnleft);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.TurnLeft;
            ActivityName = this.GetString(Resource.String.TurnLeftStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


 

        public override void InitSetting()
        {
            #region 项目语音
            
            edtTxtTurnLeftVoice.Text = ItemVoice;
            edtTxtTurnLeftEndVoice.Text =ItemEndVoice;

            #endregion
            #region 路口左转
            edtTxtTurnLeftDistance.Text = Settings.TurnLeftDistance.ToString();
            edtTxtTurnLeftPrepareD.Text = Settings.TurnLeftPrepareD.ToString();
            edtTxtTurnLeftSpeedLimit.Text = Settings.TurnLeftSpeedLimit.ToString();
            edtTxtTurnLeftBrakeSpeedUp.Text = Settings.TurnLeftBrakeSpeedUp.ToString();
            chkTurnLeftBrakeRequire.Checked = Settings.TurnLeftBrakeRequire;
            chkTurnLeftLightCheck.Checked = Settings.TurnLeftLightCheck;
            chkTurnLeftLoudSpeakerDayCheck.Checked = Settings.TurnLeftLoudSpeakerDayCheck;
            chkTurnLeftLoudSpeakerNightCheck.Checked = Settings.TurnLeftLoudSpeakerNightCheck;
            edtTxtTurnLeftAngle.Text = Settings.TurnLeftAngle.ToString();

            chkTurnLeftEndFlag.Checked = Settings.TurnLeftEndFlag;

            chkTurnLeftErrorLight.Checked = Settings.TurnLeftErrorLight;
            #endregion



        }

        public void InitControl()
        {

            #region 左转
            edtTxtTurnLeftVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftVoice);
            edtTxtTurnLeftEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftEndVoice);
            edtTxtTurnLeftDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftDistance);
            edtTxtTurnLeftPrepareD = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftPrepareD);
            edtTxtTurnLeftSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftSpeedLimit);
            edtTxtTurnLeftBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftBrakeSpeedUp);
            chkTurnLeftBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkTurnLeftBrakeRequire);
            chkTurnLeftLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLightCheck);
            chkTurnLeftLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLoudSpeakerDayCheck);
            chkTurnLeftLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLoudSpeakerNightCheck);

            edtTxtTurnLeftAngle= FindViewById<EditText>(Resource.Id.edtTxtTurnLeftAngle);

            chkTurnLeftEndFlag = FindViewById<CheckBox>(Resource.Id.chkTurnLeftEndFlag);


            chkTurnLeftErrorLight = FindViewById<CheckBox>(Resource.Id.chkTurnLeftErrorLight);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {

                ItemVoice = edtTxtTurnLeftVoice.Text;
                ItemEndVoice = edtTxtTurnLeftEndVoice.Text;
     
                #region 路口左转
                Settings.TurnLeftDistance = Convert.ToInt32(edtTxtTurnLeftDistance.Text);
                Settings.TurnLeftPrepareD = Convert.ToInt32(edtTxtTurnLeftPrepareD.Text);
                Settings.TurnLeftSpeedLimit = Convert.ToInt32(edtTxtTurnLeftSpeedLimit.Text);
                Settings.TurnLeftBrakeSpeedUp = Convert.ToInt32(edtTxtTurnLeftBrakeSpeedUp.Text);
                Settings.TurnLeftBrakeRequire = chkTurnLeftBrakeRequire.Checked;
                Settings.TurnLeftLightCheck = chkTurnLeftLightCheck.Checked;
                Settings.TurnLeftLoudSpeakerDayCheck = chkTurnLeftLoudSpeakerDayCheck.Checked;
                Settings.TurnLeftLoudSpeakerNightCheck = chkTurnLeftLoudSpeakerNightCheck.Checked;

                Settings.TurnLeftEndFlag = chkTurnLeftEndFlag.Checked;
                 Settings.TurnLeftAngle= Convert.ToDouble(edtTxtTurnLeftAngle.Text);

                Settings.TurnLeftErrorLight = chkTurnLeftErrorLight.Checked ;
                #endregion

                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 左转


                    new Setting { Key ="TurnLeftDistance", Value = Settings.TurnLeftDistance.ToString(), GroupName = "GlobalSettings" },
                    new Setting { Key ="TurnLeftPrepareD", Value = Settings.TurnLeftPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftSpeedLimit", Value = Settings.TurnLeftSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftBrakeSpeedUp", Value = Settings.TurnLeftBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftBrakeRequire", Value = Settings.TurnLeftBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLightCheck", Value = Settings.TurnLeftLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLoudSpeakerDayCheck", Value = Settings.TurnLeftLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLoudSpeakerNightCheck", Value = Settings.TurnLeftLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftAngle", Value = Settings.TurnLeftAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftEndFlag", Value = Settings.TurnLeftEndFlag.ToString(), GroupName = "GlobalSettings" },
 new Setting { Key ="TurnLeftErrorLight", Value = Settings.TurnLeftErrorLight.ToString(), GroupName = "GlobalSettings" },                   

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