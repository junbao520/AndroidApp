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
    [Activity(Label = "SharpTurnActivity")]
    public class SharpTurnActivity : BaseSettingActivity
    {

        #region 变量定义
        //

        #region 急弯坡路
        // 急坡弯路项目语音
        EditText edtTxtSharpTurnVoice;
        // 急弯破路结束语音
        EditText edtTxtSharpTurnEndVoice;
        // 急坡弯路项目距离
        EditText edtTxtSharpTurnDistance;
        // 速度限制
        EditText edtTxtSharpTurnSpeedLimit;
        // 必须踩刹车
        CheckBox chkSharpTurnBrake;
        // 急坡弯路夜间远近光交替检测
        CheckBox chkSharpTurnLightCheck;
        // 急坡弯路白考喇叭
        CheckBox chkSharpTurnLoudspeakerInDay;
        // 急坡弯路夜考喇叭
        CheckBox chkSharpTurnLoudspeakerInNight;

        //急弯坡路左转检测
        CheckBox chkSharpTurnLeftLight;

        /// <summary>
        ///急弯坡路右转就检测
        /// </summary>
        CheckBox chkSharpTurnRightLight;
        
        
        #endregion

        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sharpturn);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.SharpTurn;
            ActivityName = this.GetString(Resource.String.SharpTurnStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


   

        public override void InitSetting()
        {
            #region 项目语音
           
            edtTxtSharpTurnVoice.Text = ItemVoice;
            edtTxtSharpTurnEndVoice.Text = ItemEndVoice;
            #endregion

          
            #region 急弯破路
            edtTxtSharpTurnDistance.Text = Settings.SharpTurnDistance.ToString();
            edtTxtSharpTurnSpeedLimit.Text = Settings.SharpTurnSpeedLimit.ToString();
            chkSharpTurnBrake.Checked = Settings.SharpTurnBrake;
            chkSharpTurnLightCheck.Checked = Settings.SharpTurnLightCheck;
            chkSharpTurnLoudspeakerInDay.Checked = Settings.SharpTurnLoudspeakerInDay;
            chkSharpTurnLoudspeakerInNight.Checked = Settings.SharpTurnLoudspeakerInNight;
            chkSharpTurnRightLight.Checked = Settings.SharpTurnRightLightCheck;
            chkSharpTurnLeftLight.Checked = Settings.SharpTurnLeftLightCheck;
            #endregion


        }

        public void InitControl()
        {
        

            #region 急弯坡路
            edtTxtSharpTurnVoice = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnVoice);
            edtTxtSharpTurnEndVoice = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnEndVoice);
            edtTxtSharpTurnDistance = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnDistance);
            edtTxtSharpTurnSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnSpeedLimit);
            chkSharpTurnBrake = FindViewById<CheckBox>(Resource.Id.chkSharpTurnBrake);
            chkSharpTurnLightCheck = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLightCheck);
            chkSharpTurnLoudspeakerInDay = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLoudspeakerInDay);
            chkSharpTurnLoudspeakerInNight = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLoudspeakerInNight);
            chkSharpTurnLeftLight = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLeftLight);
            chkSharpTurnRightLight = FindViewById<CheckBox>(Resource.Id.chkSharpTurnRightLight);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
                //List<ExamItem> lstExamItem = new List<ExamItem>();
                //ExamItem examItem = new ExamItem();


                //examItem = new ExamItem();
                //examItem.ItemCode = ExamItemCodes.SharpTurn;
                //examItem.VoiceText = edtTxtSharpTurnVoice.Text;
                //examItem.EndVoiceText = edtTxtSharpTurnEndVoice.Text;
                //lstExamItem.Add(examItem);
                ItemVoice = edtTxtSharpTurnVoice.Text;
                ItemEndVoice = edtTxtSharpTurnEndVoice.Text;


                #region 急弯坡路


                Settings.SharpTurnDistance = Convert.ToInt32(edtTxtSharpTurnDistance.Text);
                Settings.SharpTurnSpeedLimit = Convert.ToInt32(edtTxtSharpTurnSpeedLimit.Text);
                Settings.SharpTurnBrake = chkSharpTurnBrake.Checked;
                Settings.SharpTurnLightCheck = chkSharpTurnLightCheck.Checked;
                Settings.SharpTurnLoudspeakerInDay = chkSharpTurnLoudspeakerInDay.Checked;
                Settings.SharpTurnLoudspeakerInNight = chkSharpTurnLoudspeakerInNight.Checked;
                Settings.SharpTurnLeftLightCheck = chkSharpTurnLeftLight.Checked;
                Settings.SharpTurnRightLightCheck = chkSharpTurnRightLight.Checked;
                #endregion


                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 急弯破路
new Setting { Key ="SharpTurnLeftLightCheck", Value = Settings.SharpTurnLeftLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnRightLightCheck", Value = Settings.SharpTurnRightLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnDistance", Value = Settings.SharpTurnDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnSpeedLimit", Value = Settings.SharpTurnSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnBrake", Value = Settings.SharpTurnBrake.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnLightCheck", Value = Settings.SharpTurnLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnLoudspeakerInDay", Value = Settings.SharpTurnLoudspeakerInDay.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnLoudspeakerInNight", Value = Settings.SharpTurnLoudspeakerInNight.ToString(), GroupName = "GlobalSettings" },
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