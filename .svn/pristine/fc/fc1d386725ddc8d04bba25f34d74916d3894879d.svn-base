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
    [Activity(Label = "ThroughSchoolAreaActivity")]
    public class ThroughSchoolAreaActivity : BaseSettingActivity
    {

        #region 变量定义
     

 

        #region 学校区域
        // 学校区域项目语音
        EditText edtTxtSchoolAreaVoice;
        // 学习区域结束语音
        EditText edtTxtSchoolAreaEndVoice;
        // 学校区域项目距离
        EditText edtTxtSchoolAreaDistance;

        EditText edtTxtSchoolAreaPrepareD;
        // 学校区域速度限制
        EditText edtTxtSchoolAreaSpeedLimit;
        // 学校区域要求踩刹车速度限制
        EditText edtTxtSchoolAreaBrakeSpeedUp;

        CheckBox chkSchoolCheckBrake;
        #endregion


        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.schoolarea);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.SchoolArea;
            ActivityName = this.GetString(Resource.String.ThroughSchoolAreaStr);
            setMyTitle(ActivityName);
            InitSetting();
        }

        public void InitControl()
        {
            #region 学校区域
            edtTxtSchoolAreaVoice = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaVoice);
            edtTxtSchoolAreaEndVoice = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaEndVoice);
            edtTxtSchoolAreaDistance = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaDistance);
            edtTxtSchoolAreaPrepareD = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaPrepareD);
            edtTxtSchoolAreaSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaSpeedLimit);
            edtTxtSchoolAreaBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaBrakeSpeedUp);
            chkSchoolCheckBrake = FindViewById<CheckBox>(Resource.Id.chkSchoolCheckBrake);
            #endregion
        }
  
        public override void InitSetting()
        {
            #region 项目语音
          
            edtTxtSchoolAreaVoice.Text =ItemVoice;
            edtTxtSchoolAreaEndVoice.Text =ItemEndVoice;

            #endregion

    

            #region 学校区域
            edtTxtSchoolAreaDistance.Text = Settings.SchoolAreaDistance.ToString();
            edtTxtSchoolAreaPrepareD.Text = Settings.SchoolAreaPrepareD.ToString();
            edtTxtSchoolAreaSpeedLimit.Text = Settings.SchoolAreaSpeedLimit.ToString();
            edtTxtSchoolAreaBrakeSpeedUp.Text = Settings.SchoolAreaBrakeSpeedUp.ToString();
            chkSchoolCheckBrake.Checked = Settings.SchoolAreaBrakeRequire;
            #endregion


        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
            
                ItemVoice = edtTxtSchoolAreaVoice.Text;
                ItemEndVoice = edtTxtSchoolAreaEndVoice.Text;

                #region 学校区域
                Settings.SchoolAreaDistance = Convert.ToInt32(edtTxtSchoolAreaDistance.Text);
                Settings.SchoolAreaPrepareD = Convert.ToInt32(edtTxtSchoolAreaPrepareD.Text);
                Settings.SchoolAreaSpeedLimit = Convert.ToInt32(edtTxtSchoolAreaSpeedLimit.Text);
                Settings.SchoolAreaBrakeSpeedUp = Convert.ToInt32(edtTxtSchoolAreaBrakeSpeedUp.Text);
                Settings.SchoolAreaBrakeRequire = chkSchoolCheckBrake.Checked;
                #endregion


                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                   
                    #region 学校区域
new Setting { Key ="SchoolAreaDistance", Value = Settings.SchoolAreaDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SchoolAreaPrepareD", Value = Settings.SchoolAreaPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SchoolAreaSpeedLimit", Value = Settings.SchoolAreaSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SchoolAreaBrakeSpeedUp", Value = Settings.SchoolAreaBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SchoolAreaBrakeRequire", Value = Settings.SchoolAreaBrakeRequire.ToString(), GroupName = "GlobalSettings" },
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