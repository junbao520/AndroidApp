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
    [Activity(Label = "ModifiedGearActivity")]
    public class ModifiedGearActivity : BaseSettingActivity
    {

        #region 变量定义
        //

        #region 加减档
        // 加减档位-项目语音
        EditText edtTxtModifiedGearVoice;
        // 加减档位项目结束语音
        EditText edtTxtModifiedGearEndVoice;
        // 加减档位-行驶距离（单位：米）
        EditText edtTxtModifiedGearDrivingDistance;
        // 加减档位-项目超时（单位：秒）
        EditText edtTxtModifiedGearTimeout;
        //加减档位-档位最低维持时间（单位：秒）
        EditText edtTxtModifiedGearIgnoreSeconds;
        // 是否播放档位语音
        CheckBox chkModifiedGearIsPlayGearVoice;
        // 是否播放操作语音
        CheckBox chkModifiedGearIsPlayActionVoice;
        //操作完成即结束项目
         CheckBox chkModifiedGearEndFlag;
        //
        EditText edtTxtModifiedGearAddVocie;
        //
        EditText edtTxtModifiedGearSubVocie;

        EditText edtTxtModifiedGearFlowOne;
        //
        EditText edtTxtModifiedGearFlowTwo;
        //
        EditText edtTxtModifiedGearFlowThree;

        //加减档流程语音 可以考虑使用一个变量来存储
        EditText edtTxtModifiedGearAddOneTwo;
        EditText edtTxtModifiedGearAddTwoThree;
        EditText edtTxtModifiedGearAddThreeFour;
        EditText edtTxtModifiedGearAddFourFive;


        EditText edtTxtModifiedGearSubTwoOne;
        EditText edtTxtModifiedGearSubThreeTwo;
        EditText edtTxtModifiedGearSubFourThree;
        EditText edtTxtModifiedGearSubFiveFour;

        #endregion

        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.modifiedgear);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.ModifiedGear;
            ActivityName = this.GetString(Resource.String.ModifiedGearStr);
            setMyTitle(ActivityName);
            InitSetting();
        }
       
        public override void InitSetting()
        {
            #region 项目语音

            edtTxtModifiedGearVoice.Text = ItemVoice ;
            edtTxtModifiedGearEndVoice.Text = ItemEndVoice ;
            #endregion
            #region 加减档位
            edtTxtModifiedGearDrivingDistance.Text = Settings.ModifiedGearDrivingDistance.ToString();
            edtTxtModifiedGearTimeout.Text = Settings.ModifiedGearTimeout.ToString();
            edtTxtModifiedGearIgnoreSeconds.Text = Settings.ModifiedGearIgnoreSeconds.ToString();
            chkModifiedGearIsPlayGearVoice.Checked = Settings.ModifiedGearIsPlayGearVoice;
            chkModifiedGearIsPlayActionVoice.Checked = Settings.ModifiedGearIsPlayActionVoice;

            chkModifiedGearEndFlag.Checked = Settings.ChkModifiedGearEndFlag;
            //加减档位从数据库取出来
            for (int j = 0; j < Settings.ModifiedGearGearFlow.Split(';').Length; j++)
            {
                if (j == 0)
                {
                    edtTxtModifiedGearFlowOne.Text = Settings.ModifiedGearGearFlow.Split(';')[0];
                }
                else if (j == 1)
                {
                    edtTxtModifiedGearFlowTwo.Text = Settings.ModifiedGearGearFlow.Split(';')[1];
                }
                else if (j == 2)
                {
                    edtTxtModifiedGearFlowThree.Text = Settings.ModifiedGearGearFlow.Split(';')[2];
                }
            }


            ////1-2,2-3,3-4,4-5,5-4,4-3,3-2,2-1

            if (string.IsNullOrEmpty(Settings.ModifiedGearFlowVoice))
            {
                Settings.ModifiedGearFlowVoice = DefaultGlobalSettings.DefaultModifiedGearFlowVoice;
            }
            edtTxtModifiedGearAddOneTwo.Text = Settings.ModifiedGearFlowVoice.Split('-')[0];
            edtTxtModifiedGearAddTwoThree.Text = Settings.ModifiedGearFlowVoice.Split('-')[1];
            edtTxtModifiedGearAddThreeFour.Text = Settings.ModifiedGearFlowVoice.Split('-')[2];
            edtTxtModifiedGearAddFourFive.Text = Settings.ModifiedGearFlowVoice.Split('-')[3];
            edtTxtModifiedGearSubFiveFour.Text = Settings.ModifiedGearFlowVoice.Split('-')[4];
            edtTxtModifiedGearSubFourThree.Text = Settings.ModifiedGearFlowVoice.Split('-')[5];
            edtTxtModifiedGearSubThreeTwo.Text = Settings.ModifiedGearFlowVoice.Split('-')[6];
            edtTxtModifiedGearSubTwoOne.Text = Settings.ModifiedGearFlowVoice.Split('-')[7];
            #endregion

        }

        public void InitControl()
        {

            #region 加减档


            edtTxtModifiedGearVoice = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearVoice);
            edtTxtModifiedGearEndVoice = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearEndVoice);
            edtTxtModifiedGearDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearDrivingDistance);
            edtTxtModifiedGearTimeout = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearTimeout);
            edtTxtModifiedGearIgnoreSeconds = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearIgnoreSeconds);
            chkModifiedGearIsPlayGearVoice = FindViewById<CheckBox>(Resource.Id.chkModifiedGearIsPlayGearVoice);
            chkModifiedGearIsPlayActionVoice = FindViewById<CheckBox>(Resource.Id.chkModifiedGearIsPlayActionVoice);

            chkModifiedGearEndFlag = FindViewById<CheckBox>(Resource.Id.chkModifiedGearEndFlag);
            

            edtTxtModifiedGearAddVocie = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearAddVocie);
            edtTxtModifiedGearSubVocie = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearSubVocie);
            edtTxtModifiedGearFlowOne = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearFlowOne);
            edtTxtModifiedGearFlowTwo = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearFlowTwo);
            edtTxtModifiedGearFlowThree = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearFlowThree);

            edtTxtModifiedGearAddOneTwo = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearAddOneTwo);
            edtTxtModifiedGearAddTwoThree = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearAddTwoThree);
            edtTxtModifiedGearAddThreeFour = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearAddThreeFour); ;
            edtTxtModifiedGearAddFourFive = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearAddFourFive); ;


            edtTxtModifiedGearSubTwoOne = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearSubTwoOne);
            edtTxtModifiedGearSubThreeTwo = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearSubThreeTwo);
            edtTxtModifiedGearSubFourThree = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearSubFourThree);
            edtTxtModifiedGearSubFiveFour = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearSubFiveFour);
            #endregion


        }


        public override void UpdateSettings()
        {
       

            try
            {
             
                ItemVoice = edtTxtModifiedGearVoice.Text;
                ItemEndVoice = edtTxtModifiedGearEndVoice.Text;
                #region 加减档
                Settings.ModifiedGearDrivingDistance = Convert.ToInt32(edtTxtModifiedGearDrivingDistance.Text);
                Settings.ModifiedGearTimeout = Convert.ToDouble(edtTxtModifiedGearTimeout.Text);
                Settings.ModifiedGearIgnoreSeconds = Convert.ToDouble(edtTxtModifiedGearIgnoreSeconds.Text);
                Settings.ModifiedGearIsPlayGearVoice = chkModifiedGearIsPlayGearVoice.Checked;
                Settings.ModifiedGearIsPlayActionVoice = chkModifiedGearIsPlayActionVoice.Checked;

                Settings.ChkModifiedGearEndFlag = chkModifiedGearEndFlag.Checked;


                Settings.ModifiedGearFlowVoice = edtTxtModifiedGearAddOneTwo.Text + "-" + edtTxtModifiedGearAddTwoThree.Text + "-" + edtTxtModifiedGearAddThreeFour.Text + "-" + edtTxtModifiedGearAddFourFive.Text + "-"
                                               + edtTxtModifiedGearSubFiveFour.Text + "-" + edtTxtModifiedGearSubFourThree.Text + "-" + edtTxtModifiedGearSubThreeTwo.Text + "-" + edtTxtModifiedGearSubTwoOne.Text;

                if (!string.IsNullOrEmpty(edtTxtModifiedGearFlowOne.Text))
                {
                    Settings.ModifiedGearGearFlow = edtTxtModifiedGearFlowOne.Text;
                }
                if (!string.IsNullOrEmpty(edtTxtModifiedGearFlowTwo.Text))
                {
                    Settings.ModifiedGearGearFlow = Settings.ModifiedGearGearFlow + ";" + edtTxtModifiedGearFlowTwo.Text;
                }
                if (!string.IsNullOrEmpty(edtTxtModifiedGearFlowThree.Text))
                {
                    Settings.ModifiedGearGearFlow = Settings.ModifiedGearGearFlow + ";" + edtTxtModifiedGearFlowThree.Text;
                }
                #endregion
             lstSetting = new List<Setting>() {
                    new Setting { Key="ModifiedGearDrivingDistance",Value=Settings.ModifiedGearDrivingDistance.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="ModifiedGearTimeout",Value=Settings.ModifiedGearTimeout.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="ModifiedGearIgnoreSeconds",Value=Settings.ModifiedGearIgnoreSeconds.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="ModifiedGearIsPlayGearVoice",Value=Settings.ModifiedGearIsPlayGearVoice.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="ModifiedGearIsPlayActionVoice",Value=Settings.ModifiedGearIsPlayActionVoice.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="ModifiedGearFlowVoice",Value=Settings.ModifiedGearFlowVoice.ToString(),GroupName="GlobalSettings"},
                    new Setting { Key="ModifiedGearGearFlow",Value=Settings.ModifiedGearGearFlow.ToString(),GroupName="GlobalSettings"},
                     new Setting { Key="ChkModifiedGearEndFlag",Value=Settings.ChkModifiedGearEndFlag.ToString(),GroupName="GlobalSettings"}
                };
                UpdateSettings(lstSetting);
                Finish();
            }
            catch (Exception ex)
            {
                string TitleText= ActivityName + "  " + "保存失败:" + ex.Message;
                setMyTitle(TitleText);
                speaker.SpeakActionVoice(TitleText);
                Logger.Error("ModifiedGearActivity" + ex.Message);
            }

        }
    }
}