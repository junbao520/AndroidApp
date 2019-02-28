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
    [Activity(Label = "TurnRoundActivity")]
    public class TurnRoundActivity : BaseSettingActivity
    {

        #region ��������
        #region ��ͷ
        // ��ͷ��Ŀ���� 
        EditText edtTxtTurnRoundVoice;
        // ��ͷ��Ŀ�������� 
        EditText edtTxtTurnRoundEndVoice;
        // ��ͷ�������������ƣ���λ���ף�
        EditText edtTxtTurnRoundMaxDistance;
        // ��ͷ����׼���������ƣ���λ���ף�
        EditText edtTxtTurnRoundPrepareD;
        // ��ͷת��ǶȲ�ȷ�Ͽ�ʼ��ͷ����λ���ȣ�
        EditText edtTxtTurnRoundStartAngleDiff;
        // ��ͷ������ͷת��ǶȲ��λ���ȣ�
        EditText edtTxtTurnRoundEndAngleDiff;
        // ��ͷ�ز�ɲ��
        CheckBox chkTurnRoundBrakeRequired;
        // ��ͷҹ��Զ���⽻����
        CheckBox chkTurnRoundLightCheck;
        // ��ͷ�׿����ȼ�� 
        CheckBox chkTurnRoundLoudSpeakerDayCheck;
        // ��ͷҹ�����ȼ�� 
        CheckBox chkTurnRoundLoudSpeakerNightCheck;

        CheckBox chkTurnRoundErrorLight;
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//ȫ�������ޱ�������������OnCreateǰ�����á�
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.turnround);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.TurnRound;
            ActivityName = this.GetString(Resource.String.TurnRoundStr);
            setMyTitle(ActivityName);
            InitSetting();
        }



        public override void InitSetting()
        {
            #region ��Ŀ����
           

            edtTxtTurnRoundVoice.Text = ItemVoice;
            edtTxtTurnRoundEndVoice.Text = ItemEndVoice;

            
            #endregion

            #region ��ͷ
            edtTxtTurnRoundMaxDistance.Text = Settings.TurnRoundMaxDistance.ToString();
            edtTxtTurnRoundPrepareD.Text = Settings.TurnRoundPrepareD.ToString();
            edtTxtTurnRoundStartAngleDiff.Text = Settings.TurnRoundStartAngleDiff.ToString();
            edtTxtTurnRoundEndAngleDiff.Text = Settings.TurnRoundEndAngleDiff.ToString();
            chkTurnRoundBrakeRequired.Checked = Settings.TurnRoundBrakeRequired;
            chkTurnRoundLightCheck.Checked = Settings.TurnRoundLightCheck;
            chkTurnRoundLoudSpeakerDayCheck.Checked = Settings.TurnRoundLoudSpeakerDayCheck;
            chkTurnRoundLoudSpeakerNightCheck.Checked = Settings.TurnRoundLoudSpeakerNightCheck;


            chkTurnRoundErrorLight.Checked = Settings.TurnRoundErrorLight;
            #endregion
        }

        public void InitControl()
        {
         

            #region ��ͷ
            edtTxtTurnRoundVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundVoice);
            edtTxtTurnRoundEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundEndVoice);
            edtTxtTurnRoundMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundMaxDistance);
            edtTxtTurnRoundPrepareD = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundPrepareD);
            edtTxtTurnRoundStartAngleDiff = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundStartAngleDiff);
            edtTxtTurnRoundEndAngleDiff = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundEndAngleDiff);
            chkTurnRoundBrakeRequired = FindViewById<CheckBox>(Resource.Id.chkTurnRoundBrakeRequired);
            chkTurnRoundLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLightCheck);
            chkTurnRoundLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLoudSpeakerDayCheck);
            chkTurnRoundLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLoudSpeakerNightCheck);

            chkTurnRoundErrorLight = FindViewById<CheckBox>(Resource.Id.chkTurnRoundErrorLight);
            
            #endregion


        }


        public override void UpdateSettings()
        {
            //��Ҫ����Ŀ�������½������ݿ�

            //��ʵ�Ҿ�����ֻ��Ҫһ��KevValue �Ϳ�����
            //key,Value

            try
            {
               
                ItemVoice= edtTxtTurnRoundVoice.Text;
                ItemEndVoice = edtTxtTurnRoundEndVoice.Text;
            
                #region ��ͷ


                Settings.TurnRoundMaxDistance = Convert.ToInt32(edtTxtTurnRoundMaxDistance.Text);
                Settings.TurnRoundPrepareD = Convert.ToInt32(edtTxtTurnRoundPrepareD.Text);
                Settings.TurnRoundStartAngleDiff = Convert.ToInt32(edtTxtTurnRoundStartAngleDiff.Text);
                Settings.TurnRoundEndAngleDiff = Convert.ToInt32(edtTxtTurnRoundEndAngleDiff.Text);
                Settings.TurnRoundBrakeRequired = chkTurnRoundBrakeRequired.Checked;
                Settings.TurnRoundLightCheck = chkTurnRoundLightCheck.Checked;
                Settings.TurnRoundLoudSpeakerDayCheck = chkTurnRoundLoudSpeakerDayCheck.Checked;
                Settings.TurnRoundLoudSpeakerNightCheck = chkTurnRoundLoudSpeakerNightCheck.Checked;

                Settings.TurnRoundErrorLight = chkTurnRoundErrorLight.Checked ;
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region ��ͷ
new Setting { Key ="TurnRoundMaxDistance", Value = Settings.TurnRoundMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundPrepareD", Value = Settings.TurnRoundPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundStartAngleDiff", Value = Settings.TurnRoundStartAngleDiff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundEndAngleDiff", Value = Settings.TurnRoundEndAngleDiff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundBrakeRequired", Value = Settings.TurnRoundBrakeRequired.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLightCheck", Value = Settings.TurnRoundLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLoudSpeakerDayCheck", Value = Settings.TurnRoundLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLoudSpeakerNightCheck", Value = Settings.TurnRoundLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundErrorLight", Value = Settings.TurnRoundErrorLight.ToString(), GroupName = "GlobalSettings" },
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