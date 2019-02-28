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
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "GlobalSetting")]
    public class GlobalSetting : BaseSettingActivity
    {

        #region 定义控件变量
        //考试里程
        EditText edtTxtExamDistance;
        //夜考里程
        EditText edtTxtNightDistance;
        //偏离行驶方向
        EditText edtTxtMaxFromEdgeDistance;
        //转向灯提前
        EditText edtTxtTurnLightAheadOfTime;
        //转向灯超时
        EditText edtTxtIndicatorLightTimeout;
        //转向灯超距
        EditText edtTxtIndicatorLightDistacneout;
        //发动机怠速
        EditText edtTxtMinEngineRpm;
        //发动机最高转速
        EditText edtTxtMaxEngineRpm;

        EditText edtTxtMaxEngineRpmTime;
        //启动最低速度
        EditText edtTxtCarStartLowSpeed;
        //发动机熄火转速
        EditText edtTxtEngineStopRmp;
        //刹车维持时间
        EditText edtTxtBrakeKeepTime;
        //空挡滑行距离
        EditText edtTxtNeutralTaxiingMaxDistance;
        //空挡滑行时间
        EditText edtTxtNeutralTaxiingMaxTime;
        //全程限速
        EditText edtTxtMaxSpeedSpeed;
        //达到一次速度
        EditText edtTxtReachSpeed;
        //达到速度时间
        EditText edtTxtReachSpeedKeepTime;
        //达到速度距离
        EditText edtTxtReachSpeedKeepDistance;
        //离合限制时间
        EditText edtTxtClutchTaxiingTimeout;
        //制动不平顺
        EditText edtTxtBrakeNotRide;
        //一档最高速度
        EditText edtTxtGearOneMaxSpeed;
        //一档限制时间
        EditText edtTxtGearOneTimeOut;
        //一档限制距离
        EditText edtTxtGearOneDistanceOut;
        //二挡最低速度
        EditText edtTxtGearTwoLowestSpeed;
        //二挡最高速度
        EditText edtTxtGearTwoMaxSpeed;
        //二挡限制时间
        EditText edtTxtGearTwoTimeOut;
        //二挡限制距离
        EditText edtTxtGearTwoDistanceOut;
        //三挡最低速度
        EditText edtTxtGearThreeLowestSpeed;
        //三挡最高速度
        EditText edtTxtGearThreeMaxSpeed;
        //3挡限制距离
        EditText edtTxtGearThreeDistanceOut;
        //四挡最低速度
        EditText edtTxtGearFourLowestSpeed;
        //四挡最高速度
        EditText edtTxtGearFourMaxSpeed;
        //全程档位要求
        EditText edtTxtGlobalContinuousGear;
        //档位达到速度
        EditText edtTxtGlobalContinuousSpeed;
        //挡位持续时间
        EditText edtTxtGlobalContinuousSeconds;
        //挡位速度不匹配
        EditText edtTxtSpeedAndGearTimeout;
        //日志输出
        CheckBox chkLogEnable;
        //手动挡自动挡
        CheckBox chkLicenseType;
        //不合格继续考试
        CheckBox chkContinueExamIfFailed;

        //触发规则语音
        CheckBox chkVoiceBrokenRule;
        //空挡启动发动机
        CheckBox chkNeutralStart;

        //考试自动结束
        CheckBox chkEndExamByDistance;

        //靠边停车达到项目启动
        CheckBox chkPullOverStartFlag;

        //是否显示公司Logo
        CheckBox chkIsShowCompanyLogo;

        //是否播放叮语音
        CheckBox chkIsPlayDingVoice;

        EditText edtTxtEngineTimeOut;
        EditText edtTxtSafetyBeltTimeOut;

        EditText edtTxtClutchSpeed;

        EditText edtTxtClutchLimitDistance;

        EditText edtTxtGlobalLowestGear;

        CheckBox chkShowStudentInfo;

        EditText edtTxtVoicePlayInterval;




        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GlobalSetting);
            InitControl();
            initHeader();
            ActivityName = ActivityName = this.GetString(Resource.String.GlobalSettingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


        public override void InitSetting()
        {

            chkPullOverStartFlag.Checked = Settings.PullOverStartFlage;
            edtTxtExamDistance.Text = Settings.ExamDistance.ToString();
            edtTxtNightDistance.Text = Settings.NightDistance.ToString();
            edtTxtMaxFromEdgeDistance.Text = Settings.MaxFromEdgeDistance.ToString();


            //TrunLightAheadOfTime
            Logger.Info("转向灯提前:" + Settings.TurnLightAheadOfTime.ToString());

            edtTxtTurnLightAheadOfTime.Text = Settings.TurnLightAheadOfTime.ToString();


            edtTxtIndicatorLightTimeout.Text = Settings.IndicatorLightTimeout.ToString();
            edtTxtIndicatorLightDistacneout.Text = Settings.IndicatorLightDistanceout.ToString();
            edtTxtMinEngineRpm.Text = Settings.MinEngineRpm.ToString();
            edtTxtMaxEngineRpm.Text = Settings.MaxEngineRpm.ToString();
            edtTxtMaxEngineRpmTime.Text = Settings.MaxRpmTime.ToString();
            edtTxtCarStartLowSpeed.Text = Settings.ParkingValueKmh.ToString();
            edtTxtEngineStopRmp.Text = Settings.EngineStopRmp.ToString();
            edtTxtBrakeKeepTime.Text = Settings.BrakeKeepTime.ToString();
            edtTxtNeutralTaxiingMaxDistance.Text = Settings.NeutralTaxiingMaxDistance.ToString();
            edtTxtNeutralTaxiingMaxTime.Text = Settings.NeutralTaxiingTimeout.ToString();
            edtTxtMaxSpeedSpeed.Text = Settings.MaxSpeed.ToString();
            edtTxtReachSpeed.Text = Settings.GlobalLowestSpeed.ToString();
            edtTxtReachSpeedKeepTime.Text = Settings.GlobalLowestSpeedHoldTimeSeconds.ToString();
            edtTxtReachSpeedKeepDistance.Text = Settings.GlobalLowestSpeedHoldDistince.ToString();
            edtTxtClutchTaxiingTimeout.Text = Settings.ClutchTaxiingTimeout.ToString();
            edtTxtBrakeNotRide.Text = Settings.BrakeNotRide.ToString();
            edtTxtGearOneMaxSpeed.Text = Settings.GearOneMaxSpeed.ToString();
            edtTxtGearOneTimeOut.Text = Settings.GearOneTimeout.ToString();//GearOneTimeOut
            edtTxtGearOneDistanceOut.Text = Settings.GearOneDrivingDistance.ToString();
            edtTxtGearTwoLowestSpeed.Text = Settings.GearTwoMinSpeed.ToString();
            edtTxtGearTwoMaxSpeed.Text = Settings.GearTwoMaxSpeed.ToString();

            edtTxtGearThreeLowestSpeed.Text = Settings.GearThreeMinSpeed.ToString();
            edtTxtGearThreeMaxSpeed.Text = Settings.GearThreeMaxSpeed.ToString();


            edtTxtGearFourLowestSpeed.Text = Settings.GearFourMinSpeed.ToString();
            edtTxtGearFourMaxSpeed.Text = Settings.GearFourMaxSpeed.ToString();

            edtTxtGearTwoTimeOut.Text = Settings.GearTwoTimeout.ToString();
            edtTxtGearTwoDistanceOut.Text = Settings.GearTwoDrivingDistance.ToString();
            edtTxtGearThreeDistanceOut.Text= Settings.GearThreeDrivingDistance.ToString();

            edtTxtGearThreeLowestSpeed.Text = Settings.GearThreeMinSpeed.ToString();
            edtTxtGearThreeMaxSpeed.Text = Settings.GearThreeMaxSpeed.ToString();

            edtTxtGearFourLowestSpeed.Text = Settings.GearFourMinSpeed.ToString();
            edtTxtGearFourMaxSpeed.Text = Settings.GearFourMaxSpeed.ToString();
            edtTxtGlobalContinuousGear.Text = ((int)Settings.GlobalContinuousGear).ToString();
            edtTxtGlobalContinuousSpeed.Text = Settings.GlobalContinuousSpeed.ToString();
            edtTxtGlobalContinuousSeconds.Text = Settings.GlobalContinuousSeconds.ToString();
            edtTxtSpeedAndGearTimeout.Text = Settings.SpeedAndGearTimeout.ToString();

            chkLogEnable.Checked = Settings.GpsLogEnable;

            edtTxtClutchSpeed.Text = Settings.ClutchTaxiingSpeedLimit.ToString();
            edtTxtClutchLimitDistance.Text = Settings.ClutchTaxiingDistance.ToString();
            //Ture
            //False
            chkLicenseType.Checked = Settings.LicenseC1;
            chkLicenseType.Checked = !Settings.LicenseC2;

            chkVoiceBrokenRule.Checked = Settings.VoiceBrokenRule;
            chkNeutralStart.Checked = Settings.NeutralStart;
            chkEndExamByDistance.Checked = Settings.EndExamByDistance;

            edtTxtSafetyBeltTimeOut.Text = Settings.CommonExamItemsSareBeltTimeOut.ToString();
            edtTxtEngineTimeOut.Text = Settings.CommonExamItemsEngineTimeOut.ToString();
            chkContinueExamIfFailed.Checked = Settings.ContinueExamIfFailed;

            chkIsShowCompanyLogo.Checked = Settings.IsShowCompanyLogo;
            chkIsPlayDingVoice.Checked = Settings.IsPlayDingVoice;

            edtTxtGlobalLowestGear.Text = Settings.GlobalLowestGear.ToString();

            chkShowStudentInfo.Checked = Settings.ShowStudentInfo;

            edtTxtVoicePlayInterval.Text = Settings.VoicePlayInterval.ToString();




        }


        public override void UpdateSettings()
        {

            try
            {
                Settings.PullOverStartFlage = chkPullOverStartFlag.Checked;
                Settings.ExamDistance = Convert.ToInt32(edtTxtExamDistance.Text);
                Settings.NightDistance = Convert.ToInt32(edtTxtNightDistance.Text);
                Settings.MaxFromEdgeDistance = Convert.ToInt32(edtTxtMaxFromEdgeDistance.Text);
                Settings.TurnLightAheadOfTime = Convert.ToDouble(edtTxtTurnLightAheadOfTime.Text);
                Settings.IndicatorLightTimeout = Convert.ToInt32(edtTxtIndicatorLightTimeout.Text);
                Settings.IndicatorLightDistanceout = Convert.ToInt32(edtTxtIndicatorLightDistacneout.Text);
                Settings.MinEngineRpm = Convert.ToInt32(edtTxtMinEngineRpm.Text);
                Settings.MaxEngineRpm = Convert.ToInt32(edtTxtMaxEngineRpm.Text);
                Settings.MaxRpmTime= Convert.ToInt32(edtTxtMaxEngineRpmTime.Text);
                Settings.ParkingValueKmh = Convert.ToDouble(edtTxtCarStartLowSpeed.Text);
                Settings.EngineStopRmp = Convert.ToInt32(edtTxtEngineStopRmp.Text);
                Settings.BrakeKeepTime = Convert.ToInt32(edtTxtBrakeKeepTime.Text);
                Settings.NeutralTaxiingMaxDistance = Convert.ToInt32(edtTxtNeutralTaxiingMaxDistance.Text);
                Settings.NeutralTaxiingTimeout = Convert.ToInt32(edtTxtNeutralTaxiingMaxTime.Text);
                Settings.MaxSpeed = Convert.ToInt32(edtTxtMaxSpeedSpeed.Text);
                Settings.GlobalLowestSpeed = Convert.ToInt32(edtTxtReachSpeed.Text);
                Settings.GlobalLowestSpeedHoldTimeSeconds = Convert.ToInt32(edtTxtReachSpeedKeepTime.Text);
                Settings.GlobalLowestSpeedHoldDistince = Convert.ToInt32(edtTxtReachSpeedKeepDistance.Text);
                Settings.ClutchTaxiingTimeout = Convert.ToInt32(edtTxtClutchTaxiingTimeout.Text);
                Settings.BrakeNotRide = Convert.ToDouble(edtTxtBrakeNotRide.Text);
                Settings.GearOneMaxSpeed = Convert.ToInt32(edtTxtGearOneMaxSpeed.Text);
                Settings.GearOneTimeout = Convert.ToInt32(edtTxtGearOneTimeOut.Text);
                Settings.GearOneDrivingDistance = Convert.ToInt32(edtTxtGearOneDistanceOut.Text);
                Settings.GearTwoMinSpeed = Convert.ToInt32(edtTxtGearTwoLowestSpeed.Text);
                Settings.GearTwoMaxSpeed = Convert.ToInt32(edtTxtGearTwoMaxSpeed.Text);
                Settings.GearTwoTimeout = Convert.ToInt32(edtTxtGearTwoTimeOut.Text);
                Settings.GearTwoDrivingDistance = Convert.ToInt32(edtTxtGearTwoDistanceOut.Text);

                Settings.GearThreeDrivingDistance = Convert.ToInt32(edtTxtGearThreeDistanceOut.Text);
                Settings.GearThreeMinSpeed = Convert.ToInt32(edtTxtGearThreeLowestSpeed.Text);
                Settings.GearThreeMaxSpeed = Convert.ToInt32(edtTxtGearThreeMaxSpeed.Text);
                Settings.GearFourMinSpeed = Convert.ToInt32(edtTxtGearFourLowestSpeed.Text);
                Settings.GearFourMaxSpeed = Convert.ToInt32(edtTxtGearFourMaxSpeed.Text);
                Settings.GlobalContinuousGear = (Gear)Convert.ToInt32(edtTxtGlobalContinuousGear.Text);
                Settings.GlobalContinuousSpeed = Convert.ToInt32(edtTxtGlobalContinuousSpeed.Text);
                Settings.GlobalContinuousSeconds = Convert.ToInt32(edtTxtGlobalContinuousSeconds.Text);
                Settings.SpeedAndGearTimeout = Convert.ToInt32(edtTxtSpeedAndGearTimeout.Text);
                Settings.GpsLogEnable = chkLogEnable.Checked;

                Settings.LicenseC1 = chkLicenseType.Checked;
                Settings.LicenseC2 = !chkLicenseType.Checked;
                Settings.VoiceBrokenRule = chkVoiceBrokenRule.Checked;
                Settings.NeutralStart = chkNeutralStart.Checked;
                Settings.EndExamByDistance = chkEndExamByDistance.Checked;
                Settings.CommonExamItemsEngineTimeOut = Convert.ToDouble(edtTxtEngineTimeOut.Text);
                Settings.CommonExamItemsSareBeltTimeOut = Convert.ToDouble(edtTxtSafetyBeltTimeOut.Text);

                Settings.ContinueExamIfFailed = chkContinueExamIfFailed.Checked;

                Settings.IsPlayDingVoice = chkIsPlayDingVoice.Checked;
                Settings.IsShowCompanyLogo = chkIsShowCompanyLogo.Checked;

                Settings.ClutchTaxiingSpeedLimit = Convert.ToInt32(edtTxtClutchSpeed.Text);

                Settings.ClutchTaxiingDistance = Convert.ToInt32(edtTxtClutchLimitDistance.Text);

                Settings.GlobalLowestGear = Convert.ToInt32(edtTxtGlobalLowestGear.Text);

                Settings.ShowStudentInfo = chkShowStudentInfo.Checked;

                Settings.VoicePlayInterval = Convert.ToInt32(edtTxtVoicePlayInterval.Text) ;

                List <Setting> lstSetting = new List<Setting>
                {
new Setting { Key ="PullOverStartFlage", Value = Settings.PullOverStartFlage.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ExamDistance", Value = Settings.ExamDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="NightDistance", Value = Settings.NightDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MaxFromEdgeDistance", Value = Settings.MaxFromEdgeDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLightAheadOfTime", Value = Settings.TurnLightAheadOfTime.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IndicatorLightTimeout", Value = Settings.IndicatorLightTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IndicatorLightDistanceout", Value = Settings.IndicatorLightDistanceout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MinEngineRpm", Value = Settings.MinEngineRpm.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MaxEngineRpm", Value = Settings.MaxEngineRpm.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MaxRpmTime", Value = Settings.MaxRpmTime.ToString(), GroupName = "GlobalSettings" },

new Setting { Key ="ParkingValueKmh", Value = Settings.ParkingValueKmh.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="EngineStopRmp", Value = Settings.EngineStopRmp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeKeepTime", Value = Settings.BrakeKeepTime.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="NeutralTaxiingMaxDistance", Value = Settings.NeutralTaxiingMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="NeutralTaxiingTimeout", Value = Settings.NeutralTaxiingTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MaxSpeed", Value = Settings.MaxSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalLowestSpeed", Value = Settings.GlobalLowestSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalLowestSpeedHoldTimeSeconds", Value = Settings.GlobalLowestSpeedHoldTimeSeconds.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalLowestSpeedHoldDistince", Value = Settings.GlobalLowestSpeedHoldDistince.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ClutchTaxiingTimeout", Value = Settings.ClutchTaxiingTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeNotRide", Value = Settings.BrakeNotRide.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearOneMaxSpeed", Value = Settings.GearOneMaxSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearOneTimeout", Value = Settings.GearOneTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearOneDrivingDistance", Value = Settings.GearOneDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearTwoMinSpeed", Value = Settings.GearTwoMinSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearTwoMaxSpeed", Value = Settings.GearTwoMaxSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearTwoTimeout", Value = Settings.GearTwoTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearTwoDrivingDistance", Value = Settings.GearTwoDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearThreeMinSpeed", Value = Settings.GearThreeMinSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearThreeMaxSpeed", Value = Settings.GearThreeMaxSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearFourMinSpeed", Value = Settings.GearFourMinSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearFourMaxSpeed", Value = Settings.GearFourMaxSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalContinuousGear", Value = Settings.GlobalContinuousGear.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalContinuousSpeed", Value = Settings.GlobalContinuousSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalContinuousSeconds", Value = Settings.GlobalContinuousSeconds.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SpeedAndGearTimeout", Value = Settings.SpeedAndGearTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GpsLogEnable", Value = Settings.GpsLogEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LicenseC1", Value = Settings.LicenseC1.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LicenseC2", Value = Settings.LicenseC2.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="VoiceBrokenRule", Value = Settings.VoiceBrokenRule.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="NeutralStart", Value = Settings.NeutralStart.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="EndExamByDistance", Value = Settings.EndExamByDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsEngineTimeOut", Value = Settings.CommonExamItemsEngineTimeOut.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsSareBeltTimeOut", Value = Settings.CommonExamItemsSareBeltTimeOut.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ContinueExamIfFailed", Value = Settings.ContinueExamIfFailed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IsPlayDingVoice", Value = Settings.IsPlayDingVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IsShowCompanyLogo", Value = Settings.IsShowCompanyLogo.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ClutchTaxiingSpeedLimit", Value = Settings.ClutchTaxiingSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ClutchTaxiingDistance", Value = Settings.ClutchTaxiingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GlobalLowestGear", Value = Settings.GlobalLowestGear.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ShowStudentInfo", Value = Settings.ShowStudentInfo.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="VoicePlayInterval", Value = Settings.VoicePlayInterval.ToString(), GroupName = "GlobalSettings" },
            };
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

        public void InitControl()
        {
            edtTxtExamDistance = FindViewById<EditText>(Resource.Id.edtTxtExamDistance);
            edtTxtNightDistance = FindViewById<EditText>(Resource.Id.edtTxtNightDistance);
            edtTxtMaxFromEdgeDistance = FindViewById<EditText>(Resource.Id.edtTxtMaxFromEdgeDistance);

            edtTxtTurnLightAheadOfTime = FindViewById<EditText>(Resource.Id.edtTxtTurnLightAheadOfTime);

            edtTxtIndicatorLightTimeout = FindViewById<EditText>(Resource.Id.edtTxtIndicatorLightTimeout);
            edtTxtIndicatorLightDistacneout = FindViewById<EditText>(Resource.Id.edtTxtIndicatorLightDistacneout);


            edtTxtMinEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtMinEngineRpm);
            edtTxtMaxEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtMaxEngineRpm);
            edtTxtMaxEngineRpmTime= FindViewById<EditText>(Resource.Id.edtTxtMaxEngineRpmTime);


            edtTxtCarStartLowSpeed = FindViewById<EditText>(Resource.Id.edtTxtCarStartLowSpeed);
            edtTxtEngineStopRmp = FindViewById<EditText>(Resource.Id.edtTxtEngineStopRmp);
            edtTxtBrakeKeepTime = FindViewById<EditText>(Resource.Id.edtTxtBrakeKeepTime);
            edtTxtNeutralTaxiingMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtNeutralTaxiingMaxDistance);
            edtTxtNeutralTaxiingMaxTime = FindViewById<EditText>(Resource.Id.edtTxtNeutralTaxiingMaxTime);
            edtTxtMaxSpeedSpeed = FindViewById<EditText>(Resource.Id.edtTxtMaxSpeedSpeed);
            edtTxtReachSpeed = FindViewById<EditText>(Resource.Id.edtTxtReachSpeed);
            edtTxtReachSpeedKeepTime = FindViewById<EditText>(Resource.Id.edtTxtReachSpeedKeepTime);
            edtTxtReachSpeedKeepDistance = FindViewById<EditText>(Resource.Id.edtTxtReachSpeedKeepDistance);
            edtTxtClutchTaxiingTimeout = FindViewById<EditText>(Resource.Id.edtTxtClutchTaxiingTimeout);
            edtTxtBrakeNotRide = FindViewById<EditText>(Resource.Id.edtTxtBrakeNotRide);
            edtTxtGearOneMaxSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearOneMaxSpeed);
            edtTxtGearOneTimeOut = FindViewById<EditText>(Resource.Id.edtTxtGearOneTimeOut);
            edtTxtGearOneDistanceOut = FindViewById<EditText>(Resource.Id.edtTxtGearOneDistanceOut);
            edtTxtGearTwoLowestSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearTwoLowestSpeed);
            edtTxtGearTwoMaxSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearTwoMaxSpeed);
            edtTxtGearTwoTimeOut = FindViewById<EditText>(Resource.Id.edtTxtGearTwoTimeOut);
            edtTxtGearTwoDistanceOut = FindViewById<EditText>(Resource.Id.edtTxtGearTwoDistanceOut);

            edtTxtGearThreeDistanceOut = FindViewById<EditText>(Resource.Id.edtTxtGearThreeDistanceOut);
            edtTxtGearThreeLowestSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearThreeLowestSpeed);
            edtTxtGearThreeMaxSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearThreeMaxSpeed);
            edtTxtGearFourLowestSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearFourLowestSpeed);
            edtTxtGearFourMaxSpeed = FindViewById<EditText>(Resource.Id.edtTxtGearFourMaxSpeed);
            edtTxtGlobalContinuousGear = FindViewById<EditText>(Resource.Id.edtTxtGlobalContinuousGear);
            edtTxtGlobalContinuousSpeed = FindViewById<EditText>(Resource.Id.edtTxtGlobalContinuousSpeed);
            edtTxtGlobalContinuousSeconds = FindViewById<EditText>(Resource.Id.edtTxtGlobalContinuousSeconds);
            edtTxtSpeedAndGearTimeout = FindViewById<EditText>(Resource.Id.edtTxtSpeedAndGearTimeout);

            chkLogEnable = FindViewById<CheckBox>(Resource.Id.chkLogEnable);

            chkLicenseType = FindViewById<CheckBox>(Resource.Id.chkLicenseType);

            chkContinueExamIfFailed = FindViewById<CheckBox>(Resource.Id.chkContinueExamIfFailed);



            chkVoiceBrokenRule = FindViewById<CheckBox>(Resource.Id.chkVoiceBrokenRule);

            chkNeutralStart = FindViewById<CheckBox>(Resource.Id.chkNeutralStart);

            chkEndExamByDistance = FindViewById<CheckBox>(Resource.Id.chkEndExamByDistance);

            edtTxtSafetyBeltTimeOut = FindViewById<EditText>(Resource.Id.edtTxtSafetyBeltTimeOut);
            edtTxtEngineTimeOut = FindViewById<EditText>(Resource.Id.edtTxtEngineTimeOut);

            chkPullOverStartFlag = FindViewById<CheckBox>(Resource.Id.chkPullOverStartFlag);


            chkIsShowCompanyLogo = FindViewById<CheckBox>(Resource.Id.chkIsShowCompanyLogo);
            chkIsPlayDingVoice = FindViewById<CheckBox>(Resource.Id.chkIsPlayDingVoice);


            edtTxtClutchSpeed = FindViewById<EditText>(Resource.Id.edtTxtClutchSpeed);
            edtTxtClutchLimitDistance = FindViewById<EditText>(Resource.Id.edtTxtClutchLimitDistance);

            //全程最低档位
            edtTxtGlobalLowestGear = FindViewById<EditText>(Resource.Id.edtTxtGlobalLowestGear);

            chkShowStudentInfo = FindViewById<CheckBox>(Resource.Id.chkShowStudentInfo);

            edtTxtVoicePlayInterval = FindViewById<EditText>(Resource.Id.edtTxtVoicePlayInterval);
        }

    }
}