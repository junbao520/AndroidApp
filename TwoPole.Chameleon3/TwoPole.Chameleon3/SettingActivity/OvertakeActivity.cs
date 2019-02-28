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

        #region ��������
   
        #region ����
        // ��������
        EditText edtTxtOvertakeVoice;
        // ������������
        EditText edtTxtOvertakeEndVoice;
        // ���������루��λ���ף�
        EditText edtTxtOvertakeMaxDistance;
        // ������ʱʱ�䣨��λ���룩
        EditText edtTxtOvertakeTimeout;
        // ���ת��Ƕ�
        EditText edtTxtOvertakeChangeLanesAngle;
        // ��������ٶ�
        EditText edtTxtOvertakeLowestSpeed;
        // �����ٶ�����
        EditText edtTxtOvertakeSpeedLimit;
        // �����ﵽһ���ٶ�
        EditText edtTxtOvertakeSpeedOnce;
        // �����Ƿ񷵻�ԭ����
        EditText edtTxtOvertakeBackToOriginalLaneVoice;
        // �����Ƿ񷵻�ԭ����
        EditText edtTxtOvertakeBackToOriginalLaneDistance;

        //����׼������
        EditText edtTxtOverTakePrepareDistance;
        // ����ҹ��Զ���⽻��
        CheckBox chkOvertakeLowAndHighBeamCheck;
        // ����ת��Ƶƹ���
        CheckBox chkOvertakeLightCheck;
        // �����׿����ȼ��
        CheckBox chkOvertakeLoudSpeakerDayCheck;
        // ����ҹ�����ȼ��
        CheckBox chkOvertakeLoudSpeakerNightCheck;
        // �����Ƿ񷵻�ԭ����
        CheckBox chkOvertakeBackToOriginalLane;
        #endregion

      
        //�ۺ�����

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//ȫ�������ޱ�������������OnCreateǰ�����á�
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
            #region ��Ŀ����
           

            edtTxtOvertakeVoice.Text = ItemVoice;
            edtTxtOvertakeEndVoice.Text = ItemEndVoice ;

         
            #endregion

           

            #region ����
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
          
            #region ����
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
            //��Ҫ����Ŀ�������½������ݿ�

            //��ʵ�Ҿ�����ֻ��Ҫһ��KevValue �Ϳ�����
            //key,Value

            try
            {
        
                ItemVoice= edtTxtOvertakeVoice.Text; ;
                ItemEndVoice = edtTxtOvertakeEndVoice.Text; ;
              

                #region ����



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
                    #region ����
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
                string HeaderText = string.Format("{0}  ����ʧ�ܣ�{1}", ActivityName, ex.Message);
                setMyTitle(HeaderText);
                Logger.Error(ActivityName, ex.Message);

            }

        }
    }
}