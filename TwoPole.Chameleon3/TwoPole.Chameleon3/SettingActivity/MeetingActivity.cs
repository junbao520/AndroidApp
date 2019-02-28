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
using System.Diagnostics;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "MeetingActivity")]
    public class MeetingActivity : BaseSettingActivity
    {

        #region 变量定义
        #region 会车
        // 会车语音
        EditText edtTxtMeetingVoice;
        // 会车结束语音
        EditText edtTxtMeetingEndVoice;
        // 会车会车距离
        EditText edtTxtMeetingDrivingDistance;

        // 会车项目距离
        EditText edtTxtMeetingPrepareD;
        // 会车速度限制
        EditText edtTxtMeetingSlowSpeedInKmh;
        // 会车向右避让角度
        EditText edtTxtMeetingAngle;
        // 会车刹车
        CheckBox chkMeetingCheckBrake;
        // 会车禁止远光
        CheckBox chkMeetingForbidHighBeamCheck;

      
        #endregion
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.meeting);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.Meeting;
            ActivityName = this.GetString(Resource.String.MeetingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


   

        public override void InitSetting()
        {
            #region 项目语音

            edtTxtMeetingVoice.Text = ItemVoice;
            edtTxtMeetingEndVoice.Text = ItemEndVoice;
            #endregion

            #region 会车
            edtTxtMeetingDrivingDistance.Text = Settings.MeetingDrivingDistance.ToString();
            edtTxtMeetingPrepareD.Text = Settings.MeetingPrepareD.ToString();
            edtTxtMeetingSlowSpeedInKmh.Text = Settings.MeetingSlowSpeedInKmh.ToString();
            edtTxtMeetingAngle.Text = Settings.MeetingAngle.ToString();
            chkMeetingCheckBrake.Checked = Settings.MeetingCheckBrake;
            chkMeetingForbidHighBeamCheck.Checked = Settings.MeetingForbidHighBeamCheck;
            #endregion

        }

        public void InitControl()
        {
          
            #region 会车
            edtTxtMeetingVoice = FindViewById<EditText>(Resource.Id.edtTxtMeetingVoice);
            edtTxtMeetingEndVoice = FindViewById<EditText>(Resource.Id.edtTxtMeetingEndVoice);
            edtTxtMeetingDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtMeetingDrivingDistance);
            edtTxtMeetingPrepareD = FindViewById<EditText>(Resource.Id.edtTxtMeetingPrepareD);
            edtTxtMeetingSlowSpeedInKmh = FindViewById<EditText>(Resource.Id.edtTxtMeetingSlowSpeedInKmh);
            edtTxtMeetingAngle = FindViewById<EditText>(Resource.Id.edtTxtMeetingAngle);
            chkMeetingCheckBrake = FindViewById<CheckBox>(Resource.Id.chkMeetingCheckBrake);
            chkMeetingForbidHighBeamCheck = FindViewById<CheckBox>(Resource.Id.chkMeetingForbidHighBeamCheck);
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
                //examItem.ItemCode = ExamItemCodes.Meeting;
                //examItem.VoiceText = edtTxtMeetingVoice.Text;
                //examItem.EndVoiceText = edtTxtMeetingEndVoice.Text;
                //lstExamItem.Add(examItem);


                //dataService.UpdateExamItemsVoice(lstExamItem);
                ItemVoice = edtTxtMeetingVoice.Text;
                ItemEndVoice = edtTxtMeetingEndVoice.Text;

                #region 会车


                Settings.MeetingDrivingDistance = Convert.ToDouble(edtTxtMeetingDrivingDistance.Text);
                Settings.MeetingPrepareD = Convert.ToDouble(edtTxtMeetingPrepareD.Text);
                Settings.MeetingSlowSpeedInKmh = Convert.ToInt32(edtTxtMeetingSlowSpeedInKmh.Text);
                Settings.MeetingAngle = Convert.ToDouble(edtTxtMeetingAngle.Text);
                Settings.MeetingCheckBrake = chkMeetingCheckBrake.Checked;
                Settings.MeetingForbidHighBeamCheck = chkMeetingForbidHighBeamCheck.Checked;
                #endregion


                lstSetting = new List<Setting>() {
                    new Setting { Key="MeetingDrivingDistance",Value=Settings.MeetingDrivingDistance.ToString(),GroupName="GlobalSettings"},
                     new Setting { Key="MeetingPrepareD",Value=Settings.MeetingPrepareD.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="MeetingSlowSpeedInKmh",Value=Settings.MeetingSlowSpeedInKmh.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="MeetingAngle",Value=Settings.MeetingAngle.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="MeetingCheckBrake",Value=Settings.MeetingCheckBrake.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="MeetingForbidHighBeamCheck",Value=Settings.MeetingForbidHighBeamCheck.ToString(),GroupName="GlobalSettings"},
                };
                UpdateSettings(lstSetting);
                Finish();
            }
            catch (Exception ex)
            {
                m_setting_head_title.Text = "专项参数" + "  " + "保存失败:" + ex.Message;
                Logger.Error("MeetingActivity" + ex.Message);
            }

        }
    }
}