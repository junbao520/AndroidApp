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
    [Activity(Label = "TurnRightActivity")]
    public class TurnRightActivity : BaseSettingActivity
    {

        #region ��������
        #region ·����ת
        // ·����ת��Ŀ����
        EditText edtTxtTurnRightVoice;
        // ·����ת��������
        EditText edtTxtTurnRightEndVoice;
        //��ת����Ƕ�
        EditText edtTxtTurnRightAngle;
        // ·����ת��Ŀ����
        EditText edtTxtTurnRightDistance;
        // ·����ת��Ŀ׼������
        EditText edtTxtTurnRightPrepareD;
        // ·����ת�ٶ�����
        EditText edtTxtTurnRightSpeedLimit;
        // ·����תҪ���ɲ���ٶ�����
        EditText edtTxtTurnRightBrakeSpeedUp;
        // ·����ת�����ɲ��
        CheckBox chkTurnRightBrakeRequire;
        //·����תר��Ƽ��
        CheckBox chkTurnRightLightCheck;
        // ·����ת�׿����ȼ��
        CheckBox chkTurnRightLoudSpeakerDayCheck;
        // ·����תҹ�����ȼ��
        CheckBox chkTurnRightLoudSpeakerNightCheck;

        CheckBox chkTurnRightEndFlag;
        CheckBox chkTurnRightErrorLight;
        #endregion
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//ȫ�������ޱ�������������OnCreateǰ�����á�
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.turnright);
            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.TurnRight;
            ActivityName = this.GetString(Resource.String.TurnRightStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


 

        public override void InitSetting()
        {
            #region ��Ŀ����
           

            edtTxtTurnRightVoice.Text = ItemVoice;
            edtTxtTurnRightEndVoice.Text = ItemEndVoice;

          
            #endregion

            #region ·����ת
            edtTxtTurnRightDistance.Text = Settings.TurnRightDistance.ToString();
            edtTxtTurnRightPrepareD.Text = Settings.TurnRightPrepareD.ToString();
            edtTxtTurnRightSpeedLimit.Text = Settings.TurnRightSpeedLimit.ToString();
            edtTxtTurnRightBrakeSpeedUp.Text = Settings.TurnRightBrakeSpeedUp.ToString();
            chkTurnRightBrakeRequire.Checked = Settings.TurnRightBrakeRequire;

            chkTurnRightLightCheck.Checked = Settings.TurnRightLightCheck;
            chkTurnRightLoudSpeakerDayCheck.Checked = Settings.TurnRightLoudSpeakerDayCheck;
            chkTurnRightLoudSpeakerNightCheck.Checked = Settings.TurnRightLoudSpeakerNightCheck;
            edtTxtTurnRightAngle.Text = Settings.TurnRightAngle.ToString();

            chkTurnRightEndFlag.Checked = Settings.TurnRightEndFlag;

            chkTurnRightErrorLight.Checked = Settings.TurnRightErrorLight;
            #endregion


        }

        public void InitControl()
        {
          

            #region ��ת
            edtTxtTurnRightVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRightVoice);
            edtTxtTurnRightEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRightEndVoice);
            edtTxtTurnRightDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnRightDistance);
            edtTxtTurnRightPrepareD = FindViewById<EditText>(Resource.Id.edtTxtTurnRightPrepareD);
            edtTxtTurnRightSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtTurnRightSpeedLimit);
            edtTxtTurnRightBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtTurnRightBrakeSpeedUp);
            chkTurnRightBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkTurnRightBrakeRequire);
            chkTurnRightLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRightLightCheck);
            chkTurnRightLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRightLoudSpeakerDayCheck);
            chkTurnRightLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRightLoudSpeakerNightCheck);

            edtTxtTurnRightAngle = FindViewById<EditText>(Resource.Id.edtTxtTurnRightAngle);

            chkTurnRightEndFlag = FindViewById<CheckBox>(Resource.Id.chkTurnRightEndFlag);

            chkTurnRightErrorLight = FindViewById<CheckBox>(Resource.Id.chkTurnRightErrorLight);
            
            #endregion

        }


        public override void UpdateSettings()
        {
            //��Ҫ����Ŀ�������½������ݿ�

            //��ʵ�Ҿ�����ֻ��Ҫһ��KevValue �Ϳ�����
            //key,Value

            try
            {
                ItemVoice= edtTxtTurnRightVoice.Text;
                ItemEndVoice = edtTxtTurnRightEndVoice.Text;
        




                #region ·����ת
                Settings.TurnRightDistance = Convert.ToInt32(edtTxtTurnRightDistance.Text);
                Settings.TurnRightPrepareD = Convert.ToInt32(edtTxtTurnRightPrepareD.Text);
                Settings.TurnRightSpeedLimit = Convert.ToInt32(edtTxtTurnRightSpeedLimit.Text);
                Settings.TurnRightBrakeSpeedUp = Convert.ToInt32(edtTxtTurnRightBrakeSpeedUp.Text);
                Settings.TurnRightBrakeRequire = chkTurnRightBrakeRequire.Checked;
                Settings.TurnRightLightCheck = chkTurnRightLightCheck.Checked;
                Settings.TurnRightLoudSpeakerDayCheck = chkTurnRightLoudSpeakerDayCheck.Checked;
                Settings.TurnRightLoudSpeakerNightCheck = chkTurnRightLoudSpeakerNightCheck.Checked;

                 Settings.TurnRightAngle= Convert.ToDouble(edtTxtTurnRightAngle.Text);

                 Settings.TurnRightEndFlag = chkTurnRightEndFlag.Checked;

               Settings.TurnRightErrorLight = chkTurnRightErrorLight.Checked;
                #endregion



                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region ��ת


                    new Setting { Key ="TurnRightDistance", Value = Settings.TurnRightDistance.ToString(), GroupName = "GlobalSettings" },
                      new Setting { Key ="TurnRightPrepareD", Value = Settings.TurnRightPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightSpeedLimit", Value = Settings.TurnRightSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightBrakeSpeedUp", Value = Settings.TurnRightBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightBrakeRequire", Value = Settings.TurnRightBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightLightCheck", Value = Settings.TurnRightLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightLoudSpeakerDayCheck", Value = Settings.TurnRightLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightLoudSpeakerNightCheck", Value = Settings.TurnRightLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightAngle", Value = Settings.TurnRightAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightEndFlag", Value = Settings.TurnRightEndFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightErrorLight", Value = Settings.TurnRightErrorLight.ToString(), GroupName = "GlobalSettings" },
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