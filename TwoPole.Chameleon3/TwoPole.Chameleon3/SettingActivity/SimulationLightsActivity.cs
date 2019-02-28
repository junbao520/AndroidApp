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
    [Activity(Label = "SimulationLightsActivity")]
    public class SimulationLightsActivity : BaseSettingActivity
    {

        #region 变量定义


        #region 灯光模拟
        EditText edtTxtLightVoice;

        EditText edtTxtLightEndVoice;
        // 灯光模拟，每个语音间隔
        EditText edtTxtSimulationLightTimeout;
        // 灯光模拟，时间间隔：单位：秒
        EditText edtTxtSimulationLightInterval;
        // 白天灯光模拟
        CheckBox chkSimulationsLightOnDay;
        // 夜间灯光模拟
        CheckBox chkSimulationsLightOnNight;
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.light);

            InitControl();
            initHeader();
            ItemCode = ExamItemCodes.Light;
            ActivityName = this.GetString(Resource.String.SimulationLightsStr);
            setMyTitle(ActivityName);
            InitSetting();
        }



        public override void InitSetting()
        {
            #region 项目语音

            edtTxtLightVoice.Text = ItemVoice;

            edtTxtLightEndVoice.Text= ItemEndVoice ;

            #endregion



            #region 灯光模拟
            edtTxtSimulationLightTimeout.Text = Settings.SimulationLightTimeout.ToString();
            edtTxtSimulationLightInterval.Text = Settings.SimulationLightInterval.ToString();
            chkSimulationsLightOnDay.Checked = Settings.SimulationsLightOnDay;
            chkSimulationsLightOnNight.Checked = Settings.SimulationsLightOnNight;
            #endregion



        }

        public void InitControl()
        {


            #region 灯光模拟
            edtTxtLightVoice = FindViewById<EditText>(Resource.Id.edtTxtLightVoice);
            edtTxtSimulationLightTimeout = FindViewById<EditText>(Resource.Id.edtTxtSimulationLightTimeout);
            edtTxtSimulationLightInterval = FindViewById<EditText>(Resource.Id.edtTxtSimulationLightInterval);
            chkSimulationsLightOnDay = FindViewById<CheckBox>(Resource.Id.chkSimulationsLightOnDay);
            chkSimulationsLightOnNight = FindViewById<CheckBox>(Resource.Id.chkSimulationsLightOnNight);

            edtTxtLightEndVoice= FindViewById<EditText>(Resource.Id.edtTxtLightEndVoice);
            #endregion


        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
            
               ItemVoice = edtTxtLightVoice.Text;

                ItemEndVoice = edtTxtLightEndVoice.Text;

                #region 灯光模拟
                Settings.SimulationLightTimeout = Convert.ToInt32(edtTxtSimulationLightTimeout.Text);
                Settings.SimulationLightInterval = Convert.ToDouble(edtTxtSimulationLightInterval.Text);
                Settings.SimulationsLightOnDay = chkSimulationsLightOnDay.Checked;
                Settings.SimulationsLightOnNight = chkSimulationsLightOnNight.Checked;
                #endregion


                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
new Setting { Key ="SimulationLightTimeout", Value = Settings.SimulationLightTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SimulationLightInterval", Value = Settings.SimulationLightInterval.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SimulationsLightOnDay", Value = Settings.SimulationsLightOnDay.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SimulationsLightOnNight", Value = Settings.SimulationsLightOnNight.ToString(), GroupName = "GlobalSettings" },
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