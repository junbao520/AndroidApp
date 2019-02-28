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
    [Activity(Label = "PullOverActivity")]
    public class PullOverActivity: BaseSettingActivity
    {

        #region 变量定义
        //

        #region 靠边停车
        // 起步闯动次数
        EditText edtTxtStartShockCount;
        // 靠边停车项目语音
        EditText edtTxtPullOverVoice;
        // 靠边停车-最大行驶距离（米）
        EditText edtTxtPullOverMaxDrivingDistance;
        //靠边停车，变道角度
        EditText edtTxtPullOverAngle;

        //靠边停车 开车门时间 0 不评判
        EditText edtTxtPullOverOpenDoorTime;

        // 靠边停车-停车后未拉手刹超时（单位：秒）
        EditText edtTxtPullOverHandbrakeTimeout;
        //靠边停车之后 开光车门时间
        EditText edtTxtPullOverDoorTimeout;
        // 靠边停车-检测转向灯
        CheckBox chkPullOverTurnLightCheck;
        // 靠边停车-下车前开启警报灯
        CheckBox chkPullOverOpenCautionBeforeGetOff;
        // 靠边停车-停车后松开安全带
        CheckBox chkPullOverOpenSafetyBeltBeforeGetOff;
        // 停车空挡检测
        CheckBox chkPullOverNeutralCheck;
        // 靠边停车-停车后发动机熄火检测
        CheckBox chkPullOverStopEngineBeforeGetOff;
        // 靠边停车-下车前是否关闭大灯
        CheckBox chkPullOverCloseLowBeamBeforeGetOff;
        // 靠边停车-结束后自动触发结束考试
        CheckBox chkPulloverEndAutoTriggerStopExam;
        // 靠边停车-结束后自动触发起步项目
        CheckBox chkPulloverEndAutoTriggerStart;
        //停车标志手刹
        RadioButton radStopFlagHandBreak;
        //停车标志车停
        RadioButton radStopFlagCarStop;
        RadioButton radPullOverCautionLight;
        RadioButton radPullOverLowBeamCheck;
        RadioButton radPullOverEngineExtinction;
        RadioButton radPullOverSafetyBelt;
        RadioButton radPullOverOpenCloseDoor;
        RadioButton radPullOverHandbrake;
        RadioButton radPullOverOpenDoor;

        Button btnPulloverImage;
        #endregion

        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.pullover);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.PullOver;
            ActivityName = this.GetString(Resource.String.PullOverStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


    

        public override void InitSetting()
        {
           
            edtTxtPullOverVoice.Text =ItemVoice;

            #region 靠边停车
            edtTxtPullOverMaxDrivingDistance.Text = Settings.PullOverMaxDrivingDistance.ToString();
            edtTxtPullOverHandbrakeTimeout.Text = Settings.PullOverHandbrakeTimeout.ToString();
            edtTxtPullOverDoorTimeout.Text = Settings.PullOverDoorTimeout.ToString();
            chkPullOverTurnLightCheck.Checked = Settings.PullOverTurnLightCheck;
            chkPullOverOpenCautionBeforeGetOff.Checked = Settings.PullOverOpenCautionBeforeGetOff;
            chkPullOverOpenSafetyBeltBeforeGetOff.Checked = Settings.PullOverOpenSafetyBeltBeforeGetOff;
            chkPullOverNeutralCheck.Checked = Settings.PullOverNeutralCheck;
            chkPullOverStopEngineBeforeGetOff.Checked = Settings.PullOverStopEngineBeforeGetOff;
            chkPullOverCloseLowBeamBeforeGetOff.Checked = Settings.PullOverCloseLowBeamBeforeGetOff;
            chkPulloverEndAutoTriggerStopExam.Checked = Settings.PulloverEndAutoTriggerStopExam;
            chkPulloverEndAutoTriggerStart.Checked = Settings.PulloverEndAutoTriggerStart;
            radStopFlagHandBreak.Checked = Settings.PullOverMark == PullOverMark.Handbrake;
            radStopFlagCarStop.Checked = Settings.PullOverMark == PullOverMark.CarStop;

            radPullOverCautionLight.Checked = Settings.PullOverEndMark == PullOverEndMark.CautionLightCheck;
            radPullOverEngineExtinction.Checked = Settings.PullOverEndMark == PullOverEndMark.EngineExtinctionCheck;
            radPullOverHandbrake.Checked = Settings.PullOverEndMark == PullOverEndMark.Handbrake;
            radPullOverLowBeamCheck.Checked = Settings.PullOverEndMark == PullOverEndMark.LowBeamCheck;
            radPullOverOpenCloseDoor.Checked = Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck;
            radPullOverSafetyBelt.Checked = Settings.PullOverEndMark == PullOverEndMark.SafetyBeltCheck;
            radPullOverOpenDoor.Checked = Settings.PullOverEndMark == PullOverEndMark.OpenDoorCheck;

            edtTxtPullOverAngle.Text = Settings.PulloverAngle.ToString();
            edtTxtPullOverOpenDoorTime.Text = Settings.PullOverOpenDoorTime.ToString();
            #endregion



        }

        public void InitControl()
        {
           
            #region 靠边停车
            edtTxtPullOverVoice = FindViewById<EditText>(Resource.Id.edtTxtPullOverVoice);
            edtTxtPullOverDoorTimeout = FindViewById<EditText>(Resource.Id.edtTxtPullOverDoorTimeout);
            edtTxtPullOverMaxDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtPullOverMaxDrivingDistance);
            edtTxtPullOverAngle = FindViewById<EditText>(Resource.Id.edtTxtPullOverAngle);

            edtTxtPullOverOpenDoorTime= FindViewById<EditText>(Resource.Id.edtTxtPullOverOpenDoorTime);
            edtTxtPullOverHandbrakeTimeout = FindViewById<EditText>(Resource.Id.edtTxtPullOverHandbrakeTimeout);
            chkPullOverTurnLightCheck = FindViewById<CheckBox>(Resource.Id.chkPullOverTurnLightCheck);
            chkPullOverOpenCautionBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverOpenCautionBeforeGetOff);
            chkPullOverOpenSafetyBeltBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverOpenSafetyBeltBeforeGetOff);
            chkPullOverNeutralCheck = FindViewById<CheckBox>(Resource.Id.chkPullOverNeutralCheck);
            chkPullOverStopEngineBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverStopEngineBeforeGetOff);
            chkPullOverCloseLowBeamBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverCloseLowBeamBeforeGetOff);
            chkPulloverEndAutoTriggerStopExam = FindViewById<CheckBox>(Resource.Id.chkPulloverEndAutoTriggerStopExam);
            chkPulloverEndAutoTriggerStart = FindViewById<CheckBox>(Resource.Id.chkPulloverEndAutoTriggerStart);
            radStopFlagHandBreak = FindViewById<RadioButton>(Resource.Id.radStopFlagHandBreak);
            radStopFlagCarStop = FindViewById<RadioButton>(Resource.Id.radStopFlagCarStop);
            radPullOverCautionLight = FindViewById<RadioButton>(Resource.Id.radPullOverCautionLight);
            radPullOverLowBeamCheck = FindViewById<RadioButton>(Resource.Id.radPullOverLowBeamCheck);
            radPullOverEngineExtinction = FindViewById<RadioButton>(Resource.Id.radPullOverEngineExtinction);
            radPullOverSafetyBelt = FindViewById<RadioButton>(Resource.Id.radPullOverSafetyBelt);
            radPullOverOpenCloseDoor = FindViewById<RadioButton>(Resource.Id.radPullOverOpenCloseDoor);
            radPullOverHandbrake = FindViewById<RadioButton>(Resource.Id.radPullOverHandbrake);
            radPullOverOpenDoor= FindViewById<RadioButton>(Resource.Id.radPullOverOpenDoor);

            btnPulloverImage = FindViewById<Button>(Resource.Id.btnPulloverImage);
            btnPulloverImage.Click += btnPulloverImage_Click;
            #endregion


        }

        private void btnPulloverImage_Click(object sender,EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this,typeof(PulloverImageActivity));
            StartActivity(intent);
        }

        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
                ItemVoice = edtTxtPullOverVoice.Text;
                #region 靠边停车
                Settings.PullOverMaxDrivingDistance = Convert.ToInt32(edtTxtPullOverMaxDrivingDistance.Text);
                Settings.PullOverHandbrakeTimeout = Convert.ToInt32(edtTxtPullOverHandbrakeTimeout.Text);
                Settings.PullOverDoorTimeout = Convert.ToInt32(edtTxtPullOverDoorTimeout.Text);
                Settings.PullOverTurnLightCheck = chkPullOverTurnLightCheck.Checked;
                Settings.PullOverOpenCautionBeforeGetOff = chkPullOverOpenCautionBeforeGetOff.Checked;
                Settings.PullOverOpenSafetyBeltBeforeGetOff = chkPullOverOpenSafetyBeltBeforeGetOff.Checked;
                Settings.PullOverNeutralCheck = chkPullOverNeutralCheck.Checked;
                Settings.PullOverStopEngineBeforeGetOff = chkPullOverStopEngineBeforeGetOff.Checked;
                Settings.PullOverCloseLowBeamBeforeGetOff = chkPullOverCloseLowBeamBeforeGetOff.Checked;
                Settings.PulloverEndAutoTriggerStopExam = chkPulloverEndAutoTriggerStopExam.Checked;
                Settings.PulloverEndAutoTriggerStart = chkPulloverEndAutoTriggerStart.Checked;

                Settings.PulloverAngle = Convert.ToDouble(edtTxtPullOverAngle.Text);
                Settings.PullOverOpenDoorTime = Convert.ToDouble(edtTxtPullOverOpenDoorTime.Text);

                if (radStopFlagHandBreak.Checked)
                {
                    Settings.PullOverMark = PullOverMark.Handbrake;
                }
                else if (radStopFlagCarStop.Checked)
                {
                    Settings.PullOverMark = PullOverMark.CarStop;
                }
                if (radPullOverCautionLight.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.CautionLightCheck;
                }
                else if (radPullOverEngineExtinction.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.EngineExtinctionCheck;
                }
                else if (radPullOverHandbrake.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.Handbrake;
                }
                else if (radPullOverLowBeamCheck.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.LowBeamCheck;
                }
                else if (radPullOverOpenCloseDoor.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.OpenCloseDoorCheck;
                }
                else if (radPullOverSafetyBelt.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.SafetyBeltCheck;
                }
                else if (radPullOverOpenDoor.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.OpenDoorCheck;
                }
                #endregion

                #region listSetting
                List<Setting> lstSetting = new List<Setting>
                {
new Setting { Key ="PullOverMaxDrivingDistance", Value = Settings.PullOverMaxDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverHandbrakeTimeout", Value = Settings.PullOverHandbrakeTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverDoorTimeout", Value = Settings.PullOverDoorTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverTurnLightCheck", Value = Settings.PullOverTurnLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverOpenCautionBeforeGetOff", Value = Settings.PullOverOpenCautionBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverOpenSafetyBeltBeforeGetOff", Value = Settings.PullOverOpenSafetyBeltBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverNeutralCheck", Value = Settings.PullOverNeutralCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverStopEngineBeforeGetOff", Value = Settings.PullOverStopEngineBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverCloseLowBeamBeforeGetOff", Value = Settings.PullOverCloseLowBeamBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PulloverEndAutoTriggerStopExam", Value = Settings.PulloverEndAutoTriggerStopExam.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverEndMark", Value = Settings.PullOverEndMark.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverMark", Value = Settings.PullOverMark.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PulloverEndAutoTriggerStart", Value = Settings.PulloverEndAutoTriggerStart.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PulloverAngle", Value = Settings.PulloverAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverOpenDoorTime", Value = Settings.PullOverOpenDoorTime.ToString(), GroupName = "GlobalSettings" },

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