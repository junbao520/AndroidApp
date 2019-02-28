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
    [Activity(Label = "ChangeLanesActivity")]
    public class ChangeLanesActivity : BaseSettingActivity
    {

        #region 变量定义
        #region 变更车道
        // 变更车道项目语音
        EditText edtTxtChangeLanesVoice;
        //第2个变道语音（有时播报两次语音）
        EditText edtTxtChangeLanesSecondVoice;
        // 变更结束车道
        EditText edtTxtChangeLanesEndVoice;
        // 变道最大距离（单位：米）
        EditText edtTxtChangeLanesMaxDistance;
        // 变道超时时间（单位：秒）
        EditText edtTxtChangeLanesTimeout;
        // 变道转向角度
        EditText edtTxtChangeLanesAngle;
        // 变更车道夜间远近光交替
        CheckBox chkChangeLanesLowAndHighBeamCheck;
        // 变更车道灯光检测
        CheckBox chkChangeLanesLightCheck;
        //变道操作完成就结束
        CheckBox chkChangeLanesEndFlag;
        // 变道准备距离
        EditText edtTxtChangeLanesPrepareDistance;

       RadioButton radChangelineBoth;
        RadioButton radChangelineLeft;
        RadioButton radChangelineRight;
        #endregion
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.changelanes);

            InitControl();
            initHeader();
            ActivityName = this.GetString(Resource.String.ChangeLanesStr);
            ItemCode = ExamItemCodes.ChangeLanes;
            setMyTitle(ActivityName);
            InitSetting();
        }


     

        public override void InitSetting()
        {
            #region 项目语音
            edtTxtChangeLanesVoice.Text = ItemVoice;
            edtTxtChangeLanesEndVoice.Text = ItemEndVoice;
            edtTxtChangeLanesSecondVoice.Text = Settings.ChangeLanesSecondVoice;
            #endregion


            #region 变道
            edtTxtChangeLanesMaxDistance.Text = Settings.ChangeLanesMaxDistance.ToString();
            edtTxtChangeLanesTimeout.Text = Settings.ChangeLanesTimeout.ToString();
            edtTxtChangeLanesAngle.Text = Settings.ChangeLanesAngle.ToString();
            chkChangeLanesLowAndHighBeamCheck.Checked = Settings.ChangeLanesLowAndHighBeamCheck;
            chkChangeLanesLightCheck.Checked = Settings.ChangeLanesLightCheck;
            edtTxtChangeLanesPrepareDistance.Text = Settings.ChangeLanesPrepareDistance.ToString();

            chkChangeLanesEndFlag.Checked = Settings.ChkChangeLanesEndFlag;


            if (Settings.ChangelineDirect.Equals(1))
            {
                this.radChangelineLeft.Checked = true;
            }
            else if (Settings.ChangelineDirect.Equals(2))
            {
                this.radChangelineRight.Checked = true;
            }
            else
            {
                this.radChangelineBoth.Checked = true;
            }
            #endregion



        }

        public void InitControl()
        {
         
            #region 变更车道
            edtTxtChangeLanesVoice = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesVoice);
            edtTxtChangeLanesEndVoice = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesEndVoice);
            edtTxtChangeLanesSecondVoice = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesSecondVoice);


            edtTxtChangeLanesMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesMaxDistance);
            edtTxtChangeLanesTimeout = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesTimeout);
            edtTxtChangeLanesAngle = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesAngle);
            chkChangeLanesLowAndHighBeamCheck = FindViewById<CheckBox>(Resource.Id.chkChangeLanesLowAndHighBeamCheck);
            chkChangeLanesLightCheck = FindViewById<CheckBox>(Resource.Id.chkChangeLanesLightCheck);
            edtTxtChangeLanesPrepareDistance = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesPrepareDistance);

            chkChangeLanesEndFlag = FindViewById<CheckBox>(Resource.Id.chkChangeLanesEndFlag);
            radChangelineRight = FindViewById<RadioButton>(Resource.Id.radChangelineRight);
            radChangelineLeft = FindViewById<RadioButton>(Resource.Id.radChangelineLeft);
            radChangelineBoth = FindViewById<RadioButton>(Resource.Id.radChangelineBoth);
            #endregion


        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value
            try
            {
                ItemVoice= edtTxtChangeLanesVoice.Text;
                ItemEndVoice = edtTxtChangeLanesEndVoice.Text;

                Settings.ChangeLanesSecondVoice= edtTxtChangeLanesSecondVoice.Text;
                #region 变更车道
                Settings.ChangeLanesMaxDistance = Convert.ToInt32(edtTxtChangeLanesMaxDistance.Text);
                Settings.ChangeLanesTimeout = Convert.ToInt32(edtTxtChangeLanesTimeout.Text);
                Settings.ChangeLanesAngle = Convert.ToDouble(edtTxtChangeLanesAngle.Text);
                Settings.ChangeLanesLowAndHighBeamCheck = chkChangeLanesLowAndHighBeamCheck.Checked;
                Settings.ChangeLanesLightCheck = chkChangeLanesLightCheck.Checked;
                Settings.ChangeLanesPrepareDistance = Convert.ToInt32(edtTxtChangeLanesPrepareDistance.Text);

                Settings.ChkChangeLanesEndFlag = chkChangeLanesEndFlag.Checked ;

                if (this.radChangelineLeft.Checked)
                {
                    Settings.ChangelineDirect = 1;
                }
                else if (this.radChangelineRight.Checked)
                {
                    Settings.ChangelineDirect = 2;
                }
                else
                {
                    Settings.ChangelineDirect =0;
                }
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 变更车道
new Setting { Key ="ChangeLanesMaxDistance", Value = Settings.ChangeLanesMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesTimeout", Value = Settings.ChangeLanesTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesAngle", Value = Settings.ChangeLanesAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesLowAndHighBeamCheck", Value = Settings.ChangeLanesLowAndHighBeamCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesLightCheck", Value = Settings.ChangeLanesLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesPrepareDistance", Value = Settings.ChangeLanesPrepareDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesSecondVoice", Value = Settings.ChangeLanesSecondVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChkChangeLanesEndFlag", Value = Settings.ChkChangeLanesEndFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangelineDirect", Value = Settings.ChangelineDirect.ToString(), GroupName = "GlobalSettings" },


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