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
    [Activity(Label = "StraightDrivingActivity")]
    public class StraightDrivingActivity : BaseSettingActivity
    {

        #region 变量定义
        //

        #region 直线行驶
        //直线行驶开始语音
        EditText edtTxtStraightDrivingVoice;
        //直线行驶结束语音
        EditText edtTxtStraightDrivingEndVoice;
        //直线行驶项目距离
        EditText edtTxtStraightDrivingDistance;
        //直线行驶角度
        EditText edtTxtStraightDrivingMaxOffsetAngle;
        //直线行驶最高速度
        EditText edtTxtStraightDrivingSpeedMaxLimit;
        //直线行驶最低速度
        EditText edtTxtStraightDrivingSpeedMinLimit;
        //直线行驶达到一次速度
        EditText edtTxtStraightDrivingReachSpeed;
        //直线行驶准备距离
        EditText edtTxtStraightDrivingPrepareDistance;
        #endregion

       
        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.straightdriving);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.StraightDriving;
            ActivityName = this.GetString(Resource.String.StraightDrivingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


      

        public override void InitSetting()
        {
            #region 项目语音
          

            edtTxtStraightDrivingVoice.Text =ItemVoice;
            edtTxtStraightDrivingEndVoice.Text =ItemEndVoice;

           
            #endregion

            #region 直线行驶


            edtTxtStraightDrivingMaxOffsetAngle.Text = Settings.StraightDrivingMaxOffsetAngle.ToString();
            edtTxtStraightDrivingDistance.Text = Settings.StraightDrivingDistance.ToString();
            edtTxtStraightDrivingPrepareDistance.Text = Settings.StraightDrivingPrepareDistance.ToString();
            edtTxtStraightDrivingReachSpeed.Text = Settings.StraightDrivingReachSpeed.ToString();
            edtTxtStraightDrivingSpeedMaxLimit.Text = Settings.StraightDrivingSpeedMaxLimit.ToString();
            edtTxtStraightDrivingSpeedMinLimit.Text = Settings.StraightDrivingSpeedMinLimit.ToString();

            #endregion


        }

        public void InitControl()
        {
           

            #region 直线行驶
            edtTxtStraightDrivingVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingVoice);
            edtTxtStraightDrivingEndVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingEndVoice);
            edtTxtStraightDrivingMaxOffsetAngle = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingMaxOffsetAngle);
            edtTxtStraightDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingDistance);
            edtTxtStraightDrivingSpeedMaxLimit = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingSpeedMaxLimit);
            edtTxtStraightDrivingSpeedMinLimit = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingSpeedMinLimit);
            edtTxtStraightDrivingReachSpeed = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingReachSpeed);
            edtTxtStraightDrivingPrepareDistance = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingPrepareDistance);
            #endregion



        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
                ItemVoice = edtTxtStraightDrivingVoice.Text;
                ItemEndVoice = edtTxtStraightDrivingEndVoice.Text;
             
                #region 直线行驶
                Settings.StraightDrivingDistance = Convert.ToDouble(edtTxtStraightDrivingDistance.Text);
                Settings.StraightDrivingMaxOffsetAngle = Convert.ToDouble(edtTxtStraightDrivingMaxOffsetAngle.Text);
                Settings.StraightDrivingPrepareDistance = Convert.ToInt32(edtTxtStraightDrivingPrepareDistance.Text);
                Settings.StraightDrivingReachSpeed = Convert.ToInt32(edtTxtStraightDrivingReachSpeed.Text);
                Settings.StraightDrivingSpeedMaxLimit = Convert.ToInt32(edtTxtStraightDrivingSpeedMaxLimit.Text);
                Settings.StraightDrivingSpeedMinLimit = Convert.ToInt32(edtTxtStraightDrivingSpeedMinLimit.Text);
                #endregion

                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 直线行驶
new Setting { Key ="StraightDrivingDistance", Value = Settings.StraightDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingMaxOffsetAngle", Value = Settings.StraightDrivingMaxOffsetAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingPrepareDistance", Value = Settings.StraightDrivingPrepareDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingReachSpeed", Value = Settings.StraightDrivingReachSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingSpeedMaxLimit", Value = Settings.StraightDrivingSpeedMaxLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingSpeedMinLimit", Value = Settings.StraightDrivingSpeedMinLimit.ToString(), GroupName = "GlobalSettings" },
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