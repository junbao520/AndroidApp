using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class DefaultGlobalSettings
    {
        /// <summary>
        /// GPS日志输出路径
        /// </summary>
        public static string DefaultGpsLogFolder = Path.Combine(Environment.CurrentDirectory, "Logs/GpsLogs");
        /// <summary>
        /// 车载信号日志输出路径
        /// </summary>
        public static string DefaultCarSignalLogFolder = Path.Combine(Environment.CurrentDirectory, "Logs/SignalLogs");
        /// <summary>
        /// OBD波特率
        /// </summary>
        public const int DefaultObdBaudRate = 38400;
        /// <summary>
        /// 减速项目的速度
        /// </summary>
        public const double DefaultSlowSpeedInKmh = 30;
        /// <summary>
        /// 减速项目发动机最大转速
        /// </summary>
        public const int DefaultSlowEngineMaxRpm = 3000;
        /// <summary>
        /// 停车系数
        /// </summary>
        public const double DefaultParkingValueKmh = 0.9;
        /// <summary>
        /// 判断停车延迟时间
        /// </summary>
        public const double DefaultParkingDelaySeconds = 0.5;

        #region 通用
        /// <summary>
        /// 检查规定时间内是否有踩刹车的动作
        /// </summary>
        public const int DefaultBrakeTimeout = 5;
        /// <summary>
        /// 检查规定距离内是否有踩刹车的动作（单位：米）
        /// </summary>
        public const int DefaultBrakeDistance = 25;
        /// <summary>
        /// 最大占道行驶距离
        /// </summary>
        public const int DefaultMaxOverLineDrivingDistance = 50;
        /// <summary>
        /// 全程检测多少档速度时间检测
        /// </summary>
        public const Gear DefaultGlobalContinuousGear = Gear.Neutral;
        /// <summary>
        /// 全程检测某档保持多少速度
        /// </summary>
        public const int DefaultGlobalContinuousSpeed = 0;
        /// <summary>
        /// 全程检测某档保持某速度多少时间
        /// </summary>
        public const int DefaultGlobalContinuousSeconds = 0;
        #endregion


        #region 的发动机转速、速度比率（档位）
        /// <summary>
        /// 一档最小齿速比
        /// </summary>
        public const int DefaultGearOneMinRatio = 120;
        /// <summary>
        /// 一档最大齿速比
        /// </summary>
        public const int DefaultGearOneMaxRatio = 175;
        /// <summary>
        /// 二档最小齿速比
        /// </summary>
        public const int DefaultGearTwoMinRatio = 70;
        /// <summary>
        /// 二档最大齿速比
        /// </summary>
        public const int DefaultGearTwoMaxRatio = 90;
        /// <summary>
        /// 三档最小齿速比
        /// </summary>
        public const int DefaultGearThreeMinRatio = 45;
        /// <summary>
        /// 三档最大齿速比
        /// </summary>
        public const int DefaultGearThreeMaxRatio = 56;
        /// <summary>
        /// 四档最小齿速比
        /// </summary>
        public const int DefaultGearFourMinRatio = 37;
        /// <summary>
        /// 四档最大齿速比
        /// </summary>
        public const int DefaultGearFourMaxRatio = 44;
        /// <summary>
        /// 五档最小齿速比
        /// </summary>
        public const int DefaultGearFiveMinRatio = 30;
        /// <summary>
        /// 五档最大齿速比
        /// </summary>
        public const int DefaultGearFiveMaxRatio = 35;
        /// <summary>
        /// 转速放大缩小的倍数
        /// </summary>
        public const float DefaultMultipleRpm = 1;
        #endregion

        #region 专项配置

        #region 上车准备
        /// <summary>
        /// 绕车一周的超时时间
        /// </summary>
        public const int DefaultAroundCarTimeout = 60;
        /// <summary>
        /// 是否启用
        /// </summary>
        public const bool DefaultPrepareDrivingEnable = false;
        /// <summary>
        /// 是否绕车一周
        /// </summary>
        public const bool DefaultAroundCarEnable = false;
        /// <summary>
        /// 绕车一周语音
        /// </summary>
        public const bool DefaultAroundCarVoiceEnable = false;
        #endregion

        #region 灯光模拟
        /// <summary>
        /// 灯光检测，灯光检测时间
        /// </summary>
        public const double DefaultSimulationLightTimeout = 5;
        /// <summary>
        /// 灯光检测，灯光检测时间间隔，单位：秒
        /// </summary>
        public const double DefaultSimulationLightInterval = 2;
        /// <summary>
        /// 白天灯光模拟
        /// </summary>
        public const bool DefaultSimulationsLightOnDay = true;
        /// <summary>
        /// 夜间灯光模拟
        /// </summary>
        public const bool DefaultSimulationsLightOnNight = false;

        #endregion

        #region 起步
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultStartDistance = 45;
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultStartVoice = true;
        /// <summary>
        /// 检测起步规定时间
        /// </summary>
        public const int DefaultStartTimeout = 120;
        /// <summary>
        /// 检测起步起步灯光
        /// </summary>
        public const bool DefaultIsCheckStartLight = true;
        /// <summary>
        /// 检测起步夜间灯光
        /// </summary>
        public const bool DefaultIsCheckStartLightOnNight = true;
        /// <summary>
        /// 手刹释放限制时间
        /// </summary>
        public const int DefaultStartReleaseHandbrakeTimeout = 10;
        /// <summary>
        /// 起步时最高转速
        /// </summary>
        public const int DefaultStartEngineRpm = 3000;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultVehicleStartingLoudSpeakerDayCheck = true;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultVehicleStartingLoudSpeakerNightCheck = false;
        /// <summary>
        /// 后溜最小距离
        /// </summary>
        public const double DefaultStartSmallBackwardDistance = 0.2;
        /// <summary>
        /// 后溜最大距离
        /// </summary>
        public const double DefaultStartLargeBackwardDistance = 0.5;
        /// <summary>
        /// 起步前进一定距离停上检测
        /// </summary>
        public const double DefaultStartStopCheckForwardDistance = 10;
        /// <summary>
        /// 夜考是否启用远近光交替检测
        /// </summary>
        public const bool DefaultStartLowAndHighBeamInNight = false;

        /// <summary>
        /// 起步闯动检测
        /// </summary>
        public const bool DefaultStartShockEnable = false;

        /// <summary>
        /// 起步闯动检测值
        /// </summary>
        public const double DefaultStartShockValue = 0.20;

        /// <summary>
        ///闯动生效次数
        /// </summary>
        public const int DefaultStartShockCount = 2;
        #endregion

        #region 路口直行
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultStraightThroughIntersectionVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultStraightThroughIntersectionDistance = 60;
        /// <summary>
        /// 灯光检测
        /// </summary>
        public const bool DefaultStraightThroughIntersectionLightCheck = true;
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public const bool DefaultStraightThroughIntersectionBrakeRequire = false;
        /// <summary>
        /// 速度限制
        /// </summary>
        public const int DefaultStraightThroughIntersectionSpeedLimit = 30;
        /// <summary>
        /// 要求踩刹车速度限制
        /// </summary>
        public const int DefaultStraightThroughIntersectionBrakeSpeedUp = 0;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultStraightThroughIntersectionLoudSpeakerDayCheck = false;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultStraightThroughIntersectionLoudSpeakerNightCheck = false;
        #endregion

        #region 左转
        /// <summary>
        /// 左转向灯检测
        /// </summary>
        public const bool DefaultTurnLeftLightCheck = true;
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultTurnLeftVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultTurnLeftDistance = 80;
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public const bool DefaultTurnLeftBrakeRequire = false;
        /// <summary>
        /// 速度限制
        /// </summary>
        public const int DefaultTurnLeftSpeedLimit = 30;
        /// <summary>
        /// 要求踩刹车速度限制
        /// </summary>
        public const int DefaultTurnLeftBrakeSpeedUp = 0;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultTurnLeftLoudSpeakerDayCheck = false;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultTurnLeftLoudSpeakerNightCheck = false;
        #endregion

        #region 右转
        /// <summary>
        /// 右转向灯检测
        /// </summary>
        public const bool DefaultTurnRightLightCheck = true;
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultTurnRightVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultTurnRightDistance = 80;
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public const bool DefaultTurnRightBrakeRequire = false;
        /// <summary>
        /// 速度限制
        /// </summary>
        public const int DefaultTurnRightSpeedLimit = 30;
        /// <summary>
        /// 要求踩刹车速度限制
        /// </summary>
        public const int DefaultTurnRightBrakeSpeedUp = 0;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultTurnRightLoudSpeakerDayCheck = false;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultTurnRightLoudSpeakerNightCheck = false;
        #endregion

        #region 掉头
        /// <summary>
        /// 开始掉头转向角度差
        /// </summary>
        public const int DefaultTurnRoundStartAngleDiff = 20;
        /// <summary>
        /// 结束掉头转向角度差
        /// </summary>
        public const int DefaultTurnRoundEndAngleDiff = 120;
        /// <summary>
        /// 掉头所用最大距离限制
        /// </summary>
        public const int DefaultTurnRoundMaxDistance = 200;
        /// <summary>
        /// 掉头灯光检测
        /// </summary>
        public const bool DefaultTurnRoundLightCheck = true;
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultTurnRoundVoice = true;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultTurnRoundLoudSpeakerDayCheck = false;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultTurnRoundLoudSpeakerNightCheck = false;
        /// <summary>
        /// 掉头必踩刹车
        /// </summary>
        public const bool DefaultTurnRoundBrakeRequired = false;
        #endregion

        #region 人行横道线
        /// <summary>
        /// 灯光检测
        /// </summary>
        public const bool DefaultPedestrianCrossingLightCheck = true;
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultPedestrianCrossingVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultPedestrianCrossingDistance = 60;
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public const bool DefaultPedestrianCrossingBrakeRequire = false;
        /// <summary>
        /// 速度限制
        /// </summary>
        public const int DefaultPedestrianCrossingSpeedLimit = 30;
        /// <summary>
        /// 要求踩刹车速度限制
        /// </summary>
        public const int DefaultPedestrianCrossingBrakeSpeedUp = 0;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultPedestrianCrossingLoudSpeakerNightCheck = false;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultPedestrianCrossingLoudSpeakerDayCheck = false;
        #endregion

        #region 通过公共汽车区域
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultBusAreaVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultBusAreaDistance = 60;
        /// <summary>
        /// 灯光检测
        /// </summary>
        public const bool DefaultBusAreaLightCheck = true;
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public const bool DefaultBusAreaBrakeRequire = false;
        /// <summary>
        /// 速度限制
        /// </summary>
        public const int DefaultBusAreaSpeedLimit = 30;
        /// <summary>
        /// 要求踩刹车速度限制
        /// </summary>
        public const int DefaultBusAreaBrakeSpeedUp = 0;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultBusAreaLoudSpeakerNightCheck = false;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultBusAreaLoudSpeakerDayCheck = false;
        #endregion

        #region 通过学校区域
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultSchoolAreaVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultSchoolAreaDistance = 60;
        /// <summary>
        /// 灯光检测
        /// </summary>
        public const bool DefaultSchoolAreaLightCheck = true;
        /// <summary>
        /// 必须踩刹车
        /// </summary>
        public const bool DefaultSchoolAreaBrakeRequire = false;
        /// <summary>
        /// 速度限制
        /// </summary>
        public const int DefaultSchoolAreaSpeedLimit = 30;
        /// <summary>
        /// 要求踩刹车速度限制
        /// </summary>
        public const int DefaultSchoolAreaBrakeSpeedUp = 0;
        /// <summary>
        /// 白考喇叭检测
        /// </summary>
        public const bool DefaultSchoolAreaLoudSpeakerDayCheck = false;
        /// <summary>
        /// 夜考喇叭检测
        /// </summary>
        public const bool DefaultSchoolAreaLoudSpeakerNightCheck = false;
        /// <summary>
        /// 禁止鸣号
        /// </summary>
        public const bool DefaultSchoolAreaForbidLoudSpeakerCheck = false;
        #endregion

        #region 环岛
        /// <summary>
        /// 项目语音
        /// </summary>
        public const bool DefaultRoundaboutVoice = true;
        /// <summary>
        /// 项目距离
        /// </summary>
        public const int DefaultRoundaboutDistance = 60;
        /// <summary>
        /// 环岛灯光检测
        /// </summary>
        public const bool DefaultRoundaboutLightCheck = true;
        #endregion

        #region 变更车道
        /// <summary>
        /// 变道的最大距离
        /// </summary>
        public const int DefaultChangeLanesMaxDistance = 150;
        /// <summary>
        /// 变道智能推导角度
        /// </summary>
        public const int DefaultChangeLanesAngle = 8;
        /// <summary>
        /// 变道超时时间
        /// </summary>
        public const int DefaultChangeLanesTimeout = 0;
        /// <summary>
        /// 变更车道项目语音
        /// </summary>
        public const bool DefaultChangeLanesVoice = true;
        /// <summary>
        /// 变更车道灯光检测
        /// </summary>
        public const bool DefaultChangeLanesLightCheck = true;
        /// <summary>
        /// 变更车道夜考远近光交替
        /// </summary>
        public const bool DefaultChangeLanesLowAndHighBeamCheck = false;
        /// <summary>
        /// 变更车道默认准备距离
        /// </summary>
        public const int DefaultChangeLanesPrepareDistance = 10;

        #endregion

        #region 会车
        /// <summary>
        /// 会车距离
        /// </summary>
        public const double DefaultMeetingDrivingDistance = 60;
        /// <summary>
        /// 会车速度限制
        /// </summary>
        public const int DefaultMeetingSlowSpeedInKmh = 30;
        /// <summary>
        /// 会车语音
        /// </summary>
        public const bool DefaultMeetingVoice = true;
        /// <summary>
        /// 会车是否踩刹车
        /// </summary>
        public const bool DefaultMeetingCheckBrake = false;
        /// <summary>
        /// 禁止远光
        /// </summary>
        public const bool DefaultMeetingForbidHighBeamCheck = false;
        /// <summary>
        /// 会车角度
        /// </summary>
        public const double DefaultMeetingAngle = 10;

        #endregion

        #region 超车
        /// <summary>
        /// 超车语音
        /// </summary>
        public const bool DefaultOvertakeVoice = true;
        /// <summary>
        /// 超车灯光检测
        /// </summary>
        public const bool DefaultOvertakeLightCheck = true;
        /// <summary>
        /// 超车最大时间和距离
        /// </summary>
        public const int DefaultOvertakeMaxDistance = 200;
        /// <summary>
        /// 超车超时
        /// </summary>
        public const int DefaultOvertakeTimeout = 0;
        /// <summary>
        /// 超车变道角度（8）
        /// </summary>
        public const double DefaultOvertakeChangeLanesAngle = 8;
        /// <summary>
        /// 操作速度限制（0）
        /// </summary>
        public const int DefaultOvertakeSpeedLimit = 0;
        /// <summary>
        /// 超车夜考喇叭检测
        /// </summary>
        public const bool DefaultOvertakeLoudSpeakerNightCheck = false;
        /// <summary>
        ///  超车白考喇叭检测
        /// </summary>
        public const bool DefaultOvertakeLoudSpeakerDayCheck = false;
        /// <summary>
        /// 超车夜考远近光交替
        /// </summary>
        public const bool DefaultOvertakeLowAndHighBeamCheck = false;
        /// <summary>
        /// 超车默认准备距离
        /// </summary>
        public const int DefaultOvertakePrepareDistance = 10;
        /// <summary>
        /// 默认超车最低速度
        /// </summary>
        public const int DefaultOvertakeLowestSpeed = 0;

        public const bool DefaultOvertakeBackToOriginalLane = true;



        #endregion

        #region 急坡弯路
        /// <summary>
        /// 急坡弯路是否启用
        /// </summary>
        public const bool DefaultSharpTurnVoice = true;
        /// <summary>
        /// 急坡弯路灯光检测
        /// </summary>
        public const bool DefaultSharpTurnLightCheck = true;
        /// <summary>
        /// 急坡弯路项目距离
        /// </summary>
        public const int DefaultSharpTurnDistance = 60;
        /// <summary>
        /// 急坡弯路白考喇叭
        /// </summary>
        public const bool DefaultSharpTurnLoudspeakerInDay = false;
        /// <summary>
        /// 急坡弯路夜考喇叭
        /// </summary>
        public const bool DefaultSharpTurnLoudspeakerInNight = false;
        /// <summary>
        /// 急坡弯路必踩刹车
        /// </summary>
        public const bool DefaultSharpTurnBrake = false;
        /// <summary>
        /// 急坡弯路速度限制
        /// </summary>
        public const int DefaultSharpTurnSpeedLimit = 0;
        /// <summary>
        /// 急坡弯路该速度以上必踩刹车
        /// </summary>
        public const int DefaultSharpTurnBrakeSpeedUp = 0;
        
        #endregion

        #region 直线行驶
        /// <summary>
        /// 直线行驶是否启用
        /// </summary>
        public const bool DefaultStraightDrivingVoice = true;
        /// <summary>
        /// 直线行驶最小速度限制
        /// </summary>
        public const int DefaultStraightDrivingSpeedMinLimit = 0;
        /// <summary>
        /// 直线行驶最高速度限制
        /// </summary>
        public const int DefaultStraightDrivingSpeedMaxLimit = 0;
        /// <summary>
        /// 直线行驶准备距离
        /// </summary>
        public const int DefaultStraightDrivingPrepareDistance = 20;
        /// <summary>
        /// 直线行驶行驶距离
        /// </summary>
        public const double DefaultStraightDrivingDistance = 100;
        /// <summary>
        /// 直线行驶最大偏移角
        /// </summary>
        public const double DefaultStraightDrivingMaxOffsetAngle = 5.0;
        #endregion

        #region 加减档位
        /// <summary>
        /// 加减档档位过滤时间
        /// </summary>
        public const double DefaultModifiedGearIgnoreSeconds = 1.2;
        /// <summary>
        /// 加减档位是否启用
        /// </summary>
        public const bool DefaultModifiedGearVoice = true;
        /// <summary>
        /// 加减档行驶最大距离
        /// </summary>
        public const int DefaultModifiedGearDrivingDistance = 200;
        /// <summary>
        /// 加减档检测最大档位
        /// </summary>
        public const int DefaultModifiedGearGearMax = 4;
        /// <summary>
        /// 加减档检测最小档位
        /// </summary>
        public const int DefaultModifiedGearGearMin = 2;
        /// <summary>
        /// 加减档换挡时间限制
        /// </summary>
        public const double DefaultModifiedGearTimeout = 2;
        /// <summary>
        /// 加减档是否加档检测
        /// </summary>
        public const bool DefaultModifiedGearAddGearCheck = true;
        /// <summary>
        /// 加减档是否减档检测
        /// </summary>
        public const bool DefaultModifiedGearReduceGearCheck = true;
        
        /// <summary>
        /// 是否播放档位语音
        /// </summary>
        public const bool DefaultModifiedGearIsPlayGearVoice=false;
        /// <summary>
        /// 完成档位操作就结束项目
        /// </summary>
        public const bool DefaultModifiedGearEndFlag = false;

       /// <summary>
       /// 是否播放操作语音
       /// </summary>
        public const bool DefaultModifiedGearIsPlayActionVoice=false;
        /// <summary>
        /// 是否是自定义流程
        /// </summary>
        public const bool DefaultModifiedGearIsCustomProcess=false;
        /// <summary>
        /// 自定义流程
        /// </summary>
        public const string DefaultModifiedGearGearFlow="2-3-2";

        public const string DefaultModifiedGearFlowVoice = "请加到二挡-请加到三档-请加到四档-请加到五档-请减到四档-请减到三档-请减到二档-请减到一档";
        #endregion

        #region 靠边停车
        /// <summary>
        /// 靠边停车停车后检测拉起驻车制动器时间
        /// </summary>
        public const int DefaultPullOverHandbrakeTimeout = 10;
        /// <summary>
        ///靠边停车项目语音
        /// </summary>
        public const bool DefaultPullOverVoice = true;
        /// <summary>
        /// 靠边停车检测发动机
        /// </summary>
        public const bool DefaultIsCheckPullOverEngineStop = true;
        /// <summary>
        /// 检测下车前安全带是否松开
        /// </summary>
        public const bool DefaultIsCheckPullOverSafetyBeltBeforeGetOff = true;
        /// <summary>
        /// 检测靠边停车下车前关闭大灯
        /// </summary>
        public const bool DefaultPullOverStopLowBeamBeforeGetOff = true;
        /// <summary>
        /// 检测靠边停车转向灯
        /// </summary>
        public const bool DefaultPullOverTurnLightCheck = true;
        /// <summary>
        /// 靠边停车行驶距离
        /// </summary>
        public const int DefaultPullOverMaxDrivingDistance = 400;
        /// <summary>
        /// 靠边停车警示灯检测
        /// </summary>
        public const bool DefaultPullOverOpenCautionBeforeGetOff = false;
        /// <summary>
        /// 靠边停车关近光灯检测
        /// </summary>
        public const bool DefaultPullOverLowBeamCheck = false;
        /// <summary>
        /// 靠边停车发动机熄火检测
        /// </summary>
        public const bool DefaultPullOverStopEngineBeforeGetOff = false;
        /// <summary>
        /// 靠边停车安全带检测
        /// </summary>
        public const bool DefaultPullOverOpenSafetyBeltBeforeGetOff = false;
        /// <summary>
        /// 靠边停车开关车门检测
        /// </summary>
        public const bool DefaultPullOverOpenCloseDoorCheck = false;
        /// <summary>
        /// 靠边停车停车开警告灯
        /// </summary>
        public const bool DefaultIsCheckPullOverCautionLight = false;
        /// <summary>
        /// 靠边停车考试结束后，考试是否自动结束
        /// </summary>
        public const bool DefaultPulloverEndAutoTriggerStopExam = false;
        /// <summary>
        /// 靠边停车考试结束后，考试是否自动触发起步
        /// </summary>
        public const bool DefaultPulloverEndAutoTriggerStart= false;
        /// <summary>
        /// 靠边停车的结束标记
        /// </summary>
        public const PullOverEndMark DefaultPullOverEndMark = PullOverEndMark.OpenCloseDoorCheck;
         /// <summary>
        /// 达到规定里程后，靠边停车是否自动触发（，不触发）
        /// </summary>
        public const bool DefaultPullOverAutoTrigger = false;
        /// <summary>
        /// 停车空挡
        /// </summary>
        public const bool DefaultPullOverNeutralCheck = false;

        /// <summary>
        /// 靠边停车30厘米检测
        /// </summary>
        public const bool DefaultPullOverMarginsEnable = false;

        /// <summary>
        /// 靠边停车默认边距0.5 米
        /// </summary>
        public const double DefaultPullOverMarginsValue = 0.5;


        /// 靠边停车停的标记
        /// </summary>
        public const PullOverMark DefaultPullOverMark = PullOverMark.CarStop;
        #endregion

        #region 限速项目

        /// <summary>
        /// 限速度项目准备距离
        /// </summary>
        public const int DefaultSpeedLimitExamDistance = 20;

        #endregion

        #endregion

        #region 全局配置

        #region 考试设置
        /// <summary>
        /// GPS日志是否输出
        /// </summary>
        public const bool DefaultGpsLogEnable = false;
        /// <summary>
        /// 白考考试里程
        /// </summary>
        public const int DefaultExamDistance = 3000;
        /// <summary>
        /// 夜考考试里程
        /// </summary>
        public const int DefaultNightDistance = 3000;
        /// <summary>
        /// 不合格是否继续考试
        /// </summary>
        public const bool DefaultContinueExamIfFailed = true;
        /// <summary>
        /// 是否灯光模拟检测
        /// </summary>
        public const bool DefaultCheckLightingSimulation = true;
        /// <summary>
        /// 触犯语音规则是否播报
        /// </summary>
        public const bool DefaultVoiceBrokenRule = true;
        /// <summary>
        /// 不按规定路线行驶距离，单位:米
        /// </summary>
        public const int DefaultMaxFromEdgeDistance = 1000;

        /// <summary>
        /// 发动机转速多少以下判定发动机熄火
        /// </summary>
        public const int DefaultEngineStopRmp = 20;
        /// <summary>
        /// 默认考试地图
        /// </summary>
        public const int DefaultExamMapId = 0;

        /// <summary>
        /// 刹车保持时间
        /// </summary>
        public const int DefaultBrakeKeepTime = 0;
        /// <summary>
        /// 驾考车型C2
        /// </summary>
        public const bool DefaultLicenseC2 = false;
        /// <summary>
        /// 驾考车型C1
        /// </summary>
        public const bool DefaultLicenseC1 = true;
        /// <summary>
        /// 车速读取OBD,默认读取OBD
        /// </summary>
        public const bool DefaultCheckOBD = true;
        /// <summary>
        /// 读取OBD转速
        /// </summary>
        public const bool DefaultCheckOBDRpm = true;
        /// <summary>
        /// 距离结束考试项目
        /// </summary>
        public const bool DefaultEndExamByDistance = false;

        /// <summary>
        /// 档位来源，默认为转速比
        /// </summary>
        public const GearSource DefaultGearSource = GearSource.SpeadRadio;

        public const AngleSource DefaultAngleSource = AngleSource.GPS;

        public const string DefaultSignalSource = "COM1";

        /// <summary>
        /// 是否安装空挡传感器
        /// </summary>
        public const bool DefaultIsNeutralGear = false;
        /// <summary>
        /// 车速放大/缩小
        /// </summary>
        public const SpeedKhmMode DefaultSpeedKhmMode = SpeedKhmMode.ZoomIn;
        /// <summary>
        /// 接线方案
        /// </summary>
        public const ConnectionScheme DefaultConnectionScheme = ConnectionScheme.Acquiesce;

        /// <summary>
        /// 车速倍率
        /// </summary>
        public const float DefaultMultiSpeed = 1;
        #endregion

        #region 发动机配置
        /// <summary>
        /// 发动机怠速
        /// </summary>
        public const int DefaultMinEngineRpm = 750;
        /// <summary>
        /// 发动机最高转速
        /// </summary>
        public const int DefaultMaxEngineRpm = 3000;
        /// <summary>
        /// 是否检测空挡打火
        /// </summary>
        public const bool DefaultNeutralStart = false;
        /// <summary>
        /// 检测两次挂档不进的时间
        /// </summary>
        public const double DefaultTwiceGearNoSuccess = 0;

        #endregion

        #region 全程档位和速度设置
        /// <summary>
        /// 全程必须达到的最低速度
        /// </summary>
        public const int DefaultGlobalLowestSpeed = 0;

        /// <summary>
        /// 全程必须达到的速度保持时间
        /// </summary>
        public const int DefaultGlobalLowestSpeedHoldTimeSeconds = 0;
        /// <summary>
        /// 全程必须达到的速度保持距离
        /// </summary>
        public const int DefaultGlobalLowestSpeedHoldDistince = 0;
        /// <summary>
        /// 全程最大速度限制
        /// </summary>
        public const int DefaultMaxSpeed = 60;
        /// <summary>
        /// 1档行驶时间
        /// </summary>
        public const int DefaultGearOneTimeout = 0;
        /// <summary>
        /// 1档行驶距离
        /// </summary>
        public const int DefaultGearOneDrivingDistance = 50;
        /// <summary>
        /// 2档行驶时间
        /// </summary>
        public const int DefaultGearTwoTimeout = 0;
        /// <summary>
        /// 2档行驶距离
        /// </summary>
        public const int DefaultGearTwoDrivingDistance = 100;
        /// <summary>
        /// 空挡滑行距离
        /// </summary>
        public const int DefaultNeutralTaxiingMaxDistance = 50;
        /// <summary>
        /// 空挡滑行时间
        /// </summary>
        public const int DefaultNeutralTaxiingTimeout = 0;

        /// <summary>
        /// 离合滑行时间
        /// </summary>
        public const int DefaultClutchTaxiingTimeout = 0;
        /// <summary>
        /// 检测档位与速度不匹配的时间
        /// </summary>
        public const int DefaultSpeedAndGearTimeout = 8;

        public const double DefaultBrakeNotRide = 0;

        public const double DefaultGlobalChangeLanesAngle = 6;
        /// <summary>
        /// 一档最大速度限制
        /// </summary>
        public const int DefaultGearOneMaxSpeed = 20;
        /// <summary>
        /// 二档最小速度限制
        /// </summary>
        public const int DefaultGearTwoMinSpeed = 15;
        /// <summary>
        /// 二档最大速度限制
        /// </summary>
        public const int DefaultGearTwoMaxSpeed = 30;
        /// <summary>
        /// 三档最小速度限制
        /// </summary>
        public const int DefaultGearThreeMinSpeed = 25;
        /// <summary>
        /// 三档最大速度限制
        /// </summary>
        public const int DefaultGearThreeMaxSpeed = 40;
        /// <summary>
        /// 四档最小速度限制
        /// </summary>
        public const int DefaultGearFourMinSpeed = 35;
        /// <summary>
        /// 四档最大速度限制
        /// </summary>
        public const int DefaultGearFourMaxSpeed = 50;
        /// <summary>
        /// 五档最小速度限制
        /// </summary>
        public const int DefaultGearFiveMinSpeed = 45;
        /// <summary>
        /// 五档最大速度限制
        /// </summary>
        public const int DefaultGearFiveMaxSpeed = 100;
        #endregion

        #region 灯光检测
        /// <summary>
        /// 夜间灯光双闪检测距离
        /// </summary>
        public const int DefaultLowAndHighBeamDistance = 45;
        /// <summary>
        /// 转向等开启时间限制，30秒
        /// </summary>
        public const int DefaultIndicatorLightTimeout = 30;
        /// <summary>
        /// 打转向灯提前（秒）
        /// </summary>
        public const double DefaultTurnLightAheadOfTime = 2.5;
        #endregion
        #endregion

        #region 成绩打印

        public const string DefaultReportSchoolName = "专业陪练";
        public const string DefaultReportTecherName = "两极软件";
        public const string DefaultReportTecherTel = "023-62645513";

        #endregion
        #region 新增版本、OBD、方向角度 来源参数设置

        /// <summary>
        /// 主控箱版本 默认是串口版本
        /// </summary>
        public const MasterControlBoxVersion DefultMasterControlBoxVersion = MasterControlBoxVersion.SimulatedData;

        /// <summary>
        /// OBD选择 接主控箱还是接平板
        /// </summary>
        public const OBDSource DeafultOBDSource = OBDSource.MasterControlBox;


        public const MileageSource DefaultMilegeSource = MileageSource.GPS;

        /// <summary>
        /// 默认OBD接平板使用COM2 口
        /// </summary>
        public const string DefaultOBDSignalSource = "COM2";

        /// <summary>
        ///默认外置陀螺仪接平板 使用 COM3口
        /// </summary>
        public const string DefaultAngleSignalSource = "COM3";


        public const string SensorProvidersIP = "192.168.1.2";
        public const int SensorProvidersPort = 4001;

       // public const string GpsProvidersIP = "127.0.0.1";
        public const string GpsProvidersIP = "192.168.1.2";
        public const int GpsProvidersPort = 4002;

        public const string OBDProvidersIP = "192.168.1.2";
        public const int OBDProvidersPort = 4003;

        #endregion

        #region 综合评判

        public const bool DefaultCommonExamItemsCheckEngineStall = true;
        public const bool DefaultCommonExamItemsCheckSafeBelt = true;
        public const double DefaultCommonExamItemsSareBeltTimeOut = 0.5;
        public const double DefaultCommonExamItemsEngineTimeOut = 0.5;
        //单位毫秒
        public const double DefaultCommonExamItemsMaxSpeedKeepTime = 1000;

        public const bool DefaultCommonExamItemsCheckChangeLanes = false;
        public const double DefaultCommonExamItemsChangeLanesTimeOut = 1;
        public const double DefaultCommonExamItemsChangeLanesAngle = 6;

        #endregion


        public const bool DefaultIsPlaybackToOriginalLane = false;
        public const int  DefaultReturnToOriginalLaneDistince = 90;


    }
}
