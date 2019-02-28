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
    [Activity(Label = "OvertakeActivity")]
    public class OvertakeActivity : BaseSettingActivity
    {

        #region 变量定义
   
        #region 超车
        // 超车语音
        EditText edtTxtOvertakeVoice;
        // 超车结束语音
        EditText edtTxtOvertakeEndVoice;
        // 超车最大距离（单位：米）
        EditText edtTxtOvertakeMaxDistance;
        // 超车超时时间（单位：秒）
        EditText edtTxtOvertakeTimeout;
        // 变道转向角度
        EditText edtTxtOvertakeChangeLanesAngle;
        // 超车最低速度
        EditText edtTxtOvertakeLowestSpeed;
        // 超车速度限制
        EditText edtTxtOvertakeSpeedLimit;
        // 超车达到一次速度
        EditText edtTxtOvertakeSpeedOnce;
        // 超车是否返回原车道
        EditText edtTxtOvertakeBackToOriginalLaneVoice;
        // 超车是否返回原车道
        EditText edtTxtOvertakeBackToOriginalLaneDistance;

        //超车准备距离
        EditText edtTxtOverTakePrepareDistance;
        // 超车夜间远近光交替
        CheckBox chkOvertakeLowAndHighBeamCheck;
        // 超车转向灯灯光检测
        CheckBox chkOvertakeLightCheck;
        // 超车白考喇叭检测
        CheckBox chkOvertakeLoudSpeakerDayCheck;
        // 超车夜考喇叭检测
        CheckBox chkOvertakeLoudSpeakerNightCheck;
        // 超车是否返回原车道
        CheckBox chkOvertakeBackToOriginalLane;
        #endregion

      
        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.overtake);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.Overtaking;
            ActivityName = this.GetString(Resource.String.OvertakeStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


       

        public override void InitSetting()
        {
            #region 项目语音
           

            edtTxtOvertakeVoice.Text = ItemVoice;
            edtTxtOvertakeEndVoice.Text = ItemEndVoice ;

         
            #endregion

           

            #region 超车
            edtTxtOvertakeMaxDistance.Text = Settings.OvertakeMaxDistance.ToString();
            edtTxtOvertakeTimeout.Text = Settings.OvertakeTimeout.ToString();
            edtTxtOvertakeChangeLanesAngle.Text = Settings.OvertakeChangeLanesAngle.ToString();
            edtTxtOvertakeLowestSpeed.Text = Settings.OvertakeLowestSpeed.ToString();
            edtTxtOvertakeSpeedLimit.Text = Settings.OvertakeSpeedLimit.ToString();
            edtTxtOvertakeSpeedOnce.Text= Settings.OvertakeSpeedOnce.ToString();
            edtTxtOvertakeBackToOriginalLaneVoice.Text = Settings.OvertakeBackToOriginalLaneVocie.ToString();
            edtTxtOvertakeBackToOriginalLaneDistance.Text = Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance.ToString();
            chkOvertakeLowAndHighBeamCheck.Checked = Settings.OvertakeLowAndHighBeamCheck;
            chkOvertakeLightCheck.Checked = Settings.OvertakeLightCheck;
            chkOvertakeLoudSpeakerDayCheck.Checked = Settings.OvertakeLoudSpeakerDayCheck;
            chkOvertakeLoudSpeakerNightCheck.Checked = Settings.OvertakeLoudSpeakerNightCheck;
            chkOvertakeBackToOriginalLane.Checked = Settings.OvertakeBackToOriginalLane;

            edtTxtOverTakePrepareDistance.Text = Settings.OvertakePrepareDistance.ToString();
            #endregion


           

        }

        public void InitControl()
        {
          
            #region 超车
            edtTxtOvertakeVoice = FindViewById<EditText>(Resource.Id.edtTxtOvertakeVoice);
            edtTxtOvertakeEndVoice = FindViewById<EditText>(Resource.Id.edtTxtOvertakeEndVoice);
            edtTxtOvertakeMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtOvertakeMaxDistance);
            edtTxtOvertakeTimeout = FindViewById<EditText>(Resource.Id.edtTxtOvertakeTimeout);
            edtTxtOvertakeChangeLanesAngle = FindViewById<EditText>(Resource.Id.edtTxtOvertakeChangeLanesAngle);
            edtTxtOvertakeLowestSpeed = FindViewById<EditText>(Resource.Id.edtTxtOvertakeLowestSpeed);
            edtTxtOvertakeSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtOvertakeSpeedLimit);
            edtTxtOvertakeSpeedOnce = FindViewById<EditText>(Resource.Id.edtTxtOvertakeSpeedOnce);
            edtTxtOverTakePrepareDistance = FindViewById<EditText>(Resource.Id.edtTxtOverTakePrepareDistance);
            edtTxtOvertakeBackToOriginalLaneVoice = FindViewById<EditText>(Resource.Id.edtTxtOvertakeBackToOriginalLaneVoice);
            edtTxtOvertakeBackToOriginalLaneDistance = FindViewById<EditText>(Resource.Id.edtTxtOvertakeBackToOriginalLaneDistance);
            chkOvertakeLowAndHighBeamCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLowAndHighBeamCheck);
            chkOvertakeLightCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLightCheck);
            chkOvertakeLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLoudSpeakerDayCheck);
            chkOvertakeLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLoudSpeakerNightCheck);
            chkOvertakeBackToOriginalLane = FindViewById<CheckBox>(Resource.Id.chkOvertakeBackToOriginalLane);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
        
                ItemVoice= edtTxtOvertakeVoice.Text; ;
                ItemEndVoice = edtTxtOvertakeEndVoice.Text; ;
              

                #region 超车



                Settings.OvertakeMaxDistance = Convert.ToInt32(edtTxtOvertakeMaxDistance.Text);
                Settings.OvertakeTimeout = Convert.ToInt32(edtTxtOvertakeTimeout.Text);
                Settings.OvertakeChangeLanesAngle = Convert.ToDouble(edtTxtOvertakeChangeLanesAngle.Text);
                Settings.OvertakeLowestSpeed = Convert.ToInt32(edtTxtOvertakeLowestSpeed.Text);
                Settings.OvertakeSpeedLimit = Convert.ToInt32(edtTxtOvertakeSpeedLimit.Text);
                Settings.OvertakeSpeedOnce= Convert.ToInt32(edtTxtOvertakeSpeedOnce.Text);
                Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance = Convert.ToInt32(edtTxtOvertakeBackToOriginalLaneDistance.Text);
                Settings.OvertakeBackToOriginalLaneVocie = edtTxtOvertakeBackToOriginalLaneVoice.Text;
                Settings.OvertakeLowAndHighBeamCheck = chkOvertakeLowAndHighBeamCheck.Checked;
                Settings.OvertakeLightCheck = chkOvertakeLightCheck.Checked;
                Settings.OvertakeLoudSpeakerDayCheck = chkOvertakeLoudSpeakerDayCheck.Checked;
                Settings.OvertakeLoudSpeakerNightCheck = chkOvertakeLoudSpeakerNightCheck.Checked;
                Settings.OvertakeBackToOriginalLane = chkOvertakeBackToOriginalLane.Checked;
                Settings.OvertakePrepareDistance = Convert.ToInt32(edtTxtOverTakePrepareDistance.Text);
                #endregion

                #region listSetting
                List<Setting> lstSetting = new List<Setting>
                { 
                    #region 超车
new Setting { Key ="OvertakeMaxDistance", Value = Settings.OvertakeMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeTimeout", Value = Settings.OvertakeTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeChangeLanesAngle", Value = Settings.OvertakeChangeLanesAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLowestSpeed", Value = Settings.OvertakeLowestSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeSpeedLimit", Value = Settings.OvertakeSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeSpeedOnce", Value = Settings.OvertakeSpeedOnce.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance", Value = Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeBackToOriginalLaneVocie", Value = Settings.OvertakeBackToOriginalLaneVocie.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLowAndHighBeamCheck", Value = Settings.OvertakeLowAndHighBeamCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLightCheck", Value = Settings.OvertakeLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLoudSpeakerDayCheck", Value = Settings.OvertakeLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLoudSpeakerNightCheck", Value = Settings.OvertakeLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeBackToOriginalLane", Value = Settings.OvertakeBackToOriginalLane.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakePrepareDistance", Value = Settings.OvertakePrepareDistance.ToString(), GroupName = "GlobalSettings" },
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