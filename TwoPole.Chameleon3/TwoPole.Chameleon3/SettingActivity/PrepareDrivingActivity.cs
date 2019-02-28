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
    [Activity(Label = "PrepareDrivingActivity")]
    public class PrepareDrivingActivity :BaseSettingActivity
    {
        #region 变量定义

        #region 上车准备
        EditText edtTxtPrepareDrivingVoice;
        // 绕车一周最大时间
        EditText edtTxtAroundCarTimeout;
        RadioButton radDoor;
        //
        RadioButton radSafeblet;

        //发动机和安全带
        RadioButton radEngineAndSafeBelt;

        //刹车结束
        RadioButton radBrake;

        //手动触发
        RadioButton radManualTrigger;
        // 绕车一周是否启用
        CheckBox chkAroundCarEnable;
        // 上车准备是否启用
        CheckBox chkPrepareDrivingEnable;

    
        //
        CheckBox chkAroundCarVoice;

        CheckBox chkPrepareDrivingOrder;
        // 3摸语音
        CheckBox chkPrepareDriving3TouchVoice;
        // 车尾
        CheckBox chkPrepareDrivingTailStockEnable;
        // 车头
        CheckBox chkPrepareDrivingHeadStockEnable;


       
        // 车尾2
        CheckBox chkPrepareDrivingTailStock2Enable;
        // 车头2
        CheckBox chkPrepareDrivingHeadStock2Enable;

        EditText edtTxtPrepareDrivingHeadstockVoice;

        EditText edtTxtPrepareDrivingTailstockVoice;
        //车头2语音
        EditText edtTxtPrepareDrivingHeadstock2Voice;
        //车尾2语音
        EditText edtTxtPrepareDrivingTailstock2Voice;

          
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.preparedriving);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.PrepareDriving;
            ActivityName = this.GetString(Resource.String.PrepareDrivingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


    

        public override void InitSetting()
        {
            #region 项目语音
            edtTxtPrepareDrivingVoice.Text = ItemVoice;
            #endregion

            #region 上车准备
            edtTxtAroundCarTimeout.Text = Settings.AroundCarTimeout.ToString();
            radSafeblet.Checked = Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.SafeBelt;
            radDoor.Checked = Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.Door;
            radManualTrigger.Checked= Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.ManualTrigger;
            radEngineAndSafeBelt.Checked = Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.EngineAndSafeBelt;
            radBrake.Checked =Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.Brake;
            chkAroundCarEnable.Checked = Settings.AroundCarEnable;
            chkPrepareDrivingEnable.Checked = Settings.PrepareDrivingEnable;
            chkAroundCarVoice.Checked = Settings.AroundCarVoiceEnable;
            chkPrepareDriving3TouchVoice.Checked = Settings.PrepareDriving3TouchVoice;
            chkPrepareDrivingTailStockEnable.Checked = Settings.PrepareDrivingTailStockEnable;
            chkPrepareDrivingHeadStockEnable.Checked = Settings.PrepareDrivingHeadStockEnable;

            chkPrepareDrivingTailStock2Enable.Checked = Settings.PrepareDrivingTailStock2Enable;
            chkPrepareDrivingHeadStock2Enable.Checked = Settings.PrepareDrivingHeadStock2Enable;


            chkPrepareDrivingOrder.Checked = Settings.PrepareDrivingOrder;

            edtTxtPrepareDrivingTailstockVoice.Text = Settings.PrepareDrivingTailstockVoice;
            edtTxtPrepareDrivingHeadstockVoice.Text = Settings.PrepareDrivingHeadstockVoice;

            edtTxtPrepareDrivingTailstock2Voice.Text = Settings.PrepareDrivingTailstock2Voice;
            edtTxtPrepareDrivingHeadstock2Voice.Text = Settings.PrepareDrivingHeadstock2Voice;
            #endregion

        }

        public void InitControl()
        {
            #region 上车准备
            chkAroundCarEnable = FindViewById<CheckBox>(Resource.Id.chkAroundCarEnable);
            edtTxtPrepareDrivingVoice = FindViewById<EditText>(Resource.Id.edtTxtPrepareDrivingVoice);
            edtTxtAroundCarTimeout = FindViewById<EditText>(Resource.Id.edtTxtAroundCarTimeout);
            //todo：上车准备Bug 经过车尾保存不成功 好像已经修复了 20180903 鲍君
            edtTxtPrepareDrivingHeadstockVoice= FindViewById<EditText>(Resource.Id.edtTxtPrepareDrivingHeadstockVoice);
            edtTxtPrepareDrivingTailstockVoice = FindViewById<EditText>(Resource.Id.edtTxtPrepareDrivingTailstockVoice);

            edtTxtPrepareDrivingHeadstock2Voice = FindViewById<EditText>(Resource.Id.edtTxtPrepareDrivingHeadstock2Voice);
            edtTxtPrepareDrivingTailstock2Voice = FindViewById<EditText>(Resource.Id.edtTxtPrepareDrivingTailstock2Voice);

            radBrake = FindViewById<RadioButton>(Resource.Id.radBrake);
            radDoor = FindViewById<RadioButton>(Resource.Id.radDoor);
            radSafeblet = FindViewById<RadioButton>(Resource.Id.radSafeblet);
            radEngineAndSafeBelt= FindViewById<RadioButton>(Resource.Id.radEngineAndSafeBelt);
            radManualTrigger= FindViewById<RadioButton>(Resource.Id.radManualTrigger);
            chkAroundCarEnable = FindViewById<CheckBox>(Resource.Id.chkAroundCarEnable);
            chkPrepareDrivingEnable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingEnable);
            chkAroundCarVoice = FindViewById<CheckBox>(Resource.Id.chkAroundCarVoice);
            chkPrepareDriving3TouchVoice = FindViewById<CheckBox>(Resource.Id.chkPrepareDriving3TouchVoice);
            chkPrepareDrivingTailStockEnable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingTailStockEnable);
            chkPrepareDrivingHeadStockEnable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingHeadStockEnable);

            chkPrepareDrivingTailStock2Enable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingTailStock2Enable);
            chkPrepareDrivingHeadStock2Enable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingHeadStock2Enable);


            chkPrepareDrivingOrder = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingOrder);
            #endregion

        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
                ItemVoice= edtTxtPrepareDrivingVoice.Text;
               
                #region 上车准备
                Settings.AroundCarTimeout = Convert.ToInt32(edtTxtAroundCarTimeout.Text);
                Settings.AroundCarEnable = chkAroundCarEnable.Checked;
                Settings.PrepareDrivingEnable = chkPrepareDrivingEnable.Checked;
                Settings.AroundCarVoiceEnable = chkAroundCarVoice.Checked;
                Settings.PrepareDriving3TouchVoice = chkPrepareDriving3TouchVoice.Checked;

                Settings.PrepareDrivingTailStockEnable = chkPrepareDrivingTailStockEnable.Checked;
                Settings.PrepareDrivingHeadStockEnable = chkPrepareDrivingHeadStockEnable.Checked;

                Settings.PrepareDrivingTailStock2Enable = chkPrepareDrivingTailStock2Enable.Checked;
                Settings.PrepareDrivingHeadStock2Enable = chkPrepareDrivingHeadStock2Enable.Checked;


                Settings.PrepareDrivingOrder = chkPrepareDrivingOrder.Checked;

                Settings.PrepareDrivingHeadstockVoice = edtTxtPrepareDrivingHeadstockVoice.Text;
                Settings.PrepareDrivingTailstockVoice = edtTxtPrepareDrivingTailstockVoice.Text;


                Settings.PrepareDrivingHeadstock2Voice = edtTxtPrepareDrivingHeadstock2Voice.Text;
                Settings.PrepareDrivingTailstock2Voice = edtTxtPrepareDrivingTailstock2Voice.Text;

                if (radDoor.Checked)
                {
                    Settings.PrepareDrivingEndFlag = PrepareDrivingEndFlag.Door;
                }
                else if (radSafeblet.Checked)
                {
                    Settings.PrepareDrivingEndFlag = PrepareDrivingEndFlag.SafeBelt;
                }
                else if (radEngineAndSafeBelt.Checked)
                {
                    Settings.PrepareDrivingEndFlag =  PrepareDrivingEndFlag.EngineAndSafeBelt;
                }
                else if (radManualTrigger.Checked)
                {
                    Settings.PrepareDrivingEndFlag = PrepareDrivingEndFlag.ManualTrigger;
                }
                else if (radBrake.Checked)
                {
                    Settings.PrepareDrivingEndFlag = PrepareDrivingEndFlag.Brake;
                }
                #endregion
                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
new Setting { Key ="AroundCarTimeout", Value = Settings.AroundCarTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="AroundCarEnable", Value = Settings.AroundCarEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingEnable", Value = Settings.PrepareDrivingEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="AroundCarVoiceEnable", Value = Settings.AroundCarVoiceEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDriving3TouchVoice", Value = Settings.PrepareDriving3TouchVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingTailStockEnable", Value = Settings.PrepareDrivingTailStockEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingHeadStockEnable", Value = Settings.PrepareDrivingHeadStockEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingEndFlag", Value = Settings.PrepareDrivingEndFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingOrder", Value = Settings.PrepareDrivingOrder.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingHeadstockVoice", Value = Settings.PrepareDrivingHeadstockVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingTailstockVoice", Value = Settings.PrepareDrivingTailstockVoice.ToString(), GroupName = "GlobalSettings" },
//todo:简单封装减少重复性代码 可以考虑使用反射获取之后变得更加的简单 
GetSetting("PrepareDrivingHeadstock2Voice",Settings.PrepareDrivingHeadstock2Voice),
GetSetting("PrepareDrivingTailstock2Voice",Settings.PrepareDrivingTailstock2Voice),
GetSetting("PrepareDrivingTailStock2Enable",Settings.PrepareDrivingTailStock2Enable),
GetSetting("PrepareDrivingHeadStock2Enable",Settings.PrepareDrivingHeadStock2Enable),
            };
                #endregion
                UpdateSettings(lstSetting);
                Finish();
            }
            catch (Exception ex)
            {
                string HeaderText = ActivityName + "  " + "保存失败:" + ex.Message;
                setMyTitle(HeaderText);
                speaker.SpeakActionVoice(HeaderText);
                Logger.Error(ActivityName + ex.Message);
            }

        }
    }
}