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
    [Activity(Label = "TurnLeftActivity")]
    public class TurnLeftActivity : BaseSettingActivity
    {

        #region ��������
        #region ·����ת

        // ·����ת��Ŀ����
        EditText edtTxtTurnLeftVoice;
        // ·����ת��Ŀ����
        EditText edtTxtTurnLeftEndVoice;
        // ·����ת��Ŀ����
        EditText edtTxtTurnLeftDistance;

        // ·����ת��Ŀ׼������
        EditText edtTxtTurnLeftPrepareD;
        //��ת����Ƕ�
        EditText edtTxtTurnLeftAngle;
        // ·����ת�ٶ�����
        EditText edtTxtTurnLeftSpeedLimit;
        // ·����תҪ���ɲ���ٶ�����
        EditText edtTxtTurnLeftBrakeSpeedUp;
        // ·����ת�����ɲ��
        CheckBox chkTurnLeftBrakeRequire;
        //·����תר��Ƽ��
        CheckBox chkTurnLeftLightCheck;
        // ·����ת�׿����ȼ��
        CheckBox chkTurnLeftLoudSpeakerDayCheck;
        // ·����תҹ�����ȼ��
        CheckBox chkTurnLeftLoudSpeakerNightCheck;
        //�Ƕȴﵽ�����
        CheckBox chkTurnLeftEndFlag;

        CheckBox chkTurnLeftErrorLight;
        #endregion
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//ȫ�������ޱ�������������OnCreateǰ�����á�
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.turnleft);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.TurnLeft;
            ActivityName = this.GetString(Resource.String.TurnLeftStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


 

        public override void InitSetting()
        {
            #region ��Ŀ����
            
            edtTxtTurnLeftVoice.Text = ItemVoice;
            edtTxtTurnLeftEndVoice.Text =ItemEndVoice;

            #endregion
            #region ·����ת
            edtTxtTurnLeftDistance.Text = Settings.TurnLeftDistance.ToString();
            edtTxtTurnLeftPrepareD.Text = Settings.TurnLeftPrepareD.ToString();
            edtTxtTurnLeftSpeedLimit.Text = Settings.TurnLeftSpeedLimit.ToString();
            edtTxtTurnLeftBrakeSpeedUp.Text = Settings.TurnLeftBrakeSpeedUp.ToString();
            chkTurnLeftBrakeRequire.Checked = Settings.TurnLeftBrakeRequire;
            chkTurnLeftLightCheck.Checked = Settings.TurnLeftLightCheck;
            chkTurnLeftLoudSpeakerDayCheck.Checked = Settings.TurnLeftLoudSpeakerDayCheck;
            chkTurnLeftLoudSpeakerNightCheck.Checked = Settings.TurnLeftLoudSpeakerNightCheck;
            edtTxtTurnLeftAngle.Text = Settings.TurnLeftAngle.ToString();

            chkTurnLeftEndFlag.Checked = Settings.TurnLeftEndFlag;

            chkTurnLeftErrorLight.Checked = Settings.TurnLeftErrorLight;
            #endregion



        }

        public void InitControl()
        {

            #region ��ת
            edtTxtTurnLeftVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftVoice);
            edtTxtTurnLeftEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftEndVoice);
            edtTxtTurnLeftDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftDistance);
            edtTxtTurnLeftPrepareD = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftPrepareD);
            edtTxtTurnLeftSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftSpeedLimit);
            edtTxtTurnLeftBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftBrakeSpeedUp);
            chkTurnLeftBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkTurnLeftBrakeRequire);
            chkTurnLeftLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLightCheck);
            chkTurnLeftLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLoudSpeakerDayCheck);
            chkTurnLeftLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLoudSpeakerNightCheck);

            edtTxtTurnLeftAngle= FindViewById<EditText>(Resource.Id.edtTxtTurnLeftAngle);

            chkTurnLeftEndFlag = FindViewById<CheckBox>(Resource.Id.chkTurnLeftEndFlag);


            chkTurnLeftErrorLight = FindViewById<CheckBox>(Resource.Id.chkTurnLeftErrorLight);
            #endregion

        }


        public override void UpdateSettings()
        {
            //��Ҫ����Ŀ�������½������ݿ�

            //��ʵ�Ҿ�����ֻ��Ҫһ��KevValue �Ϳ�����
            //key,Value

            try
            {

                ItemVoice = edtTxtTurnLeftVoice.Text;
                ItemEndVoice = edtTxtTurnLeftEndVoice.Text;
     
                #region ·����ת
                Settings.TurnLeftDistance = Convert.ToInt32(edtTxtTurnLeftDistance.Text);
                Settings.TurnLeftPrepareD = Convert.ToInt32(edtTxtTurnLeftPrepareD.Text);
                Settings.TurnLeftSpeedLimit = Convert.ToInt32(edtTxtTurnLeftSpeedLimit.Text);
                Settings.TurnLeftBrakeSpeedUp = Convert.ToInt32(edtTxtTurnLeftBrakeSpeedUp.Text);
                Settings.TurnLeftBrakeRequire = chkTurnLeftBrakeRequire.Checked;
                Settings.TurnLeftLightCheck = chkTurnLeftLightCheck.Checked;
                Settings.TurnLeftLoudSpeakerDayCheck = chkTurnLeftLoudSpeakerDayCheck.Checked;
                Settings.TurnLeftLoudSpeakerNightCheck = chkTurnLeftLoudSpeakerNightCheck.Checked;

                Settings.TurnLeftEndFlag = chkTurnLeftEndFlag.Checked;
                 Settings.TurnLeftAngle= Convert.ToDouble(edtTxtTurnLeftAngle.Text);

                Settings.TurnLeftErrorLight = chkTurnLeftErrorLight.Checked ;
                #endregion

                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region ��ת


                    new Setting { Key ="TurnLeftDistance", Value = Settings.TurnLeftDistance.ToString(), GroupName = "GlobalSettings" },
                    new Setting { Key ="TurnLeftPrepareD", Value = Settings.TurnLeftPrepareD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftSpeedLimit", Value = Settings.TurnLeftSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftBrakeSpeedUp", Value = Settings.TurnLeftBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftBrakeRequire", Value = Settings.TurnLeftBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLightCheck", Value = Settings.TurnLeftLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLoudSpeakerDayCheck", Value = Settings.TurnLeftLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLoudSpeakerNightCheck", Value = Settings.TurnLeftLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftAngle", Value = Settings.TurnLeftAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftEndFlag", Value = Settings.TurnLeftEndFlag.ToString(), GroupName = "GlobalSettings" },
 new Setting { Key ="TurnLeftErrorLight", Value = Settings.TurnLeftErrorLight.ToString(), GroupName = "GlobalSettings" },                   

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