using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Util;
using System.Collections.Generic;
using Java.Util;
using Android.Speech;
using Android.Speech.Tts;
using Java.IO;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Android.Content.PM;
using Java.Util.Zip;
using Java.Text;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "CarSensor")]
    public class CarSensor:BaseSettingActivity
    {

        CarSignalInfo carSignal;
        ILog logger;
        TextView tvEngineRpm;
        TextView tvSpeed;
        TextView tvGear;
        TextView tvEngineRatio;
        TextView tvTail;
        TextView tvHead;
        TextView tvTail2;
        TextView tvHead2;
        TextView tvLongitude;
        TextView tvLat;
        TextView tvAngle;
        TextView tvSatelliteCount;
        TextView tvAltitude;
        TextView tvHeadingAngle;
        TextView tvInnerMirror;
        TextView tvExteriorMirror;
        TextView tvSeat;
        TextView tvFingerprint;
        TextView tvVersionNumber;
        TextView tvEquipmentNumber;
        ImageView mgViewLowBeamLight;
        ImageView mgViewHighBeamLight;
        ImageView mgViewTurnRightLight;
        ImageView mgViewTurnLeftLight;
        ImageView mgViewOutLineLight;
        ImageView mgViewFogLight;
        ImageView mgViewEngine;
        ImageView mgViewDoor;
        ImageView mgViewHandBreak;
        ImageView mgViewSafetyBelt;
        ImageView mgViewBreak;
        ImageView mgViewClutch;
        ImageView mgViewLoudSpeaker;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Sensor);
            logger = Singleton.GetLogManager;
            messager = Singleton.GetMessager;
            Init();
            initHeader(false);
            setMyTitle("车辆状态");
            SetVersionNumber();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            logger.Error("AndroidEnvironmentCarSensor", e.Exception.Message);
        }

        public void SetVersionNumber()
        {
            var lstPackages = PackageManager.GetInstalledPackages(0);
            string Info = string.Empty;

            ApplicationInfo ai = PackageManager.GetApplicationInfo(PackageName, 0);
            
        
            ZipFile zf = new ZipFile(ai.SourceDir);
            ZipEntry ze = zf.GetEntry("META-INF/MANIFEST.MF");
            long time = ze.Time;
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
            string date = sdf.Format(new Java.Util.Date(ze.Time));
            zf.Close();

            tvVersionNumber.Text = "版本：" + DataBase.VersionNumber + date;
            //TODO:现在暂时没有使用
           // tvEquipmentNumber.Text = "设备号:" + DataBase.EquipmentNumber;

        }
        public void Init()
        {
            tvEngineRpm = FindViewById<TextView>(Resource.Id.tvEngineRpm);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            tvSatelliteCount = FindViewById<TextView>(Resource.Id.tvSatelliteCount);
            tvAltitude = FindViewById<TextView>(Resource.Id.tvAltitude);
            tvHeadingAngle = FindViewById<TextView>(Resource.Id.tvHeadingAngle);
            tvInnerMirror = FindViewById<TextView>(Resource.Id.tvInnerMirror);
            tvExteriorMirror = FindViewById<TextView>(Resource.Id.tvExteriorMirror);
            tvSeat = FindViewById<TextView>(Resource.Id.tvSeat);
            tvFingerprint = FindViewById<TextView>(Resource.Id.tvFingerprint);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            tvEngineRatio = FindViewById<TextView>(Resource.Id.tvEngineRatio);
            tvTail = FindViewById<TextView>(Resource.Id.tvTail);
            tvHead = FindViewById<TextView>(Resource.Id.tvHead);
            tvTail2 = FindViewById<TextView>(Resource.Id.tvTail2);
            tvHead2 = FindViewById<TextView>(Resource.Id.tvHead2);
            tvLongitude = FindViewById<TextView>(Resource.Id.tvLongitude);
            tvLat = FindViewById<TextView>(Resource.Id.tvLat);
            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);
            tvVersionNumber = FindViewById<TextView>(Resource.Id.tvVersionNumber);
           // tvEquipmentNumber= FindViewById<TextView>(Resource.Id.tvEquipmentNumber);
            mgViewLowBeamLight = FindViewById<ImageView>(Resource.Id.mgViewLowBeamLight);
            mgViewHighBeamLight = FindViewById<ImageView>(Resource.Id.mgViewHightBeamLight);
            mgViewOutLineLight = FindViewById<ImageView>(Resource.Id.mgViewOutLineLight);
            mgViewTurnLeftLight = FindViewById<ImageView>(Resource.Id.mgViewTurnLeftLight);
            mgViewTurnRightLight = FindViewById<ImageView>(Resource.Id.mgViewTurnRightLight);
            mgViewFogLight = FindViewById<ImageView>(Resource.Id.mgViewFogLight);
            mgViewEngine = FindViewById<ImageView>(Resource.Id.mgViewEngine);
            mgViewDoor = FindViewById<ImageView>(Resource.Id.mgViewDoor);
            mgViewHandBreak = FindViewById<ImageView>(Resource.Id.mgViewHandBreak);
            mgViewSafetyBelt = FindViewById<ImageView>(Resource.Id.mgViewSafetyBelt);
            mgViewBreak = FindViewById<ImageView>(Resource.Id.mgViewBreak);
            mgViewClutch = FindViewById<ImageView>(Resource.Id.mgViewClutch);
            mgViewLoudSpeaker = FindViewById<ImageView>(Resource.Id.mgViewLoudSpeaker);

            //RegisterMessages(messager);
        }
        protected void RegisterMessages(object objectmessenger)
        {
            IMessenger messenger = (IMessenger)objectmessenger;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            Finish();
            return base.OnKeyDown(keyCode, key);
        }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                carSignal = message.CarSignal;
                RunOnUiThread(UpdateCarSensor);
            }
            catch (Exception ex)
            {
                logger.Error("CarSensorOnCarSignalReceived", ex.Message);
            }

        }
        public void UpdateCarSensorState()
        {
            if (carSignal.Sensor.LowBeam)
                mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_on);
            else
                mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_off);

            if (carSignal.Sensor.HighBeam)
                mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_on);
            else
                mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_off);
            //报警灯亮起时左右转向亮
            if (carSignal.Sensor.CautionLight)
            {
                mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_on);
                mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_on);
            }
            else
            {
                if (carSignal.Sensor.LeftIndicatorLight)
                    mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_on);
                else
                    mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_off);
                if (carSignal.Sensor.RightIndicatorLight)
                    mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_on);
                else
                    mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_off);
            }
            if (carSignal.Sensor.FogLight)
                mgViewFogLight.SetImageResource(Resource.Drawable.fog_light_on);
            else
                mgViewFogLight.SetImageResource(Resource.Drawable.fog_light_off);

            if (carSignal.Sensor.Engine)
                mgViewEngine.SetImageResource(Resource.Drawable.engine_on);
            else
                mgViewEngine.SetImageResource(Resource.Drawable.engine_off);

            if (carSignal.Sensor.Door)
                mgViewDoor.SetImageResource(Resource.Drawable.door_on);
            else
                mgViewDoor.SetImageResource(Resource.Drawable.door_off);

            if (carSignal.Sensor.Handbrake)
                mgViewHandBreak.SetImageResource(Resource.Drawable.hand_break_on);
            else
                mgViewHandBreak.SetImageResource(Resource.Drawable.hand_break_off);

            if (carSignal.Sensor.SafetyBelt)
                mgViewSafetyBelt.SetImageResource(Resource.Drawable.safety_belt_off);
            else
                mgViewSafetyBelt.SetImageResource(Resource.Drawable.safety_belt_on);

            if (carSignal.Sensor.Brake)
                mgViewBreak.SetImageResource(Resource.Drawable.brake_on);
            else
                mgViewBreak.SetImageResource(Resource.Drawable.brake_off);

            if (carSignal.Sensor.Clutch)
                mgViewClutch.SetImageResource(Resource.Drawable.clutch_on);
            else
                mgViewClutch.SetImageResource(Resource.Drawable.clutch_off);

            if (carSignal.Sensor.Loudspeaker)
                mgViewLoudSpeaker.SetImageResource(Resource.Drawable.loudspeaker_on);
            else
                mgViewLoudSpeaker.SetImageResource(Resource.Drawable.loudspeaker_off);

            if (carSignal.Sensor.OutlineLight)
                mgViewOutLineLight.SetImageResource(Resource.Drawable.outline_light_on);
            else
                mgViewOutLineLight.SetImageResource(Resource.Drawable.outline_light_off);
        }


        /// <summary>
        /// 更新车辆状态
        /// </summary>
        public void UpdateCarSensor()
        {
            var Sensor = carSignal.Sensor;
            var gps = carSignal.Gps;
            var sensor = carSignal.Sensor;
            try
            {
                tvEngineRpm.Text = string.Format("{0}转/分钟", Sensor.EngineRpm);
                tvSpeed.Text = string.Format("{0}Km/h", Sensor.SpeedInKmh);

                if (sensor.IsNeutral)
                {
                    tvGear.Text = "空挡";
                }
                else
                {
                    tvGear.Text = Convert.ToInt32(Sensor.Gear).ToString();
                }
                tvEngineRatio.Text = carSignal.EngineRatio.ToString();
                tvTail.Text = Sensor.ArrivedTailstock == true ? "是" : "否";
                tvHead.Text = Sensor.ArrivedHeadstock == true ? "是" : "否";

                //更新信号车头2 车尾2
                tvTail2.Text = Sensor.ArrivedTailstock2 == true ? "是" : "否";
                tvHead2.Text = Sensor.ArrivedHeadstock2== true ? "是" : "否";


                tvInnerMirror.Text = Sensor.InnerMirror == true ? "是" : "否";
                tvExteriorMirror.Text = Sensor.ExteriorMirror == true ? "是" : "否";
                tvSeat.Text = Sensor.Seats == true ? "是" : "否";
                tvFingerprint.Text = Sensor.Fingerprint == true ? "是" : "否";
                UpdateCarSensorState();
                tvAngle.Text = string.Format("{0:N2}°", carSignal.BearingAngle);

                tvHeadingAngle.Text = string.Format("{0:N2}°", carSignal.Gps.AngleDegrees);
                tvAltitude.Text = string.Format("{0:#0}米", gps.AltitudeMeters);
                tvAltitude.Text = tvEngineRatio.Text+"/"+ tvGear.Text;
                tvSatelliteCount.Text = string.Format("{0}颗", gps.FixedSatelliteCount);
                tvLat.Text = string.Format("{0:#0.00000}°", gps.LatitudeDegrees);
                tvLongitude.Text = string.Format("{0:#0.00000}°", gps.LongitudeDegrees);

            }
            catch (Exception exp)
            {
                logger.ErrorFormat("CarSensorUpdateCarSensor发生异常,原因:{0} 原始信号{1}", exp.Message, string.Join(",", carSignal.commands));
            }

        }
    }
}