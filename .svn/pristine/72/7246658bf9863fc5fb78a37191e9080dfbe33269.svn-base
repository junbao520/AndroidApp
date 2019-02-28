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
    [Activity(Label = "SensorLineSetting")]
    public class SensorLineSetting : BaseSettingActivity
    {
        //车头取反
        CheckBox chkArrivedHeadstock;
        Spinner spinnerArrivedHeadstockAddress;

        //车头2
        CheckBox chkArrivedHeadstock2;
        Spinner spinnerArrivedHeadstock2Address;
        //雾灯取反
        CheckBox chkFogLight;
        Spinner spinnerFogLightAddress;

        //远光取反
        CheckBox chkHighBeam;
        Spinner spinnerHighBeamAddress;
        //喇叭取反
        CheckBox chkLoudspeaker;
        Spinner spinnerLoudspeakerAddress;
        //近光取反
        CheckBox chkLowBeam;
        Spinner spinnerLowBeamAddress;
        //车尾取反
        CheckBox chkArrivedTailstock;
        Spinner spinnerArrivedTailstockAddress;

        //车尾2
        CheckBox chkArrivedTailstock2;
        Spinner spinnerArrivedTailstock2Address;

        //车门取反
        CheckBox chkDoor;
        Spinner spinnerDoorAddress;
        //四廓灯取反
        CheckBox chkOutlineLight;
        Spinner spinnerOutlineLightAddress;
        //左转取反
        CheckBox chkLeftIndicatorLight;
        Spinner spinnerLeftIndicatorLightAddress;
        //手刹取反
        CheckBox chkHandbrake;
        Spinner spinnerHandbrakeAddress;
        //右转取反
        CheckBox chkRightIndicatorLight;
        Spinner spinnerRightIndicatorLightAddress;
        //离合取反
        CheckBox chkClutch;
        Spinner spinnerClutchAddress;
        //安全带取反
        CheckBox chkSafetyBelt;
        Spinner spinnerSafetyBeltAddress;
        //内后视镜取反
        CheckBox chkInnerMirror;
        Spinner spinnerInnerMirrorAddress;
        //座椅取反
        CheckBox chkSeats;
        Spinner spinnerSeatsAddress;
        //发动机取反
        CheckBox chkEngine;
        Spinner spinnerEngineAddress;
        //外后视镜取反
        CheckBox chkExteriorMirror;
        Spinner spinnerExteriorMirrorAddress;
        //刹车取反
        CheckBox chkBrake;
        Spinner spinnerBrakeAddress;

        /// <summary>
        /// 档显1
        /// </summary>
        CheckBox chkGearDisplay1;
        Spinner spinnerGearDisplay1Address;
        /// <summary>
        /// 档显2
        /// </summary>
        CheckBox chkGearDisplay2;
        Spinner spinnerGearDisplay2Address;

        /// <summary>
        /// 档显3
        /// </summary>
        CheckBox chkGearDisplay3;
        Spinner spinnerGearDisplay3Address;

        /// <summary>
        /// 档显4
        /// </summary>
        CheckBox chkGearDisplay4;
        Spinner spinnerGearDisplay4Address;


        //接线方案
        RadioButton radConnectionSchemeDefalut;
        RadioButton radConnectionSchemeNewMulberry;
        RadioButton radConnectionSchemeNewJetta;
        RadioButton radConnectionSchemeOBDElysee;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SensorLineSetting);
            InitControl();
            initHeader();
            ActivityName = this.GetString(Resource.String.SensorLineSettingStr);
            setMyTitle(ActivityName);
            InitSetting();
            // Create your application here
        }
        private void BindSpinner(List<string> lstDataSource, Spinner spinner)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstDataSource);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.Visibility = ViewStates.Visible;

        }
        List<string> lstSignalAddress = new List<string> {"OBD","默认", "R-转速", "S-脉冲", "1-雾灯", "2-近光", "3-远光", "4-喇叭", "5-左转", "6-右转", "7-小灯"
        ,"8-刹车","9-车门","10-离合","11-安全带","12-手刹","13-车头","14-车尾","15-空挡","16-扩展0","17-扩展1","18-扩展2","19-扩展3","20-扩展4","21-扩展5","22-扩展6"};
        public override void InitSetting()
        {
            radConnectionSchemeDefalut.Checked = Settings.ConnectionScheme == ConnectionScheme.Acquiesce;
            radConnectionSchemeNewJetta.Checked = Settings.ConnectionScheme == ConnectionScheme.Jetta;
            radConnectionSchemeNewMulberry.Checked = Settings.ConnectionScheme == ConnectionScheme.NewMulberry;
            radConnectionSchemeOBDElysee.Checked = Settings.ConnectionScheme == ConnectionScheme.Elysee;
            //车头取反

            chkArrivedHeadstock.Checked = Settings.ArrivedHeadstockReverseFlag;
            BindSpinner(lstSignalAddress, spinnerArrivedHeadstockAddress);
            spinnerArrivedHeadstockAddress.SetSelection(Settings.ArrivedHeadstockAddress + 2, true);

            //车尾取反
            chkArrivedTailstock.Checked = Settings.ArrivedTailstockReverseFlag;
            BindSpinner(lstSignalAddress, spinnerArrivedTailstockAddress);
            spinnerArrivedTailstockAddress.SetSelection(Settings.ArrivedTailstockAddress + 2, true);

            //车头2
            chkArrivedHeadstock2.Checked = Settings.ArrivedHeadstock2ReverseFlag;
            BindSpinner(lstSignalAddress, spinnerArrivedHeadstock2Address);
            spinnerArrivedHeadstock2Address.SetSelection(Settings.ArrivedHeadstock2Address + 2, true);

            //车尾2
            chkArrivedTailstock2.Checked = Settings.ArrivedTailstock2ReverseFlag;
            BindSpinner(lstSignalAddress, spinnerArrivedTailstock2Address);
            spinnerArrivedTailstock2Address.SetSelection(Settings.ArrivedTailstock2Address + 2, true);



            //雾灯取反
            chkFogLight.Checked = Settings.FogLightReverseFlag;

            BindSpinner(lstSignalAddress, spinnerFogLightAddress);
            spinnerFogLightAddress.SetSelection(Settings.FogLightAddress + 2, true);
            //远光取反
            chkHighBeam.Checked = Settings.HighBeamReverseFlag;

            BindSpinner(lstSignalAddress, spinnerHighBeamAddress);
            spinnerHighBeamAddress.SetSelection(Settings.HighBeamAddress + 2, true);
            //喇叭取反
            chkLoudspeaker.Checked = Settings.LoudspeakerReverseFlag;
            BindSpinner(lstSignalAddress, spinnerLoudspeakerAddress);
            spinnerLoudspeakerAddress.SetSelection(Settings.LoudspeakerAddress + 2, true);
            //近光取反
            chkLowBeam.Checked = Settings.LowBeamReverseFlag;
            BindSpinner(lstSignalAddress, spinnerLowBeamAddress);
            spinnerLowBeamAddress.SetSelection(Settings.LowBeamAddress + 2, true);
      
            //车门取反
            chkDoor.Checked = Settings.DoorReverseFlag;
            BindSpinner(lstSignalAddress, spinnerDoorAddress);
            spinnerDoorAddress.SetSelection(Settings.DoorAddress + 2, true);
            //四廓灯取反
            chkOutlineLight.Checked = Settings.OutlineLightReverseFlag;
            BindSpinner(lstSignalAddress, spinnerOutlineLightAddress);
            spinnerOutlineLightAddress.SetSelection(Settings.OutlineLightAddress + 2, true);
            //左转取反
            chkLeftIndicatorLight.Checked = Settings.LeftIndicatorLightReverseFlag;
            BindSpinner(lstSignalAddress, spinnerLeftIndicatorLightAddress);
            spinnerLeftIndicatorLightAddress.SetSelection(Settings.LeftIndicatorLightAddress + 2, true);
            //手刹取反
            chkHandbrake.Checked = Settings.HandbrakeReverseFlag;
            BindSpinner(lstSignalAddress, spinnerHandbrakeAddress);
            spinnerHandbrakeAddress.SetSelection(Settings.HandbrakeAddress + 2, true);
            //右转取反
            chkRightIndicatorLight.Checked = Settings.RightIndicatorLightReverseFlag;

            BindSpinner(lstSignalAddress, spinnerRightIndicatorLightAddress);
            spinnerRightIndicatorLightAddress.SetSelection(Settings.RightIndicatorLightAddress + 2, true);
            //离合取反
            chkClutch.Checked = Settings.ClutchReverseFlag;
            BindSpinner(lstSignalAddress, spinnerClutchAddress);
            spinnerClutchAddress.SetSelection(Settings.ClutchAddress + 2, true);
            //安全带取反
            chkSafetyBelt.Checked = Settings.SafetyBeltReverseFlag;
            BindSpinner(lstSignalAddress, spinnerSafetyBeltAddress);
            spinnerSafetyBeltAddress.SetSelection(Settings.SafetyBeltAddress + 2, true);
            //内后视镜取反
            chkInnerMirror.Checked = Settings.InnerMirrorReverseFlag;
            BindSpinner(lstSignalAddress, spinnerInnerMirrorAddress);
            spinnerInnerMirrorAddress.SetSelection(Settings.InnerMirrorAddress + 2, true);
            //座椅取反
            chkSeats.Checked = Settings.SeatsReverseFlag;

            BindSpinner(lstSignalAddress, spinnerSeatsAddress);
            spinnerSeatsAddress.SetSelection(Settings.SeatsAddress + 2, true);
            //发动机取反
            chkEngine.Checked = Settings.EngineReverseFlag;
            BindSpinner(lstSignalAddress, spinnerEngineAddress);
            spinnerEngineAddress.SetSelection(Settings.EngineAddress + 2, true);
            //外后视镜取反
            chkExteriorMirror.Checked = Settings.ExteriorMirrorReverseFlag;

            BindSpinner(lstSignalAddress, spinnerExteriorMirrorAddress);
            spinnerExteriorMirrorAddress.SetSelection(Settings.ExteriorMirrorAddress + 2, true);
            //刹车取反
            chkBrake.Checked = Settings.BrakeReverseFlag;
            BindSpinner(lstSignalAddress, spinnerBrakeAddress);
            spinnerBrakeAddress.SetSelection(Settings.BrakeAddress + 2, true);


            chkGearDisplay1.Checked = Settings.GearDisplayD1ReverseFlag;
            BindSpinner(lstSignalAddress, spinnerGearDisplay1Address);
            spinnerGearDisplay1Address.SetSelection(Settings.GearDisplayD1Address + 2, true);


            chkGearDisplay2.Checked = Settings.GearDisplayD2ReverseFlag;
            BindSpinner(lstSignalAddress, spinnerGearDisplay2Address);
            spinnerGearDisplay2Address.SetSelection(Settings.GearDisplayD2Address + 2, true);


            chkGearDisplay3.Checked = Settings.GearDisplayD3ReverseFlag;
            BindSpinner(lstSignalAddress, spinnerGearDisplay3Address);
            spinnerGearDisplay3Address.SetSelection(Settings.GearDisplayD3Address + 2, true);


            chkGearDisplay4.Checked = Settings.GearDisplayD4ReverseFlag;
            BindSpinner(lstSignalAddress, spinnerGearDisplay4Address);
            spinnerGearDisplay4Address.SetSelection(Settings.GearDisplayD4Address + 2, true);
            //把发动机线的接法流出来

            //初始化接线方案



            base.InitSetting();
        }
        public override void UpdateSettings()
        {
            try
            {
                if (radConnectionSchemeDefalut.Checked)
                {
                    Settings.ConnectionScheme = ConnectionScheme.Acquiesce;
                }
                else if (radConnectionSchemeNewJetta.Checked)
                {
                    Settings.ConnectionScheme = ConnectionScheme.Jetta;
                }
                else if (radConnectionSchemeNewMulberry.Checked)
                {
                    Settings.ConnectionScheme = ConnectionScheme.NewMulberry;
                }
                else if (radConnectionSchemeOBDElysee.Checked)
                {
                    Settings.ConnectionScheme = ConnectionScheme.Elysee;
                }
                //车头
                Settings.ArrivedHeadstockReverseFlag = chkArrivedHeadstock.Checked;
                Settings.ArrivedHeadstockAddress = spinnerArrivedHeadstockAddress.SelectedItemPosition - 2;

                //车尾
                Settings.ArrivedTailstockReverseFlag = chkArrivedTailstock.Checked;
                Settings.ArrivedTailstockAddress = spinnerArrivedTailstockAddress.SelectedItemPosition - 2;

                //车头2
                Settings.ArrivedHeadstock2ReverseFlag = chkArrivedHeadstock2.Checked;
                Settings.ArrivedHeadstock2Address = spinnerArrivedHeadstock2Address.SelectedItemPosition - 2;

                //车尾2
                Settings.ArrivedTailstock2ReverseFlag = chkArrivedTailstock2.Checked;
                Settings.ArrivedTailstock2Address = spinnerArrivedTailstock2Address.SelectedItemPosition - 2;


                //雾灯取反
                Settings.FogLightReverseFlag = chkFogLight.Checked;
                Settings.FogLightAddress = spinnerFogLightAddress.SelectedItemPosition - 2;
                //远光取反
                Settings.HighBeamReverseFlag = chkHighBeam.Checked;
                Settings.HighBeamAddress = spinnerHighBeamAddress.SelectedItemPosition - 2;
                //喇叭取反
                Settings.LoudspeakerReverseFlag = chkLoudspeaker.Checked;
                Settings.LoudspeakerAddress = spinnerLoudspeakerAddress.SelectedItemPosition - 2;
                //近光取反
                Settings.LowBeamReverseFlag = chkLowBeam.Checked;
                Settings.LowBeamAddress = spinnerLowBeamAddress.SelectedItemPosition - 2;
                //车尾取反

                //车门取反
                Settings.DoorReverseFlag = chkDoor.Checked;
                Settings.DoorAddress = spinnerDoorAddress.SelectedItemPosition - 2;
                //四廓灯取反
                Settings.OutlineLightReverseFlag = chkOutlineLight.Checked;
                Settings.OutlineLightAddress = spinnerOutlineLightAddress.SelectedItemPosition - 2;
                //左转取反
                Settings.LeftIndicatorLightReverseFlag = chkLeftIndicatorLight.Checked;
                Settings.LeftIndicatorLightAddress = spinnerLeftIndicatorLightAddress.SelectedItemPosition - 2;
                //手刹取反
                Settings.HandbrakeReverseFlag = chkHandbrake.Checked;
                Settings.HandbrakeAddress = spinnerHandbrakeAddress.SelectedItemPosition - 2;
                //右转取反
                Settings.RightIndicatorLightReverseFlag = chkRightIndicatorLight.Checked;
                Settings.RightIndicatorLightAddress = spinnerRightIndicatorLightAddress.SelectedItemPosition - 2;
                //离合取反
                Settings.ClutchReverseFlag = chkClutch.Checked;
                Settings.ClutchAddress = spinnerClutchAddress.SelectedItemPosition - 2;
                //安全带取反
                Settings.SafetyBeltReverseFlag = chkSafetyBelt.Checked;
                Settings.SafetyBeltAddress = spinnerSafetyBeltAddress.SelectedItemPosition - 2;
                //内后视镜取反
                Settings.InnerMirrorReverseFlag = chkInnerMirror.Checked;
                Settings.InnerMirrorAddress = spinnerInnerMirrorAddress.SelectedItemPosition - 2;
                //座椅取反
                Settings.SeatsReverseFlag = chkSeats.Checked;
                Settings.SeatsAddress = spinnerSeatsAddress.SelectedItemPosition - 2;
                //发动机取反
                Settings.EngineReverseFlag = chkEngine.Checked;
                Settings.EngineAddress = spinnerEngineAddress.SelectedItemPosition - 2;
                //外后视镜取反
                Settings.ExteriorMirrorReverseFlag = chkExteriorMirror.Checked;
                Settings.ExteriorMirrorAddress = spinnerExteriorMirrorAddress.SelectedItemPosition - 2;
                //刹车取反
                Settings.BrakeReverseFlag = chkBrake.Checked;
                Settings.BrakeAddress = spinnerBrakeAddress.SelectedItemPosition - 2;

                //档位显1
                Settings.GearDisplayD1ReverseFlag = chkGearDisplay1.Checked;
                Settings.GearDisplayD1Address = spinnerGearDisplay1Address.SelectedItemPosition - 2;
                //档位显2
                Settings.GearDisplayD2ReverseFlag = chkGearDisplay2.Checked;
                Settings.GearDisplayD2Address = spinnerGearDisplay2Address.SelectedItemPosition - 2;
                //档位显3
                Settings.GearDisplayD3ReverseFlag = chkGearDisplay3.Checked;
                Settings.GearDisplayD3Address = spinnerGearDisplay3Address.SelectedItemPosition - 2;
                //档位显4
                Settings.GearDisplayD4ReverseFlag = chkGearDisplay4.Checked;
                Settings.GearDisplayD4Address = spinnerGearDisplay4Address.SelectedItemPosition - 2;


                #region listSetting
                List<Setting> lstSetting = new List<Setting>
                {
new Setting { Key ="ConnectionScheme", Value = Settings.ConnectionScheme.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedHeadstockReverseFlag", Value = Settings.ArrivedHeadstockReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedHeadstockAddress", Value = Settings.ArrivedHeadstockAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedHeadstock2ReverseFlag", Value = Settings.ArrivedHeadstock2ReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedHeadstock2Address", Value = Settings.ArrivedHeadstock2Address.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="FogLightReverseFlag", Value = Settings.FogLightReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="FogLightAddress", Value = Settings.FogLightAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="HighBeamReverseFlag", Value = Settings.HighBeamReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="HighBeamAddress", Value = Settings.HighBeamAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LoudspeakerReverseFlag", Value = Settings.LoudspeakerReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LoudspeakerAddress", Value = Settings.LoudspeakerAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LowBeamReverseFlag", Value = Settings.LowBeamReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LowBeamAddress", Value = Settings.LowBeamAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedTailstockReverseFlag", Value = Settings.ArrivedTailstockReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedTailstockAddress", Value = Settings.ArrivedTailstockAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedTailstock2ReverseFlag", Value = Settings.ArrivedTailstock2ReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ArrivedTailstock2Address", Value = Settings.ArrivedTailstock2Address.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="DoorReverseFlag", Value = Settings.DoorReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="DoorAddress", Value = Settings.DoorAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OutlineLightReverseFlag", Value = Settings.OutlineLightReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OutlineLightAddress", Value = Settings.OutlineLightAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LeftIndicatorLightReverseFlag", Value = Settings.LeftIndicatorLightReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="LeftIndicatorLightAddress", Value = Settings.LeftIndicatorLightAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="HandbrakeReverseFlag", Value = Settings.HandbrakeReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="HandbrakeAddress", Value = Settings.HandbrakeAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="RightIndicatorLightReverseFlag", Value = Settings.RightIndicatorLightReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="RightIndicatorLightAddress", Value = Settings.RightIndicatorLightAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ClutchReverseFlag", Value = Settings.ClutchReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ClutchAddress", Value = Settings.ClutchAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SafetyBeltReverseFlag", Value = Settings.SafetyBeltReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SafetyBeltAddress", Value = Settings.SafetyBeltAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="InnerMirrorReverseFlag", Value = Settings.InnerMirrorReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="InnerMirrorAddress", Value = Settings.InnerMirrorAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SeatsReverseFlag", Value = Settings.SeatsReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SeatsAddress", Value = Settings.SeatsAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="EngineReverseFlag", Value = Settings.EngineReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="EngineAddress", Value = Settings.EngineAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ExteriorMirrorReverseFlag", Value = Settings.ExteriorMirrorReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ExteriorMirrorAddress", Value = Settings.ExteriorMirrorAddress.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeReverseFlag", Value = Settings.BrakeReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BrakeAddress", Value = Settings.BrakeAddress.ToString(), GroupName = "GlobalSettings" },
//档显
new Setting { Key ="GearDisplayD1ReverseFlag", Value = Settings.GearDisplayD1ReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearDisplayD1Address", Value = Settings.GearDisplayD1Address.ToString(), GroupName = "GlobalSettings" },

new Setting { Key ="GearDisplayD2ReverseFlag", Value = Settings.GearDisplayD2ReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearDisplayD2Address", Value = Settings.GearDisplayD2Address.ToString(), GroupName = "GlobalSettings" },

new Setting { Key ="GearDisplayD3ReverseFlag", Value = Settings.GearDisplayD3ReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearDisplayD3Address", Value = Settings.GearDisplayD3Address.ToString(), GroupName = "GlobalSettings" },

new Setting { Key ="GearDisplayD4ReverseFlag", Value = Settings.GearDisplayD4ReverseFlag.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="GearDisplayD4Address", Value = Settings.GearDisplayD4Address.ToString(), GroupName = "GlobalSettings" },

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
            //车头取反
            chkArrivedHeadstock = FindViewById<CheckBox>(Resource.Id.chkArrivedHeadstock);
            spinnerArrivedHeadstockAddress = FindViewById<Spinner>(Resource.Id.spinnerArrivedHeadstockAddress);
            //车头2
            //todo:bug
            chkArrivedHeadstock2 = FindViewById<CheckBox>(Resource.Id.chkArrivedHeadstock2);
            spinnerArrivedHeadstock2Address = FindViewById<Spinner>(Resource.Id.spinnerArrivedHeadstock2Address);
            //车尾
            chkArrivedTailstock = FindViewById<CheckBox>(Resource.Id.chkArrivedTailstock);
            spinnerArrivedTailstockAddress = FindViewById<Spinner>(Resource.Id.spinnerArrivedTailstockAddress);
            //车尾2
            chkArrivedTailstock2 = FindViewById<CheckBox>(Resource.Id.chkArrivedTailstock2);
            spinnerArrivedTailstock2Address = FindViewById<Spinner>(Resource.Id.spinnerArrivedTailstock2Address);

            //雾灯取反
            chkFogLight = FindViewById<CheckBox>(Resource.Id.chkFogLight);
            spinnerFogLightAddress = FindViewById<Spinner>(Resource.Id.spinnerFogLightAddress);
            //远光取反
            chkHighBeam = FindViewById<CheckBox>(Resource.Id.chkHighBeam);
            spinnerHighBeamAddress = FindViewById<Spinner>(Resource.Id.spinnerHighBeamAddress);
            //喇叭取反
            chkLoudspeaker = FindViewById<CheckBox>(Resource.Id.chkLoudspeaker);
            spinnerLoudspeakerAddress = FindViewById<Spinner>(Resource.Id.spinnerLoudspeakerAddress);
            //近光取反
            chkLowBeam = FindViewById<CheckBox>(Resource.Id.chkLowBeam);
            spinnerLowBeamAddress = FindViewById<Spinner>(Resource.Id.spinnerLowBeamAddress);
            //车尾取反

            //车门取反
            chkDoor = FindViewById<CheckBox>(Resource.Id.chkDoor);
            spinnerDoorAddress = FindViewById<Spinner>(Resource.Id.spinnerDoorAddress);
            //四廓灯取反
            chkOutlineLight = FindViewById<CheckBox>(Resource.Id.chkOutlineLight);
            spinnerOutlineLightAddress = FindViewById<Spinner>(Resource.Id.spinnerOutlineLightAddress);
            //左转取反
            chkLeftIndicatorLight = FindViewById<CheckBox>(Resource.Id.chkLeftIndicatorLight);
            spinnerLeftIndicatorLightAddress = FindViewById<Spinner>(Resource.Id.spinnerLeftIndicatorLightAddress);
            //手刹取反
            chkHandbrake = FindViewById<CheckBox>(Resource.Id.chkHandbrake);
            spinnerHandbrakeAddress = FindViewById<Spinner>(Resource.Id.spinnerHandbrakeAddress);
            //右转取反
            chkRightIndicatorLight = FindViewById<CheckBox>(Resource.Id.chkRightIndicatorLight);
            spinnerRightIndicatorLightAddress = FindViewById<Spinner>(Resource.Id.spinnerRightIndicatorLightAddress);
            //离合取反
            chkClutch = FindViewById<CheckBox>(Resource.Id.chkClutch);
            spinnerClutchAddress = FindViewById<Spinner>(Resource.Id.spinnerClutchAddress);
            //安全带取反
            chkSafetyBelt = FindViewById<CheckBox>(Resource.Id.chkSafetyBelt);
            spinnerSafetyBeltAddress = FindViewById<Spinner>(Resource.Id.spinnerSafetyBeltAddress);
            //内后视镜取反
            chkInnerMirror = FindViewById<CheckBox>(Resource.Id.chkInnerMirror);
            spinnerInnerMirrorAddress = FindViewById<Spinner>(Resource.Id.spinnerInnerMirrorAddress);
            //座椅取反
            chkSeats = FindViewById<CheckBox>(Resource.Id.chkSeats);
            spinnerSeatsAddress = FindViewById<Spinner>(Resource.Id.spinnerSeatsAddress);
            //发动机取反
            chkEngine = FindViewById<CheckBox>(Resource.Id.chkEngine);
            spinnerEngineAddress = FindViewById<Spinner>(Resource.Id.spinnerEngineAddress);
            //外后视镜取反
            chkExteriorMirror = FindViewById<CheckBox>(Resource.Id.chkExteriorMirror);
            spinnerExteriorMirrorAddress = FindViewById<Spinner>(Resource.Id.spinnerExteriorMirrorAddress);
            //刹车取反
            chkBrake = FindViewById<CheckBox>(Resource.Id.chkBrake);
            spinnerBrakeAddress = FindViewById<Spinner>(Resource.Id.spinnerBrakeAddress);
            // Settings.CommonExamItemsChangeLanesAngle
            // Settings.CommonExamItemsCheckChangeLanes
            //Settings.CommonExamItemsChangeLanesTimeOut
            //档显
            chkGearDisplay1 = FindViewById<CheckBox>(Resource.Id.chkGearDisplay1);
            spinnerGearDisplay1Address = FindViewById<Spinner>(Resource.Id.spinnerGearDisplay1Address);

            chkGearDisplay2 = FindViewById<CheckBox>(Resource.Id.chkGearDisplay2);
            spinnerGearDisplay2Address = FindViewById<Spinner>(Resource.Id.spinnerGearDisplay2Address);

            chkGearDisplay3 = FindViewById<CheckBox>(Resource.Id.chkGearDisplay3);
            spinnerGearDisplay3Address = FindViewById<Spinner>(Resource.Id.spinnerGearDisplay3Address);

            chkGearDisplay4 = FindViewById<CheckBox>(Resource.Id.chkGearDisplay4);
            spinnerGearDisplay4Address = FindViewById<Spinner>(Resource.Id.spinnerGearDisplay4Address);


            radConnectionSchemeDefalut = FindViewById<RadioButton>(Resource.Id.radConnectionSchemeDefalut);
            radConnectionSchemeNewMulberry = FindViewById<RadioButton>(Resource.Id.radConnectionSchemeNewMulberry);
            radConnectionSchemeNewJetta = FindViewById<RadioButton>(Resource.Id.radConnectionSchemeNewJetta);
            radConnectionSchemeOBDElysee = FindViewById<RadioButton>(Resource.Id.radConnectionSchemeOBDElysee);

            radConnectionSchemeDefalut.CheckedChange += RadConnectionSchemeDefalut_CheckedChange;
            radConnectionSchemeNewJetta.CheckedChange += RadConnectionSchemeNewJetta_CheckedChange;
            radConnectionSchemeNewMulberry.CheckedChange += RadConnectionSchemeNewMulberry_CheckedChange;
            radConnectionSchemeOBDElysee.CheckedChange += RadConnectionSchemeOBDElysee_CheckedChange;
        }

        private void RadConnectionSchemeOBDElysee_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radConnectionSchemeOBDElysee.Checked)
            {
                NoWiring_Alise_Check(-2);
            }
        }

        private void RadConnectionSchemeNewMulberry_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radConnectionSchemeNewMulberry.Checked)
            {
                NoWiringCheck(-2);
            }
        }

        private void RadConnectionSchemeNewJetta_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radConnectionSchemeNewJetta.Checked)
            {
                NoWiringCheck(-2);
            }
        }

        private void RadConnectionSchemeDefalut_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (radConnectionSchemeDefalut.Checked)
            {
                NoWiringCheck(-1);
            }
        }

        private void NoWiringCheck(int value)
        {
            //近光灯不能从OBD读
            //Settings.LowBeamAddress = -1;
            //Settings.FogLightAddress = value;
            //Settings.LoudspeakerAddress = value;
            //Settings.DoorAddress = value;
            //Settings.HandbrakeAddress = value;
            //Settings.ClutchAddress = value;
            //Settings.BrakeAddress = value;
            //Settings.HighBeamAddress = value;
            //Settings.OutlineLightAddress = value;
            //Settings.ReversingLightAddress = value;
            //Settings.SafetyBeltAddress = value;
            //Settings.LeftIndicatorLightAddress = value;
            //Settings.RightIndicatorLightAddress = value;

            //-2
            spinnerLowBeamAddress.SetSelection(1, true);
            spinnerFogLightAddress.SetSelection(value + 2, true);
            spinnerLoudspeakerAddress.SetSelection(value + 2, true);
            spinnerDoorAddress.SetSelection(value + 2, true);
            spinnerHandbrakeAddress.SetSelection(value + 2, true);
            spinnerHighBeamAddress.SetSelection(value + 2, true);
            spinnerOutlineLightAddress.SetSelection(value + 2, true);
            spinnerSafetyBeltAddress.SetSelection(value + 2, true);
            spinnerLeftIndicatorLightAddress.SetSelection(value + 2, true);
            spinnerRightIndicatorLightAddress.SetSelection(value + 2, true);
            //新捷达，离合和刹车obd
            spinnerClutchAddress.SetSelection(value + 2, true);
            spinnerBrakeAddress.SetSelection(value + 2, true);



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
            //Settings.LowBeamAddress = -1;
            //Settings.FogLightAddress = -1;
            //Settings.LoudspeakerAddress = -1;
            //Settings.DoorAddress = value;
            //Settings.HandbrakeAddress = value;
            //Settings.ClutchAddress = value;
            //Settings.BrakeAddress = value;
            //Settings.HighBeamAddress = value;
            //Settings.OutlineLightAddress = value;
            //Settings.ReversingLightAddress = value;
            //Settings.SafetyBeltAddress = value;
            //Settings.LeftIndicatorLightAddress = value;
            //Settings.RightIndicatorLightAddress = value;


            spinnerLowBeamAddress.SetSelection(1, true);
            spinnerFogLightAddress.SetSelection(1, true);
            spinnerLoudspeakerAddress.SetSelection(1, true);
            spinnerDoorAddress.SetSelection(value + 2, true);
            spinnerHandbrakeAddress.SetSelection(value + 2, true);
            spinnerHighBeamAddress.SetSelection(value + 2, true);
            spinnerOutlineLightAddress.SetSelection(value + 2, true);
            spinnerSafetyBeltAddress.SetSelection(value + 2, true);
            spinnerLeftIndicatorLightAddress.SetSelection(value + 2, true);
            spinnerRightIndicatorLightAddress.SetSelection(value + 2, true);
        }
    }
}