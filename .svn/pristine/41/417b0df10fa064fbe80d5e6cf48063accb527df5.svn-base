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
    [Activity(Label = "RoundaboutActivity")]
    public class RoundaboutActivity : BaseSettingActivity
    {

        #region 变量定义
        //



        #region 环岛
        // 环岛项目语音
        EditText edtTxtRoundaboutVoice;
        // 环岛项目结束语音
        EditText edtTxtRoundaboutEndVoice;
        // 环岛项目距离
        EditText edtTxtRoundaboutDistance;
        // 环岛默认环岛灯光检测
        CheckBox chkRoundaboutLightCheck;
        #endregion

        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.roundabout);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.Roundabout;
            ActivityName = this.GetString(Resource.String.RoundaboutStr);
            setMyTitle(ActivityName);
            InitSetting();
        }



        public override void InitSetting()
        {
            #region 项目语音
            

            edtTxtRoundaboutVoice.Text =ItemVoice ;
            edtTxtRoundaboutEndVoice.Text =ItemEndVoice ;

            
            #endregion
            #region 环岛
            edtTxtRoundaboutDistance.Text = Settings.RoundaboutDistance.ToString();
            chkRoundaboutLightCheck.Checked = Settings.RoundaboutLightCheck;
            #endregion

          
        }

        public void InitControl()
        {
           
            #region 环岛
            edtTxtRoundaboutVoice = FindViewById<EditText>(Resource.Id.edtTxtRoundaboutVoice);
            edtTxtRoundaboutEndVoice = FindViewById<EditText>(Resource.Id.edtTxtRoundaboutEndVoice);
            edtTxtRoundaboutDistance = FindViewById<EditText>(Resource.Id.edtTxtRoundaboutDistance);
            chkRoundaboutLightCheck = FindViewById<CheckBox>(Resource.Id.chkRoundaboutLightCheck);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
  
                ItemVoice= edtTxtRoundaboutVoice.Text;
                ItemEndVoice = edtTxtRoundaboutEndVoice.Text;
   
                #region 环岛



                Settings.RoundaboutDistance = Convert.ToInt32(edtTxtRoundaboutDistance.Text);
                Settings.RoundaboutLightCheck = chkRoundaboutLightCheck.Checked;
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                   
                    #region 环岛
new Setting { Key ="RoundaboutDistance", Value = Settings.RoundaboutDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="RoundaboutLightCheck", Value = Settings.RoundaboutLightCheck.ToString(), GroupName = "GlobalSettings" },
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