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
using System.ComponentModel;
using TwoPole.Chameleon3.Infrastructure.Devices.CarSignal;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "SensorSetting")]
    public class SensorSetting : BaseSettingActivity
    {

        //主机信号信号来源
        RadioButton radSignalSourceSimulatedData;
        RadioButton radSignalSourceUSB;
        RadioButton radSignalSourceWifiUdp;
        RadioButton radSignalSourceBluetooth;

        //角度来源
        RadioButton radAngleSourceGyroscope;
        RadioButton radAngleSourceGps;


        //里程来源
        RadioButton radMileageSourceGps;
        RadioButton radMileageSourceOBD;
        //速度来源
        RadioButton radSpeedSourceGps;
        RadioButton radSpeedSourceOBD;
        //转速来源
        RadioButton radEngineRpmPlus;
        RadioButton radEngineRpmOBD;
        //档位来源
        RadioButton radGearSourceGearDisplay;
        RadioButton radGearSourceRatio;
        RadioButton radGearSourceOBD;
        CheckBox chkNeutral;

        //车型
        RadioButton radCarTypeYiDong;
        RadioButton radCarTypeXinShang;
        RadioButton radCarTypeXinJieDa;
        RadioButton radCarTypeAiLiShe;

        //转速比
        EditText edtTxtGearOneLowEngineRpm;
        EditText edtTxtGearOneHighEngineRpm;
        EditText edtTxtGearTwoLowEngineRpm;
        EditText edtTxtGearTwoHighEngineRpm;
        EditText edtTxtGearThreeLowEngineRpm;
        EditText edtTxtGearThreeHighEngineRpm;
        EditText edtTxtGearFourLowEngineRpm;
        EditText edtTxtGearFourHighEngineRpm;
        EditText edtTxtGearFiveLowEngineRpm;
        EditText edtTxtGearFiveHighEngineRpm;
        //空挡传感器地址

        Spinner spinnerNeturalAddress;
        Button btnSearchBluetooth;
        Spinner spinnerBluetooth;
        EditText edtTxtBluetoothAddress;
        private bool isSearch = false;
        List<KeyValuePair<string, string>> lstBluetoothSource = new List<KeyValuePair<string, string>>();
        List<string> lstSignalAddress = new List<string> {"OBD","默认", "R-转速", "S-脉冲", "1-雾灯", "2-近光", "3-远光", "4-喇叭", "5-左转", "6-右转", "7-小灯"
        ,"8-刹车","9-车门","10-离合","11-安全带","12-手刹","13-车头","14-车尾","15-空挡","16-扩展0","17-扩展1","18-扩展2","19-扩展3","20-扩展4","21-扩展5","22-扩展6"};


        //转速比学习按钮

        Button btnGearStudyOne;
        Button btnGearStudyTwo;
        Button btnGearStudyThree;
        Button btnGearStudyFour;
        Button btnGearStudyFive;
        public LearnEnginRadio learnEnginRadio { get; set; }
        public int EngineRatio
        {
            set
            {
                if (value > 0 && learnEnginRadio != LearnEnginRadio.None)
                {
                    switch (learnEnginRadio)
                    {
                        case LearnEnginRadio.One:
                            if (GetEdtTxtValue(edtTxtGearOneLowEngineRpm) > value || GetEdtTxtValue(edtTxtGearOneLowEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearOneLowEngineRpm, value);
                            if (GetEdtTxtValue(edtTxtGearOneHighEngineRpm) < value || GetEdtTxtValue(edtTxtGearOneHighEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearOneHighEngineRpm, value);
                            break;
                        case LearnEnginRadio.Two:
                            if (GetEdtTxtValue(edtTxtGearTwoLowEngineRpm) > value || GetEdtTxtValue(edtTxtGearTwoLowEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearTwoLowEngineRpm, value);
                            if (GetEdtTxtValue(edtTxtGearTwoHighEngineRpm) < value || GetEdtTxtValue(edtTxtGearTwoHighEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearTwoHighEngineRpm, value);
                            break;
                        case LearnEnginRadio.Three:
                            if (GetEdtTxtValue(edtTxtGearThreeLowEngineRpm) > value || GetEdtTxtValue(edtTxtGearThreeLowEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearThreeLowEngineRpm, value);
                            if (GetEdtTxtValue(edtTxtGearThreeHighEngineRpm) < value || GetEdtTxtValue(edtTxtGearThreeHighEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearThreeHighEngineRpm, value);
                            break;
                        case LearnEnginRadio.Four:
                            if (GetEdtTxtValue(edtTxtGearFourLowEngineRpm) > value || GetEdtTxtValue(edtTxtGearFourLowEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearFourLowEngineRpm, value);
                            if (GetEdtTxtValue(edtTxtGearFourHighEngineRpm) < value || GetEdtTxtValue(edtTxtGearFourHighEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearFourHighEngineRpm, value);
                            break;
                        case LearnEnginRadio.Five:
                            if (GetEdtTxtValue(edtTxtGearFiveLowEngineRpm) > value || GetEdtTxtValue(edtTxtGearFiveLowEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearFiveLowEngineRpm, value);
                            if (GetEdtTxtValue(edtTxtGearFiveHighEngineRpm) < value || GetEdtTxtValue(edtTxtGearFiveHighEngineRpm) == 0)
                                SetEdtTxtValue(edtTxtGearFiveHighEngineRpm, value);
                            break;

                    }
                }
            }
        }

        public int GetEdtTxtValue(EditText edtTxt)
        {
            return Convert.ToInt32(edtTxt.Text);
        }
        public void SetEdtTxtValue(EditText edtTxt, int Value)
        {
            RunOnUiThread(()=> {
                edtTxt.Text = Value.ToString();
            });
            
        }
        protected void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
        }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            if (message.CarSignal == null)
            {
                return;
            }
            if (message.CarSignal.EngineRatio > 0)
            {
                this.EngineRatio = message.CarSignal.EngineRatio;
            }


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SensorSetting);
            InitControl();
            initHeader();
            ActivityName = this.GetString(Resource.String.SensorSettingStr);
            setMyTitle(ActivityName);
            InitSetting();
            RegisterMessages(messager);
        }

        public override void InitSetting()
        {
            //Settings.masterControlBoxVersion

            radSignalSourceBluetooth.Checked = Settings.masterControlBoxVersion == MasterControlBoxVersion.Bluetooth;
            radSignalSourceUSB.Checked = Settings.masterControlBoxVersion == MasterControlBoxVersion.USB;

            radSignalSourceWifiUdp.Checked = Settings.masterControlBoxVersion == MasterControlBoxVersion.WifiUdp;
            radSignalSourceSimulatedData.Checked = Settings.masterControlBoxVersion == MasterControlBoxVersion.SimulatedData;


            radMileageSourceGps.Checked = Settings.MileageSource == MileageSource.GPS;
            radMileageSourceOBD.Checked = Settings.MileageSource == MileageSource.OBD;


            radAngleSourceGps.Checked = Settings.AngleSource == AngleSource.GPS;
            radAngleSourceGyroscope.Checked = Settings.AngleSource == AngleSource.Gyroscope;

            radGearSourceOBD.Checked = Settings.GearSource == GearSource.OBD;
            radGearSourceRatio.Checked = Settings.GearSource == GearSource.SpeadRadio;
            radGearSourceGearDisplay.Checked = Settings.GearSource == GearSource.GearDisplay;


            radSpeedSourceOBD.Checked = Settings.CheckOBD;
            radSpeedSourceGps.Checked = !Settings.CheckOBD;

            radEngineRpmOBD.Checked = Settings.CheckOBDRpm;
            radEngineRpmPlus.Checked = !Settings.CheckOBDRpm;

            chkNeutral.Checked = Settings.NeutralStart;


            //radConnectionSchemeDefalut.Checked = Settings.ConnectionScheme == ConnectionScheme.Acquiesce;
            //radConnectionSchemeNewMulberry.Checked = Settings.ConnectionScheme == ConnectionScheme.NewMulberry;
            //radConnectionSchemeNewJetta.Checked = Settings.ConnectionScheme == ConnectionScheme.Jetta;
            //radConnectionSchemeOBDElysee.Checked= Settings.ConnectionScheme == ConnectionScheme.Elysee;

            radCarTypeYiDong.Checked = Settings.CarType == CarType.YiDong;
            radCarTypeXinShang.Checked = Settings.CarType == CarType.XinShang;
            radCarTypeXinJieDa.Checked = Settings.CarType == CarType.XinJieDa;
            radCarTypeAiLiShe.Checked = Settings.CarType == CarType.AiLiShe;



            //
            edtTxtBluetoothAddress.Text = Settings.BluetoothName;

            BindSpinner(lstSignalAddress, spinnerNeturalAddress);

            spinnerNeturalAddress.SetSelection(Settings.IsNeutralAddress + 2, true);

            radCarTypeAiLiShe.CheckedChange += RadCarTypeAiLiShe_CheckedChange;
            radCarTypeXinJieDa.CheckedChange += RadCarTypeXinJieDa_CheckedChange;
            radCarTypeXinShang.CheckedChange += RadCarTypeXinShang_CheckedChange;
            radCarTypeYiDong.CheckedChange += RadCarTypeYiDong_CheckedChange;


            edtTxtGearOneLowEngineRpm.Text = Settings.GearOneMinRatio.ToString();
            edtTxtGearOneHighEngineRpm.Text = Settings.GearOneMaxRatio.ToString();

            edtTxtGearTwoLowEngineRpm.Text = Settings.GearTwoMinRatio.ToString();
            edtTxtGearTwoHighEngineRpm.Text = Settings.GearTwoMaxRatio.ToString();

            edtTxtGearThreeLowEngineRpm.Text = Settings.GearThreeMinRatio.ToString();
            edtTxtGearThreeHighEngineRpm.Text = Settings.GearThreeMaxRatio.ToString();

            edtTxtGearFourLowEngineRpm.Text = Settings.GearFourMinRatio.ToString();
            edtTxtGearFourHighEngineRpm.Text = Settings.GearFourMaxRatio.ToString();

            edtTxtGearFiveLowEngineRpm.Text = Settings.GearFiveMinRatio.ToString();
            edtTxtGearFiveHighEngineRpm.Text = Settings.GearFiveMaxRatio.ToString();

            //初始化设置蓝牙
            //初始化暂时不管

        }
        //List<List<string>> lst = new { List<string> {"1" },List<string> {"2" } };
        //逸动 新桑塔纳 新捷达 爱丽舍
        //其实这个值㤇
        List<List<int>> lstEngineRatioSource = new List<List<int>> { new List<int> { 120, 75, 70, 90, 45, 56, 37, 44, 30, 35 }, new List<int> { 118, 145, 68, 79, 43, 46, 31, 34, 25, 26 }, new List<int> { 118, 145, 68, 79, 43, 46, 31, 34, 25, 26 }, new List<int> { 140, 190, 69, 86, 50, 56, 37, 42, 28, 32 } };
        private void RadCarTypeYiDong_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            //需要开始的时候 不要进行数据值的初始化
            if (radCarTypeYiDong.Checked)
            {
                edtTxtGearOneLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][0].ToString();
                edtTxtGearOneHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][1].ToString(); ;

                edtTxtGearTwoLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][2].ToString(); ;
                edtTxtGearTwoHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][3].ToString(); ;

                edtTxtGearThreeLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][4].ToString(); ;
                edtTxtGearThreeHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][5].ToString(); ;

                edtTxtGearFourLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][6].ToString(); ;
                edtTxtGearFourHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][7].ToString(); ;

                edtTxtGearFiveLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][8].ToString(); ;
                edtTxtGearFiveHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.YiDong)][9].ToString(); ;
            }
        }

        private void RadCarTypeXinShang_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radCarTypeXinShang.Checked)
            {
                edtTxtGearOneLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][0].ToString();
                edtTxtGearOneHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][1].ToString(); ;

                edtTxtGearTwoLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][2].ToString(); ;
                edtTxtGearTwoHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][3].ToString(); ;

                edtTxtGearThreeLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][4].ToString(); ;
                edtTxtGearThreeHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][5].ToString(); ;

                edtTxtGearFourLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][6].ToString(); ;
                edtTxtGearFourHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][7].ToString(); ;

                edtTxtGearFiveLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][8].ToString(); ;
                edtTxtGearFiveHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinShang)][9].ToString(); ;
            }
        }

        private void RadCarTypeXinJieDa_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radCarTypeXinJieDa.Checked)
            {
                edtTxtGearOneLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][0].ToString();
                edtTxtGearOneHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][1].ToString(); ;

                edtTxtGearTwoLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][2].ToString(); ;
                edtTxtGearTwoHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][3].ToString(); ;

                edtTxtGearThreeLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][4].ToString(); ;
                edtTxtGearThreeHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][5].ToString(); ;

                edtTxtGearFourLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][6].ToString(); ;
                edtTxtGearFourHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][7].ToString(); ;

                edtTxtGearFiveLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][8].ToString(); ;
                edtTxtGearFiveHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.XinJieDa)][9].ToString(); ;
            }
        }

        private void RadCarTypeAiLiShe_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radCarTypeAiLiShe.Checked)
            {
                edtTxtGearOneLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][0].ToString();
                edtTxtGearOneHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][1].ToString(); ;

                edtTxtGearTwoLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][2].ToString(); ;
                edtTxtGearTwoHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][3].ToString(); ;

                edtTxtGearThreeLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][4].ToString(); ;
                edtTxtGearThreeHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][5].ToString(); ;

                edtTxtGearFourLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][6].ToString(); ;
                edtTxtGearFourHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][7].ToString(); ;

                edtTxtGearFiveLowEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][8].ToString(); ;
                edtTxtGearFiveHighEngineRpm.Text = lstEngineRatioSource[Convert.ToInt16(CarType.AiLiShe)][9].ToString(); ;
            }
        }

        public override void UpdateSettings()
        {

            try
            {
                if (radSignalSourceBluetooth.Checked)
                {
                    Settings.masterControlBoxVersion = MasterControlBoxVersion.Bluetooth;
                }
                else if (radSignalSourceUSB.Checked)
                {
                    Settings.masterControlBoxVersion = MasterControlBoxVersion.USB;
                }
                else if (radSignalSourceWifiUdp.Checked)
                {
                    Settings.masterControlBoxVersion = MasterControlBoxVersion.WifiUdp;
                }
                else if (radSignalSourceSimulatedData.Checked)
                {
                    Settings.masterControlBoxVersion = MasterControlBoxVersion.SimulatedData;
                }
                else
                {
                    Settings.masterControlBoxVersion = MasterControlBoxVersion.SimulatedData;
                }



                Settings.MileageSource = radMileageSourceGps.Checked ? MileageSource.GPS : MileageSource.OBD;
                Settings.AngleSource = radAngleSourceGps.Checked ? AngleSource.GPS : AngleSource.Gyroscope;

                Settings.GearSource = radGearSourceOBD.Checked ? GearSource.OBD : radGearSourceRatio.Checked ? GearSource.SpeadRadio : GearSource.GearDisplay;


                Settings.CheckOBDRpm = radEngineRpmOBD.Checked;
                Settings.CheckOBD = radSpeedSourceOBD.Checked;
                Settings.NeutralStart = chkNeutral.Checked;

                if (radCarTypeAiLiShe.Checked)
                {
                    Settings.CarType = CarType.AiLiShe;
                }
                else if (radCarTypeXinJieDa.Checked)
                {
                    Settings.CarType = CarType.XinJieDa;
                }
                else if (radCarTypeXinShang.Checked)
                {
                    Settings.CarType = CarType.XinShang;
                }
                else if (radCarTypeYiDong.Checked)
                {
                    Settings.CarType = CarType.YiDong;
                }
                Settings.GearOneMinRatio = Convert.ToInt32(edtTxtGearOneLowEngineRpm.Text);
                Settings.GearOneMaxRatio = Convert.ToInt32(edtTxtGearOneHighEngineRpm.Text);
                Settings.GearTwoMinRatio = Convert.ToInt32(edtTxtGearTwoLowEngineRpm.Text);
                Settings.GearTwoMaxRatio = Convert.ToInt32(edtTxtGearTwoHighEngineRpm.Text);
                Settings.GearThreeMinRatio = Convert.ToInt32(edtTxtGearThreeLowEngineRpm.Text);
                Settings.GearThreeMaxRatio = Convert.ToInt32(edtTxtGearThreeHighEngineRpm.Text);
                Settings.GearFourMinRatio = Convert.ToInt32(edtTxtGearFourLowEngineRpm.Text);
                Settings.GearFourMaxRatio = Convert.ToInt32(edtTxtGearFourHighEngineRpm.Text);
                Settings.GearFiveMinRatio = Convert.ToInt32(edtTxtGearFiveLowEngineRpm.Text);
                Settings.GearFiveMaxRatio = Convert.ToInt32(edtTxtGearFiveHighEngineRpm.Text);

                Settings.IsNeutralAddress = spinnerNeturalAddress.SelectedItemPosition - 2;
                Settings.BluetoothAddress = edtTxtBluetoothAddress.Text.ToString();
                Settings.BluetoothName = edtTxtBluetoothAddress.Text.ToString();
                #region listSetting
                List<Setting> lstSetting = new List<Setting>
                {
new Setting { Key ="masterControlBoxVersion", Value = Settings.masterControlBoxVersion.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MileageSource", Value = Settings.MileageSource.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="AngleSource", Value = Settings.AngleSource.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearSource", Value = Settings.GearSource.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CheckOBDRpm", Value = Settings.CheckOBDRpm.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CheckOBD", Value = Settings.CheckOBD.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="NeutralStart", Value = Settings.NeutralStart.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CarType", Value = Settings.CarType.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearOneMinRatio", Value = Settings.GearOneMinRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearOneMaxRatio", Value = Settings.GearOneMaxRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearTwoMinRatio", Value = Settings.GearTwoMinRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearTwoMaxRatio", Value = Settings.GearTwoMaxRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearThreeMinRatio", Value = Settings.GearThreeMinRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearThreeMaxRatio", Value = Settings.GearThreeMaxRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearFourMinRatio", Value = Settings.GearFourMinRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearFourMaxRatio", Value = Settings.GearFourMaxRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearFiveMinRatio", Value = Settings.GearFiveMinRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearFiveMaxRatio", Value = Settings.GearFiveMaxRatio.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IsNeutralAddress", Value = Settings.IsNeutralAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BluetoothAddress", Value = Settings.BluetoothAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BluetoothName", Value = Settings.BluetoothName.ToString(), GroupName = "GlobalSettings" },

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
        public void InitControl()
        {
            radSignalSourceSimulatedData = FindViewById<RadioButton>(Resource.Id.radSignalSourceSimulatedData);
            radSignalSourceUSB = FindViewById<RadioButton>(Resource.Id.radSignalSourceUSB);
            radSignalSourceWifiUdp = FindViewById<RadioButton>(Resource.Id.radSignalSourceWifiUdp);

            radSignalSourceBluetooth = FindViewById<RadioButton>(Resource.Id.radSignalSourceBluetooth);
            radAngleSourceGyroscope = FindViewById<RadioButton>(Resource.Id.radAngleSourceGyroscope);
            radAngleSourceGps = FindViewById<RadioButton>(Resource.Id.radAngleSourceGps);
            radMileageSourceGps = FindViewById<RadioButton>(Resource.Id.radMileageSourceGps);
            radMileageSourceOBD = FindViewById<RadioButton>(Resource.Id.radMileageSourceOBD);
            radSpeedSourceGps = FindViewById<RadioButton>(Resource.Id.radSpeedSourceGps);
            radSpeedSourceOBD = FindViewById<RadioButton>(Resource.Id.radSpeedSourceOBD);
            radEngineRpmPlus = FindViewById<RadioButton>(Resource.Id.radEngineRpmPlus);
            radEngineRpmOBD = FindViewById<RadioButton>(Resource.Id.radEngineRpmOBD);
            radGearSourceGearDisplay = FindViewById<RadioButton>(Resource.Id.radGearSourceGearDisplay);
            radGearSourceRatio = FindViewById<RadioButton>(Resource.Id.radGearSourceRatio);
            radGearSourceOBD = FindViewById<RadioButton>(Resource.Id.radGearSourceOBD);
            chkNeutral = FindViewById<CheckBox>(Resource.Id.chkNeutral);

            radCarTypeAiLiShe = FindViewById<RadioButton>(Resource.Id.radCarTypeAiLiShe);
            radCarTypeXinJieDa = FindViewById<RadioButton>(Resource.Id.radCarTypeXinJieDa);
            radCarTypeXinShang = FindViewById<RadioButton>(Resource.Id.radCarTypeXinShang);
            radCarTypeYiDong = FindViewById<RadioButton>(Resource.Id.radCarTypeYiDong);

            //radConnectionSchemeDefalut = FindViewById<RadioButton>(Resource.Id.radConnectionSchemeDefalut);
            //radConnectionSchemeNewMulberry= FindViewById<RadioButton>(Resource.Id.radConnectionSchemeNewMulberry);
            //radConnectionSchemeNewJetta= FindViewById<RadioButton>(Resource.Id.radConnectionSchemeNewJetta);
            //radConnectionSchemeOBDElysee= FindViewById<RadioButton>(Resource.Id.radConnectionSchemeOBDElysee);


            edtTxtGearOneLowEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearOneLowEngineRpm);
            edtTxtGearOneHighEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearOneHighEngineRpm);
            edtTxtGearTwoLowEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearTwoLowEngineRpm);
            edtTxtGearTwoHighEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearTwoHighEngineRpm);
            edtTxtGearThreeLowEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearThreeLowEngineRpm);
            edtTxtGearThreeHighEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearThreeHighEngineRpm);
            edtTxtGearFourLowEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearFourLowEngineRpm);
            edtTxtGearFourHighEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearFourHighEngineRpm);

            edtTxtGearFiveLowEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearFiveLowEngineRpm);
            edtTxtGearFiveHighEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtGearFiveHighEngineRpm);

            spinnerNeturalAddress = (Spinner)FindViewById(Resource.Id.spinnerNeturalAddress);
            spinnerBluetooth = (Spinner)FindViewById(Resource.Id.spinnerBluetooth);
            btnSearchBluetooth = FindViewById<Button>(Resource.Id.btnSearchBluetooth);
            edtTxtBluetoothAddress = FindViewById<EditText>(Resource.Id.edtTxtBluetoothAddress);

            btnGearStudyOne = FindViewById<Button>(Resource.Id.btnStudyGearOne);
            btnGearStudyTwo = FindViewById<Button>(Resource.Id.btnStudyGearTwo);
            btnGearStudyThree = FindViewById<Button>(Resource.Id.btnStudyGearThree);
            btnGearStudyFour = FindViewById<Button>(Resource.Id.btnStudyGearFour);
            btnGearStudyFive = FindViewById<Button>(Resource.Id.btnStudyGearFive);

            btnSearchBluetooth.Click += BtnSearchBluetooth_Click;
            spinnerBluetooth.ItemSelected += SpinnerBluetooth_ItemSelected;
            btnGearStudyOne.Click += BtnGearStudyOne_Click;
            btnGearStudyTwo.Click += BtnGearStudyTwo_Click;
            btnGearStudyThree.Click += BtnGearStudyThree_Click;
            btnGearStudyFour.Click += BtnGearStudyFour_Click;
            btnGearStudyFive.Click += BtnGearStudyFive_Click;



        }


        private void BtnGearStudyFive_Click(object sender, EventArgs e)
        {
            if (btnGearStudyFive.Text == "学习")
            {
                btnGearStudyOne.Enabled = false;
                btnGearStudyTwo.Enabled = false;
                btnGearStudyFour.Enabled = false;
                btnGearStudyThree.Enabled = false;

                SetEdtTxtValue(edtTxtGearFiveLowEngineRpm, 0);
                SetEdtTxtValue(edtTxtGearFiveHighEngineRpm, 0);
                learnEnginRadio = LearnEnginRadio.Five;
                btnGearStudyFive.Text = "完成";

            }
            else if (btnGearStudyFive.Text == "完成")
            {
                btnGearStudyOne.Enabled = true;
                btnGearStudyTwo.Enabled = true;
                btnGearStudyThree.Enabled = true;
                btnGearStudyFour.Enabled = true;
                btnGearStudyFive.Enabled = true;

                btnGearStudyFive.Text = "学习";

                SetEdtTxtValue(edtTxtGearFiveLowEngineRpm, GetEdtTxtValue(edtTxtGearFiveLowEngineRpm) == 0 ? 0 : GetEdtTxtValue(edtTxtGearFiveLowEngineRpm) - 1);
                SetEdtTxtValue(edtTxtGearFiveHighEngineRpm, GetEdtTxtValue(edtTxtGearFiveHighEngineRpm) + 1);

                learnEnginRadio = LearnEnginRadio.None;
            }

        }

        private void BtnGearStudyFour_Click(object sender, EventArgs e)
        {
            if (btnGearStudyFour.Text == "学习")
            {
                btnGearStudyOne.Enabled = false;
                btnGearStudyTwo.Enabled = false;
                btnGearStudyThree.Enabled = false;
                btnGearStudyFive.Enabled = false;

                SetEdtTxtValue(edtTxtGearFourLowEngineRpm, 0);
                SetEdtTxtValue(edtTxtGearFourHighEngineRpm, 0);
                learnEnginRadio = LearnEnginRadio.Four;
                btnGearStudyFour.Text = "完成";

            }
            else if (btnGearStudyFour.Text == "完成")
            {
                btnGearStudyOne.Enabled = true;
                btnGearStudyTwo.Enabled = true;
                btnGearStudyThree.Enabled = true;
                btnGearStudyFour.Enabled = true;
                btnGearStudyFive.Enabled = true;

                btnGearStudyFour.Text = "学习";

                SetEdtTxtValue(edtTxtGearFourLowEngineRpm, GetEdtTxtValue(edtTxtGearFourLowEngineRpm) == 0 ? 0 : GetEdtTxtValue(edtTxtGearFourLowEngineRpm) - 1);
                SetEdtTxtValue(edtTxtGearFourHighEngineRpm, GetEdtTxtValue(edtTxtGearFourHighEngineRpm) + 1);

                learnEnginRadio = LearnEnginRadio.None;
            }
        }

        private void BtnGearStudyThree_Click(object sender, EventArgs e)
        {
            if (btnGearStudyThree.Text == "学习")
            {
                btnGearStudyOne.Enabled = false;
                btnGearStudyTwo.Enabled = false;
                btnGearStudyFour.Enabled = false;
                btnGearStudyFive.Enabled = false;

                SetEdtTxtValue(edtTxtGearThreeLowEngineRpm, 0);
                SetEdtTxtValue(edtTxtGearThreeHighEngineRpm, 0);
                learnEnginRadio = LearnEnginRadio.Three;
                btnGearStudyThree.Text = "完成";

            }
            else if (btnGearStudyThree.Text == "完成")
            {
                btnGearStudyOne.Enabled = true;
                btnGearStudyTwo.Enabled = true;
                btnGearStudyThree.Enabled = true;
                btnGearStudyFour.Enabled = true;
                btnGearStudyFive.Enabled = true;

                btnGearStudyThree.Text = "学习";

                SetEdtTxtValue(edtTxtGearThreeLowEngineRpm, GetEdtTxtValue(edtTxtGearThreeLowEngineRpm) == 0 ? 0 : GetEdtTxtValue(edtTxtGearThreeLowEngineRpm) - 1);
                SetEdtTxtValue(edtTxtGearThreeHighEngineRpm, GetEdtTxtValue(edtTxtGearThreeHighEngineRpm) + 1);

                learnEnginRadio = LearnEnginRadio.None;
            }
        }

        private void BtnGearStudyTwo_Click(object sender, EventArgs e)
        {
            if (btnGearStudyTwo.Text == "学习")
            {
                btnGearStudyOne.Enabled = false;
                btnGearStudyThree.Enabled = false;
                btnGearStudyFour.Enabled = false;
                btnGearStudyFive.Enabled = false;

                SetEdtTxtValue(edtTxtGearTwoLowEngineRpm, 0);
                SetEdtTxtValue(edtTxtGearTwoHighEngineRpm, 0);
                learnEnginRadio = LearnEnginRadio.Two;
                btnGearStudyTwo.Text = "完成";

            }
            else if (btnGearStudyTwo.Text == "完成")
            {
                btnGearStudyOne.Enabled = true;
                btnGearStudyTwo.Enabled = true;
                btnGearStudyThree.Enabled = true;
                btnGearStudyFour.Enabled = true;
                btnGearStudyFive.Enabled = true;

                btnGearStudyTwo.Text = "学习";

                SetEdtTxtValue(edtTxtGearTwoLowEngineRpm, GetEdtTxtValue(edtTxtGearTwoLowEngineRpm) == 0 ? 0 : GetEdtTxtValue(edtTxtGearTwoLowEngineRpm) - 1);
                SetEdtTxtValue(edtTxtGearTwoHighEngineRpm, GetEdtTxtValue(edtTxtGearTwoHighEngineRpm) + 1);

                learnEnginRadio = LearnEnginRadio.None;
            }
        }

        private void BtnGearStudyOne_Click(object sender, EventArgs e)
        {
            if (btnGearStudyOne.Text == "学习")
            {
                btnGearStudyTwo.Enabled = false;
                btnGearStudyThree.Enabled = false;
                btnGearStudyFour.Enabled = false;
                btnGearStudyFive.Enabled = false;

                SetEdtTxtValue(edtTxtGearOneLowEngineRpm, 0);
                SetEdtTxtValue(edtTxtGearOneHighEngineRpm, 0);
                learnEnginRadio = LearnEnginRadio.One;
                btnGearStudyOne.Text = "完成";

            }
            else if (btnGearStudyOne.Text == "完成")
            {
                btnGearStudyOne.Enabled = true;
                btnGearStudyTwo.Enabled = true;
                btnGearStudyThree.Enabled = true;
                btnGearStudyFour.Enabled = true;
                btnGearStudyFive.Enabled = true;

                btnGearStudyOne.Text = "学习";

                SetEdtTxtValue(edtTxtGearOneLowEngineRpm, GetEdtTxtValue(edtTxtGearOneLowEngineRpm) == 0 ? 0 : GetEdtTxtValue(edtTxtGearOneLowEngineRpm) - 1);
                SetEdtTxtValue(edtTxtGearOneHighEngineRpm, GetEdtTxtValue(edtTxtGearOneHighEngineRpm) + 1);

                learnEnginRadio = LearnEnginRadio.None;
            }

        }

        public enum LearnEnginRadio
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        private void SpinnerBluetooth_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //Settings.BluetoothName= lstBluetoothSource[e.Position].Key;
            //Settings.BluetoothAddress = lstBluetoothSource[e.Position].Value;
            if (isSearch)
            {
                edtTxtBluetoothAddress.Text = lstBluetoothSource[e.Position].Value;
            }

        }

        private void BtnSearchBluetooth_Click(object sender, EventArgs e)
        {
            //点击进行搜索蓝牙

            lstBluetoothSource = BluetoothCarSignalSeed.GetPairBluetooth();
            List<string> lstBluetooth = new List<string>();

            foreach (var item in lstBluetoothSource)
            {
                lstBluetooth.Add(item.Key);
            }

            BindSpinner(lstBluetooth, spinnerBluetooth);
            isSearch = true;
        }

        public enum SignalAddress
        {
            [Description("默认")]
            Default = -1,
            [Description("OBD")]
            DefaultOBD = -2,
            [Description("R-转速")]
            EngineRpm = 0,
            [Description("S-脉冲")]
            Speed = 1,
            [Description("1-雾灯")]
            FogLight = 2,
            [Description("2-近光")]
            LowBeam = 3,
            [Description("3-远光")]
            HighBeam = 4,
            [Description("4-喇叭")]
            Loudspeaker = 5,
            [Description("5-左转")]
            LeftIndicatorLight = 6,
            [Description("6-右转")]
            RightIndicatorLight = 7,
            [Description("7-小灯")]
            OutlineLight = 8,
            [Description("8-刹车")]
            Brake = 9,
            [Description("9-车门")]
            Door = 10,
            [Description("10-离合")]
            Clutch = 11,
            [Description("11-安全带")]
            SafetyBelt = 12,
            [Description("12-手刹")]
            Handbrake = 13,
            [Description("13-车头")]
            Headstock = 14,
            [Description("14-车尾")]
            Tailstock = 15,
            [Description("15-空挡")]
            Neutral = 16,
            [Description("16-扩展0")]
            Ext0 = 17,
            [Description("17-扩展1")]
            Ext1 = 18,
            [Description("18-扩展2")]
            Ext2 = 19,
            [Description("19-扩展3")]
            Ext3 = 20,
            [Description("20-扩展4")]
            Ext4 = 21,
            [Description("21-扩展5")]
            Ext5 = 22,
            [Description("22-扩展6")]
            Ext6 = 23
        }

        private void BindSpinner(List<string> lstDataSource, Spinner spinner)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstDataSource);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.Visibility = ViewStates.Visible;

        }


        private void NoWiringCheck(int value)
        {
            //近光灯不能从OBD读
            Settings.LowBeamAddress = -1;
            Settings.FogLightAddress = value;
            Settings.LoudspeakerAddress = value;
            Settings.DoorAddress = value;
            Settings.HandbrakeAddress = value;
            Settings.ClutchAddress = value;
            Settings.BrakeAddress = value;
            Settings.HighBeamAddress = value;
            Settings.OutlineLightAddress = value;
            Settings.ReversingLightAddress = value;
            Settings.SafetyBeltAddress = value;
            Settings.LeftIndicatorLightAddress = value;
            Settings.RightIndicatorLightAddress = value;


        }

        private void NoWiring_Alise_Check(int value)
        {

            //this.cmbFogLightAddress.SelectedValue = (int)SignalAddress.Default;
            //this.cmbLoudspeakerAddress.SelectedValue = (int)SignalAddress.Default;
            //this.cmbOutlineLightAddress.SelectedValue = (int)SignalAddress.Default;
            //this.cmbDoorAddress.SelectedValue = value;
            //this.cmbHandbrakeAddress.SelectedValue = value;
            //this.cmbClutchAddress.SelectedValue = value;
            //this.cmbBrakeAddress.SelectedValue = value;
            //this.cmbHighBeamAddress.SelectedValue = value;
            //this.cmbLowBeamAddress.SelectedValue = value;

            //this.cmbReversingLightAddress.SelectedValue = value;
            //this.cmbSafetyBeltAddress.SelectedValue = value;
            //this.cmbLeftIndicatorLightAddress.SelectedValue = value;
            //this.cmbRightIndicatorLightAddress.SelectedValue = value;
            //雾灯，喇叭，示廓灯不能从OBD读
            Settings.LowBeamAddress = -1;
            Settings.FogLightAddress = -1;
            Settings.LoudspeakerAddress = -1;
            Settings.DoorAddress = value;
            Settings.HandbrakeAddress = value;
            Settings.ClutchAddress = value;
            Settings.BrakeAddress = value;
            Settings.HighBeamAddress = value;
            Settings.OutlineLightAddress = value;
            Settings.ReversingLightAddress = value;
            Settings.SafetyBeltAddress = value;
            Settings.LeftIndicatorLightAddress = value;
            Settings.RightIndicatorLightAddress = value;
        }
    }
}