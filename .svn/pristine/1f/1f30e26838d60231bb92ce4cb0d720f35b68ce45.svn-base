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
    [Activity(Label = "CommonExamItemActivity")]
    public class CommonExamItemActivity : BaseSettingActivity
    {

        #region 变量定义
        //

        #region 综合评判
        CheckBox chkBreakVoice;
        CheckBox ChkbeforeSimionLightStartEngine;
        EditText edtTxtExamSuccess;
        EditText edtTxtExamFailed;
        EditText edtTxtCommonExamItemMaxSpeedKeepTime;
        EditText edtTxtStartExamVoice;
        EditText edtTxtEndExamVoice;
        EditText edtbeforeSimionLightStartEngineVoice;
        //启动发动机后延时，用于中控打火黑屏
        EditText edtTxtStartEngineDeleyTime;

        CheckBox chkCommonExamItemsCheckChangeLanes;
        CheckBox chkCommonExamItemsCheckHandBreake;
        EditText edtTxtCommonExamItemsChangeLanesAngle;
        EditText edtTxtCommonExamItemsChangeLanesTimeOut;

        CheckBox chkBrakeMustInitem;

        //考试失败播放扣分规则语音
        //CheckBox chkExamFailPlayBrekeRule;

        //优先播放不合格语音
        CheckBox chkFirstPlayExamFailVoice;

        CheckBox chkPlayFail;
        //IsEnableBrakeIrregularity
        CheckBox chkBrakeIrregularity;

        EditText edtTxtBrakeIrregularitySignal;
        EditText edtTxtBrakeIrregularitySpeed;
        EditText edtTxtBrakeIrregularitySpeedZero;
        EditText edtTxtBrakeIrregularitySpeedOver;

        #endregion


        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.commonexamitem);

            InitControl();
            initHeader();
            ActivityName = this.GetString(Resource.String.CommonExamItemStr);
            setMyTitle(ActivityName);
            InitSetting();
        }




        public override void InitSetting()
        {

            edtTxtStartExamVoice.Text = Settings.CommonExamStarExamVoice;
            edtTxtEndExamVoice.Text = Settings.CommonExamEndExamVoice;
            chkBreakVoice.Checked = Settings.CommonExamItemsBreakVoice;
            edtTxtExamFailed.Text = Settings.CommonExamItemExamFailVoice;
            edtTxtExamSuccess.Text = Settings.CommonExamItemExamSuccessVoice;
            edtTxtCommonExamItemMaxSpeedKeepTime.Text = Settings.CommonExamItemsMaxSpeedKeepTime.ToString();

            ChkbeforeSimionLightStartEngine.Checked = Settings.CommonExambeforeSimionLightStartEngine;
            edtbeforeSimionLightStartEngineVoice.Text = Settings.CommonExambeforeSimionLightStartEngineVoice;

            edtTxtStartEngineDeleyTime.Text = Settings.CommonStartEngineDeleyTime.ToString();

            //全程变道
            chkCommonExamItemsCheckHandBreake.Checked = Settings.CommonExamItemsCheckHandBreake;
            chkCommonExamItemsCheckChangeLanes.Checked = Settings.CommonExamItemsCheckChangeLanes;
            edtTxtCommonExamItemsChangeLanesAngle.Text = Settings.CommonExamItemsChangeLanesAngle.ToString();
            edtTxtCommonExamItemsChangeLanesTimeOut.Text = Settings.CommonExamItemsChangeLanesTimeOut.ToString();

            chkBrakeMustInitem.Checked = Settings.CheckBrakeMustInitem;
            chkFirstPlayExamFailVoice.Checked = Settings.FirstPlayExamFailVoice;
            chkPlayFail.Checked = Settings.PlayFail;
            chkBrakeIrregularity.Checked = Settings.IsEnableBrakeIrregularity;

            edtTxtBrakeIrregularitySignal.Text = Settings.BrakeIrregularitySignal.ToString();
            edtTxtBrakeIrregularitySpeed.Text = Settings.BrakeIrregularitySpeed.ToString();
            edtTxtBrakeIrregularitySpeedZero.Text = Settings.BrakeIrregularitySpeedZero.ToString();
            edtTxtBrakeIrregularitySpeedOver.Text = Settings.BrakeIrregularitySpeedOver.ToString();
        }

        public void InitControl()
        {

            #region 综合评判


            //综合评判

            edtbeforeSimionLightStartEngineVoice = FindViewById<EditText>(Resource.Id.edtTxtBeforeSimionLightStartEngineVoice);
            ChkbeforeSimionLightStartEngine = FindViewById<CheckBox>(Resource.Id.chkBeforeSimionLightStartEngine);
            chkBreakVoice = FindViewById<CheckBox>(Resource.Id.chkBreakVoice);
            edtTxtExamSuccess = FindViewById<EditText>(Resource.Id.edtTxtExamSuccess);
            edtTxtExamFailed = FindViewById<EditText>(Resource.Id.edtTxtExamFailed);
            edtTxtCommonExamItemMaxSpeedKeepTime = FindViewById<EditText>(Resource.Id.edtTxtCommonExamItemMaxSpeedKeepTime);
            edtTxtStartExamVoice = FindViewById<EditText>(Resource.Id.edtTxtStartExamVoice);
            edtTxtEndExamVoice = FindViewById<EditText>(Resource.Id.edtTxtEndExamVoice);

            edtTxtStartEngineDeleyTime = FindViewById<EditText>(Resource.Id.edtTxtStartEngineDeleyTime);

            chkCommonExamItemsCheckHandBreake = FindViewById<CheckBox>(Resource.Id.chkCommonExamItemsCheckHandBreake);
            chkCommonExamItemsCheckChangeLanes = FindViewById<CheckBox>(Resource.Id.chkCommonExamItemsCheckChangeLanes);
            edtTxtCommonExamItemsChangeLanesTimeOut = FindViewById<EditText>(Resource.Id.edtTxtCommonExamItemsChangeLanesTimeOut);
            edtTxtCommonExamItemsChangeLanesAngle = FindViewById<EditText>(Resource.Id.edtTxtCommonExamItemsChangeLanesAngle);
            chkBrakeMustInitem = FindViewById<CheckBox>(Resource.Id.chkBrakeMustInitem);

            chkFirstPlayExamFailVoice = FindViewById<CheckBox>(Resource.Id.chkFirstPlayExamFailVoice);


            chkPlayFail = FindViewById<CheckBox>(Resource.Id.chkPlayFail);

            chkBrakeIrregularity = FindViewById<CheckBox>(Resource.Id.chkBrakeIrregularity);

            edtTxtBrakeIrregularitySignal = FindViewById<EditText>(Resource.Id.edtTxtBrakeIrregularitySignal);
            edtTxtBrakeIrregularitySpeed = FindViewById<EditText>(Resource.Id.edtTxtBrakeIrregularitySpeed);
            edtTxtBrakeIrregularitySpeedZero = FindViewById<EditText>(Resource.Id.edtTxtBrakeIrregularitySpeedZero);
            edtTxtBrakeIrregularitySpeedOver = FindViewById<EditText>(Resource.Id.edtTxtBrakeIrregularitySpeedOver);
            //  chkExamFailPlayBrekeRule= FindViewById<CheckBox>(Resource.Id.chkExamFailPlayBrekeRule);
            #endregion
        }


        public override void UpdateSettings()
        {
            try
            {
                #region 综合评判
                Settings.CommonExamItemExamFailVoice = edtTxtExamFailed.Text;
                Settings.CommonExamItemExamSuccessVoice = edtTxtExamSuccess.Text;
                Settings.CommonExamItemsBreakVoice = chkBreakVoice.Checked;
                Settings.CommonExamItemsMaxSpeedKeepTime = Convert.ToDouble(edtTxtCommonExamItemMaxSpeedKeepTime.Text);

                Settings.CommonExamStarExamVoice = edtTxtStartExamVoice.Text;
                Settings.CommonExamEndExamVoice = edtTxtEndExamVoice.Text;

                Settings.CommonExambeforeSimionLightStartEngine = ChkbeforeSimionLightStartEngine.Checked;
                Settings.CommonExambeforeSimionLightStartEngineVoice = edtbeforeSimionLightStartEngineVoice.Text;

                Settings.CommonStartEngineDeleyTime = Convert.ToInt32(edtTxtStartEngineDeleyTime.Text);


                Settings.CommonExamItemsChangeLanesTimeOut = Convert.ToDouble(edtTxtCommonExamItemsChangeLanesTimeOut.Text);
                Settings.CommonExamItemsChangeLanesAngle = Convert.ToDouble(edtTxtCommonExamItemsChangeLanesAngle.Text);
                Settings.CommonExamItemsCheckChangeLanes = chkCommonExamItemsCheckChangeLanes.Checked;
                Settings.CommonExamItemsCheckHandBreake = chkCommonExamItemsCheckHandBreake.Checked;

                Settings.CheckBrakeMustInitem = chkBrakeMustInitem.Checked;

                Settings.FirstPlayExamFailVoice = chkFirstPlayExamFailVoice.Checked;
                Settings.PlayFail = chkPlayFail.Checked;
                Settings.IsEnableBrakeIrregularity = chkBrakeIrregularity.Checked;

                Settings.BrakeIrregularitySignal = Convert.ToInt32(edtTxtBrakeIrregularitySignal.Text);
                Settings.BrakeIrregularitySpeed = Convert.ToInt32(edtTxtBrakeIrregularitySpeed.Text);
                Settings.BrakeIrregularitySpeedZero = Convert.ToInt32(edtTxtBrakeIrregularitySpeedZero.Text);
                Settings.BrakeIrregularitySpeedOver = Convert.ToInt32(edtTxtBrakeIrregularitySpeedOver.Text);
                #endregion
                #region listSetting
                List<Setting> lstSetting = new List<Setting>
                {
                    #region 综合评判
new Setting { Key ="CommonExamItemExamFailVoice", Value = Settings.CommonExamItemExamFailVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemExamSuccessVoice", Value = Settings.CommonExamItemExamSuccessVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsBreakVoice", Value = Settings.CommonExamItemsBreakVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsMaxSpeedKeepTime", Value = Settings.CommonExamItemsMaxSpeedKeepTime.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamStarExamVoice", Value = Settings.CommonExamStarExamVoice, GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamEndExamVoice", Value = Settings.CommonExamEndExamVoice, GroupName = "GlobalSettings" },
new Setting { Key ="CommonExambeforeSimionLightStartEngine", Value = Settings.CommonExambeforeSimionLightStartEngine.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExambeforeSimionLightStartEngineVoice", Value = Settings.CommonExambeforeSimionLightStartEngineVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonStartEngineDeleyTime", Value = Settings.CommonStartEngineDeleyTime.ToString(), GroupName = "GlobalSettings" },

new Setting { Key ="CommonExamItemsChangeLanesTimeOut", Value = Settings.CommonExamItemsChangeLanesTimeOut.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsChangeLanesAngle", Value = Settings.CommonExamItemsChangeLanesAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsCheckChangeLanes", Value = Settings.CommonExamItemsCheckChangeLanes.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsCheckHandBreake", Value = Settings.CommonExamItemsCheckHandBreake.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CheckBrakeMustInitem", Value = Settings.CheckBrakeMustInitem.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="FirstPlayExamFailVoice", Value =Settings.FirstPlayExamFailVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PlayFail", Value =Settings.PlayFail.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IsEnableBrakeIrregularity", Value =Settings.IsEnableBrakeIrregularity.ToString(), GroupName = "GlobalSettings" },

new Setting { Key ="BrakeIrregularitySignal", Value =Settings.BrakeIrregularitySignal.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeIrregularitySpeed", Value =Settings.BrakeIrregularitySpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeIrregularitySpeedZero", Value =Settings.BrakeIrregularitySpeedZero.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeIrregularitySpeedOver", Value =Settings.BrakeIrregularitySpeedOver.ToString(), GroupName = "GlobalSettings" },

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