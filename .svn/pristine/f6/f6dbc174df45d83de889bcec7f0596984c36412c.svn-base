using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Gps;
using TwoPole.Chameleon3.Infrastructure.Devices.CarSignal;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders
{

    //2-3-45
    public abstract class BaseCarSignalParser : ICarSignalParser
    {
        public const int FirstSensorAddress = 2;
        public const int CarSensorCachedMilliseconds = 5 * 1000;
        public const int TurnLightDelayMilliseconds = 1500;

        public ILog Logger { get; protected set; }
        private readonly Queue<Tuple<DateTime, int[]>> _historyInputs;
        public GlobalSettings Settings { get; private set; }


        /// <summary>
        /// 分辨高精度版本和其它版本（true时高精度）
        /// </summary>
        private static bool tempVersion = false;

        /// <summary>
        /// 当OBD为-2时为OBD传感器
        /// </summary>
        private int ObdSensorAddress = -2;

        #region 车载传感器信号地址
        protected virtual int FogLightAddress { get { return Settings.FogLightAddress < 0 ? FirstSensorAddress + 0 : Settings.FogLightAddress; } }
        protected virtual int LowBeamAddress { get { return Settings.LowBeamAddress < 0 ? FirstSensorAddress + 1 : Settings.LowBeamAddress; } }
        protected virtual int HighBeamAddress { get { return Settings.HighBeamAddress < 0 ? FirstSensorAddress + 2 : Settings.HighBeamAddress; } }
        protected virtual int LoudspeakerAddress { get { return Settings.LoudspeakerAddress < 0 ? FirstSensorAddress + 3 : Settings.LoudspeakerAddress; } }
        protected virtual int LeftIndicatorLightAddress { get { return Settings.LeftIndicatorLightAddress < 0 ? FirstSensorAddress + 4 : Settings.LeftIndicatorLightAddress; } }
        protected virtual int RightIndicatorLightAddress { get { return Settings.RightIndicatorLightAddress < 0 ? FirstSensorAddress + 5 : Settings.RightIndicatorLightAddress; } }
        protected virtual int OutlineLightAddress { get { return Settings.OutlineLightAddress < 0 ? FirstSensorAddress + 6 : Settings.OutlineLightAddress; } }
        protected virtual int BrakeAddress { get { return Settings.BrakeAddress < 0 ? 9 : Settings.BrakeAddress; } }
        protected virtual int DoorAddress { get { return Settings.DoorAddress < 0 ? 10 : Settings.DoorAddress; } }
        protected virtual int ClutchAddress { get { return Settings.ClutchAddress < 0 ? FirstSensorAddress + 9 : Settings.ClutchAddress; } }
        protected virtual int SafetyBeltAddress { get { return Settings.SafetyBeltAddress < 0 ? FirstSensorAddress + 10 : Settings.SafetyBeltAddress; } }
        protected virtual int HandbrakeAddress { get { return Settings.HandbrakeAddress < 0 ? FirstSensorAddress + 11 : Settings.HandbrakeAddress; } }
        protected virtual int ArrivedHeadstockAddress { get { return Settings.ArrivedHeadstockAddress < 0 ? FirstSensorAddress + 12 : Settings.ArrivedHeadstockAddress; } }
        protected virtual int ArrivedTailstockAddress { get { return Settings.ArrivedTailstockAddress < 0 ? FirstSensorAddress + 13 : Settings.ArrivedTailstockAddress; } }

        //车头车尾2 默认和 车头车尾一样的地址
        protected virtual int ArrivedHeadstock2Address { get { return Settings.ArrivedHeadstock2Address < 0 ? FirstSensorAddress + 12 : Settings.ArrivedHeadstock2Address; } }
        protected virtual int ArrivedTailstock2Address { get { return Settings.ArrivedTailstock2Address < 0 ? FirstSensorAddress + 13 : Settings.ArrivedTailstock2Address; } }
        protected virtual int IsNeutralAddress { get { return Settings.IsNeutralAddress < 0 ? FirstSensorAddress + 14 : Settings.IsNeutralAddress; } }
        protected virtual int ReversingLightAddress { get { return Settings.ReversingLightAddress < 0 ? FirstSensorAddress + 15 : Settings.ReversingLightAddress; } }
        /*12P*/
        protected virtual int SeatsAddress { get { return Settings.SeatsAddress < 0 ? FirstSensorAddress + 16 : Settings.SeatsAddress; } }
        protected virtual int ExteriorMirrorAddress { get { return Settings.ExteriorMirrorAddress < 0 ? FirstSensorAddress + 17 : Settings.ExteriorMirrorAddress; } }
        protected virtual int InnerMirrorAddress { get { return Settings.InnerMirrorAddress < 0 ? FirstSensorAddress + 18 : Settings.InnerMirrorAddress; } }
        protected virtual int FingerprintAddress { get { return 1; } }

        protected virtual int EngineAddress { get { return Settings.EngineAddress < 0 ? FirstSensorAddress + 21 : Settings.EngineAddress; } }

        //原始信号 //原始档显信号线定义地址       protected virtual int BrakeAddress { get { return Settings.BrakeAddress < 0 ? 9 : Settings.BrakeAddress; } }
        //protected virtual int GearDisplayD1Address { get { return FirstSensorAddress + 14; } }
        //protected virtual int GearDisplayD2Address { get { return FirstSensorAddress + 19; } }
        //protected virtual int GearDisplayD3Address { get { return FirstSensorAddress + 20; } }
        //protected virtual int GearDisplayD4Address { get { return FirstSensorAddress + 21; } }


        //protected virtual int GearDisplayD1Address { get { return FirstSensorAddress + 14; } }
        //protected virtual int GearDisplayD2Address { get { return FirstSensorAddress + 18; } }
        //protected virtual int GearDisplayD3Address { get { return FirstSensorAddress + 17; } }
        //protected virtual int GearDisplayD4Address { get { return FirstSensorAddress + 16; } }

        #region 定义档显地址
        protected virtual int GearDisplayD1Address { get { return Settings.GearDisplayD1Address < 0 ? FirstSensorAddress + 14 : Settings.GearDisplayD1Address; ; } }
        protected virtual int GearDisplayD2Address { get { return Settings.GearDisplayD2Address < 0 ? FirstSensorAddress + 19 : Settings.GearDisplayD2Address; ; } }
        protected virtual int GearDisplayD3Address { get { return Settings.GearDisplayD3Address < 0 ? FirstSensorAddress + 20 : Settings.GearDisplayD3Address; ; } }
        protected virtual int GearDisplayD4Address { get { return Settings.GearDisplayD4Address < 0 ? FirstSensorAddress + 21 : Settings.GearDisplayD4Address; ; } }
        #endregion

        #region 定义拉线地址 

        protected virtual int PullLineD1Address { get { return FirstSensorAddress + 16; } }

        protected virtual int PullLineD2Address { get { return FirstSensorAddress + 17; } }

        protected virtual int PullLineD3Address { get { return FirstSensorAddress + 18; } }

        protected virtual int PullLineD4Address { get { return FirstSensorAddress + 19; } }

        protected virtual int PullLineD5Address { get { return FirstSensorAddress + 20; } }

        // protected virtual int PullLineD6Address { get { return FirstSensorAddress + 21; } }


        #endregion

        #region OBD 传感器地址
        protected virtual int ObdClutchAddress { get { return 0; } }
        protected virtual int ObdSafetyBeltAddress { get { return 1; } }
        protected virtual int ObdDoorAddress { get { return 2; } }
        protected virtual int ObdHandbrakeAddress { get { return 3; } }
        protected virtual int ObdBrakeAddress { get { return 4; } }
        protected virtual int ObdLoudspeakerAddress { get { return 5; } }
        protected virtual int ObdUndefinedAddress1 { get { return 6; } }
        //预留位是两位
        //前雾灯（海口爱丽舍分为前雾灯和后雾灯，10为后雾灯）
        protected virtual int ObdFontFogLightAddress { get { return 7; } }
        protected virtual int ObdHighBeamAddress { get { return 8; } }
        protected virtual int ObdLowBeamAddress { get { return 9; } }
        protected virtual int ObdFogLightAddress { get { return 10; } }
        protected virtual int ObdOutlineLightAddress { get { return 11; } }
        protected virtual int ObdCautionLightAddress { get { return 12; } }
        protected virtual int ObdLeftIndicatorLightAddress { get { return 13; } }
        protected virtual int ObdRightIndicatorLightAddress { get { return 14; } }
        protected virtual int ObdUndefinedAddress2 { get { return 15; } }

        #endregion

        #endregion


        protected BaseCarSignalParser()
        {
            _historyInputs = new Queue<Tuple<DateTime, int[]>>();

            bool isTest =false;
            if (isTest)
            {
                Settings = new GlobalSettings(); ;
            }
            else
            {
                Settings = Singleton.GetDataService.GetSettings();
                Logger = Singleton.GetLogManager;
            }
      
            //左右转向取反后，值变为1，转向灯取反，20170702
            if (Settings.LeftIndicatorLightReverseFlag && Settings.RightIndicatorLightReverseFlag)
            {
                IndicatorValue = 1;
            }
        }

        public void SetSetting(GlobalSettings Settings)
        {
            this.Settings = Settings;

        }

        public virtual CarSignalInfo Parse(string[] commands)
        {
            if (commands == null || commands.Length == 0)
                return null;

            var carSignalInfo = new CarSignalInfo();
            carSignalInfo.Gps = ParseGpsInfo(commands);
            carSignalInfo.Sensor = new CarSensorInfo();
            carSignalInfo.commands = commands;
            var obdSensorBody = commands.FirstOrDefault(x => x.StartsWith("$OBD_SENSOR,", StringComparison.OrdinalIgnoreCase));
            if (obdSensorBody != null)
            {
                ProcessObdCarSensor(carSignalInfo, obdSensorBody);
                carSignalInfo.OBDSensorBody = obdSensorBody;
            }
            else { carSignalInfo.ObdSensor = new ObdCarSensorInfo(); }

            var sensorBody = commands.FirstOrDefault(x => x.StartsWith("$SENSOR,", StringComparison.OrdinalIgnoreCase));
            if (sensorBody != null)
            {
                ProcessCarSensor(carSignalInfo, sensorBody);
                carSignalInfo.SensorBody = sensorBody;
            }

            var grpoBody = commands.FirstOrDefault(x => x.StartsWith("$GYRO,", StringComparison.OrdinalIgnoreCase));
            if (grpoBody != null)
            {
                ProcessGyroscope(carSignalInfo, grpoBody);
                carSignalInfo.GyroSensorBody = grpoBody;
                //Logger.DebugFormat("陀螺仪：{0}", sensorBody);
            }
            return carSignalInfo;
        }

        protected virtual void ProcessGyroscope(CarSignalInfo carSignalInfo, string grpoBody)
        {
            var acc = ParseAcc(grpoBody);
            var angle = ParseAngle(grpoBody);
            var gyro = ParseGyro(grpoBody);

            if (acc != null)
            {
                carSignalInfo.AccelerationX = acc[0];
                carSignalInfo.AccelerationY = acc[1];
                carSignalInfo.AccelerationZ = acc[2];
            }

            if (gyro != null)
            {
                carSignalInfo.AngleSpeedX = gyro[0];
                carSignalInfo.AngleSpeedY = gyro[1];
                carSignalInfo.AngleSpeedZ = gyro[2];
            }

            if (angle != null)
            {
                carSignalInfo.AngleX = angle[0];
                carSignalInfo.AngleY = angle[1];
                carSignalInfo.AngleZ = angle[2];
            }
        }

        protected virtual void ProcessCarSensor(CarSignalInfo carSignalInfo, string sensorBody)
        {
            var inputs = ParseDigitalInputs(sensorBody);
            if (inputs.Length < 20)
            {
                carSignalInfo.IsSensorValid = false;
                Logger.WarnFormat("信号不正常忽略：{0}", sensorBody);
                return;
            }
            //保存信号数组
            carSignalInfo.inputs = inputs;

            AddToHistory(DateTime.Now, inputs);
            //读取信号
            ProcessCarSensorCore(carSignalInfo, inputs, sensorBody);
        }

        protected virtual void ProcessObdCarSensor(CarSignalInfo carSignalInfo, string sensorBody)
        {
            var inputs = ParseDigitalInputs(sensorBody);

            if (inputs.Length < 16)
            {
                carSignalInfo.IsSensorValid = false;
                Logger.WarnFormat("信号不正常忽略：{0}", sensorBody);
                return;
            }

            //保存信号数组
            carSignalInfo.inputs = inputs;


            //读取信号
            ProcessCarObdSensor(carSignalInfo, inputs, sensorBody);
        }

        public virtual void ProcessCarSensorCore(CarSignalInfo carSignalInfo, int[] inputs, string sensorBody)
        {
            //处理转向灯延迟；
            var reversedHistoryInputs = QueryHistory(TurnLightDelayMilliseconds);
            ProcessCautionLight(carSignalInfo.Sensor, reversedHistoryInputs);

            //获取原始的左右转灯信号(默认信号为0表示有信号，1则没有信号)
            carSignalInfo.Sensor.OriginalLeftIndicatorLight = !ReadDigitalInput(inputs, LeftIndicatorLightAddress, IndicatorValue != 0);
            carSignalInfo.Sensor.OriginalRightIndicatorLight = !ReadDigitalInput(inputs, RightIndicatorLightAddress, IndicatorValue != 0);
            carSignalInfo.Sensor.FogLight = ReadDigitalInput(inputs, FogLightAddress, true);

            carSignalInfo.Sensor.HighBeam = ReadDigitalInput(inputs, HighBeamAddress, true);

            //原来信号的远光是亮的，OBD信号的远光没有亮
            //carSignalInfo.Sensor.LowBeam = carSignalInfo.Sensor.HighBeam || ReadDigitalInput(inputs, LowBeamAddress, true);
            carSignalInfo.Sensor.LowBeam = ReadDigitalInput(inputs, LowBeamAddress, true);

            carSignalInfo.Sensor.Loudspeaker = ReadDigitalInput(inputs, LoudspeakerAddress, true);
            carSignalInfo.Sensor.OutlineLight = ReadDigitalInput(inputs, OutlineLightAddress, true);
            carSignalInfo.Sensor.Brake = ReadDigitalInput(inputs, BrakeAddress, true);

            carSignalInfo.Sensor.Door = ReadDigitalInput(inputs, DoorAddress);
            carSignalInfo.Sensor.Clutch = ReadDigitalInput(inputs, ClutchAddress);
            carSignalInfo.Sensor.SafetyBelt = !ReadDigitalInput(inputs, SafetyBeltAddress);
            carSignalInfo.Sensor.Handbrake = ReadDigitalInput(inputs, HandbrakeAddress);

            carSignalInfo.Sensor.ArrivedHeadstock = ReadDigitalInput(inputs, ArrivedHeadstockAddress);
            carSignalInfo.Sensor.ArrivedTailstock = ReadDigitalInput(inputs, ArrivedTailstockAddress);

            //车头车尾地址2
            carSignalInfo.Sensor.ArrivedHeadstock2 = ReadDigitalInput(inputs, ArrivedHeadstock2Address);
            carSignalInfo.Sensor.ArrivedTailstock2= ReadDigitalInput(inputs, ArrivedTailstock2Address);


            carSignalInfo.Sensor.IsNeutral = ReadDigitalInput(inputs, IsNeutralAddress);
            carSignalInfo.Sensor.ReversingLight = ReadDigitalInput(inputs, ReversingLightAddress, true);

            carSignalInfo.Sensor.ExteriorMirror = ReadDigitalInput(inputs, ExteriorMirrorAddress);
            carSignalInfo.Sensor.InnerMirror = ReadDigitalInput(inputs, InnerMirrorAddress);
            carSignalInfo.Sensor.Seats = ReadDigitalInput(inputs, SeatsAddress);
            carSignalInfo.Sensor.Fingerprint = ReadDigitalInput(inputs, FingerprintAddress);

            //OBD传感器
            if (Settings.ClutchAddress == ObdSensorAddress)
                carSignalInfo.Sensor.Clutch = carSignalInfo.ObdSensor.Clutch;
            if (Settings.SafetyBeltAddress == ObdSensorAddress)
                carSignalInfo.Sensor.SafetyBelt = carSignalInfo.ObdSensor.SafetyBelt;
            if (Settings.DoorAddress == ObdSensorAddress)
                carSignalInfo.Sensor.Door = carSignalInfo.ObdSensor.Door;
            if (Settings.HandbrakeAddress == ObdSensorAddress)
                carSignalInfo.Sensor.Handbrake = carSignalInfo.ObdSensor.Handbrake;
            if (Settings.BrakeAddress == ObdSensorAddress)
                carSignalInfo.Sensor.Brake = carSignalInfo.ObdSensor.Brake;
            if (Settings.LoudspeakerAddress == ObdSensorAddress)
                carSignalInfo.Sensor.Loudspeaker = carSignalInfo.ObdSensor.Loudspeaker;

            if (Settings.HighBeamAddress == ObdSensorAddress)
                carSignalInfo.Sensor.HighBeam = carSignalInfo.ObdSensor.HighBeam;
            if (Settings.LowBeamAddress == ObdSensorAddress)
                carSignalInfo.Sensor.LowBeam = carSignalInfo.ObdSensor.LowBeam;
            //近光没有接OBD,与远光接OBD   近光 信号就等于 原来的近光信号 或者  OBD的远光信号
            //else if (Settings.LowBeamAddress != ObdSensorAddress && Settings.HighBeamAddress == ObdSensorAddress)
            //    carSignalInfo.Sensor.LowBeam =carSignalInfo.Sensor.LowBeam | carSignalInfo.ObdSensor.HighBeam;
            if (Settings.FogLightAddress == ObdSensorAddress)
                carSignalInfo.Sensor.FogLight = carSignalInfo.ObdSensor.FogLight;
            if (Settings.OutlineLightAddress == ObdSensorAddress)

                carSignalInfo.Sensor.OutlineLight = carSignalInfo.ObdSensor.OutlineLight;

            if (Settings.LeftIndicatorLightAddress == ObdSensorAddress && Settings.RightIndicatorLightAddress == ObdSensorAddress)
                carSignalInfo.Sensor.CautionLight = carSignalInfo.ObdSensor.CautionLight;

            if (Settings.LeftIndicatorLightAddress == ObdSensorAddress)
                carSignalInfo.Sensor.LeftIndicatorLight = carSignalInfo.ObdSensor.LeftIndicatorLight;


            if (Settings.RightIndicatorLightAddress == ObdSensorAddress)
                carSignalInfo.Sensor.RightIndicatorLight = carSignalInfo.ObdSensor.RightIndicatorLight;

            //protected virtual int GearDisplayD1Address { get { return FirstSensorAddress + 14; } }
            //protected virtual int GearDisplayD2Address { get { return FirstSensorAddress + 19; } }
            //protected virtual int GearDisplayD3Address { get { return FirstSensorAddress + 20; } }
            //protected virtual int GearDisplayD4Address { get { return FirstSensorAddress + 21; } }


            //接法2
            //protected virtual int GearDisplayD1Address { get { return FirstSensorAddress + 14; } }
            //protected virtual int GearDisplayD2Address { get { return FirstSensorAddress + 18; } }
            //protected virtual int GearDisplayD3Address { get { return FirstSensorAddress + 17; } }
            //protected virtual int GearDisplayD4Address { get { return FirstSensorAddress + 16; } }

            carSignalInfo.Sensor.GearDisplayD1 = !ReadDigitalInput(inputs, GearDisplayD1Address);
            carSignalInfo.Sensor.GearDisplayD2 = !ReadDigitalInput(inputs, GearDisplayD2Address);
            carSignalInfo.Sensor.GearDisplayD3 = !ReadDigitalInput(inputs, GearDisplayD3Address);
            carSignalInfo.Sensor.GearDisplayD4 = !ReadDigitalInput(inputs, GearDisplayD4Address);

            carSignalInfo.Sensor.PullLineD1 = ReadDigitalInput(inputs, PullLineD1Address);
            carSignalInfo.Sensor.PullLineD2 = ReadDigitalInput(inputs, PullLineD2Address);
            carSignalInfo.Sensor.PullLineD3 = ReadDigitalInput(inputs, PullLineD3Address);
            carSignalInfo.Sensor.PullLineD4 = ReadDigitalInput(inputs, PullLineD4Address);
            carSignalInfo.Sensor.PullLineD5 = ReadDigitalInput(inputs, PullLineD5Address);
            //如果发动机线的地址是默认的就不要处理！
            carSignalInfo.Sensor.SpecialEngine = ReadDigitalInput(inputs, EngineAddress);
            //速度、档位等；
            carSignalInfo.Sensor.EngineRpm = ParseCarEngineRpm(sensorBody);

            ////处理OBD来源中的档位
            //if (sensorBody.Contains("gear"))
            //{
            //    int GearValue = ParseObdGear(sensorBody);
            //    if (GearValue>=0&&GearValue<=6)
            //    {
            //        carSignalInfo.Sensor.OBDGear = (Gear)GearValue;
            //    }
            //}

            if (Settings.CheckOBD)
            {
                carSignalInfo.Sensor.SpeedInKmh = ParseObdSpeed(sensorBody);
            }
            else
            {
                carSignalInfo.Sensor.SpeedInKmh = carSignalInfo.Gps.SpeedInKmh;
            }
            carSignalInfo.Sensor.Engine = carSignalInfo.Sensor.EngineRpm > 0;
            //carSignalInfo.Sensor.EngineLoad = ParseEngineLoad(sensorBody);
            //速度小于2，发动机都要熄火了 todo 李兴亮
            Logger.WarnFormat("Settings.MultipleRpm:{0}，carSignalInfo.Sensor.EngineRpm {1}，carSignalInfo.Sensor.SpeedInKmh {2}", Settings.MultipleRpm, carSignalInfo.Sensor.EngineRpm, carSignalInfo.Sensor.SpeedInKmh);

            if (carSignalInfo.Sensor.EngineRpm > 0 && carSignalInfo.Sensor.SpeedInKmh > 2 && Settings.MultipleRpm > 0 && Settings.MultiSpeed > 0)
            {
                //20160106，给转速添加系数  settings.SpeedKhmMode
                //转速比 应该是转速除以速度
                double Rpm = 0;
                double Speed = 0;

                if (Settings.EngineRpmRatioMode == EngineRpmRatioMode.ZoomIn)
                {
                    Rpm = carSignalInfo.Sensor.EngineRpm * Math.Abs(Settings.MultipleRpm);
                }
                else
                {
                    Rpm = carSignalInfo.Sensor.EngineRpm / Math.Abs(Settings.MultipleRpm);
                }

                if (Settings.SpeedKhmMode == SpeedKhmMode.ZoomIn)
                {
                    Speed = carSignalInfo.Sensor.SpeedInKmh * Settings.MultiSpeed;
                }
                else
                {
                    Speed = carSignalInfo.Sensor.SpeedInKmh / Settings.MultiSpeed;
                }

                carSignalInfo.EngineRatio = Convert.ToInt32(Rpm / Speed);
                //if (Settings.MultipleRpm > 0)
                //{
                //    carSignalInfo.EngineRatio =
                //        Convert.ToInt32(carSignalInfo.Sensor.EngineRpm * Settings.MultipleRpm /
                //                        carSignalInfo.Sensor.SpeedInKmh);
                //}
                //else
                //{
                //    carSignalInfo.EngineRatio =
                //       Convert.ToInt32(carSignalInfo.Sensor.EngineRpm /
                //                       (carSignalInfo.Sensor.SpeedInKmh * Math.Abs(Settings.MultipleRpm)));
                //}

            }
            else
                carSignalInfo.EngineRatio = 0;
            carSignalInfo.Sensor.Gear = ParseCarGear(inputs, sensorBody);

            carSignalInfo.IsSensorValid = inputs != null && inputs.Length > 0;
            //信号取反
            if (Settings.FogLightReverseFlag)
                carSignalInfo.Sensor.FogLight = !carSignalInfo.Sensor.FogLight;
            if (Settings.HighBeamReverseFlag)
                carSignalInfo.Sensor.HighBeam = !carSignalInfo.Sensor.HighBeam;
            if (Settings.LowBeamReverseFlag)
                carSignalInfo.Sensor.LowBeam = !carSignalInfo.Sensor.LowBeam;
            //carSignalInfo.Sensor.LowBeam = !carSignalInfo.Sensor.LowBeam || carSignalInfo.Sensor.HighBeam;
            if (Settings.LoudspeakerReverseFlag)
                carSignalInfo.Sensor.Loudspeaker = !carSignalInfo.Sensor.Loudspeaker;
            if (Settings.OutlineLightReverseFlag)
                carSignalInfo.Sensor.OutlineLight = !carSignalInfo.Sensor.OutlineLight;
            if (Settings.BrakeReverseFlag)
                carSignalInfo.Sensor.Brake = !carSignalInfo.Sensor.Brake;
            if (Settings.DoorReverseFlag)
                carSignalInfo.Sensor.Door = !carSignalInfo.Sensor.Door;
            if (Settings.ClutchReverseFlag)
                carSignalInfo.Sensor.Clutch = !carSignalInfo.Sensor.Clutch;
            if (Settings.SafetyBeltReverseFlag)
                carSignalInfo.Sensor.SafetyBelt = !carSignalInfo.Sensor.SafetyBelt;
            if (Settings.HandbrakeReverseFlag)
                carSignalInfo.Sensor.Handbrake = !carSignalInfo.Sensor.Handbrake;

            //车头车尾处理
            if (Settings.ArrivedHeadstockReverseFlag)
                carSignalInfo.Sensor.ArrivedHeadstock = !carSignalInfo.Sensor.ArrivedHeadstock;
            if (Settings.ArrivedTailstockReverseFlag)
                carSignalInfo.Sensor.ArrivedTailstock = !carSignalInfo.Sensor.ArrivedTailstock;

            if (Settings.ArrivedHeadstock2ReverseFlag)
                carSignalInfo.Sensor.ArrivedHeadstock2 = !carSignalInfo.Sensor.ArrivedHeadstock2;
            if (Settings.ArrivedTailstock2ReverseFlag)
                carSignalInfo.Sensor.ArrivedTailstock2 = !carSignalInfo.Sensor.ArrivedTailstock2;

            if (Settings.IsNeutralReverseFlag)
                carSignalInfo.Sensor.IsNeutral = !carSignalInfo.Sensor.IsNeutral;
            if (Settings.ReversingLightReverseFlag)
                carSignalInfo.Sensor.ReversingLight = !carSignalInfo.Sensor.ReversingLight;
            if (Settings.ExteriorMirrorReverseFlag)
                carSignalInfo.Sensor.ExteriorMirror = !carSignalInfo.Sensor.ExteriorMirror;
            if (Settings.InnerMirrorReverseFlag)
                carSignalInfo.Sensor.InnerMirror = !carSignalInfo.Sensor.InnerMirror;
            if (Settings.SeatsReverseFlag)
                carSignalInfo.Sensor.Seats = !carSignalInfo.Sensor.Seats;


            if (Settings.GearDisplayD1ReverseFlag)
                carSignalInfo.Sensor.GearDisplayD1 = !carSignalInfo.Sensor.GearDisplayD1;
            if (Settings.GearDisplayD2ReverseFlag)
                carSignalInfo.Sensor.GearDisplayD2 = !carSignalInfo.Sensor.GearDisplayD2;
            if (Settings.GearDisplayD3ReverseFlag)
                carSignalInfo.Sensor.GearDisplayD3 = !carSignalInfo.Sensor.GearDisplayD3;
            if (Settings.GearDisplayD4ReverseFlag)
                carSignalInfo.Sensor.GearDisplayD4 = !carSignalInfo.Sensor.GearDisplayD4;
            //发动机外接线取反
            if (Settings.EngineReverseFlag)
                carSignalInfo.Sensor.SpecialEngine = !carSignalInfo.Sensor.SpecialEngine;

            //航向角：heading，陀螺仪、电子罗盘等
            carSignalInfo.Sensor.Heading = ParseHeading(sensorBody);

            carSignalInfo.Sensor.LowBeam = carSignalInfo.Sensor.HighBeam || carSignalInfo.Sensor.LowBeam;
            //左右转信号是从OBD读取时,20160623
            if (Settings.LeftIndicatorLightAddress == ObdSensorAddress &&
                Settings.RightIndicatorLightAddress == ObdSensorAddress)
            {
                if (carSignalInfo.Sensor.CautionLight)
                {
                    carSignalInfo.Sensor.OriginalLeftIndicatorLight = true;
                    carSignalInfo.Sensor.OriginalRightIndicatorLight = true;
                }
                else
                {

                    carSignalInfo.Sensor.OriginalLeftIndicatorLight = carSignalInfo.Sensor.LeftIndicatorLight;
                    carSignalInfo.Sensor.OriginalRightIndicatorLight = carSignalInfo.Sensor.RightIndicatorLight;
                }
            }



        }



        private static int signalRate = 8;
        Queue<int[]> queueInput = new Queue<int[]>(signalRate);
        /// <summary>
        /// 处理OBD转向灯， 20170701，
        /// </summary>
        /// <param name="inputs"></param>
        public void ProcessOBDIndicatorLight(int[] inputs, CarSignalInfo carSignalInfo)
        {
            queueInput.Enqueue(inputs);
            if (queueInput.Count > signalRate)
            {
                queueInput.Dequeue();
            }
            var tempQueue = queueInput.Reverse().Take(signalRate - 1);

            carSignalInfo.ObdSensor.CautionLight = false;
            carSignalInfo.ObdSensor.LeftIndicatorLight = false;
            carSignalInfo.ObdSensor.RightIndicatorLight = false;

            foreach (var intps in tempQueue)
            {
                var tempsensor = ReadDigitalInput(intps, ObdCautionLightAddress, false);
                if (tempsensor)
                {
                    carSignalInfo.ObdSensor.CautionLight = true;
                    carSignalInfo.ObdSensor.LeftIndicatorLight = false;
                    carSignalInfo.ObdSensor.RightIndicatorLight = false;
                    return;
                }
            }
            //左转，右转都有则报警灯
            foreach (var intps in tempQueue)
            {
                var tempsensor = ReadDigitalInput(intps, ObdLeftIndicatorLightAddress, false);
                var tempsensor2 = ReadDigitalInput(intps, ObdRightIndicatorLightAddress, false);
                if (tempsensor && tempsensor2)
                {
                    carSignalInfo.ObdSensor.CautionLight = true;
                    carSignalInfo.ObdSensor.LeftIndicatorLight = false;
                    carSignalInfo.ObdSensor.RightIndicatorLight = false;
                    return;
                }
            }
            //左转
            foreach (var intps in tempQueue)
            {
                var tempsensor = ReadDigitalInput(intps, ObdLeftIndicatorLightAddress, false);
                if (tempsensor)
                {
                    carSignalInfo.ObdSensor.LeftIndicatorLight = true;
                    return;
                }
            }
            //右转
            foreach (var intps in tempQueue)
            {
                var tempsensor = ReadDigitalInput(intps, ObdRightIndicatorLightAddress, false);
                if (tempsensor)
                {
                    carSignalInfo.ObdSensor.RightIndicatorLight = true;
                    return;
                }
            }


        }
        public void ProcessCarObdSensor(CarSignalInfo carSignalInfo, int[] inputs, string sensorBody)
        {
            carSignalInfo.ObdSensor = new ObdCarSensorInfo();
            carSignalInfo.ObdSensor.Clutch = ReadDigitalInput(inputs, ObdClutchAddress, false);
            carSignalInfo.ObdSensor.SafetyBelt = ReadDigitalInput(inputs, ObdSafetyBeltAddress, false);
            carSignalInfo.ObdSensor.Door = ReadDigitalInput(inputs, ObdDoorAddress, true);
            carSignalInfo.ObdSensor.Handbrake = ReadDigitalInput(inputs, ObdHandbrakeAddress, false);
            carSignalInfo.ObdSensor.Brake = ReadDigitalInput(inputs, ObdBrakeAddress, false);
            carSignalInfo.ObdSensor.Loudspeaker = ReadDigitalInput(inputs, ObdLoudspeakerAddress, false);
            carSignalInfo.ObdSensor.Undefined1 = ReadDigitalInput(inputs, ObdUndefinedAddress1, false);


            carSignalInfo.ObdSensor.HighBeam = ReadDigitalInput(inputs, ObdHighBeamAddress, false);
            carSignalInfo.ObdSensor.LowBeam = ReadDigitalInput(inputs, ObdLowBeamAddress, false);


            carSignalInfo.ObdSensor.LowBeam = carSignalInfo.ObdSensor.HighBeam || carSignalInfo.ObdSensor.LowBeam;

            carSignalInfo.ObdSensor.FogLight = ReadDigitalInput(inputs, ObdFogLightAddress, false);
            carSignalInfo.ObdSensor.OutlineLight = ReadDigitalInput(inputs, ObdOutlineLightAddress, false);
            ////carSignalInfo.ObdSensor.CautionLight = ReadDigitalInput(inputs, ObdCautionLightAddress, false);

            //处理免破线转向灯常亮问题，20170701
            ProcessOBDIndicatorLight(inputs, carSignalInfo);

            // //原OBD左右转处理，会出现转向灯常亮，20170701
            // //if (carSignalInfo.ObdSensor.CautionLight)
            // //{
            // //    carSignalInfo.ObdSensor.LeftIndicatorLight = false;
            // //    carSignalInfo.ObdSensor.RightIndicatorLight = false;
            // //}
            // //else
            // //{
            // //   carSignalInfo.ObdSensor.LeftIndicatorLight = ReadDigitalInput(inputs, ObdLeftIndicatorLightAddress, false);
            // //   carSignalInfo.ObdSensor.RightIndicatorLight = ReadDigitalInput(inputs, ObdRightIndicatorLightAddress, false);
            // //}

            carSignalInfo.ObdSensor.Undefined2 = ReadDigitalInput(inputs, ObdUndefinedAddress2, false);
            //OBD档位
            if (sensorBody.Contains("gear"))
            {
                int GearValue = ParseObdGear(sensorBody);
                if (GearValue >= 0 && GearValue <= 6)
                {
                    carSignalInfo.Sensor.OBDGear = (Gear)GearValue;
                }
            }
            //OBD,方向盘角度
            if (sensorBody.Contains("angle"))
            {
                double wheelAngle = ParseObdWheelAngle(sensorBody);
                //carSignalInfo.Sensor.WheelAngle = wheelAngle;
            }
        }

        /// <summary>
        /// 左右转向的值，默认是0，取反为1
        /// </summary>
        private int IndicatorValue = 0;
        #region Caution
        protected void ProcessCautionLight(CarSensorInfo sensor, List<int[]> reversedHistoryInputs)
        {
            //if (reversedHistoryInputs.Count < LeftIndicatorLightAddress ||
            //    reversedHistoryInputs.Count < RightIndicatorLightAddress)
            //{
            //    sensor.LeftIndicatorLight = false;
            //    sensor.RightIndicatorLight = false;
            //    return;
            //}


            var isCaution = false;
            foreach (var inputs in reversedHistoryInputs)
            {
                if (inputs[LeftIndicatorLightAddress] != inputs[RightIndicatorLightAddress])
                    break;

                if (inputs[LeftIndicatorLightAddress] == IndicatorValue)
                {
                    isCaution = true;
                    break;
                }
            }

            sensor.CautionLight = isCaution;
            if (sensor.CautionLight)
            {
                sensor.LeftIndicatorLight =
                 sensor.RightIndicatorLight = false;
                return;
            }

            bool hasIndicatorLight = false;
            foreach (var items in reversedHistoryInputs)
            {
                if (items[LeftIndicatorLightAddress] == IndicatorValue)
                {
                    sensor.LeftIndicatorLight = true;
                    sensor.RightIndicatorLight = false;
                    hasIndicatorLight = true;
                    break;
                }
                if (items[RightIndicatorLightAddress] == IndicatorValue)
                {
                    sensor.LeftIndicatorLight = false;
                    sensor.RightIndicatorLight = true;
                    hasIndicatorLight = true;
                    break;
                }
            }

            if (!hasIndicatorLight)
                sensor.LeftIndicatorLight = sensor.RightIndicatorLight = false;
        }
        protected void AddToHistory(DateTime recordTime, int[] inputs)
        {
            lock (this)
            {
                _historyInputs.Enqueue(new Tuple<DateTime, int[]>(recordTime, inputs));
                RemoveOldInputs();
            }
        }
        private void RemoveOldInputs()
        {
            lock (this)
            {
                var cachedTime = DateTime.Now.AddMilliseconds(-CarSensorCachedMilliseconds);
                while (_historyInputs.Count > 0 &&
                    _historyInputs.Peek().Item1 < cachedTime)
                {
                    _historyInputs.Dequeue();
                }
            }
        }
        private List<int[]> QueryHistory(int milliseconds)
        {
            lock (this)
            {
                var cachedTime = DateTime.Now.AddMilliseconds(-milliseconds);
                var items = _historyInputs.Reverse()
                    .TakeWhile(x => x.Item1 > cachedTime)
                    .Select(x => x.Item2)
                    .ToList();
                return items;
            }

        }
        #endregion

        public virtual int ParseEngineLoad(string body)
        {
            return ParseInt(body, "obd_engine_load");
        }

        /// <summary>
        /// 智能匹配obd或者车速信号
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        protected virtual int ParseCarSpeed(string body)
        {
            var obdSpeed = ParseObdSpeed(body);
            //暂时不用
            //int speed =  != -1 ? obdSpeed : ParseSpeed(body);

            return obdSpeed;
        }

        /// <summary>
        /// 解析加速度
        /// $GYRO,acc:-0.03 0.02 1.02,gyro:0.00 0.00 0.00,angle:0.88 1.74 -8.07;
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        protected virtual double[] ParseAcc(string body)
        {
            var items = Parse3DItems(body, "acc");
            return items;
        }

        /// <summary>
        /// 解析角度
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        protected virtual double[] ParseAngle(string body)
        {
            var items = Parse3DItems(body, "angle");
            return items;
        }

        /// <summary>
        /// 解析角速度
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        protected virtual double[] ParseGyro(string body)
        {
            var items = Parse3DItems(body, "gyro");
            return items;
        }

        protected virtual double[] Parse3DItems(string body, string name)
        {
            var pattern = name + @":([\d\.-]+) ([\d\.-]+) ([\d\.-]+)";
            var m = Regex.Match(body, pattern);
            if (m.Success)
            {
                return new[]
                {
                     m.Groups[1].Value.ToDouble(0),
                     m.Groups[2].Value.ToDouble(0),
                     m.Groups[3].Value.ToDouble(0),
                };
            }

            return null;
        }

        /// <summary>
        /// 智能匹配发动机转速，从obd或者发动机信号线读取
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        protected virtual int ParseCarEngineRpm(string body)
        {

            //Logger.Debug(Settings.CheckOBDRpm);
            if (Settings.CheckOBDRpm)
            {
                var obdRpm = ParseObdRpm(body);
                return obdRpm;
            }
            else
            {
                int rpm = ParseRpm(body);
                return rpm;
            }
        }
        protected abstract Gear ParseCarGear(int[] inputs, string body);

        protected virtual GpsInfo ParseGpsInfo(string[] commands)
        {
            var reading = GpsReading.Parse(commands);
            if (reading.FixedGpsData == null)
                return null;

            var gpsInfo = new GpsInfo();
            if (reading.FixedGpsData.Elevation != null)
                gpsInfo.AltitudeMeters = Convert.ToDouble(reading.FixedGpsData.Elevation.Value);
            if (reading.FixedGpsData.Position != null)
            {
                gpsInfo.LatitudeDegrees = Convert.ToDouble(reading.FixedGpsData.Position.Latitude.Degrees);
                gpsInfo.LongitudeDegrees = Convert.ToDouble(reading.FixedGpsData.Position.Longitude.Degrees);
            }

            //采用gps本身提供的日期，20160509
            gpsInfo.UtcTime = reading.Summary.UtcDateTime;

            //gpsInfo.UtcTime = reading.FixedGpsData.UtcTime;
            gpsInfo.LocalTime = gpsInfo.UtcTime.ToLocalTime();
            gpsInfo.FixedSatelliteCount = reading.FixedGpsData.NumberOfSatelitesInUse;
            gpsInfo.TrackedSatelliteCount = gpsInfo.FixedSatelliteCount;
            gpsInfo.FixQuality = reading.FixedGpsData.Quality;
            if (reading.GroundVector != null && reading.GroundVector.GroundSpeedInKmh != 0)
            {
                gpsInfo.SpeedInKmh = reading.GroundVector.GroundSpeedInKmh;
            }
            if (reading.Summary != null && reading.Summary.GroundSpeed != 0)
            {
                var summary = reading.Summary;
                gpsInfo.SpeedInKmh = Convert.ToDouble(summary.GroundSpeed);
                gpsInfo.AngleDegrees = Convert.ToDouble(summary.Heading);
            }
            if (reading.Heading != null)
            {
                //20160111，使用高精度版本
                tempVersion = true;
                gpsInfo.AngleDegrees = reading.Heading.BearingDegrees;
                //高精度和普通科三gps角度会差180度，如：高精度3°，普通184°
                //gpsInfo.AngleDegrees = gpsInfo.AngleDegrees + 180;
                //if (gpsInfo.AngleDegrees > 360)
                //    gpsInfo.AngleDegrees = gpsInfo.AngleDegrees - 360;

                gpsInfo.FixedSatelliteCount = reading.Heading.SatellitesInSolution;
                gpsInfo.TrackedSatelliteCount = reading.Heading.SatellitesInTrack;
                //记录上一次角度
                lastAngleDegrees = gpsInfo.AngleDegrees;
            }
            if (tempVersion)
            {
                //20160111，使用高精度版本,(有时Heading为空的情况)
                gpsInfo.HighPrecisionVersion = true;
                if (reading.Heading == null)
                {
                    gpsInfo.AngleDegrees = lastAngleDegrees;
                }
            }
            return gpsInfo;
        }

        /// <summary>
        /// 记录上一次角度
        /// </summary>
        private double lastAngleDegrees = 0;

        protected bool ReadDigitalInput(int[] inputs, int index, bool reverse = false)
        {
            if (inputs.Length < index || index < 0)
                return false;

            return reverse ? inputs[index] == 0 : inputs[index] == 1;
        }

        protected int[] ParseDigitalInputs(string body)
        {
            //$OBD_SENSOR,band:1,throttlePosition:0,gear:6,angle:0.00,io:00000000,0011001100011110
            var match = Regex.Match(body, @",([\d\.]+);", RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value.Select(x => x.ToString().ToInt32(0)).ToArray();

            return new int[0];
        }

        protected double ParseHeading(string body)
        {
            return ParseDouble(body, "heading");
        }

        protected int ParseA0(string body)
        {
            return ParseInt(body, "a0");
        }
        protected int ParseA1(string body)
        {
            return ParseInt(body, "a1");
        }
        protected int ParseObdGear(string body)
        {
            return ParseInt(body, "gear");
        }
        protected double ParseObdWheelAngle(string body)
        {
            return ParseDouble(body, "angle");
        }
        protected int ParseObdSpeed(string body)
        {
            return ParseInt(body, "obd_speed");
        }
        protected int ParseObdRpm(string body)
        {
            return ParseInt(body, "obd_rpm");
        }
        protected int ParseSpeed(string body)
        {
            return ParseInt(body, "speed");
        }
        protected int ParseRpm(string body)
        {
            return ParseInt(body, "rpm");
        }
        protected int ParseTime(string body)
        {
            return ParseInt(body, "time");
        }
        protected int ParseCount(string body)
        {
            return ParseInt(body, "count");
        }

        protected int ParseInt(string body, string name)
        {
            var match = Regex.Match(body, name + @":([-\d.]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                double result = -1;
                var isok = double.TryParse(match.Groups[1].Value, out result);
                if (isok)
                {
                    return Convert.ToInt32(result);
                }
            }
            //return match.Groups[1].Value.ToInt32(-1);

            return -1;
        }

        protected double ParseDouble(string body, string name)
        {
            var match = Regex.Match(body, name + @":([-\d.]+)", RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value.ToDouble(-1);

            return -1;
        }
    }
}
