using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Providers;

namespace TwoPole.Chameleon3.Infrastructure
{
    /// <summary>
    /// 全局配置设定
    /// </summary>
    public class GlobalSettings : ObservableObject, ISettings
    {


        #region 系统设置

        private bool _isShowCompanyLogo;
        /// <summary>
        /// 当前车型编号
        /// </summary>
        public bool IsShowCompanyLogo
        {
            get { return _isShowCompanyLogo; }
            set { Set(() => IsShowCompanyLogo, ref _isShowCompanyLogo, value); }
        }

        #endregion
        #region 蓝牙主控箱
        private string _deviceAddress;
        /// <summary>
        /// 设置蓝牙主控箱的地址
        /// </summary>
        public string DeviceAddress
        {
            get { return _deviceAddress; }
            set { Set(() => DeviceAddress, ref _deviceAddress, value); }
        }

        #endregion

        #region 蓝牙Obd

        private string _obdAddress;
        /// <summary>
        /// 设置Obd的地址
        /// </summary>
        public string ObdAddress
        {
            get { return _obdAddress; }
            set { Set(() => ObdAddress, ref _obdAddress, value); }
        }

        #endregion

        #region GPS 串口地址

        private string _gpsSerialAddress;
        /// <summary>
        /// 设置GPS串口的地址
        /// </summary>
        public string GpsSerialAddress
        {
            get { return _gpsSerialAddress; }
            set { Set(() => GpsSerialAddress, ref _gpsSerialAddress, value); }
        }

        #endregion

        #region 传感器设定
        private int _vechileModelId;
        /// <summary>
        /// 当前车型编号
        /// </summary>
        public int VechileModelId
        {
            get { return _vechileModelId; }
            set { Set(() => VechileModelId, ref _vechileModelId, value); }
        }

        private string _lightSettingsContent;
        /// <summary>
        /// 灯光配置参数（序列化成Json）
        /// 仅用于导入导出配置
        /// </summary>
        public string LightSettingsContent
        {
            get { return _lightSettingsContent; }
            set { Set(() => LightSettingsContent, ref _lightSettingsContent, value); }
        }

        private float _engineRpmRatio = 1;
        private EngineRpmRatioMode _engineRpmRatioMode = EngineRpmRatioMode.ZoomIn;
        /// <summary>
        /// 发动机转速放大比例因子
        /// </summary>
        public float EngineRpmRatio
        {
            get { return _engineRpmRatio; }
            set { Set(() => EngineRpmRatio, ref _engineRpmRatio, value); }
        }
        /// <summary>
        /// 发动机转速调节模式
        /// </summary>
        public EngineRpmRatioMode EngineRpmRatioMode
        {
            get { return _engineRpmRatioMode; }
            set { Set(() => EngineRpmRatioMode, ref _engineRpmRatioMode, value); }
        }

        #region Obd
        private bool _obdEnable = true;
        private string _obdPortName = "COM2";
        private int _obdBaudRate = DefaultGlobalSettings.DefaultObdBaudRate;
        /// <summary>
        /// 是否启用OBD
        /// </summary>
        public bool ObdEnable
        {
            get { return _obdEnable; }
            set { Set(() => ObdEnable, ref _obdEnable, value); }
        }
        /// <summary>
        /// OBD串口名称
        /// </summary>
        public string ObdPortName
        {
            get { return _obdPortName; }
            set { Set(() => ObdPortName, ref _obdPortName, value); }
        }
        /// <summary>
        /// OBD波特率
        /// </summary>
        public int ObdBaudRate
        {
            get { return _obdBaudRate; }
            set { Set(() => ObdBaudRate, ref _obdBaudRate, value); }
        }
        #endregion

        #region 档位齿速比 - 发动机转速与档位的特征比率

        private int _gearOneMinRatio = DefaultGlobalSettings.DefaultGearOneMinRatio;
        private int _gearOneMaxRatio = DefaultGlobalSettings.DefaultGearOneMaxRatio;
        private int _gearTwoMinRatio = DefaultGlobalSettings.DefaultGearTwoMinRatio;
        private int _gearTwoMaxRatio = DefaultGlobalSettings.DefaultGearTwoMaxRatio;
        private int _gearThreeMinRatio = DefaultGlobalSettings.DefaultGearThreeMinRatio;
        private int _gearThreeMaxRatio = DefaultGlobalSettings.DefaultGearThreeMaxRatio;
        private int _gearFourMinRatio = DefaultGlobalSettings.DefaultGearFourMinRatio;
        private int _gearFourMaxRatio = DefaultGlobalSettings.DefaultGearFourMaxRatio;
        private int _gearFiveMinRatio = DefaultGlobalSettings.DefaultGearFiveMinRatio;
        private int _gearFiveMaxRatio = DefaultGlobalSettings.DefaultGearFiveMaxRatio;

        private float _multipleRpm = DefaultGlobalSettings.DefaultMultipleRpm;
        /// <summary>
        /// 转速放大缩小的倍数
        /// </summary>
        public float MultipleRpm
        {
            get { return _multipleRpm; }
            set { Set(() => MultipleRpm, ref _multipleRpm, value); }
        }
        /// <summary>
        /// 一档最小齿速比
        /// </summary>
        public int GearOneMinRatio
        {
            get { return _gearOneMinRatio; }
            set { Set(() => GearOneMinRatio, ref _gearOneMinRatio, value); }
        }
        /// <summary>
        /// 一档最小齿速比
        /// </summary>
        public int GearOneMaxRatio
        {
            get { return _gearOneMaxRatio; }
            set { Set(() => GearOneMaxRatio, ref _gearOneMaxRatio, value); }
        }
        /// <summary>
        /// 二档最小齿速比
        /// </summary>
        public int GearTwoMinRatio
        {
            get { return _gearTwoMinRatio; }
            set { Set(() => GearTwoMinRatio, ref _gearTwoMinRatio, value); }
        }
        /// <summary>
        ///二档最大齿速比
        /// </summary>
        public int GearTwoMaxRatio
        {
            get { return _gearTwoMaxRatio; }
            set { Set(() => GearTwoMaxRatio, ref _gearTwoMaxRatio, value); }
        }
        /// <summary>
        /// 三档最小齿速比
        /// </summary>
        public int GearThreeMinRatio
        {
            get { return _gearThreeMinRatio; }
            set { Set(() => GearThreeMinRatio, ref _gearThreeMinRatio, value); }
        }
        /// <summary>
        /// 三档最大齿速比
        /// </summary>
        public int GearThreeMaxRatio
        {
            get { return _gearThreeMaxRatio; }
            set { Set(() => GearThreeMaxRatio, ref _gearThreeMaxRatio, value); }
        }
        /// <summary>
        /// 四档最小齿速比
        /// </summary>
        public int GearFourMinRatio
        {
            get { return _gearFourMinRatio; }
            set { Set(() => GearFourMinRatio, ref _gearFourMinRatio, value); }
        }
        /// <summary>
        /// 四档最大齿速比
        /// </summary>
        public int GearFourMaxRatio
        {
            get { return _gearFourMaxRatio; }
            set { Set(() => GearFourMaxRatio, ref _gearFourMaxRatio, value); }
        }
        /// <summary>
        /// 五档最小齿速比
        /// </summary>
        public int GearFiveMinRatio
        {
            get { return _gearFiveMinRatio; }
            set { Set(() => GearFiveMinRatio, ref _gearFiveMinRatio, value); }
        }
        /// <summary>
        /// 五档最大齿速比
        /// </summary>
        public int GearFiveMaxRatio
        {
            get { return _gearFiveMaxRatio; }
            set { Set(() => GearFiveMaxRatio, ref _gearFiveMaxRatio, value); }
        }

        #endregion

        #region 档线传感器最低、最高值

        private int _gearLineNeutralMinRatio = 0;
        private int _gearLineNeutralMaxRatio = 0;
        private int _gearLineOneMinRatio = 0;
        private int _gearLineOneMaxRatio = 0;
        private int _gearLineTwoMinRatio = 0;
        private int _gearLineTwoMaxRatio = 0;
        private int _gearLineThreeMinRatio = 0;
        private int _gearLineThreeMaxRatio = 0;
        private int _gearLineFourMinRatio = 0;
        private int _gearLineFourMaxRatio = 0;
        private int _gearLineFiveMinRatio = 0;
        private int _gearLineFiveMaxRatio = 0;
        /// <summary>
        /// 档线空挡最小值
        /// </summary>
        public int GearLineNeutralMinRatio
        {
            get { return _gearLineNeutralMinRatio; }
            set { Set(() => GearLineNeutralMinRatio, ref _gearLineNeutralMinRatio, value); }
        }
        /// <summary>
        /// 档线空挡最大值
        /// </summary>
        public int GearLineNeutralMaxRatio
        {
            get { return _gearLineNeutralMaxRatio; }
            set { Set(() => GearLineNeutralMaxRatio, ref _gearLineNeutralMaxRatio, value); }
        }
        /// <summary>
        /// 档线一档最小值
        /// </summary>
        public int GearLineOneMinRatio
        {
            get { return _gearLineOneMinRatio; }
            set { Set(() => GearLineOneMinRatio, ref _gearLineOneMinRatio, value); }
        }
        /// <summary>
        /// 档线一档最大值
        /// </summary>
        public int GearLineOneMaxRatio
        {
            get { return _gearLineOneMaxRatio; }
            set { Set(() => GearLineOneMaxRatio, ref _gearLineOneMaxRatio, value); }
        }
        /// <summary>
        /// 档线二挡最小值
        /// </summary>
        public int GearLineTwoMinRatio
        {
            get { return _gearLineTwoMinRatio; }
            set { Set(() => GearLineTwoMinRatio, ref _gearLineTwoMinRatio, value); }
        }
        /// <summary>
        /// 档线二挡最大值
        /// </summary>
        public int GearLineTwoMaxRatio
        {
            get { return _gearLineTwoMaxRatio; }
            set { Set(() => GearLineTwoMaxRatio, ref _gearLineTwoMaxRatio, value); }
        }
        /// <summary>
        /// 档线三挡最小值
        /// </summary>
        public int GearLineThreeMinRatio
        {
            get { return _gearLineThreeMinRatio; }
            set { Set(() => GearLineThreeMinRatio, ref _gearLineThreeMinRatio, value); }
        }
        /// <summary>
        /// 档线三挡最大值
        /// </summary>
        public int GearLineThreeMaxRatio
        {
            get { return _gearLineThreeMaxRatio; }
            set { Set(() => GearLineThreeMaxRatio, ref _gearLineThreeMaxRatio, value); }
        }
        /// <summary>
        /// 档线四挡最小值
        /// </summary>
        public int GearLineFourMinRatio
        {
            get { return _gearLineFourMinRatio; }
            set { Set(() => GearLineFourMinRatio, ref _gearLineFourMinRatio, value); }
        }
        /// <summary>
        /// 档线四挡最大值
        /// </summary>
        public int GearLineFourMaxRatio
        {
            get { return _gearLineFourMaxRatio; }
            set { Set(() => GearLineFourMaxRatio, ref _gearLineFourMaxRatio, value); }
        }
        /// <summary>
        /// 档线五挡最小值
        /// </summary>
        public int GearLineFiveMinRatio
        {
            get { return _gearLineFiveMinRatio; }
            set { Set(() => GearLineFiveMinRatio, ref _gearLineFiveMinRatio, value); }
        }
        /// <summary>
        /// 档线五挡最大值
        /// </summary>
        public int GearLineFiveMaxRatio
        {
            get { return _gearLineFiveMaxRatio; }
            set { Set(() => GearLineFiveMaxRatio, ref _gearLineFiveMaxRatio, value); }
        }

        #endregion

        #region 信号取反

        private bool _fogLightReverseFlag = false;
        private bool _highBeamReverseFlag = false;
        private bool _lowBeamReverseFlag = false;
        private bool _loudspeakerReverseFlag = false;
        private bool _outlineLightReverseFlag = false;
        private bool _brakeReverseFlag = false;
        private bool _doorReverseFlag = false;
        private bool _clutchReverseFlag = false;
        private bool _safetyBeltReverseFlag = false;
        private bool _handbrakeReverseFlag = false;


        private bool _arrivedHeadstockReverseFlag = false;
        private bool _arrivedTailstockReverseFlag = false;

        //车头车尾2
        private bool _arrivedHeadstock2ReverseFlag = false;
        private bool _arrivedTailstock2ReverseFlag = false;


        private bool _isNeutralReverseFlag = false;
        private bool _reversingLightReverseFlag = false;
        private bool _exteriorMirrorReverseFlag = false;
        private bool _innerMirrorReverseFlag = false;
        private bool _seatsReverseFlag = false;
        private bool _leftIndicatorLightReverseFlag = false;
        private bool _rightIndicatorLightReverseFlag = false;


        private bool _gearDisplayD1ReverseFlag = false;
        private bool _gearDisplayD2ReverseFlag = false;
        private bool _gearDisplayD3ReverseFlag = false;
        private bool _gearDisplayD4ReverseFlag = false;

        private bool _engineReverseFlag = false;

        public bool LeftIndicatorLightReverseFlag
        {
            get { return _leftIndicatorLightReverseFlag; }
            set { Set(() => LeftIndicatorLightReverseFlag, ref _leftIndicatorLightReverseFlag, value); }
        }
        public bool RightIndicatorLightReverseFlag
        {
            get { return _rightIndicatorLightReverseFlag; }
            set { Set(() => RightIndicatorLightReverseFlag, ref _rightIndicatorLightReverseFlag, value); }
        }
        public bool SeatsReverseFlag
        {
            get { return _seatsReverseFlag; }
            set { Set(() => SeatsReverseFlag, ref _seatsReverseFlag, value); }
        }
        public bool InnerMirrorReverseFlag
        {
            get { return _innerMirrorReverseFlag; }
            set { Set(() => InnerMirrorReverseFlag, ref _innerMirrorReverseFlag, value); }
        }
        public bool ExteriorMirrorReverseFlag
        {
            get { return _exteriorMirrorReverseFlag; }
            set { Set(() => ExteriorMirrorReverseFlag, ref _exteriorMirrorReverseFlag, value); }
        }
        public bool ReversingLightReverseFlag
        {
            get { return _reversingLightReverseFlag; }
            set { Set(() => ReversingLightReverseFlag, ref _reversingLightReverseFlag, value); }
        }
        public bool IsNeutralReverseFlag
        {
            get { return _isNeutralReverseFlag; }
            set { Set(() => IsNeutralReverseFlag, ref _isNeutralReverseFlag, value); }
        }
        public bool ArrivedTailstockReverseFlag
        {
            get { return _arrivedTailstockReverseFlag; }
            set { Set(() => ArrivedTailstockReverseFlag, ref _arrivedTailstockReverseFlag, value); }
        }
        public bool ArrivedHeadstockReverseFlag
        {
            get { return _arrivedHeadstockReverseFlag; }
            set { Set(() => ArrivedHeadstockReverseFlag, ref _arrivedHeadstockReverseFlag, value); }
        }
        public bool HandbrakeReverseFlag
        {
            get { return _handbrakeReverseFlag; }
            set { Set(() => HandbrakeReverseFlag, ref _handbrakeReverseFlag, value); }
        }
        //车头2取反
        public bool ArrivedHeadstock2ReverseFlag
        {
            get { return _arrivedHeadstock2ReverseFlag; }
            set { Set(() => ArrivedHeadstock2ReverseFlag, ref _arrivedHeadstock2ReverseFlag, value); }
        }
        //车尾2取反
        public bool ArrivedTailstock2ReverseFlag
        {
            get { return _arrivedTailstock2ReverseFlag; }
            set { Set(() => ArrivedTailstock2ReverseFlag, ref _arrivedTailstock2ReverseFlag, value); }
        }
        public bool SafetyBeltReverseFlag
        {
            get { return _safetyBeltReverseFlag; }
            set { Set(() => SafetyBeltReverseFlag, ref _safetyBeltReverseFlag, value); }
        }
        public bool ClutchReverseFlag
        {
            get { return _clutchReverseFlag; }
            set { Set(() => ClutchReverseFlag, ref _clutchReverseFlag, value); }
        }
        public bool DoorReverseFlag
        {
            get { return _doorReverseFlag; }
            set { Set(() => DoorReverseFlag, ref _doorReverseFlag, value); }
        }
        public bool BrakeReverseFlag
        {
            get { return _brakeReverseFlag; }
            set { Set(() => BrakeReverseFlag, ref _brakeReverseFlag, value); }
        }
        public bool OutlineLightReverseFlag
        {
            get { return _outlineLightReverseFlag; }
            set { Set(() => OutlineLightReverseFlag, ref _outlineLightReverseFlag, value); }
        }
        public bool LoudspeakerReverseFlag
        {
            get { return _loudspeakerReverseFlag; }
            set { Set(() => LoudspeakerReverseFlag, ref _loudspeakerReverseFlag, value); }
        }
        public bool LowBeamReverseFlag
        {
            get { return _lowBeamReverseFlag; }
            set { Set(() => LowBeamReverseFlag, ref _lowBeamReverseFlag, value); }
        }
        public bool FogLightReverseFlag
        {
            get { return _fogLightReverseFlag; }
            set { Set(() => FogLightReverseFlag, ref _fogLightReverseFlag, value); }
        }
        public bool HighBeamReverseFlag
        {
            get { return _highBeamReverseFlag; }
            set { Set(() => HighBeamReverseFlag, ref _highBeamReverseFlag, value); }
        }


        public bool GearDisplayD1ReverseFlag
        {
            get { return _gearDisplayD1ReverseFlag; }
            set { Set(() => _gearDisplayD1ReverseFlag, ref _gearDisplayD1ReverseFlag, value); }
        }

        public bool GearDisplayD2ReverseFlag
        {
            get { return _gearDisplayD2ReverseFlag; }
            set { Set(() => GearDisplayD2ReverseFlag, ref _gearDisplayD2ReverseFlag, value); }
        }
        public bool GearDisplayD3ReverseFlag
        {
            get { return _gearDisplayD3ReverseFlag; }
            set { Set(() => GearDisplayD3ReverseFlag, ref _gearDisplayD3ReverseFlag, value); }
        }
        public bool GearDisplayD4ReverseFlag
        {
            get { return _gearDisplayD4ReverseFlag; }
            set { Set(() => GearDisplayD4ReverseFlag, ref _gearDisplayD4ReverseFlag, value); }
        }
        public bool EngineReverseFlag
        {
            get { return _engineReverseFlag; }
            set { Set(() => EngineReverseFlag, ref _engineReverseFlag, value); }
        }

        #endregion

        #region 信号地址
        private int _doorAddress = -1;
        private int _brakeAddress = -1;

        private int _fogLightAddress = -1;
        private int _highBeamAddress = -1;
        private int _lowBeamAddress = -1;
        private int _loudspeakerAddress = -1;
        private int _outlineLightAddress = -1;
        private int _clutchAddress = -1;
        private int _safetyBeltAddress = -1;
        private int _handbrakeAddress = -1;

        private int _arrivedHeadstockAddress = -1;
        private int _arrivedTailstockAddress = -1;

        //车头车尾2
        private int _arrivedHeadstock2Address = -1;
        private int _arrivedTailstock2Address = -1;


        private int _isNeutralAddress = -1;
        private int _reversingLightAddress = -1;
        private int _exteriorMirrorAddress = -1;
        private int _innerMirrorAddress = -1;
        private int _seatsAddress = -1;
        private int _rightIndicatorLightAddress = -1;
        private int _leftIndicatorLightAddress = -1;

        private int _gearDisplayD1Address = -1;
        private int _gearDisplayD2Address = -1;
        private int _gearDisplayD3Address = -1;
        private int _gearDisplayD4Address = -1;

        private int _engineAddress = -1;

        public int DoorAddress
        {
            get { return _doorAddress; }
            set { Set(() => DoorAddress, ref _doorAddress, value); }
        }
        public int FogLightAddress
        {
            get { return _fogLightAddress; }
            set { Set(() => FogLightAddress, ref _fogLightAddress, value); }
        }
        public int LowBeamAddress
        {
            get { return _lowBeamAddress; }
            set { Set(() => LowBeamAddress, ref _lowBeamAddress, value); }
        }

        public int HighBeamAddress
        {
            get { return _highBeamAddress; }
            set { Set(() => HighBeamAddress, ref _highBeamAddress, value); }
        }
        public int ClutchAddress
        {
            get { return _clutchAddress; }
            set { Set(() => ClutchAddress, ref _clutchAddress, value); }
        }


        public int LoudspeakerAddress
        {
            get { return _loudspeakerAddress; }
            set { Set(() => LoudspeakerAddress, ref _loudspeakerAddress, value); }
        }
        public int OutlineLightAddress
        {
            get { return _outlineLightAddress; }
            set { Set(() => OutlineLightAddress, ref _outlineLightAddress, value); }
        }
        public int SafetyBeltAddress
        {
            get { return _safetyBeltAddress; }
            set { Set(() => SafetyBeltAddress, ref _safetyBeltAddress, value); }
        }
        public int HandbrakeAddress
        {
            get { return _handbrakeAddress; }
            set { Set(() => HandbrakeAddress, ref _handbrakeAddress, value); }
        }
        public int ArrivedHeadstockAddress
        {
            get { return _arrivedHeadstockAddress; }
            set { Set(() => ArrivedHeadstockAddress, ref _arrivedHeadstockAddress, value); }
        }

        public int ArrivedTailstockAddress
        {
            get { return _arrivedTailstockAddress; }
            set { Set(() => ArrivedTailstockAddress, ref _arrivedTailstockAddress, value); }
        }

        public int ArrivedHeadstock2Address
        {
            get { return _arrivedHeadstock2Address; }
            set { Set(() => ArrivedHeadstock2Address, ref _arrivedHeadstock2Address, value); }
        }

        public int ArrivedTailstock2Address
        {
            get { return _arrivedTailstock2Address; }
            set { Set(() => ArrivedTailstock2Address, ref _arrivedTailstock2Address, value); }
        }
        public int IsNeutralAddress
        {
            get { return _isNeutralAddress; }
            set { Set(() => IsNeutralAddress, ref _isNeutralAddress, value); }
        }
        public int ReversingLightAddress
        {
            get { return _reversingLightAddress; }
            set { Set(() => ReversingLightAddress, ref _reversingLightAddress, value); }
        }
        public int ExteriorMirrorAddress
        {
            get { return _exteriorMirrorAddress; }
            set { Set(() => ExteriorMirrorAddress, ref _exteriorMirrorAddress, value); }
        }

        public int InnerMirrorAddress
        {
            get { return _innerMirrorAddress; }
            set { Set(() => InnerMirrorAddress, ref _innerMirrorAddress, value); }
        }

        public int SeatsAddress
        {
            get { return _seatsAddress; }
            set { Set(() => SeatsAddress, ref _seatsAddress, value); }
        }

        public int RightIndicatorLightAddress
        {
            get { return _rightIndicatorLightAddress; }
            set { Set(() => RightIndicatorLightAddress, ref _rightIndicatorLightAddress, value); }
        }
        public int LeftIndicatorLightAddress
        {
            get { return _leftIndicatorLightAddress; }
            set { Set(() => LeftIndicatorLightAddress, ref _leftIndicatorLightAddress, value); }
        }




        public int BrakeAddress
        {
            get { return _brakeAddress; }
            set { Set(() => BrakeAddress, ref _brakeAddress, value); }
        }

        public int GearDisplayD1Address
        {
            get { return _gearDisplayD1Address; }
            set { Set(() => GearDisplayD1Address, ref _gearDisplayD1Address, value); }
        }
        public int GearDisplayD2Address
        {
            get { return _gearDisplayD2Address; }
            set { Set(() => GearDisplayD2Address, ref _gearDisplayD2Address, value); }
        }
        public int GearDisplayD3Address
        {
            get { return _gearDisplayD3Address; }
            set { Set(() => GearDisplayD3Address, ref _gearDisplayD3Address, value); }
        }
        public int GearDisplayD4Address
        {
            get { return _gearDisplayD4Address; }
            set { Set(() => GearDisplayD4Address, ref _gearDisplayD4Address, value); }
        }

        public int EngineAddress
        {
            get { return _engineAddress; }
            set { Set(() => EngineAddress, ref _engineAddress, value); }
        }
        #endregion
        #endregion

        #region 全局评判参数
        #region 停车系数

        private double _parkingValueKmh = DefaultGlobalSettings.DefaultParkingValueKmh;
        private double _parkingDelaySeconds = DefaultGlobalSettings.DefaultParkingDelaySeconds;

        /// <summary>
        /// 停车系数（单位：公里/小时）
        /// </summary>
        public virtual double ParkingValueKmh
        {
            get { return _parkingValueKmh; }
            set { Set(() => ParkingValueKmh, ref _parkingValueKmh, value); }
        }
        /// <summary>
        /// 停车延迟(单位：毫秒）
        /// </summary>
        public virtual double ParkingDelaySeconds
        {
            get { return _parkingDelaySeconds; }
            set { Set(() => ParkingDelaySeconds, ref _parkingDelaySeconds, value); }
        }
        #endregion

        #region 考试设置


        private bool _JeepEdition = false;
        private int _examDistance = DefaultGlobalSettings.DefaultExamDistance;
        private int _nightDistance = DefaultGlobalSettings.DefaultNightDistance;
        private bool _gpsLogEnable = DefaultGlobalSettings.DefaultGpsLogEnable;
        private bool _checkLightingSimulation = true;
        private bool _continueExamIfFailed = true;
        private int _maxFromEdgeDistance = DefaultGlobalSettings.DefaultMaxFromEdgeDistance;
        private bool _voiceBrokenRule = true;
        private int _engineStopRmp = DefaultGlobalSettings.DefaultEngineStopRmp;
        //private int _vehicleReleaseHandbreakTimeout = DefaultGlobalSettings.DefaultReleaseHandbrakeTimeout;

        private int _examMapId = DefaultGlobalSettings.DefaultExamMapId;
        private int _brakeKeepTime = DefaultGlobalSettings.DefaultBrakeKeepTime;
        private bool _licenseC2 = DefaultGlobalSettings.DefaultLicenseC2;
        private bool _licenseC1 = DefaultGlobalSettings.DefaultLicenseC1;
        private bool _checkOBD = DefaultGlobalSettings.DefaultCheckOBD;

        private bool _endExamByDistance = DefaultGlobalSettings.DefaultEndExamByDistance;

        private AngleSource _angleSource = DefaultGlobalSettings.DefaultAngleSource;

        private GearSource _gearSource = DefaultGlobalSettings.DefaultGearSource;



        private bool _isNeutralGear = DefaultGlobalSettings.DefaultIsNeutralGear;

        private SpeedKhmMode _speedKhmMode = DefaultGlobalSettings.DefaultSpeedKhmMode;

        private ConnectionScheme _connectionScheme = DefaultGlobalSettings.DefaultConnectionScheme;

        private CarType _carType = CarType.XinShang;

        private float _multiSpeed = DefaultGlobalSettings.DefaultMultiSpeed;

        private bool _checkOBDRpm = DefaultGlobalSettings.DefaultCheckOBDRpm;


        private MileageSource _mileageSource = DefaultGlobalSettings.DefaultMilegeSource;


        private bool _pullOverStartFlage = false;



        public CarType CarType
        {
            get { return _carType; }
            set { Set(() => CarType, ref _carType, value); }
        }


        /// <summary>
        /// 读取OBD转速
        /// </summary>
        public bool JeepEdition
        {
            get { return _JeepEdition; }
            set { Set(() => JeepEdition, ref _JeepEdition, value); }
        }
        /// <su
        /// <summary>
        /// 读取OBD转速
        /// </summary>
        public bool CheckOBDRpm
        {
            get { return _checkOBDRpm; }
            set { Set(() => CheckOBDRpm, ref _checkOBDRpm, value); }
        }
        /// <summary>
        /// 速度放大/缩小倍率
        /// </summary>
        public float MultiSpeed
        {
            get { return _multiSpeed; }
            set { Set(() => MultiSpeed, ref _multiSpeed, value); }
        }
        /// <summary>
        /// 速度模式放大/缩小
        /// </summary>
        public SpeedKhmMode SpeedKhmMode
        {
            get { return _speedKhmMode; }
            set { Set(() => SpeedKhmMode, ref _speedKhmMode, value); }
        }
        /// <summary>
        /// 接线方案
        /// </summary>
        public ConnectionScheme ConnectionScheme
        {
            get { return _connectionScheme; }
            set { Set(() => ConnectionScheme, ref _connectionScheme, value); }
        }
        /// <summary>
        /// 是否安装空挡传感器
        /// </summary> 
        public bool IsNeutralGear
        {
            get { return _isNeutralGear; }
            set { Set(() => IsNeutralGear, ref _isNeutralGear, value); }
        }

        /// <summary>
        /// 读取OBD速度
        /// </summary>
        public bool CheckOBD
        {
            get { return _checkOBD; }
            set { Set(() => CheckOBD, ref _checkOBD, value); }
        }
        /// <summary>
        /// 驾考车型C2
        /// </summary>
        public bool LicenseC2
        {
            get { return _licenseC2; }
            set { Set(() => LicenseC2, ref _licenseC2, value); }
        }
        /// <summary>
        /// 驾考车型C1
        /// </summary>
        public bool LicenseC1
        {
            get { return _licenseC1; }
            set { Set(() => LicenseC1, ref _licenseC1, value); }
        }

        /// <summary>
        /// 踩刹车保持时间（减速项目）
        /// </summary>
        public int BrakeKeepTime
        {
            get { return _brakeKeepTime; }
            set { Set(() => BrakeKeepTime, ref _brakeKeepTime, value); }
        }

        /// <summary>
        /// 默认考试地图ID
        /// </summary>
        public int ExamMapId
        {
            get { return _examMapId; }
            set { Set(() => ExamMapId, ref _examMapId, value); }
        }

        /// <summary>
        /// 是否输出GPS日志
        /// </summary>
        public bool GpsLogEnable
        {
            get { return _gpsLogEnable; }
            set { Set(() => GpsLogEnable, ref _gpsLogEnable, value); }
        }

        /// <summary>
        /// 发动机熄火转速度设置
        /// </summary>
        public int EngineStopRmp
        {
            get { return _engineStopRmp; }
            set { Set(() => EngineStopRmp, ref _engineStopRmp, value); }
        }

        private bool _carSignalLogEnable = false;
        /// <summary>
        /// 是否启用车载信号
        /// </summary>
        public bool CarSignalLogEnable
        {
            get { return _carSignalLogEnable; }
            set { Set(() => CarSignalLogEnable, ref _carSignalLogEnable, value); }
        }

        private bool _debugLogEnable = false;
        /// <summary>
        /// 是否启动调试日志
        /// </summary>
        public bool DebugLogEnable
        {
            get { return _debugLogEnable; }
            set { Set(() => DebugLogEnable, ref _debugLogEnable, value); }
        }

        /// <summary>
        /// 档位来源
        /// </summary>
        public GearSource GearSource
        {
            get { return _gearSource; }
            set { Set(() => GearSource, ref _gearSource, value); }
        }




   


        /// <summary>
        /// 角度来源
        /// </summary>
        public virtual AngleSource AngleSource
        {
            get { return _angleSource; }
            set { Set(() => AngleSource, ref _angleSource, value); }
        }

        /// <summary>
        /// 白考考试里程
        /// </summary>
        public int ExamDistance
        {
            get { return _examDistance; }
            set { Set(() => ExamDistance, ref _examDistance, value); }
        }
        /// <summary>
        /// 夜考考试里程
        /// </summary>
        public int NightDistance
        {
            get { return _nightDistance; }
            set { Set(() => NightDistance, ref _nightDistance, value); }
        }


        /// <summary>
        /// 里程达到结束项目
        /// </summary>
        public bool EndExamByDistance
        {
            get { return _endExamByDistance; }
            set { Set(() => EndExamByDistance, ref _endExamByDistance, value); }
        }

        /// <summary>
        /// 考试模式，考试失败后是否继续考试
        /// </summary>
        public bool ContinueExamIfFailed
        {
            get { return _continueExamIfFailed; }
            set { Set(() => ContinueExamIfFailed, ref _continueExamIfFailed, value); }
        }
        /// <summary>
        /// 是否检测灯光模拟项
        /// </summary>
        public bool CheckLightingSimulation
        {
            get { return _checkLightingSimulation; }
            set { Set(() => CheckLightingSimulation, ref _checkLightingSimulation, value); }
        }
        /// <summary>
        /// 偏离道路最大距离（单位：米）
        /// </summary>
        public int MaxFromEdgeDistance
        {
            get { return _maxFromEdgeDistance; }
            set { Set(() => MaxFromEdgeDistance, ref _maxFromEdgeDistance, value); }
        }
        /// <summary>
        /// 语音播报犯规动作
        /// </summary>
        public bool VoiceBrokenRule
        {
            get { return _voiceBrokenRule; }
            set { Set(() => VoiceBrokenRule, ref _voiceBrokenRule, value); }
        }


        private bool _defaultSignalSourceEnable = true;

        /// <summary>
        /// 默认采用COM1作为信号来源
        /// </summary>
        public bool DefaultSignalSourceEnable
        {
            get { return _defaultSignalSourceEnable; }
            set { Set(() => DefaultSignalSourceEnable, ref _defaultSignalSourceEnable, value); }
        }




        public string SignalSource
        {
            get { return _signalSource; }
            set { Set(() => SignalSource, ref _signalSource, value); }
        }


        ///// <summary>
        ///// 不松手刹最小时间（单位：秒）
        ///// 不松手刹能纠正时间
        ///// </summary>
        //public int ReleaseHandbrakeTimeout
        //{
        //    get { return _vehicleReleaseHandbreakTimeout; }
        //    set { Set(() => ReleaseHandbrakeTimeout, ref _vehicleReleaseHandbreakTimeout, value); }
        //}

        #endregion

        #region 新增版本、OBD、方向角度 来源参数设置
        /// <summary>
        /// 串口 网口版本信号来源
        /// </summary>
        private string _signalSource = DefaultGlobalSettings.DefaultSignalSource;

        /// <summary>
        /// 主控箱版本
        /// </summary>
        private MasterControlBoxVersion _masterControlBoxVersion = DefaultGlobalSettings.DefultMasterControlBoxVersion;

        /// <summary>
        /// OBD 来源  机箱  平板
        /// </summary>
        private OBDSource _obdSource = DefaultGlobalSettings.DeafultOBDSource;

        /// <summary>
        /// OBD 接平板 默认 COM2 口 信号来源
        /// </summary>
        private string _obdSignalSource = DefaultGlobalSettings.DefaultOBDSignalSource;

        /// <summary>
        /// 陀螺仪接平板 默认 COM3 口信号来源
        /// </summary>
        private string _AngleSignalSource = DefaultGlobalSettings.DefaultAngleSignalSource;


        private GearConnectionMethod _gearConnectionMethod = GearConnectionMethod.Second;

        private string _bluetoothName;


        private string _bluetoothAddress;

        public string BluetoothAddress
        {
            get { return _bluetoothAddress; }
            set { Set(() => BluetoothAddress, ref _bluetoothAddress, value); }
        }


        public string BluetoothName
        {
            get { return _bluetoothName; }
            set { Set(() => BluetoothName, ref _bluetoothName, value); }
        }

        #endregion


        #region 发动机配置
        private int _maxEngineRpm = DefaultGlobalSettings.DefaultMaxEngineRpm;
        private int _maxRpmTime = 10;
        private int _minEngineRpm = DefaultGlobalSettings.DefaultMinEngineRpm;
        private bool _neutralStart = DefaultGlobalSettings.DefaultNeutralStart;
        private double _twiceGearNoSuccess = DefaultGlobalSettings.DefaultTwiceGearNoSuccess;

        /// <summary>
        /// 两次挂档不进
        /// </summary>
        public double TwiceGearNoSuccess
        {
            get { return _twiceGearNoSuccess; }
            set { Set(() => TwiceGearNoSuccess, ref _twiceGearNoSuccess, value); }
        }
        /// <summary>
        /// 必须空挡状态下启动发动机
        /// </summary>
        public bool NeutralStart
        {
            get { return _neutralStart; }
            set { Set(() => NeutralStart, ref _neutralStart, value); }
        }
        /// <summary>
        /// 发动机最高转速
        /// </summary>
        public int MaxEngineRpm
        {
            get { return _maxEngineRpm; }
            set { Set(() => MaxEngineRpm, ref _maxEngineRpm, value); }
        }

        public int MaxRpmTime
        {
            get { return _maxRpmTime; }
            set { Set(() => MaxRpmTime, ref _maxRpmTime, value); }
        }
        /// <summary>
        /// 发动机怠速
        /// </summary>
        public int MinEngineRpm
        {
            get { return _minEngineRpm; }
            set { Set(() => MinEngineRpm, ref _minEngineRpm, value); }
        }
        #endregion

        #region 全程档位和速度设置
        private int _globalLowestSpeed = DefaultGlobalSettings.DefaultGlobalLowestSpeed;
        private int _maxSpeed = DefaultGlobalSettings.DefaultMaxSpeed;
        private int _neutralTaxiingTimeout = DefaultGlobalSettings.DefaultNeutralTaxiingTimeout;
        private int _neutralTaxiingMaxDistance = DefaultGlobalSettings.DefaultNeutralTaxiingMaxDistance;
        private int _clutchTaxiingTimeout = DefaultGlobalSettings.DefaultClutchTaxiingTimeout;
        private int _gearOneMaxDistance = DefaultGlobalSettings.DefaultGearOneDrivingDistance;
        private int _gearOneTimeoutSencods = DefaultGlobalSettings.DefaultGearOneTimeout;
        private int _gearTwoMaxDistance = DefaultGlobalSettings.DefaultGearTwoDrivingDistance;
        private int _gearTwoTimeoutSencods = DefaultGlobalSettings.DefaultGearTwoTimeout;
        private double _brakeNotRide = DefaultGlobalSettings.DefaultBrakeNotRide;
        private double _globalChangeLanesAngle = DefaultGlobalSettings.DefaultGlobalChangeLanesAngle;
        //全程达到最低速度 保持时间
        private int _globalLowestSpeedHoldTimeSeconds = DefaultGlobalSettings.DefaultGlobalLowestSpeedHoldDistince;
        //全程达到最低速度 保持距离
        private int _globalLowestSpeedHoldDistince = DefaultGlobalSettings.DefaultGlobalLowestSpeedHoldTimeSeconds;

        //全程最低档位要求
        private int _globalLowestGear = 0;
        private int _gearThreeMaxDistance = 0;


        private int _clutchTaxiingDistance = 0;
        private int _clutchTaxiingSpeedLimit = 0;

        /// <summary>
        /// 全程最低速度保持距离
        /// </summary>
        public int GlobalLowestSpeedHoldDistince
        {
            get { return _globalLowestSpeedHoldDistince; }
            set { Set(() => GlobalLowestSpeedHoldDistince, ref _globalLowestSpeedHoldDistince, value); }
        }
         
        /// <summary>
        /// 全程最低档位
        /// </summary>
         public int GlobalLowestGear
        {
            get { return _globalLowestGear; }
            set { Set(() => GlobalLowestGear, ref _globalLowestGear, value); }
        }
        /// <summary>
        /// 全程最低速度保持时间
        /// </summary>
        /// 
        public int GlobalLowestSpeedHoldTimeSeconds
        {
            get { return _globalLowestSpeedHoldTimeSeconds; }
            set { Set(() => GlobalLowestSpeedHoldTimeSeconds, ref _globalLowestSpeedHoldTimeSeconds, value); }
        }
        /// <summary>
        /// 全程必须达到的最低速度
        /// </summary>
        public int GlobalLowestSpeed
        {
            get { return _globalLowestSpeed; }
            set { Set(() => GlobalLowestSpeed, ref _globalLowestSpeed, value); }
        }
        /// <summary>
        /// 全程最大速度限制
        /// </summary>
        public int MaxSpeed
        {
            get { return _maxSpeed; }
            set { Set(() => MaxSpeed, ref _maxSpeed, value); }
        }
        /// <summary>
        /// 空挡滑行最大时间
        /// </summary>
        public int NeutralTaxiingTimeout
        {
            get { return _neutralTaxiingTimeout; }
            set { Set(() => NeutralTaxiingTimeout, ref _neutralTaxiingTimeout, value); }
        }


        /// <summary>
        /// 离合滑行最大时间
        /// </summary>
        public int ClutchTaxiingTimeout
        {
            get { return _clutchTaxiingTimeout; }
            set { Set(() => ClutchTaxiingTimeout, ref _clutchTaxiingTimeout, value); }
        }
        /// <summary>
        /// 离合距离限制
        /// </summary>
        public int ClutchTaxiingDistance
        {
            get { return _clutchTaxiingDistance; }
            set { Set(() => ClutchTaxiingDistance, ref _clutchTaxiingDistance, value); }
        }
        /// <summary>
        /// 离合速度限制（该速度以下不检测）
        /// </summary>
        public int ClutchTaxiingSpeedLimit
        {
            get { return _clutchTaxiingSpeedLimit; }
            set { Set(() => ClutchTaxiingSpeedLimit, ref _clutchTaxiingSpeedLimit, value); }
        }
        /// <summary>
        /// 空挡滑行最大距离
        /// </summary>
        public int NeutralTaxiingMaxDistance
        {
            get { return _neutralTaxiingMaxDistance; }
            set { Set(() => NeutralTaxiingMaxDistance, ref _neutralTaxiingMaxDistance, value); }
        }

        /// <summary>
        /// 制动不平顺加速度
        /// </summary>
        public double BrakeNotRide
        {
            get { return _brakeNotRide; }
            set { Set(() => BrakeNotRide, ref _brakeNotRide, value); }
        }

        /// <summary>
        /// 全局变道角度
        /// </summary>
        public double GlobalChangeLanesAngle
        {
            get { return _globalChangeLanesAngle; }
            set { Set(() => GlobalChangeLanesAngle, ref _globalChangeLanesAngle, value); }
        }

        /// <summary>
        /// 一档最大行驶距离（单位：米）
        /// </summary>
        public int GearOneDrivingDistance
        {
            get { return _gearOneMaxDistance; }
            set { Set(() => GearOneDrivingDistance, ref _gearOneMaxDistance, value); }
        }
        /// <summary>
        /// 一档最大行驶时间（单位：秒）
        /// </summary>
        public int GearOneTimeout
        {
            get { return _gearOneTimeoutSencods; }
            set { Set(() => GearOneTimeout, ref _gearOneTimeoutSencods, value); }
        }
        /// <summary>
        /// 二档最大行驶距离（单位：米）
        /// </summary>
        public int GearTwoDrivingDistance
        {
            get { return _gearTwoMaxDistance; }
            set { Set(() => GearTwoDrivingDistance, ref _gearTwoMaxDistance, value); }
        }
        /// <summary>
        /// 3档累计行驶距离（单位：米）
        /// </summary>
        public int GearThreeDrivingDistance
        {
            get { return _gearThreeMaxDistance; }
            set { Set(() => GearThreeDrivingDistance, ref _gearThreeMaxDistance, value); }
        }
        /// <summary>
        /// 二档最最大驶时间（单位：秒）
        /// </summary>
        public int GearTwoTimeout
        {
            get { return _gearTwoTimeoutSencods; }
            set { Set(() => GearTwoTimeout, ref _gearTwoTimeoutSencods, value); }
        }
        #endregion

        #region 灯光检测
        private int _lowAndHighBeamDistance = DefaultGlobalSettings.DefaultLowAndHighBeamDistance;
        private int _indicatorLightTimeout = DefaultGlobalSettings.DefaultIndicatorLightTimeout;
        private double _turnLightAheadOfTime = DefaultGlobalSettings.DefaultTurnLightAheadOfTime;
        private int _indicatorLightDistanceOut = 0;

        /// <summary>
        /// 夜间远近光交替检测距离（单位：米）
        /// </summary>
        public int LowAndHighBeamDistance
        {
            get { return _lowAndHighBeamDistance; }
            set { Set(() => LowAndHighBeamDistance, ref _lowAndHighBeamDistance, value); }
        }
        /// <summary>
        /// 转向灯提前时间（单位：秒）
        /// </summary>
        public double TurnLightAheadOfTime
        {
            get { return _turnLightAheadOfTime; }
            set { Set(() => TurnLightAheadOfTime, ref _turnLightAheadOfTime, value); }
        }
        /// <summary>
        /// 左、右转向灯超时时间（单位：秒）；
        /// </summary>
        public int IndicatorLightTimeout
        {
            get { return _indicatorLightTimeout; }
            set { Set(() => IndicatorLightTimeout, ref _indicatorLightTimeout, value); }
        }

        /// <summary>
        /// 左、右转向灯超距（单位：秒）；
        /// </summary>
        public int IndicatorLightDistanceout
        {

            get { return _indicatorLightDistanceOut; }
            set { Set(() => IndicatorLightDistanceout, ref _indicatorLightDistanceOut, value); }
        }

        #endregion

        #region 档位速度控制

        private Gear _globalContinuousGear = DefaultGlobalSettings.DefaultGlobalContinuousGear;
        private int _globalContinuousSpeed = DefaultGlobalSettings.DefaultGlobalContinuousSpeed;
        private int _globalContinuousSeconds = DefaultGlobalSettings.DefaultGlobalContinuousSeconds;

        //private double _gpsDistance = 1;
        //private double _gpsAngle = 10;


        ///// <summary>
        ///// Gps距离
        ///// </summary>
        //public double GpsDistance
        //{
        //    get { return _gpsDistance; }
        //    set { Set(() => GpsDistance, ref _gpsDistance, value); }
        //}

        //public double GpsAngle
        //{
        //    get { return _gpsAngle; }
        //    set { Set(() => GpsAngle, ref _gpsAngle, value); }
        //}
        //
        /// <summary>
        /// 全程要求档位
        /// </summary>
        public Gear GlobalContinuousGear
        {
            get { return _globalContinuousGear; }
            set { Set(() => GlobalContinuousGear, ref _globalContinuousGear, value); }
        }
        /// <summary>
        /// 全程要求档位速度（单位：KM/H）
        /// </summary>
        public int GlobalContinuousSpeed
        {
            get { return _globalContinuousSpeed; }
            set { Set(() => GlobalContinuousSpeed, ref _globalContinuousSpeed, value); }
        }
        /// <summary>
        /// 全程要求档位持续时间（单位：秒）
        /// </summary>
        public int GlobalContinuousSeconds
        {
            get { return _globalContinuousSeconds; }
            set { Set(() => GlobalContinuousSeconds, ref _globalContinuousSeconds, value); }
        }
        #endregion

        #region 速度、档位控制
        private int _gearOneMaxSpeed = DefaultGlobalSettings.DefaultGearOneMaxSpeed;
        private int _gearTwoMinSpeed = DefaultGlobalSettings.DefaultGearTwoMinSpeed;
        private int _gearTwoMaxSpeed = DefaultGlobalSettings.DefaultGearTwoMaxSpeed;
        private int _gearThreeMinSpeed = DefaultGlobalSettings.DefaultGearThreeMinSpeed;
        private int _gearThreeMaxSpeed = DefaultGlobalSettings.DefaultGearThreeMaxSpeed;
        private int _gearFourMinSpeed = DefaultGlobalSettings.DefaultGearFourMinSpeed;
        private int _gearFourMaxSpeed = DefaultGlobalSettings.DefaultGearFourMaxSpeed;
        private int _gearFiveMinSpeed = DefaultGlobalSettings.DefaultGearFiveMinSpeed;
        private int _gearFiveMaxSpeed = DefaultGlobalSettings.DefaultGearFiveMaxSpeed;
        private int _speedAndGearTimeout = DefaultGlobalSettings.DefaultSpeedAndGearTimeout;

        /// <summary>
        /// 速度档位、超时时间，单位：秒；配置0，无效
        /// </summary>
        public int SpeedAndGearTimeout
        {
            get { return _speedAndGearTimeout; }
            set { Set(() => SpeedAndGearTimeout, ref _speedAndGearTimeout, value); }
        }
        /// <summary>
        /// 一档最大速度
        /// </summary>
        public int GearOneMaxSpeed
        {
            get { return _gearOneMaxSpeed; }
            set { Set(() => GearOneMaxSpeed, ref _gearOneMaxSpeed, value); }
        }
        /// <summary>
        /// 二档最低速度
        /// </summary>
        public int GearTwoMinSpeed
        {
            get { return _gearTwoMinSpeed; }
            set { Set(() => GearTwoMinSpeed, ref _gearTwoMinSpeed, value); }
        }
        /// <summary>
        /// 二档最大速度
        /// </summary>
        public int GearTwoMaxSpeed
        {
            get { return _gearTwoMaxSpeed; }
            set { Set(() => GearTwoMaxSpeed, ref _gearTwoMaxSpeed, value); }
        }
        /// <summary>
        /// 三档最低速度
        /// </summary>
        public int GearThreeMinSpeed
        {
            get { return _gearThreeMinSpeed; }
            set { Set(() => GearThreeMinSpeed, ref _gearThreeMinSpeed, value); }
        }
        /// <summary>
        /// 三档最大速度
        /// </summary>
        public int GearThreeMaxSpeed
        {
            get { return _gearThreeMaxSpeed; }
            set { Set(() => GearThreeMaxSpeed, ref _gearThreeMaxSpeed, value); }
        }
        /// <summary>
        /// 四档最低速度
        /// </summary>
        public int GearFourMinSpeed
        {
            get { return _gearFourMinSpeed; }
            set { Set(() => GearFourMinSpeed, ref _gearFourMinSpeed, value); }
        }
        /// <summary>
        /// 四档最大速度
        /// </summary>
        public int GearFourMaxSpeed
        {
            get { return _gearFourMaxSpeed; }
            set { Set(() => GearFourMaxSpeed, ref _gearFourMaxSpeed, value); }
        }
        /// <summary>
        /// 五档最低速度
        /// </summary>
        public int GearFiveMinSpeed
        {
            get { return _gearFiveMinSpeed; }
            set { Set(() => GearFiveMinSpeed, ref _gearFiveMinSpeed, value); }
        }
        /// <summary>
        /// 五档最大速度
        /// </summary>
        public int GearFiveMaxSpeed
        {
            get { return _gearFiveMaxSpeed; }
            set { Set(() => GearFiveMaxSpeed, ref _gearFiveMaxSpeed, value); }
        }
        #endregion

        #endregion

        #region 专项评判参数

        #region 上车准备

        private bool _aroundCarEnable = DefaultGlobalSettings.DefaultAroundCarEnable;
        private bool _prepareDrivingEnable = DefaultGlobalSettings.DefaultPrepareDrivingEnable;
        private int _aroundCarTimeout = DefaultGlobalSettings.DefaultAroundCarTimeout;
        private bool _aroundCarVoiceEnable = DefaultGlobalSettings.DefaultAroundCarVoiceEnable;
        private bool _prepareDriving3TouchVoice = true;

        private bool _prepareDrivingTailStockEnable = true;
        private bool _prepareDrivingHeadStockEnable = true;
        private bool _prepareDrivingTailStock2Enable = true;
        private bool _prepareDrivingHeadStock2Enable = true;
        private PrepareDrivingEndFlag _prepareDrivingEndFlag = PrepareDrivingEndFlag.SafeBelt;

        private string _prepareDrivingHeadstockVoice = "学员正经过车头";
        private string _prepareDrivingTailstockVoice = "学员正经过车尾";
        private string _prepareDrivingHeadstock2Voice = "学员正经过车头2";
        private string _prepareDrivingTailstock2Voice = "学员正经过车尾2";
        private string _prepareDrivingTest = "test";

        
        private bool _PrepareDrivingOrder = false;
        public string PrepareDrivingTest
        {
            get { return _prepareDrivingTest; }
            set { Set(() => _prepareDrivingTest, ref _prepareDrivingTest, value); }
        }

        public bool PrepareDrivingOrder
        {
            get { return _PrepareDrivingOrder; }
            set { Set(() => PrepareDrivingOrder, ref _PrepareDrivingOrder, value); }
        }
        /// <summary>
        /// 通过车头语音
        /// </summary>
        public string PrepareDrivingHeadstockVoice
        {
            get { return _prepareDrivingHeadstockVoice; }
            set { Set(() => PrepareDrivingHeadstockVoice, ref _prepareDrivingHeadstockVoice, value); }
        }
        /// <summary>
        /// 通过车尾语音
        /// </summary>
        public string PrepareDrivingTailstockVoice
        {
            get { return _prepareDrivingTailstockVoice; }
            set { Set(() => PrepareDrivingTailstockVoice, ref _prepareDrivingTailstockVoice, value); }
        }
        /// <summary>
        /// 通过车头2语音
        /// </summary>
        public string PrepareDrivingHeadstock2Voice
        {
            get { return _prepareDrivingHeadstock2Voice; }
            set { Set(() => PrepareDrivingHeadstock2Voice, ref _prepareDrivingHeadstock2Voice, value); }
        }
        /// <summary>
        /// 通过车尾2语音
        /// </summary>
        public string PrepareDrivingTailstock2Voice
        {
            get { return _prepareDrivingTailstock2Voice; }
            set { Set(() => PrepareDrivingTailstock2Voice, ref _prepareDrivingTailstock2Voice, value); }
        }
        /// <summary>
        /// 绕车一周语音是否启用
        /// </summary>
        public bool AroundCarVoiceEnable
        {
            get { return _aroundCarVoiceEnable; }
            set { Set(() => AroundCarVoiceEnable, ref _aroundCarVoiceEnable, value); }
        }
        /// <summary>
        /// 上车准备是否启用
        /// </summary>
        public bool PrepareDrivingEnable
        {
            get { return _prepareDrivingEnable; }
            set { Set(() => PrepareDrivingEnable, ref _prepareDrivingEnable, value); }
        }
        /// <summary>
        /// 绕车一周是否启用
        /// </summary>
        public bool AroundCarEnable
        {
            get { return _aroundCarEnable; }
            set { Set(() => AroundCarEnable, ref _aroundCarEnable, value); }
        }
        /// <summary>
        /// 绕车一周最大时间
        /// </summary>
        public int AroundCarTimeout
        {
            get { return _aroundCarTimeout; }
            set { Set(() => AroundCarTimeout, ref _aroundCarTimeout, value); }
        }
        /// <summary>
        /// 3摸语音
        /// </summary>
        public bool PrepareDriving3TouchVoice
        {
            get { return _prepareDriving3TouchVoice; }
            set { Set(() => PrepareDriving3TouchVoice, ref _prepareDriving3TouchVoice, value); }
        }


        /// <summary>
        /// 车尾
        /// </summary>
        public bool PrepareDrivingTailStockEnable
        {
            get { return _prepareDrivingTailStockEnable; }
            set { Set(() => PrepareDrivingTailStockEnable, ref _prepareDrivingTailStockEnable, value); }
        }

        /// <summary>
        /// 车头
        /// </summary>
        public bool PrepareDrivingHeadStockEnable
        {
            get { return _prepareDrivingHeadStockEnable; }
            set { Set(() => PrepareDrivingHeadStockEnable, ref _prepareDrivingHeadStockEnable, value); }
        }

        /// <summary>
        /// 车尾2
        /// </summary>
        public bool PrepareDrivingTailStock2Enable
        {
            get { return _prepareDrivingTailStock2Enable; }
            set { Set(() => PrepareDrivingTailStock2Enable, ref _prepareDrivingTailStock2Enable, value); }
        }
        /// <summary>
        /// 车头2
        /// </summary>
        public bool PrepareDrivingHeadStock2Enable
        {
            get { return _prepareDrivingHeadStock2Enable; }
            set { Set(() => PrepareDrivingHeadStock2Enable, ref _prepareDrivingHeadStock2Enable, value); }
        }
        /// <summary>
        /// 上车结束标志
        /// </summary>
        public PrepareDrivingEndFlag PrepareDrivingEndFlag
        {
            get { return _prepareDrivingEndFlag; }
            set { Set(() => PrepareDrivingEndFlag, ref _prepareDrivingEndFlag, value); }
        }


        #endregion

        #region 灯光模拟
        private double _simulationLightTimeout = DefaultGlobalSettings.DefaultSimulationLightTimeout;
        private double _simulationLightInterval = DefaultGlobalSettings.DefaultSimulationLightInterval;

        private bool _simulationsLightOnDay = DefaultGlobalSettings.DefaultSimulationsLightOnDay;
        private bool _simulationsLightOnNight = DefaultGlobalSettings.DefaultSimulationsLightOnNight;

        private bool _lightVoice = DefaultGlobalSettings.DefaultSimulationsLightOnNight;
        private bool _lightEndVoice = DefaultGlobalSettings.DefaultSimulationsLightOnNight;

        //是否播放默认的叮的一声
        private bool _isPlayDingVoice = false;


        public bool IsPlayDingVoice
        {
            get { return _isPlayDingVoice; }
            set { Set(() => IsPlayDingVoice, ref _isPlayDingVoice, value); }
        }
        /// <summary>
        /// 灯光模拟开始语音
        /// </summary>
        public bool LightVoice
        {
            get { return _lightVoice; }
            set { Set(() => LightVoice, ref _lightVoice, value); }
        }
        /// <summary>
        /// 灯光模拟结束语音
        /// </summary>
        public bool LightEndVoice
        {
            get { return _lightEndVoice; }
            set { Set(() => LightEndVoice, ref _lightEndVoice, value); }
        }

        /// <summary>
        /// 白天灯光模拟
        /// </summary>
        public bool SimulationsLightOnDay
        {
            get { return _simulationsLightOnDay; }
            set { Set(() => SimulationsLightOnDay, ref _simulationsLightOnDay, value); }
        }

        /// <summary>
        /// 夜间灯光模拟
        /// </summary>
        public bool SimulationsLightOnNight
        {
            get { return _simulationsLightOnNight; }
            set { Set(() => SimulationsLightOnNight, ref _simulationsLightOnNight, value); }
        }

        /// <summary>
        /// 灯光模拟，每个语音间隔
        /// </summary>
        public double SimulationLightTimeout
        {
            get { return _simulationLightTimeout; }
            set { Set(() => SimulationLightTimeout, ref _simulationLightTimeout, value); }
        }
        /// <summary>
        /// 灯光模拟，时间间隔：单位：秒
        /// </summary>
        public double SimulationLightInterval
        {
            get { return _simulationLightInterval; }
            set { Set(() => SimulationLightInterval, ref _simulationLightInterval, value); }
        }

        #endregion

        #region 起步

        private double _startVehicleBackforwardMinDistance = DefaultGlobalSettings.DefaultStartSmallBackwardDistance;
        private double _startVehicleBackforwardMaxDistance = DefaultGlobalSettings.DefaultStartLargeBackwardDistance;
        private double _startStopCheckForwardDistance = DefaultGlobalSettings.DefaultStartStopCheckForwardDistance;
        private int _startVehicleTimeout = DefaultGlobalSettings.DefaultStartTimeout;
        private int _startEngineRpm = DefaultGlobalSettings.DefaultStartEngineRpm;
        private bool _isCheckStartLight = DefaultGlobalSettings.DefaultIsCheckStartLight;
        private bool _isCheckStartLightOnNight = DefaultGlobalSettings.DefaultIsCheckStartLightOnNight;
        private bool _vehicleStartingLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultVehicleStartingLoudSpeakerDayCheck;
        private bool _vehicleStartingLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultVehicleStartingLoudSpeakerNightCheck;
        private bool _startVoice = DefaultGlobalSettings.DefaultStartVoice;
        //这个值默认第一次加载回去数据库读取

        private int _startReleaseHandbrakeTimeout = DefaultGlobalSettings.DefaultStartReleaseHandbrakeTimeout;
        private bool _startLowAndHighBeamInNight = DefaultGlobalSettings.DefaultStartLowAndHighBeamInNight;

        private bool _startShockEnable = DefaultGlobalSettings.DefaultStartShockEnable;
        private double _startShockValue = DefaultGlobalSettings.DefaultStartShockValue;
        private double _startShockCount = DefaultGlobalSettings.DefaultStartShockCount;

        private bool _startIsMustGearOne = false;


        public bool StartIsMustGearOne
        {
            get { return _startIsMustGearOne; }
            set { Set(() => StartIsMustGearOne, ref _startIsMustGearOne, value); }
        }
        /// <summary>
        /// 起步手刹超时时间（单位：秒）
        /// </summary>
        public int StartReleaseHandbrakeTimeout
        {
            get { return _startReleaseHandbrakeTimeout; }
            set { Set(() => StartReleaseHandbrakeTimeout, ref _startReleaseHandbrakeTimeout, value); }
        }
        /// <summary>
        /// 起步项目语音
        /// </summary>
        public bool StartVoice
        {
            get { return _startVoice; }
            set { Set(() => StartVoice, ref _startVoice, value); }
        }


        /// <summary>
        /// 起步白考喇叭检测
        /// </summary>
        public bool VehicleStartingLoudSpeakerDayCheck
        {
            get { return _vehicleStartingLoudSpeakerDayCheck; }
            set { Set(() => VehicleStartingLoudSpeakerDayCheck, ref _vehicleStartingLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 起步夜考喇叭检测
        /// </summary>
        public bool VehicleStartingLoudSpeakerNightCheck
        {
            get { return _vehicleStartingLoudSpeakerNightCheck; }
            set { Set(() => VehicleStartingLoudSpeakerNightCheck, ref _vehicleStartingLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 起步后溜最小距离（单位：米）
        /// </summary>
        public double StartSmallBackwardDistance
        {
            get { return _startVehicleBackforwardMinDistance; }
            set { Set(() => StartSmallBackwardDistance, ref _startVehicleBackforwardMinDistance, value); }
        }
        /// <summary>
        /// 起步后溜最大距离（单位：米）
        /// </summary>
        public double StartLargeBackwardDistance
        {
            get { return _startVehicleBackforwardMaxDistance; }
            set { Set(() => StartLargeBackwardDistance, ref _startVehicleBackforwardMaxDistance, value); }
        }
        /// <summary>
        /// 起步项目距离，单位：米
        /// </summary>
        public double StartStopCheckForwardDistance
        {
            get { return _startStopCheckForwardDistance; }
            set { Set(() => StartStopCheckForwardDistance, ref _startStopCheckForwardDistance, value); }
        }
        /// <summary>
        /// 起步时间（单位：秒）
        /// </summary>
        public int StartTimeout
        {
            get { return _startVehicleTimeout; }
            set { Set(() => StartTimeout, ref _startVehicleTimeout, value); }
        }
        /// <summary>
        /// 起步时发动机最高转速（配置0，不评判）
        /// </summary>
        public int StartEngineRpm
        {
            get { return _startEngineRpm; }
            set { Set(() => StartEngineRpm, ref _startEngineRpm, value); }
        }
        /// <summary>
        /// 起步检测白考左转向灯
        /// </summary>
        public bool IsCheckStartLight
        {
            get { return _isCheckStartLight; }
            set { Set(() => IsCheckStartLight, ref _isCheckStartLight, value); }
        }
        /// <summary>
        /// 起步检测夜间左转向灯
        /// </summary>
        public bool IsCheckStartLightOnNight
        {
            get { return _isCheckStartLightOnNight; }
            set { Set(() => IsCheckStartLightOnNight, ref _isCheckStartLightOnNight, value); }
        }
        /// <summary>
        /// 起步夜间检测远近光交替
        /// </summary>
        public bool StartLowAndHighBeamInNight
        {
            get { return _startLowAndHighBeamInNight; }
            set { Set(() => StartLowAndHighBeamInNight, ref _startLowAndHighBeamInNight, value); }
        }

        /// <summary>
        /// 起步闯动检测
        /// </summary>
        public bool StartShockEnable
        {
            get { return _startShockEnable; }
            set { Set(() => StartShockEnable, ref _startShockEnable, value); }
        }


        /// <summary>
        /// 起步闯动值
        /// </summary>
        public double StartShockValue
        {
            get { return _startShockValue; }
            set { Set(() => StartShockValue, ref _startShockValue, value); }
        }

        /// <summary>
        /// 起步闯动次数
        /// </summary>
        public double StartShockCount
        {
            get { return _startShockCount; }
            set { Set(() => StartShockCount, ref _startShockCount, value); }
        }

        #endregion

        #region 路口直行
        private bool _straightThroughIntersectionVoice = DefaultGlobalSettings.DefaultStraightThroughIntersectionVoice;
        private bool _straightThroughIntersectionBrakeRequire = DefaultGlobalSettings.DefaultStraightThroughIntersectionBrakeRequire;
        private int _straightThroughIntersectionDistance = DefaultGlobalSettings.DefaultStraightThroughIntersectionDistance;
        private int _straightThroughIntersectionSpeedLimit = DefaultGlobalSettings.DefaultStraightThroughIntersectionSpeedLimit;
        private int _straightThroughIntersectionBrakeSpeedUp = DefaultGlobalSettings.DefaultStraightThroughIntersectionBrakeSpeedUp;
        private bool _straightThroughIntersectionLightCheck = DefaultGlobalSettings.DefaultStraightThroughIntersectionLightCheck;
        private bool _straightThroughIntersectionLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultStraightThroughIntersectionLoudSpeakerDayCheck;
        private bool _straightThroughIntersectionLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultStraightThroughIntersectionLoudSpeakerNightCheck;
        private double _throughStraightPrepareD = 0;
        /// <summary>
        /// 路口直行准备距离
        /// </summary>
        public double ThroughStraightPrepareD
        {
            get { return _throughStraightPrepareD; }
            set { Set(() => ThroughStraightPrepareD, ref _throughStraightPrepareD, value); }
        }
        /// <summary>
        /// 路口直行项目语音
        /// </summary>
        public bool StraightThroughIntersectionVoice
        {
            get { return _straightThroughIntersectionVoice; }
            set { Set(() => StraightThroughIntersectionVoice, ref _straightThroughIntersectionVoice, value); }
        }
        /// <summary>
        /// 路口直行白考喇叭检测
        /// </summary>
        public bool StraightThroughIntersectionLoudSpeakerDayCheck
        {
            get { return _straightThroughIntersectionLoudSpeakerDayCheck; }
            set { Set(() => StraightThroughIntersectionLoudSpeakerDayCheck, ref _straightThroughIntersectionLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 路口直行夜考喇叭检测
        /// </summary>
        public bool StraightThroughIntersectionLoudSpeakerNightCheck
        {
            get { return _straightThroughIntersectionLoudSpeakerNightCheck; }
            set { Set(() => StraightThroughIntersectionLoudSpeakerNightCheck, ref _straightThroughIntersectionLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 路口直行项目距离
        /// </summary>
        public int StraightThroughIntersectionDistance
        {
            get { return _straightThroughIntersectionDistance; }
            set { Set(() => StraightThroughIntersectionDistance, ref _straightThroughIntersectionDistance, value); }
        }
        /// <summary>
        /// 路口直行夜考远近光交替
        /// </summary>
        public bool StraightThroughIntersectionLightCheck
        {
            get { return _straightThroughIntersectionLightCheck; }
            set { Set(() => StraightThroughIntersectionLightCheck, ref _straightThroughIntersectionLightCheck, value); }
        }
        /// <summary>
        /// 路口直行必须踩刹车
        /// </summary>
        public bool StraightThroughIntersectionBrakeRequire
        {
            get { return _straightThroughIntersectionBrakeRequire; }
            set { Set(() => StraightThroughIntersectionBrakeRequire, ref _straightThroughIntersectionBrakeRequire, value); }
        }
        /// <summary>
        /// 路口直行速度限制
        /// </summary>
        public int StraightThroughIntersectionSpeedLimit
        {
            get { return _straightThroughIntersectionSpeedLimit; }
            set { Set(() => StraightThroughIntersectionSpeedLimit, ref _straightThroughIntersectionSpeedLimit, value); }
        }
        /// <summary>
        /// 路口直行要求踩刹车速度限制
        /// </summary>
        public int StraightThroughIntersectionBrakeSpeedUp
        {
            get { return _straightThroughIntersectionBrakeSpeedUp; }
            set { Set(() => StraightThroughIntersectionBrakeSpeedUp, ref _straightThroughIntersectionBrakeSpeedUp, value); }
        }
        #endregion

        #region 路口右转
        private bool _turnRightVoice = DefaultGlobalSettings.DefaultTurnRightVoice;
        private bool _turnRightBrakeRequire = DefaultGlobalSettings.DefaultTurnRightBrakeRequire;
        private int _turnRightDistance = DefaultGlobalSettings.DefaultTurnRightDistance;
        private int _turnRightSpeedLimit = DefaultGlobalSettings.DefaultTurnRightSpeedLimit;
        private int _turnRightBrakeSpeedUp = DefaultGlobalSettings.DefaultTurnRightBrakeSpeedUp;
        private bool _turnRightLightCheck = DefaultGlobalSettings.DefaultTurnRightLightCheck;
        private bool _turnRightLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultTurnRightLoudSpeakerDayCheck;
        private bool _turnRightLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultTurnRightLoudSpeakerNightCheck;
        private double _turnRightAngle = 0;

        private bool _turnRightEndFlag = false;

        private bool _turnRightErrorLight = false;
        private double _turnRightPrepareD = 0;
        /// <summary>
        /// 准备距离
        /// </summary>
        public double TurnRightPrepareD
        {
            get { return _turnRightPrepareD; }
            set { Set(() => TurnRightPrepareD, ref _turnRightPrepareD, value); }
        }
        /// <summary>
        /// 打错转向灯
        /// </summary>
        public bool TurnRightErrorLight
        {
            get { return _turnRightErrorLight; }
            set { Set(() => TurnRightErrorLight, ref _turnRightErrorLight, value); }
        }
        /// <summary>
        /// 角度达到就结束
        /// </summary>
        public bool TurnRightEndFlag
        {
            get { return _turnRightEndFlag; }
            set { Set(() => TurnRightEndFlag, ref _turnRightEndFlag, value); }
        }
        /// <summary>
        /// 右转角度
        /// </summary>
        public double TurnRightAngle
        {
            get { return _turnRightAngle; }
            set { Set(() => TurnRightAngle, ref _turnRightAngle, value); }
        }
        /// <summary>
        /// 路口右转白考喇叭检测
        /// </summary>
        public bool TurnRightLoudSpeakerDayCheck
        {
            get { return _turnRightLoudSpeakerDayCheck; }
            set { Set(() => TurnRightLoudSpeakerDayCheck, ref _turnRightLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 路口右转夜考喇叭检测
        /// </summary>
        public bool TurnRightLoudSpeakerNightCheck
        {
            get { return _turnRightLoudSpeakerNightCheck; }
            set { Set(() => TurnRightLoudSpeakerNightCheck, ref _turnRightLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 路口右转右转向灯检测
        /// </summary>
        public bool TurnRightLightCheck
        {
            get { return _turnRightLightCheck; }
            set { Set(() => TurnRightLightCheck, ref _turnRightLightCheck, value); }
        }
        /// <summary>
        /// 路口右转项目语音
        /// </summary>
        public bool TurnRightVoice
        {
            get { return _turnRightVoice; }
            set { Set(() => TurnRightVoice, ref _turnRightVoice, value); }
        }
        /// <summary>
        /// 路口右转项目距离
        /// </summary>
        public int TurnRightDistance
        {
            get { return _turnRightDistance; }
            set { Set(() => TurnRightDistance, ref _turnRightDistance, value); }
        }
        /// <summary>
        /// 路口右转必须踩刹车
        /// </summary>
        public bool TurnRightBrakeRequire
        {
            get { return _turnRightBrakeRequire; }
            set { Set(() => TurnRightBrakeRequire, ref _turnRightBrakeRequire, value); }
        }
        /// <summary>
        /// 路口右转速度限制
        /// </summary>
        public int TurnRightSpeedLimit
        {
            get { return _turnRightSpeedLimit; }
            set { Set(() => TurnRightSpeedLimit, ref _turnRightSpeedLimit, value); }
        }
        /// <summary>
        /// 路口右转要求踩刹车速度限制
        /// </summary>
        public int TurnRightBrakeSpeedUp
        {
            get { return _turnRightBrakeSpeedUp; }
            set { Set(() => TurnRightBrakeSpeedUp, ref _turnRightBrakeSpeedUp, value); }
        }
        #endregion

        #region 路口左转
        private bool _turnLeftVoice = DefaultGlobalSettings.DefaultTurnLeftVoice;
        private bool _turnLeftBrakeRequire = DefaultGlobalSettings.DefaultTurnLeftBrakeRequire;
        private int _turnLeftDistance = DefaultGlobalSettings.DefaultTurnLeftDistance;
        private int _turnLeftSpeedLimit = DefaultGlobalSettings.DefaultTurnLeftSpeedLimit;
        private int _turnLeftBrakeSpeedUp = DefaultGlobalSettings.DefaultTurnLeftBrakeSpeedUp;
        private bool _turnLeftLightCheck = DefaultGlobalSettings.DefaultTurnLeftLightCheck;
        private bool _turnLeftLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultTurnLeftLoudSpeakerDayCheck;
        private bool _turnLeftLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultTurnLeftLoudSpeakerNightCheck;
        private double _turnLeftAngle = 0;
        private bool _turnLeftEndFlag = false;

        private bool _turnLeftErrorLight = false;

        private double _turnLeftPrepareD = 0;

        /// <summary>
        /// 准备距离
        /// </summary>
        public double TurnLeftPrepareD
        {
            get { return _turnLeftPrepareD; }
            set { Set(() => TurnLeftPrepareD, ref _turnLeftPrepareD, value); }
        }
        /// <summary>
        /// 检测打错专项灯
        /// </summary>
        public bool TurnLeftErrorLight
        {
            get { return _turnLeftErrorLight; }
            set { Set(() => TurnLeftErrorLight, ref _turnLeftErrorLight, value); }
        }

        /// <summary>
        /// 角度达到就完成
        /// </summary>
        public bool TurnLeftEndFlag
        {
            get { return _turnLeftEndFlag; }
            set { Set(() => TurnLeftEndFlag, ref _turnLeftEndFlag, value); }
        }
        /// <summary>
        /// 左转角度检测
        /// </summary>
        public double TurnLeftAngle
        {
            get { return _turnLeftAngle; }
            set { Set(() => TurnLeftAngle, ref _turnLeftAngle, value); }
        }
        /// <summary>
        /// 路口左转白考喇叭检测
        /// </summary>
        public bool TurnLeftLoudSpeakerDayCheck
        {
            get { return _turnLeftLoudSpeakerDayCheck; }
            set { Set(() => TurnLeftLoudSpeakerDayCheck, ref _turnLeftLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 路口左转夜考喇叭检测
        /// </summary>
        public bool TurnLeftLoudSpeakerNightCheck
        {
            get { return _turnLeftLoudSpeakerNightCheck; }
            set { Set(() => TurnLeftLoudSpeakerNightCheck, ref _turnLeftLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 路口左转转向灯检测
        /// </summary>
        public bool TurnLeftLightCheck
        {
            get { return _turnLeftLightCheck; }
            set { Set(() => TurnLeftLightCheck, ref _turnLeftLightCheck, value); }
        }
        /// <summary>
        /// 路口左转项目语音
        /// </summary>
        public bool TurnLeftVoice
        {
            get { return _turnLeftVoice; }
            set { Set(() => TurnLeftVoice, ref _turnLeftVoice, value); }
        }
        /// <summary>
        /// 路口左转项目距离
        /// </summary>
        public int TurnLeftDistance
        {
            get { return _turnLeftDistance; }
            set { Set(() => TurnLeftDistance, ref _turnLeftDistance, value); }
        }
        /// <summary>
        /// 路口左转必须踩刹车
        /// </summary>
        public bool TurnLeftBrakeRequire
        {
            get { return _turnLeftBrakeRequire; }
            set { Set(() => TurnLeftBrakeRequire, ref _turnLeftBrakeRequire, value); }
        }
        /// <summary>
        /// 路口左转速度限制
        /// </summary>
        public int TurnLeftSpeedLimit
        {
            get { return _turnLeftSpeedLimit; }
            set { Set(() => TurnLeftSpeedLimit, ref _turnLeftSpeedLimit, value); }
        }
        /// <summary>
        /// 路口左转要求踩刹车速度限制
        /// </summary>
        public int TurnLeftBrakeSpeedUp
        {
            get { return _turnLeftBrakeSpeedUp; }
            set { Set(() => TurnLeftBrakeSpeedUp, ref _turnLeftBrakeSpeedUp, value); }
        }
        #endregion

        #region 掉头
        private int _turnRoundStartAngleDiff = DefaultGlobalSettings.DefaultTurnRoundStartAngleDiff;
        private int _turnRoundEndAngleDiff = DefaultGlobalSettings.DefaultTurnRoundEndAngleDiff;
        private int _turnRoundDistance = DefaultGlobalSettings.DefaultTurnRoundMaxDistance;
        private bool _turnRoundVoice = DefaultGlobalSettings.DefaultTurnRoundVoice;
        private bool _turnRoundLightCheck = DefaultGlobalSettings.DefaultTurnRoundLightCheck;
        private bool _turnRoundLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultTurnRoundLoudSpeakerDayCheck;
        private bool _turnRoundLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultTurnRoundLoudSpeakerNightCheck;
        private bool _turnRoundBrakeRequired = DefaultGlobalSettings.DefaultTurnRoundBrakeRequired;

        private bool _turnRoundErrorLight = false;

        private int _turnRoundPrepareD = 0;
        /// <summary>
        /// 掉头准备距离
        /// </summary>
        public int TurnRoundPrepareD
        {
            get { return _turnRoundPrepareD; }
            set { Set(() => TurnRoundPrepareD, ref _turnRoundPrepareD, value); }
        }

        /// <summary>
        /// 打错转向灯
        /// </summary>
        public bool TurnRoundErrorLight
        {
            get { return _turnRoundErrorLight; }
            set { Set(() => TurnRoundErrorLight, ref _turnRoundErrorLight, value); }
        }

        /// <summary>
        /// 掉头项目语音 
        /// </summary>
        public bool TurnRoundVoice
        {
            get { return _turnRoundVoice; }
            set { Set(() => TurnRoundVoice, ref _turnRoundVoice, value); }
        }
        /// <summary>
        /// 掉头白考喇叭检测 
        /// </summary>
        public bool TurnRoundLoudSpeakerDayCheck
        {
            get { return _turnRoundLoudSpeakerDayCheck; }
            set { Set(() => TurnRoundLoudSpeakerDayCheck, ref _turnRoundLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 掉头夜考喇叭检测 
        /// </summary>
        public bool TurnRoundLoudSpeakerNightCheck
        {
            get { return _turnRoundLoudSpeakerNightCheck; }
            set { Set(() => TurnRoundLoudSpeakerNightCheck, ref _turnRoundLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 掉头转向角度差确认开始掉头（单位：度）
        /// </summary>
        public int TurnRoundStartAngleDiff
        {
            get { return _turnRoundStartAngleDiff; }
            set { Set(() => TurnRoundStartAngleDiff, ref _turnRoundStartAngleDiff, value); }
        }
        /// <summary>
        /// 掉头结束掉头转向角度差（单位：度）
        /// </summary>
        public int TurnRoundEndAngleDiff
        {
            get { return _turnRoundEndAngleDiff; }
            set { Set(() => TurnRoundEndAngleDiff, ref _turnRoundEndAngleDiff, value); }
        }
        /// <summary>
        /// 掉头所用最大距离限制（单位：米）
        /// </summary>
        public int TurnRoundMaxDistance
        {
            get { return _turnRoundDistance; }
            set { Set(() => TurnRoundMaxDistance, ref _turnRoundDistance, value); }
        }
        /// <summary>
        /// 掉头夜间远近光交替检查
        /// </summary>
        public bool TurnRoundLightCheck
        {
            get { return _turnRoundLightCheck; }
            set { Set(() => TurnRoundLightCheck, ref _turnRoundLightCheck, value); }
        }
        /// <summary>
        /// 掉头必踩刹车
        /// </summary>
        public bool TurnRoundBrakeRequired
        {
            get { return _turnRoundBrakeRequired; }
            set { Set(() => _turnRoundBrakeRequired, ref _turnRoundBrakeRequired, value); }
        }

        #endregion

        #region 学校区域

        private bool _schoolAreaVoice = DefaultGlobalSettings.DefaultSchoolAreaVoice;
        private bool _schoolAreaBrakeRequire = DefaultGlobalSettings.DefaultSchoolAreaBrakeRequire;
        private int _schoolAreaDistance = DefaultGlobalSettings.DefaultSchoolAreaDistance;
        private int _schoolAreaSpeedLimit = DefaultGlobalSettings.DefaultSchoolAreaSpeedLimit;
        private int _schoolAreaBrakeSpeedUp = DefaultGlobalSettings.DefaultSchoolAreaBrakeSpeedUp;
        private bool _schoolAreaLightCheck = DefaultGlobalSettings.DefaultSchoolAreaLightCheck;
        private bool _schoolAreaLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultSchoolAreaLoudSpeakerDayCheck;
        private bool _schoolAreaLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultSchoolAreaLoudSpeakerNightCheck;
        private bool _schoolAreaForbidLoudSpeakerCheck = DefaultGlobalSettings.DefaultSchoolAreaForbidLoudSpeakerCheck;

        private double _schoolAreaPrepareD = 0;
        /// <summary>
        /// 学校区域准备距离
        /// </summary>
        public double SchoolAreaPrepareD
        {
            get { return _schoolAreaPrepareD; }
            set { Set(() => SchoolAreaPrepareD, ref _schoolAreaPrepareD, value); }
        }

        /// <summary>
        /// 学校区域项目语音
        /// </summary>
        public bool SchoolAreaVoice
        {
            get { return _schoolAreaVoice; }
            set { Set(() => SchoolAreaVoice, ref _schoolAreaVoice, value); }
        }
        /// <summary>
        /// 学校区域白考喇叭检测
        /// </summary>
        public bool SchoolAreaLoudSpeakerDayCheck
        {
            get { return _schoolAreaLoudSpeakerDayCheck; }
            set { Set(() => SchoolAreaLoudSpeakerDayCheck, ref _schoolAreaLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 学校区域夜考喇叭检测
        /// </summary>
        public bool SchoolAreaLoudSpeakerNightCheck
        {
            get { return _schoolAreaLoudSpeakerNightCheck; }
            set { Set(() => SchoolAreaLoudSpeakerNightCheck, ref _schoolAreaLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 学校区域禁止鸣号
        /// </summary>
        public bool SchoolAreaForbidLoudSpeakerCheck
        {
            get { return _schoolAreaForbidLoudSpeakerCheck; }
            set { Set(() => SchoolAreaForbidLoudSpeakerCheck, ref _schoolAreaForbidLoudSpeakerCheck, value); }
        }
        /// <summary>
        /// 学校区域灯光检测
        /// </summary>
        public bool SchoolAreaLightCheck
        {
            get { return _schoolAreaLightCheck; }
            set { Set(() => SchoolAreaLightCheck, ref _schoolAreaLightCheck, value); }
        }
        /// <summary>
        /// 学校区域项目距离
        /// </summary>
        public int SchoolAreaDistance
        {
            get { return _schoolAreaDistance; }
            set { Set(() => SchoolAreaDistance, ref _schoolAreaDistance, value); }
        }
        /// <summary>
        /// 学校区域必须踩刹车
        /// </summary>
        public bool SchoolAreaBrakeRequire
        {
            get { return _schoolAreaBrakeRequire; }
            set { Set(() => SchoolAreaBrakeRequire, ref _schoolAreaBrakeRequire, value); }
        }
        /// <summary>
        /// 学校区域速度限制
        /// </summary>
        public int SchoolAreaSpeedLimit
        {
            get { return _schoolAreaSpeedLimit; }
            set { Set(() => SchoolAreaSpeedLimit, ref _schoolAreaSpeedLimit, value); }
        }
        /// <summary>
        /// 学校区域要求踩刹车速度限制
        /// </summary>
        public int SchoolAreaBrakeSpeedUp
        {
            get { return _schoolAreaBrakeSpeedUp; }
            set { Set(() => SchoolAreaBrakeSpeedUp, ref _schoolAreaBrakeSpeedUp, value); }
        }
        #endregion

        #region 公交车站
        private bool _busAreaVoice = DefaultGlobalSettings.DefaultBusAreaVoice;
        private bool _busAreaBrakeRequire = DefaultGlobalSettings.DefaultBusAreaBrakeRequire;
        private int _busAreaDistance = DefaultGlobalSettings.DefaultBusAreaDistance;
        private int _busAreaSpeedLimit = DefaultGlobalSettings.DefaultBusAreaSpeedLimit;
        private int _busAreaBrakeSpeedUp = DefaultGlobalSettings.DefaultBusAreaBrakeSpeedUp;
        private bool _busAreaLightCheck = DefaultGlobalSettings.DefaultBusAreaLightCheck;
        private bool _busAreaLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultBusAreaLoudSpeakerNightCheck;
        private bool _busAreaLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultBusAreaLoudSpeakerDayCheck;

        private double _busAreaPrepareD = 0;
        /// <summary>
        /// 公交车站准备距离
        /// </summary>
        public double BusAreaPrepareD
        {
            get { return _busAreaPrepareD; }
            set { Set(() => BusAreaPrepareD, ref _busAreaPrepareD, value); }
        }
        /// <summary>
        /// 公交车站夜考喇叭检测
        /// </summary>
        public bool BusAreaLoudSpeakerNightCheck
        {
            get { return _busAreaLoudSpeakerNightCheck; }
            set { Set(() => BusAreaLoudSpeakerNightCheck, ref _busAreaLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 公交车站白考喇叭检测
        /// </summary>
        public bool BusAreaLoudSpeakerDayCheck
        {
            get { return _busAreaLoudSpeakerDayCheck; }
            set { Set(() => BusAreaLoudSpeakerDayCheck, ref _busAreaLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 公交车站远近光交替
        /// </summary>
        public bool BusAreaLightCheck
        {
            get { return _busAreaLightCheck; }
            set { Set(() => BusAreaLightCheck, ref _busAreaLightCheck, value); }
        }
        /// <summary>
        /// 公交车站项目语音
        /// </summary>
        public bool BusAreaVoice
        {
            get { return _busAreaVoice; }
            set { Set(() => BusAreaVoice, ref _busAreaVoice, value); }
        }
        /// <summary>
        /// 公交车站项目距离
        /// </summary>
        public int BusAreaDistance
        {
            get { return _busAreaDistance; }
            set { Set(() => BusAreaDistance, ref _busAreaDistance, value); }
        }
        /// <summary>
        /// 公交车站必须踩刹车
        /// </summary>
        public bool BusAreaBrakeRequire
        {
            get { return _busAreaBrakeRequire; }
            set { Set(() => BusAreaBrakeRequire, ref _busAreaBrakeRequire, value); }
        }
        /// <summary>
        /// 公交车站速度限制
        /// </summary>
        public int BusAreaSpeedLimit
        {
            get { return _busAreaSpeedLimit; }
            set { Set(() => BusAreaSpeedLimit, ref _busAreaSpeedLimit, value); }
        }
        /// <summary>
        /// 公交车站要求踩刹车速度限制
        /// </summary>
        public int BusAreaBrakeSpeedUp
        {
            get { return _busAreaBrakeSpeedUp; }
            set { Set(() => BusAreaBrakeSpeedUp, ref _busAreaBrakeSpeedUp, value); }
        }
        #endregion

        #region 人行横道
        private bool _pedestrianCrossingVoice = DefaultGlobalSettings.DefaultPedestrianCrossingVoice;
        private bool _pedestrianCrossingBrakeRequire = DefaultGlobalSettings.DefaultPedestrianCrossingBrakeRequire;
        private int _pedestrianCrossingDistance = DefaultGlobalSettings.DefaultPedestrianCrossingDistance;
        private int _pedestrianCrossingSpeedLimit = DefaultGlobalSettings.DefaultPedestrianCrossingSpeedLimit;
        private int _pedestrianCrossingBrakeSpeedUp = DefaultGlobalSettings.DefaultPedestrianCrossingBrakeSpeedUp;
        private bool _pedestrianCrossingLightCheck = DefaultGlobalSettings.DefaultPedestrianCrossingLightCheck;
        private bool _pedestrianCrossingLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultPedestrianCrossingLoudSpeakerNightCheck;
        private bool _pedestrianCrossingLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultPedestrianCrossingLoudSpeakerDayCheck;
        private double _pedestrainCrossingPrepareD = 0;
        /// <summary>
        /// 人行横道准备距离
        /// </summary>
        /// 
        
        public double PedestrainCrossingPrepareD
        {
            get { return _pedestrainCrossingPrepareD; }
            set { Set(() => PedestrainCrossingPrepareD, ref _pedestrainCrossingPrepareD, value); }
        }
        /// <summary>
        /// 人行横道夜考喇叭检测
        /// </summary>
        public bool PedestrianCrossingLoudSpeakerNightCheck
        {
            get { return _pedestrianCrossingLoudSpeakerNightCheck; }
            set { Set(() => PedestrianCrossingLoudSpeakerNightCheck, ref _pedestrianCrossingLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 人行横道白考喇叭检测
        /// </summary>
        public bool PedestrianCrossingLoudSpeakerDayCheck
        {
            get { return _pedestrianCrossingLoudSpeakerDayCheck; }
            set { Set(() => PedestrianCrossingLoudSpeakerDayCheck, ref _pedestrianCrossingLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 人行横道远近光交替灯光检测
        /// </summary>
        public bool PedestrianCrossingLightCheck
        {
            get { return _pedestrianCrossingLightCheck; }
            set { Set(() => PedestrianCrossingLightCheck, ref _pedestrianCrossingLightCheck, value); }
        }
        /// <summary>
        /// 人行横道项目语音
        /// </summary>
        public bool PedestrianCrossingVoice
        {
            get { return _pedestrianCrossingVoice; }
            set { Set(() => PedestrianCrossingVoice, ref _pedestrianCrossingVoice, value); }
        }
        /// <summary>
        /// 人行横道项目距离
        /// </summary>
        public int PedestrianCrossingDistance
        {
            get { return _pedestrianCrossingDistance; }
            set { Set(() => PedestrianCrossingDistance, ref _pedestrianCrossingDistance, value); }
        }
        /// <summary>
        /// 人行横道必须踩刹车
        /// </summary>
        public bool PedestrianCrossingBrakeRequire
        {
            get { return _pedestrianCrossingBrakeRequire; }
            set { Set(() => PedestrianCrossingBrakeRequire, ref _pedestrianCrossingBrakeRequire, value); }
        }
        /// <summary>
        /// 人行横道速度限制
        /// </summary>
        public int PedestrianCrossingSpeedLimit
        {
            get { return _pedestrianCrossingSpeedLimit; }
            set { Set(() => PedestrianCrossingSpeedLimit, ref _pedestrianCrossingSpeedLimit, value); }
        }
        /// <summary>
        /// 人行横道要求踩刹车速度限制
        /// </summary>
        public int PedestrianCrossingBrakeSpeedUp
        {
            get { return _pedestrianCrossingBrakeSpeedUp; }
            set { Set(() => PedestrianCrossingBrakeSpeedUp, ref _pedestrianCrossingBrakeSpeedUp, value); }
        }
        #endregion

        #region 环岛

        private bool _roundaboutVoice = DefaultGlobalSettings.DefaultRoundaboutVoice;
        private int _roundaboutDistance = DefaultGlobalSettings.DefaultRoundaboutDistance;
        private bool _roundaboutLightCheck = DefaultGlobalSettings.DefaultRoundaboutLightCheck;

        /// <summary>
        /// 环岛项目语音
        /// </summary>
        public bool RoundaboutVoice
        {
            get { return _roundaboutVoice; }
            set { Set(() => RoundaboutVoice, ref _roundaboutVoice, value); }
        }

        /// <summary>
        /// 环岛项目距离
        /// </summary>
        public int RoundaboutDistance
        {
            get { return _roundaboutDistance; }
            set { Set(() => RoundaboutDistance, ref _roundaboutDistance, value); }
        }

        /// <summary>
        /// 环岛默认环岛灯光检测
        /// </summary>
        public bool RoundaboutLightCheck
        {
            get { return _roundaboutLightCheck; }
            set { Set(() => RoundaboutLightCheck, ref _roundaboutLightCheck, value); }
        }
        #endregion

        #region 变更车道

        private int _changeLanesMaxDistance = DefaultGlobalSettings.DefaultChangeLanesMaxDistance;
        private int _changeLanesTimeout = DefaultGlobalSettings.DefaultChangeLanesTimeout;
        private double _changeLanesAngle = DefaultGlobalSettings.DefaultChangeLanesAngle;
        private bool _changeLanesLightCheck = DefaultGlobalSettings.DefaultChangeLanesLightCheck;
        private bool _changeLanesVoice = DefaultGlobalSettings.DefaultChangeLanesVoice;
        private bool _changeLanesLowAndHighBeamCheck = DefaultGlobalSettings.DefaultChangeLanesLowAndHighBeamCheck;

        private int _changeLanesPrepareDistance = DefaultGlobalSettings.DefaultChangeLanesPrepareDistance;
        private string _changeLanesSecondVoice = string.Empty;
        private bool _chkChangeLanesEndFlag = false;
        private int _changelineDirect = 0;
        /// <summary>
        /// 允许变道方向（0:均可，1:只能左，2:只能右）
        /// </summary>
        public int ChangelineDirect
        {
            get { return _changelineDirect; }
            set { Set(() => ChangelineDirect, ref _changelineDirect, value); }
        }
        /// <summary>
        /// 变道完成就结束
        /// </summary>
        public bool ChkChangeLanesEndFlag
        {
            get { return _chkChangeLanesEndFlag; }
            set { Set(() => ChkChangeLanesEndFlag, ref _chkChangeLanesEndFlag, value); }
        }

        /// <summary>
        /// 第2个播报语音
        /// </summary>
        public string ChangeLanesSecondVoice
        {
            get { return _changeLanesSecondVoice; }
            set { Set(() => ChangeLanesSecondVoice, ref _changeLanesSecondVoice, value); }
        }
        /// <summary>
        /// 变道准备距离
        /// </summary>
        public int ChangeLanesPrepareDistance
        {
            get { return _changeLanesPrepareDistance; }
            set { Set(() => ChangeLanesPrepareDistance, ref _changeLanesPrepareDistance, value); }
        }
        /// <summary>
        /// 变更车道夜间远近光交替
        /// </summary>
        public bool ChangeLanesLowAndHighBeamCheck
        {
            get { return _changeLanesLowAndHighBeamCheck; }
            set { Set(() => ChangeLanesLowAndHighBeamCheck, ref _changeLanesLowAndHighBeamCheck, value); }
        }
        /// <summary>
        /// 变更车道项目语音
        /// </summary>
        public bool ChangeLanesVoice
        {
            get { return _changeLanesVoice; }
            set { Set(() => ChangeLanesVoice, ref _changeLanesVoice, value); }
        }
        /// <summary>
        /// 变更车道灯光检测
        /// </summary>
        public bool ChangeLanesLightCheck
        {
            get { return _changeLanesLightCheck; }
            set { Set(() => ChangeLanesLightCheck, ref _changeLanesLightCheck, value); }
        }
        /// <summary>
        /// 变道最大距离（单位：米）
        /// </summary>
        public int ChangeLanesMaxDistance
        {
            get { return _changeLanesMaxDistance; }
            set { Set(() => ChangeLanesMaxDistance, ref _changeLanesMaxDistance, value); }
        }
        /// <summary>
        /// 变道超时时间（单位：秒）
        /// </summary>
        public int ChangeLanesTimeout
        {
            get { return _changeLanesTimeout; }
            set { Set(() => ChangeLanesTimeout, ref _changeLanesTimeout, value); }
        }
        /// <summary>
        /// 变道转向角度
        /// </summary>
        public double ChangeLanesAngle
        {
            get { return _changeLanesAngle; }
            set { Set(() => ChangeLanesAngle, ref _changeLanesAngle, value); }
        }
        #endregion

        #region 会车

        private double _meetingDrivingDistance = DefaultGlobalSettings.DefaultMeetingDrivingDistance;
        private int _meetingSlowSpeedInKmh = DefaultGlobalSettings.DefaultMeetingSlowSpeedInKmh;
        private bool _meetingVoice = DefaultGlobalSettings.DefaultMeetingVoice;
        private bool _meetingCheckBrake = DefaultGlobalSettings.DefaultMeetingCheckBrake;
        private bool _meetingForbidHighBeamCheck = DefaultGlobalSettings.DefaultMeetingForbidHighBeamCheck;
        private double _meetingAngle = DefaultGlobalSettings.DefaultMeetingAngle;
        private bool _reverseMeetingCheck = false;


        private double _meetingPrepareD = 0;
        /// <summary>
        /// 会车准备距离
        /// </summary>
        public double MeetingPrepareD
        {
            get { return _meetingPrepareD; }
            set { Set(() => MeetingPrepareD, ref _meetingPrepareD, value); }
        }
        /// <summary>
        /// 会车向右避让角度
        /// </summary>
        public double MeetingAngle
        {
            get { return _meetingAngle; }
            set { Set(() => MeetingAngle, ref _meetingAngle, value); }
        }
        /// <summary>
        /// 会车禁止远光
        /// </summary>
        public bool MeetingForbidHighBeamCheck
        {
            get { return _meetingForbidHighBeamCheck; }
            set { Set(() => MeetingForbidHighBeamCheck, ref _meetingForbidHighBeamCheck, value); }
        }
        /// <summary>
        /// 反向会车检测
        /// </summary>
        public bool ReverseMeetingCheck
        {
            get { return _reverseMeetingCheck; }
            set { Set(() => ReverseMeetingCheck, ref _reverseMeetingCheck, value); }
        }
        /// <summary>
        /// 会车会车距离
        /// </summary>
        public double MeetingDrivingDistance
        {
            get { return _meetingDrivingDistance; }
            set { Set(() => MeetingDrivingDistance, ref _meetingDrivingDistance, value); }
        }
        /// <summary>
        /// 会车速度限制
        /// </summary>
        public int MeetingSlowSpeedInKmh
        {
            get { return _meetingSlowSpeedInKmh; }
            set { Set(() => MeetingSlowSpeedInKmh, ref _meetingSlowSpeedInKmh, value); }
        }
        /// <summary>
        /// 会车语音
        /// </summary>
        public bool MeetingVoice
        {
            get { return _meetingVoice; }
            set { Set(() => MeetingVoice, ref _meetingVoice, value); }
        }
        /// <summary>
        /// 会车刹车
        /// </summary>
        public bool MeetingCheckBrake
        {
            get { return _meetingCheckBrake; }
            set { Set(() => MeetingCheckBrake, ref _meetingCheckBrake, value); }
        }
        #endregion

        #region 超车

        private bool _overtakeVoice = DefaultGlobalSettings.DefaultOvertakeVoice;
        private bool _overtakeLightCheck = DefaultGlobalSettings.DefaultOvertakeLightCheck;
        private int _overtakeMaxDistance = DefaultGlobalSettings.DefaultOvertakeMaxDistance;
        private int _overtakeTimeout = DefaultGlobalSettings.DefaultOvertakeTimeout;
        private double _overtakeChangeLanesAngle = DefaultGlobalSettings.DefaultOvertakeChangeLanesAngle;
        private int _overtakeSpeedLimit = DefaultGlobalSettings.DefaultOvertakeSpeedLimit;
        private bool _overtakeLoudSpeakerNightCheck = DefaultGlobalSettings.DefaultOvertakeLoudSpeakerNightCheck;
        private bool _overtakeLoudSpeakerDayCheck = DefaultGlobalSettings.DefaultOvertakeLoudSpeakerDayCheck;
        private bool _overtakeLowAndHighBeamCheck = DefaultGlobalSettings.DefaultOvertakeLowAndHighBeamCheck;

        private bool _reverseOvertakeCheck = true;

        private int _overtakePrepareDistance = DefaultGlobalSettings.DefaultOvertakePrepareDistance;
        private int _overtakeLowestSpeed = DefaultGlobalSettings.DefaultOvertakeLowestSpeed;
        private int _overtakeSpeedRange = 3;
        private int _overtakeSpeedOnce = 0;

        private bool _overtakeBackToOriginalLane = DefaultGlobalSettings.DefaultOvertakeBackToOriginalLane;

        private string _overtakeBackToOriginalLaneVocie = "请返回原车道";

        private int _overtakeChangingLanesSuccessOrBackToOriginalLanceDistance = 30;


        public string OvertakeBackToOriginalLaneVocie
        {
            get { return _overtakeBackToOriginalLaneVocie; }
            set { Set(() => OvertakeBackToOriginalLaneVocie, ref _overtakeBackToOriginalLaneVocie, value); }
        }
        public int OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance
        {
            get { return _overtakeChangingLanesSuccessOrBackToOriginalLanceDistance; }
            set { Set(() => OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance, ref _overtakeChangingLanesSuccessOrBackToOriginalLanceDistance, value); }
        }

        /// <summary>
        /// 超车最低速度
        /// </summary>
        public int OvertakeLowestSpeed
        {
            get { return _overtakeLowestSpeed; }
            set { Set(() => OvertakeLowestSpeed, ref _overtakeLowestSpeed, value); }
        }

        /// <summary>
        /// 超车准备距离,（米）
        /// </summary>
        public int OvertakePrepareDistance
        {
            get { return _overtakePrepareDistance; }
            set { Set(() => OvertakePrepareDistance, ref _overtakePrepareDistance, value); }
        }
        /// <summary>
        /// 超车夜间远近光交替
        /// </summary>
        public bool OvertakeLowAndHighBeamCheck
        {
            get { return _overtakeLowAndHighBeamCheck; }
            set { Set(() => OvertakeLowAndHighBeamCheck, ref _overtakeLowAndHighBeamCheck, value); }
        }
        /// <summary>
        /// 超车语音
        /// </summary>
        public bool OvertakeVoice
        {
            get { return _overtakeVoice; }
            set { Set(() => OvertakeVoice, ref _overtakeVoice, value); }
        }
        /// <summary>
        /// 超车夜考喇叭检测
        /// </summary>
        public bool OvertakeLoudSpeakerNightCheck
        {
            get { return _overtakeLoudSpeakerNightCheck; }
            set { Set(() => OvertakeLoudSpeakerNightCheck, ref _overtakeLoudSpeakerNightCheck, value); }
        }
        /// <summary>
        /// 超车白考喇叭检测
        /// </summary>
        public bool OvertakeLoudSpeakerDayCheck
        {
            get { return _overtakeLoudSpeakerDayCheck; }
            set { Set(() => OvertakeLoudSpeakerDayCheck, ref _overtakeLoudSpeakerDayCheck, value); }
        }
        /// <summary>
        /// 超车转向灯灯光检测
        /// </summary>
        public bool OvertakeLightCheck
        {
            get { return _overtakeLightCheck; }
            set { Set(() => OvertakeLightCheck, ref _overtakeLightCheck, value); }
        }
        /// <summary>
        /// 超车最大距离（单位：米）
        /// </summary>
        public int OvertakeMaxDistance
        {
            get { return _overtakeMaxDistance; }
            set { Set(() => OvertakeMaxDistance, ref _overtakeMaxDistance, value); }
        }
        /// <summary>
        /// 超车超时时间（单位：秒）
        /// </summary>
        public int OvertakeTimeout
        {
            get { return _overtakeTimeout; }
            set { Set(() => OvertakeTimeout, ref _overtakeTimeout, value); }
        }
        /// <summary>
        /// 反向超车检测
        /// </summary>
        public bool ReverseOvertakeCheck
        {
            get { return _reverseOvertakeCheck; }
            set { Set(() => ReverseOvertakeCheck, ref _reverseOvertakeCheck, value); }
        }
        /// <summary>
        /// 超车变道角度
        /// </summary>
        public double OvertakeChangeLanesAngle
        {
            get { return _overtakeChangeLanesAngle; }
            set { Set(() => OvertakeChangeLanesAngle, ref _overtakeChangeLanesAngle, value); }
        }
        /// <summary>
        /// 超车速度限制
        /// </summary>
        public int OvertakeSpeedLimit
        {
            get { return _overtakeSpeedLimit; }
            set { Set(() => OvertakeSpeedLimit, ref _overtakeSpeedLimit, value); }
        }

        /// <summary>
        /// 超车速度范围
        /// </summary>
        public int OvertakeSpeedRange
        {
            get { return _overtakeSpeedRange; }
            set { Set(() => OvertakeSpeedRange, ref _overtakeSpeedRange, value); }
        }
        /// <summary>
        /// 超车达到一次速度
        /// </summary>
        public int OvertakeSpeedOnce
        {
            get { return _overtakeSpeedOnce; }
            set { Set(() => OvertakeSpeedOnce, ref _overtakeSpeedOnce, value); }
        }
        /// <summary>
        /// 超车是否返回原车道
        /// </summary>
        public bool OvertakeBackToOriginalLane
        {
            get { return _overtakeBackToOriginalLane; }
            set { Set(() => OvertakeBackToOriginalLane, ref _overtakeBackToOriginalLane, value); }
        }


        #endregion

        #region 急坡弯路

        private bool _sharpTurnLoudspeakerInDay = DefaultGlobalSettings.DefaultSharpTurnLoudspeakerInDay;
        private bool _sharpTurnLoudspeakerInNight = DefaultGlobalSettings.DefaultSharpTurnLoudspeakerInNight;
        private bool _sharpTurnVoice = DefaultGlobalSettings.DefaultSharpTurnVoice;
        private bool _sharpTurnLightCheck = DefaultGlobalSettings.DefaultSharpTurnLightCheck;
        private int _sharpTurnDistance = DefaultGlobalSettings.DefaultSharpTurnDistance;

        private bool _sharpTurnBrake = DefaultGlobalSettings.DefaultSharpTurnBrake;
        private int _sharpTurnSpeedLimit = DefaultGlobalSettings.DefaultSharpTurnSpeedLimit;
        private int _sharpTurnBrakeSpeedUp = DefaultGlobalSettings.DefaultSharpTurnBrakeSpeedUp;
        //急弯坡路 左转检查
        private bool _sharpTurnLeftLightCheck = false;
        //急弯坡路 右转检测
        private bool _sharpTurnRightLightCheck = false;

        /// <summary>
        /// 左转
        /// </summary>
        public bool SharpTurnLeftLightCheck
        {
            get { return _sharpTurnLeftLightCheck; }
            set { Set(() => SharpTurnLeftLightCheck, ref _sharpTurnLeftLightCheck, value); }
        }
        /// <summary>
        /// 右转
        /// </summary>
        public bool SharpTurnRightLightCheck
        {
            get { return _sharpTurnRightLightCheck; }
            set { Set(() => SharpTurnRightLightCheck, ref _sharpTurnRightLightCheck, value); }
        }
        /// <summary>
        /// 该速度以上必须踩刹车
        /// </summary>
        public int SharpTurnBrakeSpeedUp
        {
            get { return _sharpTurnBrakeSpeedUp; }
            set { Set(() => SharpTurnBrakeSpeedUp, ref _sharpTurnBrakeSpeedUp, value); }
        }
        /// <summary>
        /// 速度限制
        /// </summary>
        public int SharpTurnSpeedLimit
        {
            get { return _sharpTurnSpeedLimit; }
            set { Set(() => SharpTurnSpeedLimit, ref _sharpTurnSpeedLimit, value); }
        }
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public bool SharpTurnBrake
        {
            get { return _sharpTurnBrake; }
            set { Set(() => SharpTurnBrake, ref _sharpTurnBrake, value); }
        }

        /// <summary>
        /// 急坡弯路项目语音
        /// </summary>
        public bool SharpTurnVoice
        {
            get { return _sharpTurnVoice; }
            set { Set(() => SharpTurnVoice, ref _sharpTurnVoice, value); }
        }
        /// <summary>
        /// 急坡弯路夜间远近光交替检测
        /// </summary>
        public bool SharpTurnLightCheck
        {
            get { return _sharpTurnLightCheck; }
            set { Set(() => SharpTurnLightCheck, ref _sharpTurnLightCheck, value); }
        }
        /// <summary>
        /// 急坡弯路项目距离
        /// </summary>
        public int SharpTurnDistance
        {
            get { return _sharpTurnDistance; }
            set { Set(() => SharpTurnDistance, ref _sharpTurnDistance, value); }
        }
        /// <summary>
        /// 急坡弯路白考喇叭
        /// </summary>
        public bool SharpTurnLoudspeakerInDay
        {
            get { return _sharpTurnLoudspeakerInDay; }
            set { Set(() => SharpTurnLoudspeakerInDay, ref _sharpTurnLoudspeakerInDay, value); }
        }
        /// <summary>
        /// 急坡弯路夜考喇叭
        /// </summary>
        public bool SharpTurnLoudspeakerInNight
        {
            get { return _sharpTurnLoudspeakerInNight; }
            set { Set(() => SharpTurnLoudspeakerInNight, ref _sharpTurnLoudspeakerInNight, value); }
        }
        #endregion

        #region 直线行驶
        private double _straightDrivingDistance = DefaultGlobalSettings.DefaultStraightDrivingDistance;
        private double _straightDrivingMaxOffsetAngle = DefaultGlobalSettings.DefaultStraightDrivingMaxOffsetAngle;
        private bool _straightDrivingVoice = DefaultGlobalSettings.DefaultStraightDrivingVoice;
        private int _straightDrivingSpeedMinLimit = DefaultGlobalSettings.DefaultStraightDrivingSpeedMinLimit;
        private int _straightDrivingSpeedMaxLimit = DefaultGlobalSettings.DefaultStraightDrivingSpeedMaxLimit;
        private int _straightDrivingPrepareDistance = DefaultGlobalSettings.DefaultStraightDrivingPrepareDistance;
        private int _straightDrivingReachSpeed = 0;
        /// <summary>
        /// 直线行驶距离
        /// </summary>
        public double StraightDrivingDistance
        {
            get { return _straightDrivingDistance; }
            set { Set(() => StraightDrivingDistance, ref _straightDrivingDistance, value); }
        }
        /// <summary>
        /// 直线行驶项目语音
        /// </summary>
        public bool StraightDrivingVoice
        {
            get { return _straightDrivingVoice; }
            set { Set(() => StraightDrivingVoice, ref _straightDrivingVoice, value); }
        }
        /// <summary>
        /// 直线行驶最大角度偏移
        /// </summary>
        public double StraightDrivingMaxOffsetAngle
        {
            get { return _straightDrivingMaxOffsetAngle; }
            set { Set(() => StraightDrivingMaxOffsetAngle, ref _straightDrivingMaxOffsetAngle, value); }
        }
        /// <summary>
        /// 直线行驶最低速度要求
        /// </summary>
        public int StraightDrivingSpeedMinLimit
        {
            get { return _straightDrivingSpeedMinLimit; }
            set { Set(() => StraightDrivingSpeedMinLimit, ref _straightDrivingSpeedMinLimit, value); }
        }
        /// <summary>
        /// 直线行驶最大速度限制
        /// </summary>
        public int StraightDrivingSpeedMaxLimit
        {
            get { return _straightDrivingSpeedMaxLimit; }
            set { Set(() => StraightDrivingSpeedMaxLimit, ref _straightDrivingSpeedMaxLimit, value); }
        }
        /// <summary>
        /// 直线行驶准备距离
        /// </summary>
        public int StraightDrivingPrepareDistance
        {
            get { return _straightDrivingPrepareDistance; }
            set { Set(() => StraightDrivingPrepareDistance, ref _straightDrivingPrepareDistance, value); }
        }
        /// <summary>
        /// 直线行驶达到一次速度
        /// </summary>
        public int StraightDrivingReachSpeed
        {
            get { return _straightDrivingReachSpeed; }
            set { Set(() => StraightDrivingReachSpeed, ref _straightDrivingReachSpeed, value); }
        }
        #endregion

        #region 靠边停车
        private int _pullOverHandbrakeTimeout = DefaultGlobalSettings.DefaultPullOverHandbrakeTimeout;
        private int _pullOverDoorTimeout = 10;
        private int _pullOverMaxDrivingDistance = DefaultGlobalSettings.DefaultPullOverMaxDrivingDistance;
        private bool _pullOverAutoTrigger = DefaultGlobalSettings.DefaultPullOverAutoTrigger;
        private bool _isCheckPullOverSafetyBeltBeforeGetOff = DefaultGlobalSettings.DefaultIsCheckPullOverSafetyBeltBeforeGetOff;
        private bool _pullOverCloseLowBeamBeforeGetOff = DefaultGlobalSettings.DefaultPullOverStopLowBeamBeforeGetOff;
        private bool _pullOverTurnLightCheck = DefaultGlobalSettings.DefaultPullOverTurnLightCheck;
        private bool _pullOverVoice = DefaultGlobalSettings.DefaultPullOverVoice;
        private bool _pulloverEndAutoTriggerStopExam = DefaultGlobalSettings.DefaultPulloverEndAutoTriggerStopExam;
        private bool _pulloverEndAutoTriggerStart= DefaultGlobalSettings.DefaultPulloverEndAutoTriggerStart;
        private bool _pullOverOpenCautionBeforeGetOff = DefaultGlobalSettings.DefaultPullOverOpenCautionBeforeGetOff;
        private bool _pullOverStopEngineBeforeGetOff = DefaultGlobalSettings.DefaultPullOverStopEngineBeforeGetOff;
        private bool _pullOverCloseDoorBeforeGetOff = DefaultGlobalSettings.DefaultPullOverOpenCloseDoorCheck;

        private bool _pullOverOpenSafetyBeltBeforeGetOff = DefaultGlobalSettings.DefaultPullOverOpenSafetyBeltBeforeGetOff;
        private PullOverEndMark _pullOverEndMark = DefaultGlobalSettings.DefaultPullOverEndMark;
        private bool _pullOverNeutralCheck = DefaultGlobalSettings.DefaultPullOverNeutralCheck;

        private bool _pullOverMarginsEnable = DefaultGlobalSettings.DefaultPullOverMarginsEnable;
        private double _pullOverMarginsValue = DefaultGlobalSettings.DefaultPullOverMarginsValue;

        private bool _IsPlaybackToOriginalLane = DefaultGlobalSettings.DefaultIsPlaybackToOriginalLane;
        private int _ReturnToOriginalLaneDistince = DefaultGlobalSettings.DefaultReturnToOriginalLaneDistince;

        private PullOverMark _pullOverMark = DefaultGlobalSettings.DefaultPullOverMark;

        private double _pulloverAngle = 0;

        private double _pullOverOpenDoorTime = 0;
        /// <summary>
        /// 靠边停车转向角度
        /// </summary>
        public double PulloverAngle
        {
            get { return _pulloverAngle; }
            set { Set(() => PulloverAngle, ref _pulloverAngle, value); }
        }


        public double PullOverOpenDoorTime
        {
            get { return _pullOverOpenDoorTime; }
            set { Set(() => _pullOverOpenDoorTime, ref _pullOverOpenDoorTime, value); }
        }
        
        //添加一个停车状态

        /// <summary>
        /// 停车空挡检测
        /// </summary>
        public bool PullOverNeutralCheck
        {
            get { return _pullOverNeutralCheck; }
            set { Set(() => PullOverNeutralCheck, ref _pullOverNeutralCheck, value); }
        }
        /// <summary>
        /// 靠边停车项目语音
        /// </summary>
        public bool PullOverVoice
        {
            get { return _pullOverVoice; }
            set { Set(() => PullOverVoice, ref _pullOverVoice, value); }
        }
        /// <summary>
        /// 靠边停车-最大行驶距离（米）
        /// </summary>
        public int PullOverMaxDrivingDistance
        {
            get { return _pullOverMaxDrivingDistance; }
            set { Set(() => PullOverMaxDrivingDistance, ref _pullOverMaxDrivingDistance, value); }
        }

        /// <summary>
        /// 靠边停车-停车后未拉手刹超时（单位：秒）
        /// </summary>
        /// 

       
        public int PullOverDoorTimeout
        {
            get { return _pullOverDoorTimeout; }
            set { Set(() => PullOverDoorTimeout, ref _pullOverDoorTimeout, value); }
        }

        public int PullOverHandbrakeTimeout
        {
            get { return _pullOverHandbrakeTimeout; }
            set { Set(() => PullOverHandbrakeTimeout, ref _pullOverHandbrakeTimeout, value); }
        }
        /// <summary>
        /// 靠边停车-检测转向灯
        /// </summary>
        public bool PullOverTurnLightCheck
        {
            get { return _pullOverTurnLightCheck; }
            set { Set(() => PullOverTurnLightCheck, ref _pullOverTurnLightCheck, value); }
        }
        /// <summary>
        /// 靠边停车-下车前开启警报灯
        /// </summary>
        public bool PullOverOpenCautionBeforeGetOff
        {
            get { return _pullOverOpenCautionBeforeGetOff; }
            set { Set(() => PullOverTurnLightCheck, ref _pullOverOpenCautionBeforeGetOff, value); }
        }
        /// <summary>
        /// 靠边停车-停车后发动机熄火检测
        /// </summary>
        public bool PullOverStopEngineBeforeGetOff
        {
            get { return _pullOverStopEngineBeforeGetOff; }
            set { Set(() => PullOverStopEngineBeforeGetOff, ref _pullOverStopEngineBeforeGetOff, value); }
        }
        /// <summary>
        /// 靠边停车-停车后松开安全带
        /// </summary>
        public bool PullOverOpenSafetyBeltBeforeGetOff
        {
            get { return _pullOverOpenSafetyBeltBeforeGetOff; }
            set { Set(() => PullOverOpenSafetyBeltBeforeGetOff, ref _pullOverOpenSafetyBeltBeforeGetOff, value); }
        }
        /// <summary>
        /// 靠边停车-下车前是否关闭大灯
        /// </summary>
        public bool PullOverCloseLowBeamBeforeGetOff
        {
            get { return _pullOverCloseLowBeamBeforeGetOff; }
            set { Set(() => PullOverCloseLowBeamBeforeGetOff, ref _pullOverCloseLowBeamBeforeGetOff, value); }
        }
        /// <summary>
        /// 靠边停车-下车前关闭车门
        /// </summary>
        public bool PullOverCloseDoorBeforeGetOff
        {
            get { return _pullOverCloseDoorBeforeGetOff; }
            set { Set(() => PullOverCloseDoorBeforeGetOff, ref _pullOverCloseDoorBeforeGetOff, value); }
        }
        /// <summary>
        /// 靠边停车停车结束标记
        /// </summary>
        public PullOverEndMark PullOverEndMark
        {
            get { return _pullOverEndMark; }
            set { Set(() => PullOverEndMark, ref _pullOverEndMark, value); }
        }
        /// <summary>
        /// 靠边停车-结束后自动触发结束考试
        /// </summary>
        public bool PulloverEndAutoTriggerStopExam
        {
            get { return _pulloverEndAutoTriggerStopExam; }
            set { Set(() => PulloverEndAutoTriggerStopExam, ref _pulloverEndAutoTriggerStopExam, value); }
        }
        /// <summary>
        /// 靠边停车-结束后自动触发起步
        /// </summary>
        public bool PulloverEndAutoTriggerStart
        {
            get { return _pulloverEndAutoTriggerStart; }
            set { Set(() => PulloverEndAutoTriggerStart, ref _pulloverEndAutoTriggerStart, value); }
        }

        /// <summary>
        /// 靠边停车-达到里程后自动触发靠边停车
        /// </summary>
        public bool PullOverAutoTrigger
        {
            get { return _pullOverAutoTrigger; }
            set { Set(() => PullOverAutoTrigger, ref _pullOverAutoTrigger, value); }
        }


        /// <summary>
        /// 靠边停车-启动靠边停车30公分
        /// </summary>
        public bool PullOverMarginsEnable
        {
            get { return _pullOverMarginsEnable; }
            set { Set(() => PullOverMarginsEnable, ref _pullOverMarginsEnable, value); }
        }


        /// <summary>
        /// 靠边停车-启动靠边停车30公分边距值
        /// </summary>
        public double PullOverMarginsValue
        {
            get { return _pullOverMarginsValue; }
            set { Set(() => PullOverMarginsValue, ref _pullOverMarginsValue, value); }
        }

        #endregion

        #region 加减档位
        private int _modifiedGearDrivingDistance = DefaultGlobalSettings.DefaultModifiedGearDrivingDistance;
        private double _modifiedGearTimeout = DefaultGlobalSettings.DefaultModifiedGearTimeout;
        private bool _modifiedGearVoice = DefaultGlobalSettings.DefaultModifiedGearVoice;
        private int _modifiedGearGearMax = DefaultGlobalSettings.DefaultModifiedGearGearMax;
        private int _modifiedGearGearMin = DefaultGlobalSettings.DefaultModifiedGearGearMin;
        private double _modifiedGearIgnoreSeconds = DefaultGlobalSettings.DefaultModifiedGearIgnoreSeconds;
        private bool _modifiedGearAddGearCheck = DefaultGlobalSettings.DefaultModifiedGearAddGearCheck;
        private bool _modifiedGearReduceGearCheck = DefaultGlobalSettings.DefaultModifiedGearReduceGearCheck;

        private bool _modifiedGearIsPlayGearVoice = DefaultGlobalSettings.DefaultModifiedGearIsPlayGearVoice;
        private bool _modifiedGearIsPlayActionVoice = DefaultGlobalSettings.DefaultModifiedGearIsPlayActionVoice;
        private bool _modifiedGearIsCustomProcess = DefaultGlobalSettings.DefaultModifiedGearIsCustomProcess;
        private string _modifiedGearGearFlow = DefaultGlobalSettings.DefaultModifiedGearGearFlow;

        //加减档流程语音比如-分隔,1-2,2-3,3-4,4-5,5-4,4-3,3-2,2-1
        private string _modifiedGearFlowVoice = DefaultGlobalSettings.DefaultModifiedGearFlowVoice;

        private bool _chkModifiedGearEndFlag = DefaultGlobalSettings.DefaultModifiedGearEndFlag;
        public string ModifiedGearFlowVoice
        {
            get { return _modifiedGearFlowVoice; }
            set { Set(() => ModifiedGearFlowVoice, ref _modifiedGearFlowVoice, value); }
        }

        /// <summary>
        ///加减档位- 加档检测
        /// </summary>
        public bool ModifiedGearAddGearCheck
        {
            get { return _modifiedGearAddGearCheck; }
            set { Set(() => ModifiedGearAddGearCheck, ref _modifiedGearAddGearCheck, value); }
        }
        /// <summary>
        /// 加减档位-减档检测
        /// </summary>
        public bool ModifiedGearReduceGearCheck
        {
            get { return _modifiedGearReduceGearCheck; }
            set { Set(() => ModifiedGearReduceGearCheck, ref _modifiedGearReduceGearCheck, value); }
        }
        /// <summary>
        /// 加减档位-行驶距离（单位：米）
        /// </summary>
        public int ModifiedGearDrivingDistance
        {
            get { return _modifiedGearDrivingDistance; }
            set { Set(() => ModifiedGearDrivingDistance, ref _modifiedGearDrivingDistance, value); }
        }
        /// <summary>
        /// 加减档位-项目超时（单位：秒）
        /// </summary>
        public double ModifiedGearTimeout
        {
            get { return _modifiedGearTimeout; }
            set { Set(() => ModifiedGearTimeout, ref _modifiedGearTimeout, value); }
        }
        /// <summary>
        /// 加减档位-项目语音
        /// </summary>
        public bool ModifiedGearVoice
        {
            get { return _modifiedGearVoice; }
            set { Set(() => ModifiedGearVoice, ref _modifiedGearVoice, value); }
        }
        /// <summary>
        /// 加减档位-最大档位
        /// </summary>
        public int ModifiedGearGearMax
        {
            get { return _modifiedGearGearMax; }
            set { Set(() => ModifiedGearGearMax, ref _modifiedGearGearMax, value); }
        }
        /// <summary>
        /// 加减档位-最低档位
        /// </summary>
        public int ModifiedGearGearMin
        {
            get { return _modifiedGearGearMin; }
            set { Set(() => ModifiedGearGearMin, ref _modifiedGearGearMin, value); }
        }
        /// <summary>
        ///加减档位-档位最低维持时间（单位：秒）
        /// </summary>
        public double ModifiedGearIgnoreSeconds
        {
            get { return _modifiedGearIgnoreSeconds; }
            set { Set(() => ModifiedGearIgnoreSeconds, ref _modifiedGearIgnoreSeconds, value); }
        }

        /// <summary>
        /// 是否播放档位语音
        /// </summary>
        public bool ModifiedGearIsPlayGearVoice
        {
            get { return _modifiedGearIsPlayGearVoice; }
            set { Set(() => ModifiedGearIsPlayGearVoice, ref _modifiedGearIsPlayGearVoice, value); }
        }
        /// <summary>
        /// 是否播放操作语音
        /// </summary>
        public bool ModifiedGearIsPlayActionVoice
        {
            get { return _modifiedGearIsPlayActionVoice; }
            set { Set(() => ModifiedGearIsPlayActionVoice, ref _modifiedGearIsPlayActionVoice, value); }
        }
        /// <summary>
        /// 操作完成即结束项目（否则按距离结束）
        /// </summary>
        public bool ChkModifiedGearEndFlag
        {
            get { return _chkModifiedGearEndFlag; }
            set { Set(() => ChkModifiedGearEndFlag, ref _chkModifiedGearEndFlag, value); }
        }
        /// <summary>
        /// 是否是自定义流程
        /// </summary>
        public bool ModifiedGearIsCustomProcess
        {
            get { return _modifiedGearIsCustomProcess; }
            set { Set(() => ModifiedGearIsCustomProcess, ref _modifiedGearIsCustomProcess, value); }
        }

        /// <summary>
        /// 档位流程
        /// </summary>
        public string ModifiedGearGearFlow
        {
            get { return _modifiedGearGearFlow; }
            set { Set(() => ModifiedGearGearFlow, ref _modifiedGearGearFlow, value); }
        }

        #endregion

        #region 综合评判


        private bool _commonExamItemsCheckEngineStall = DefaultGlobalSettings.DefaultCommonExamItemsCheckEngineStall;
        private bool _commonExamItemsCheckSafeBelt = DefaultGlobalSettings.DefaultCommonExamItemsCheckSafeBelt;
        private double _commonExamItemsSareBeltTimeOut = DefaultGlobalSettings.DefaultCommonExamItemsSareBeltTimeOut;
        private double _commonExamItemsEngineTimeOut = DefaultGlobalSettings.DefaultCommonExamItemsEngineTimeOut;
        private double _commonExamItemsMaxSpeedKeepTime = DefaultGlobalSettings.DefaultCommonExamItemsMaxSpeedKeepTime;

        private bool _commonExamItemsCheckChangeLanes;
        private double _commonExamItemsChangeLanesTimeOut;
        private double _commonExamItemsChangeLanesAngle;

        private bool _commonExamItemsBreakVoice;
        private string _commonExamItemExamSuccessVoice;
        private string _commonExamItemExamFailVoice;

        private string _commonExamStarExamVoice = "开始考试";
        private string _commonExamEndExamVoice="考试结束";

        private bool _CommonExambeforeSimionLightStartEngine=false;

        private string _CommonExambeforeSimionLightStartEngineVoice="请启动发动机";
        private int _commonStartEngineDeleyTime = 0;

        //开始考试之前手刹检测
        private bool _commonExamItemsCheckHandBreake;

        private bool _checkBrakeMustInitem;

        //有限播放不合格语音
        private bool _firstPlayExamFailVoice = true;

        private bool _playFail = false;

        private bool _showStudentInfo = true;

        private int _voicePlayInterval = 10;

        //信号个数
        //信号速度位0个数
        //速度临界值
        //信号大于速度临界值个数

        //信号个数
        private int _brakeIrregularitySignal = 20;

        public int BrakeIrregularitySignal
        {
            get { return _brakeIrregularitySignal; }
            set { Set(() => BrakeIrregularitySignal,ref _brakeIrregularitySignal, value); }
        }

        private int _brakeIrregularitySpeedZero = 5;

        public int BrakeIrregularitySpeedZero
        {
            get { return _brakeIrregularitySpeedZero; }
            set { Set(() => BrakeIrregularitySpeedZero, ref _brakeIrregularitySpeedZero, value); }
        }

        private int _brakeIrregularitySpeed = 10;

        public int BrakeIrregularitySpeed
        {
            get { return _brakeIrregularitySpeed; }
            set { Set(() => BrakeIrregularitySpeed, ref _brakeIrregularitySpeed, value); }
        }

        private int _brakeIrregularitySpeedOver = 5;

        public int BrakeIrregularitySpeedOver
        {
            get { return _brakeIrregularitySpeedOver; }
            set { Set(() => BrakeIrregularitySpeedOver, ref _brakeIrregularitySpeedOver, value); }
        }



        private bool _isEnableBrakeIrregularity = true;

        /// <summary>
        /// 是否启用制动不平顺
        /// </summary>
        public bool IsEnableBrakeIrregularity
        {
            get { return _isEnableBrakeIrregularity; }
            set { Set(() => IsEnableBrakeIrregularity, ref _isEnableBrakeIrregularity, value); }
        }


        /// <summary>
        /// 显示学员信息(三联 广州界面)
        /// </summary>
        public bool ShowStudentInfo
        {
            get { return _showStudentInfo; }
            set { Set(() => ShowStudentInfo, ref _showStudentInfo, value); }
        }
        /// <summary>
        /// 播放语音间隔(三联 广州界面)
        /// </summary>
        public int VoicePlayInterval
        {
            get { return _voicePlayInterval; }
            set { Set(() => VoicePlayInterval, ref _voicePlayInterval, value); }
        }


        /// <summary>
        /// 必须项目内踩刹车才有效
        /// </summary>
        public bool CheckBrakeMustInitem
        {
            get { return _checkBrakeMustInitem; }
            set { Set(() => CheckBrakeMustInitem, ref _checkBrakeMustInitem, value); }
        }
        public bool FirstPlayExamFailVoice
        {
            get { return _firstPlayExamFailVoice; }
            set { Set(() => FirstPlayExamFailVoice, ref _firstPlayExamFailVoice, value); }
        }
        public bool PlayFail
        {
            get { return _playFail; }
            set { Set(() => PlayFail, ref _playFail, value); }
        }
        

        /// <summary>
        /// 开始考试之前是否需要拉手刹
        /// </summary>
        public bool CommonExamItemsCheckHandBreake
        {
            get { return _commonExamItemsCheckHandBreake; }
            set { Set(() => CommonExamItemsCheckHandBreake, ref _commonExamItemsCheckHandBreake, value); }
        }


        public int CommonStartEngineDeleyTime
        {
            get { return _commonStartEngineDeleyTime; }
            set { Set(() => CommonStartEngineDeleyTime, ref _commonStartEngineDeleyTime, value); }
        }

        public string CommonExambeforeSimionLightStartEngineVoice
        {
            get { return _CommonExambeforeSimionLightStartEngineVoice; }
            set { Set(() => CommonExambeforeSimionLightStartEngineVoice, ref _CommonExambeforeSimionLightStartEngineVoice, value); }
        }

        public bool CommonExambeforeSimionLightStartEngine
        {
            get { return _CommonExambeforeSimionLightStartEngine; }
            set { Set(() => CommonExambeforeSimionLightStartEngine, ref _CommonExambeforeSimionLightStartEngine, value); }
        }

        public string CommonExamStarExamVoice
        {
            get { return _commonExamStarExamVoice; }
            set { Set(() => CommonExamStarExamVoice, ref _commonExamStarExamVoice, value); }
        }
        public string CommonExamEndExamVoice
        {
            get { return _commonExamEndExamVoice; }
            set { Set(() => CommonExamEndExamVoice, ref _commonExamEndExamVoice, value); }
        }
        public double CommonExamItemsMaxSpeedKeepTime
        {
            get { return _commonExamItemsMaxSpeedKeepTime; }
            set {Set(() => CommonExamItemsMaxSpeedKeepTime, ref _commonExamItemsMaxSpeedKeepTime, value);}
        }
        public bool CommonExamItemsBreakVoice

        {
            get { return _commonExamItemsBreakVoice; }
            set { Set(() => CommonExamItemsBreakVoice, ref _commonExamItemsBreakVoice, value); }
        }

        public string CommonExamItemExamSuccessVoice
        {
            get { return _commonExamItemExamSuccessVoice; }
            set { Set(() => CommonExamItemExamSuccessVoice, ref _commonExamItemExamSuccessVoice, value); }
        }

        public string CommonExamItemExamFailVoice
        {
            get { return _commonExamItemExamFailVoice; }
            set { Set(() => CommonExamItemExamFailVoice, ref _commonExamItemExamFailVoice, value); }
        }

        public bool CommonExamItemsCheckChangeLanes
        {
            get { return _commonExamItemsCheckChangeLanes; }
            set { Set(() => CommonExamItemsCheckChangeLanes, ref _commonExamItemsCheckChangeLanes, value); }
        }
        public double CommonExamItemsChangeLanesTimeOut
        {
            get { return _commonExamItemsChangeLanesTimeOut; }
            set { Set(() => CommonExamItemsChangeLanesTimeOut, ref _commonExamItemsChangeLanesTimeOut, value); }
        }

        public double CommonExamItemsChangeLanesAngle
        {
            get { return _commonExamItemsChangeLanesAngle; }
            set { Set(() => CommonExamItemsChangeLanesAngle, ref _commonExamItemsChangeLanesAngle, value); }
        }


        /// <summary>
        ///综合评判发动机熄火
        /// </summary>
        public bool CommonExamItemsCheckEngineStall
        {
            get { return _commonExamItemsCheckEngineStall; }
            set { Set(() => CommonExamItemsCheckEngineStall, ref _commonExamItemsCheckEngineStall, value); }
        }


        /// <summary>
        /// 发动机熄火时间
        /// </summary>
        public double CommonExamItemsEngineTimeOut
        {
            get { return _commonExamItemsEngineTimeOut; }
            set { Set(() => CommonExamItemsEngineTimeOut, ref _commonExamItemsEngineTimeOut, value); }
        }


        /// <summary>
        /// 综合评判是否检测安全带
        /// </summary>
        public bool CommonExamItemsCheckSafeBelt
        {
            get { return _commonExamItemsCheckSafeBelt; }
            set { Set(() => CommonExamItemsCheckSafeBelt, ref _commonExamItemsCheckSafeBelt, value); }
        }

        /// <summary>
        /// 综合评判安全带检测超时
        /// </summary>
        public double CommonExamItemsSareBeltTimeOut
        {
            get { return _commonExamItemsSareBeltTimeOut; }
            set { Set(() => CommonExamItemsSareBeltTimeOut, ref _commonExamItemsSareBeltTimeOut, value); }
        }


        #endregion

        #region 限速

        private int _speedLimitExamDistance = DefaultGlobalSettings.DefaultSpeedLimitExamDistance;
        /// <summary>
        ///限速项目距离
        /// </summary>
        public int SpeedLimitExamDistance
        {
            get { return _speedLimitExamDistance; }
            set { Set(() => SpeedLimitExamDistance, ref _speedLimitExamDistance, value); }
        }
        #endregion

        #region 其它

        private int _maxOverLineDriving = 5;
        /// <summary>
        /// 占道行驶最大距离（单位：米）
        /// </summary>
        public int MaxOverLineDriving
        {
            get { return _maxOverLineDriving; }
            set { Set(() => MaxOverLineDriving, ref _maxOverLineDriving, value); }
        }

        #endregion

        #endregion

        #region 成绩打印

        private string _reportSchoolName = DefaultGlobalSettings.DefaultReportSchoolName;
        /// <summary>
        /// 成绩打印驾校名称
        /// </summary>
        public string ReportSchoolName
        {
            get { return _reportSchoolName; }
            set { Set(() => ReportSchoolName, ref _reportSchoolName, value); }
        }


        private string _reportTeacherName = DefaultGlobalSettings.DefaultReportTecherName;
        /// <summary>
        /// 成绩打印教练名称
        /// </summary>
        public string ReportTecherName
        {
            get { return _reportTeacherName; }
            set { Set(() => ReportTecherName, ref _reportTeacherName, value); }
        }

        private string _reportTeacherTel = DefaultGlobalSettings.DefaultReportTecherTel;
        /// <summary>
        /// 成绩打印教练名称
        /// </summary>
        public string ReportTecherTel
        {
            get { return _reportTeacherTel; }
            set { Set(() => ReportTecherTel, ref _reportTeacherTel, value); }
        }




        #endregion

        #region 新增版本、OBD、方向角度 来源参数设置
        public MasterControlBoxVersion masterControlBoxVersion
        {
            get
            {
                return _masterControlBoxVersion;
            }

            set
            {
                _masterControlBoxVersion = value;
            }
        }

        public OBDSource ObdSource
        {
            get
            {
                return _obdSource;
            }

            set
            {
                _obdSource = value;
            }
        }

        public string ObdSignalSource
        {
            get
            {
                return _obdSignalSource;
            }

            set
            {
                _obdSignalSource = value;
            }
        }

        public string AngleSignalSource
        {
            get
            {
                return _AngleSignalSource;
            }

            set
            {
                _AngleSignalSource = value;
            }
        }

        public GearConnectionMethod gearConnectionMethod
        {
            get
            {
                return _gearConnectionMethod;
            }

            set
            {
                _gearConnectionMethod = value;
            }
        }

        //public ConnectionScheme gearConnectionMethod
        //{
        //    get
        //    {
        //        return _gearConnectionMethod;
        //    }

        //    set
        //    {
        //        _gearConnectionMethod = value;
        //    }
        //}

        public MileageSource MileageSource
        {
            get
            {
                return _mileageSource;
            }

            set
            {
                _mileageSource = value;
            }
        }

        public bool IsPlaybackToOriginalLane
        {
            get
            {
                return _IsPlaybackToOriginalLane;
            }

            set
            {
                _IsPlaybackToOriginalLane = value;
            }
        }

        public int ReturnToOriginalLaneDistince
        {
            get
            {
                return _ReturnToOriginalLaneDistince;
            }

            set
            {
                _ReturnToOriginalLaneDistince = value;
            }
        }

        public PullOverMark PullOverMark
        {
            get { return _pullOverMark; }
            set { Set(() => PullOverMark, ref _pullOverMark, value); }
        }
        #endregion



        #region 项目结束语音 为了加快速度不考虑采用反色直接使用Switch Case 进行处理 
        /// <summary>
        /// 初始化结束语音的值 /
        /// </summary>
        private bool _initEndVoice = false;
        public bool InitEndVoice
        {
            get { return _initEndVoice; }
            set { Set(() => InitEndVoice, ref _initEndVoice, value); }
        }
        private bool _vehicleStartingEndVoice = false;
        /// <summary>
        /// 起步项目结束语音
        /// </summary>
        public bool VehicleStartingEndVoice
        {
            get { return _vehicleStartingEndVoice; }
            set { Set(() => VehicleStartingEndVoice, ref _vehicleStartingEndVoice, value); }
        }
        /// <summary>
        /// 路口直行项目结束语音
        /// </summary>
        private bool _straightThroughIntersectionEndVoice = false;
        public bool StraightThroughIntersectionEndVoice
        {
            get { return _straightThroughIntersectionEndVoice; }
            set { Set(() => StraightThroughIntersectionEndVoice, ref _straightThroughIntersectionEndVoice, value); }
        }
        /// <summary>
        /// 路口右转结束语音
        /// </summary>
        private bool _turnRightEndVoice = false;
        public bool TurnRightEndVoice
        {
            get { return _turnRightEndVoice; }
            set { Set(() => TurnRightEndVoice, ref _turnRightEndVoice, value); }
        }
        /// <summary>
        /// 掉头项目结束语音 
        /// </summary>
        private bool _turnRoundEndVoice;

        public bool TurnRoundEndVoice
        {
            get { return _turnRoundEndVoice; }
            set { Set(() => TurnRoundEndVoice, ref _turnRoundEndVoice, value); }
        }

        /// <summary>
        /// 学习区域结束语音
        /// </summary>
        private bool _schoolAreaEndVoice;
        public bool SchoolAreaEndVoice
        {
            get { return _schoolAreaEndVoice; }
            set { Set(() => SchoolAreaEndVoice, ref _schoolAreaEndVoice, value); }
        }

        /// <summary>
        /// 公交区域结束语音
        /// </summary>
        private bool _busAreaEndVoice;
        public bool BusAreaEndVoice
        {
            get { return _busAreaEndVoice; }
            set { Set(() => BusAreaEndVoice, ref _busAreaEndVoice, value); }
        }

        private bool _pedestrianCrossingEndVoice;
        /// <summary>
        /// 人行横道项目结束语音
        /// </summary>
        public bool PedestrianCrossingEndVoice
        {
            get { return _pedestrianCrossingEndVoice; }
            set { Set(() => PedestrianCrossingEndVoice, ref _pedestrianCrossingEndVoice, value); }
        }
        /// <summary>
        /// 环岛项目结束语音
        /// </summary>
        private bool _roundaboutEndVoice = false;

        public bool RoundaboutEndVoice
        {
            get { return _roundaboutEndVoice; }
            set { Set(() => RoundaboutEndVoice, ref _roundaboutVoice, value); }
        }
        /// <summary>
        /// 变更结束车道
        /// </summary>
        private bool _changeLanesEndVoice = false;

        public bool ChangeLanesEndVoice
        {
            get { return _changeLanesEndVoice; }
            set { Set(() => ChangeLanesEndVoice, ref _changeLanesEndVoice, value); }
        }

        /// <summary>
        /// 会车结束语音
        /// </summary>
        private bool _meetingEndVoice;

        public bool MeetingEndVoice
        {
            get { return _meetingEndVoice; }
            set { Set(() => MeetingEndVoice, ref _meetingEndVoice, value); }
        }

        /// <summary>
        /// 超车结束语音
        /// </summary>
        private bool _overtakeEndVoice;
        public bool OvertakeEndVoice
        {
            get { return _overtakeEndVoice; }
            set { Set(() => OvertakeEndVoice, ref _overtakeEndVoice, value); }
        }


        /// <summary>
        /// 急弯破路结束语音
        /// </summary>
        private bool _sharpTurnEndVoice;
        public bool SharpTurnEndVoice
        {
            get { return _sharpTurnEndVoice; }
            set { Set(() => SharpTurnEndVoice, ref _sharpTurnEndVoice, value); }
        }

        private bool _straightDrivingEndVoice;

        /// <summary>
        /// 直线行驶项目结束语音
        /// </summary>
        public bool StraightDrivingEndVoice
        {
            get { return _straightDrivingEndVoice; }
            set { Set(() => StraightDrivingEndVoice, ref _straightDrivingEndVoice, value); }
        }

        private bool _pullOverEndVoice;

        /// <summary>
        /// 靠边停车项目结束语音
        /// </summary>
        public bool PullOverEndVoice
        {
            get { return _pullOverEndVoice; }
            set { Set(() => PullOverEndVoice, ref _pullOverEndVoice, value); }
        }

        /// <summary>
        /// 加减档位项目结束语音
        /// </summary>
        private bool _modifiedGearEndVoice;
        public bool ModifiedGearEndVoice
        {
            get { return _modifiedGearEndVoice; }
            set { Set(() => ModifiedGearEndVoice, ref _modifiedGearEndVoice, value); }
        }


        private bool _turnLeftEndVoice;

        /// <summary>
        /// 路口右转项目语音
        /// </summary>
        public bool TurnLeftEndVoice
        {
            get { return _turnLeftEndVoice; }
            set { Set(() => TurnLeftEndVoice, ref _turnLeftEndVoice, value); }
        }

        public bool PullOverStartFlage
        {
            get
            {
                return _pullOverStartFlage;
            }

            set
            {
                _pullOverStartFlage = value;
            }
        }

      



        #endregion

    }

    [Description("转速模式")]
    public enum EngineRpmRatioMode
    {
        [Description("放大")]
        ZoomIn,
        [Description("缩小")]
        ZoomOut,
    }

    [Description("车速模式")]
    public enum SpeedKhmMode
    {
        [Description("放大")]
        ZoomIn,
        [Description("缩小")]
        ZoomOut,
    }
}