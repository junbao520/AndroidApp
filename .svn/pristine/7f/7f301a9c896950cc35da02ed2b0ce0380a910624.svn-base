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
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.CustomControls
{
    [Activity(Label = "SensorControlLuzhou")]
   public  class SensorControlLuzhou:Activity
    {
        ImageView imgEngine;
        ImageView imgSafety;

        ImageView imgHandbreak;
        ImageView imgDoor;
        ImageView imgClutch;
        ImageView imgBrake;
        ImageView imgLoudspeaker;
        ImageView imgLeftlight;

        ImageView imgRightlight;
        ImageView imgOutline;
        ImageView imgLowBeam;
        ImageView imgHighBeam;
        ImageView imgFoglight;

        public void Load()
        {
            InitControl();
            RegisterMessage();
        }
      
        /// <summary>
        /// 当前车载信号
        /// </summary>
        protected CarSignalInfo carSignal;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SensorControlLuzhou);
           

        }

        protected void InitControl()
        {
            imgEngine = FindViewById<ImageView>(Resource.Id.imgEngine);
            imgSafety = FindViewById<ImageView>(Resource.Id.imgSafety);
            imgHandbreak = FindViewById<ImageView>(Resource.Id.imgHandbreak);
            imgDoor = FindViewById<ImageView>(Resource.Id.imgDoor);
            imgClutch = FindViewById<ImageView>(Resource.Id.imgClutch);
            imgBrake = FindViewById<ImageView>(Resource.Id.imgBrake);
            imgLoudspeaker = FindViewById<ImageView>(Resource.Id.imgLoudspeaker);
            imgLeftlight = FindViewById<ImageView>(Resource.Id.imgLeftlight);
            imgRightlight = FindViewById<ImageView>(Resource.Id.imgRightlight);
            imgOutline = FindViewById<ImageView>(Resource.Id.imgOutline);
            imgLowBeam = FindViewById<ImageView>(Resource.Id.imgLowBeam);
            imgHighBeam = FindViewById<ImageView>(Resource.Id.imgHighBeam);
            imgFoglight = FindViewById<ImageView>(Resource.Id.imgFoglight);
        }
        protected void RegisterMessage()
        {
            IMessenger messenger = Singleton.GetMessager;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
        }

        protected  void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            if (message == null || message.CarSignal != null)
                return;
            
            carSignal = message.CarSignal;
            RunOnUiThread(UpdateCarSensorState);



        }
        protected void UpdateCarSensorState()
        {
            
            var Sensor = carSignal.Sensor;
            if (Sensor == null)
                return;

            if(Sensor.Brake)
                imgBrake.SetImageResource(Resource.Drawable.luozhou_brake_on);
            else
                imgBrake.SetImageResource(Resource.Drawable.luozhou_brake_off);

            if (Sensor.CautionLight)
            {
                imgLeftlight.SetImageResource(Resource.Drawable.luozhou_left_light_on);
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_on);
            }
            else
            {
                imgLeftlight.SetImageResource(Resource.Drawable.luozhou_left_light_off);
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_off);
            }

            if (Sensor.Clutch)
                imgClutch.SetImageResource(Resource.Drawable.luozhou_clutch_on);
            else
                imgClutch.SetImageResource(Resource.Drawable.luozhou_clutch_off);

            if (Sensor.Door)
                imgDoor.SetImageResource(Resource.Drawable.luozhou_door_on);
            else
                imgDoor.SetImageResource(Resource.Drawable.luozhou_door_off);

            if (Sensor.Engine)
                imgEngine.SetImageResource(Resource.Drawable.luozhou_engine_on);
            else
                imgEngine.SetImageResource(Resource.Drawable.luozhou_engine_off);

            if (Sensor.FogLight)
                imgFoglight.SetImageResource(Resource.Drawable.luozhou_fog_light_on);
            else
                imgFoglight.SetImageResource(Resource.Drawable.luozhou_fog_light_off);

            if (Sensor.Handbrake)
                imgHandbreak.SetImageResource(Resource.Drawable.hand_break_on);
            else
                imgHandbreak.SetImageResource(Resource.Drawable.hand_break_off);

            if (Sensor.HighBeam)
                imgHighBeam.SetImageResource(Resource.Drawable.high_beam_on);
            else
                imgHighBeam.SetImageResource(Resource.Drawable.high_beam_off);

            if (Sensor.LeftIndicatorLight)
                imgLeftlight.SetImageResource(Resource.Drawable.left_light_on);
            else
                imgLeftlight.SetImageResource(Resource.Drawable.left_light_off);

            if (Sensor.Loudspeaker)
                imgLoudspeaker.SetImageResource(Resource.Drawable.luozhou_loudspeaker_on);
            else
                imgLoudspeaker.SetImageResource(Resource.Drawable.luozhou_loudspeaker_off);

            if (Sensor.LowBeam)
                imgLowBeam.SetImageResource(Resource.Drawable.luozhou_low_beam_on);
            else
                imgLowBeam.SetImageResource(Resource.Drawable.luozhou_low_beam_off);

            if (Sensor.OutlineLight)
                imgOutline.SetImageResource(Resource.Drawable.luozhou_outline_light_on);
            else
                imgOutline.SetImageResource(Resource.Drawable.luozhou_outline_light_off);

            if (Sensor.RightIndicatorLight)
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_on);
            else
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_off);

            if (Sensor.SafetyBelt)
                imgSafety.SetImageResource(Resource.Drawable.luozhou_safety_belt_on);
            else
                imgSafety.SetImageResource(Resource.Drawable.luozhou_safety_belt_off);



        }
    }


    
}