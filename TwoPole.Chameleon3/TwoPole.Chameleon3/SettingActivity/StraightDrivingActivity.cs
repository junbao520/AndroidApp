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

        #region ��������
        //

        #region ֱ����ʻ
        //ֱ����ʻ��ʼ����
        EditText edtTxtStraightDrivingVoice;
        //ֱ����ʻ��������
        EditText edtTxtStraightDrivingEndVoice;
        //ֱ����ʻ��Ŀ����
        EditText edtTxtStraightDrivingDistance;
        //ֱ����ʻ�Ƕ�
        EditText edtTxtStraightDrivingMaxOffsetAngle;
        //ֱ����ʻ����ٶ�
        EditText edtTxtStraightDrivingSpeedMaxLimit;
        //ֱ����ʻ����ٶ�
        EditText edtTxtStraightDrivingSpeedMinLimit;
        //ֱ����ʻ�ﵽһ���ٶ�
        EditText edtTxtStraightDrivingReachSpeed;
        //ֱ����ʻ׼������
        EditText edtTxtStraightDrivingPrepareDistance;
        #endregion

       
        //�ۺ�����

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//ȫ�������ޱ�������������OnCreateǰ�����á�
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
            #region ��Ŀ����
          

            edtTxtStraightDrivingVoice.Text =ItemVoice;
            edtTxtStraightDrivingEndVoice.Text =ItemEndVoice;

           
            #endregion

            #region ֱ����ʻ


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
           

            #region ֱ����ʻ
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
            //��Ҫ����Ŀ�������½������ݿ�

            //��ʵ�Ҿ�����ֻ��Ҫһ��KevValue �Ϳ�����
            //key,Value

            try
            {
                ItemVoice = edtTxtStraightDrivingVoice.Text;
                ItemEndVoice = edtTxtStraightDrivingEndVoice.Text;
             
                #region ֱ����ʻ
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
                    #region ֱ����ʻ
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
                string HeaderText = string.Format("{0}  ����ʧ�ܣ�{1}", ActivityName, ex.Message);
                setMyTitle(HeaderText);
                Logger.Error(ActivityName, ex.Message);

            }

        }
    }
}