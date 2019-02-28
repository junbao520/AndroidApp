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
    [Activity(Label = "ItemSetting")]
    public class ItemSetting : BaseSettingActivity
    {

        #region 变量定义
        //

        #region 上车准备
        EditText edtTxtPrepareDrivingVoice;
        // 绕车一周最大时间
        EditText edtTxtAroundCarTimeout;
        RadioButton radDoor;
        //
        RadioButton radSafeblet;
        // 绕车一周是否启用
        CheckBox chkAroundCarEnable;
        // 上车准备是否启用
        CheckBox chkPrepareDrivingEnable;
        //
        CheckBox chkAroundCarVoice;
        // 3摸语音
        CheckBox chkPrepareDriving3TouchVoice;
        // 车尾
        CheckBox chkPrepareDrivingTailStockEnable;
        // 车头
        CheckBox chkPrepareDrivingHeadStockEnable;
        #endregion


        #region 灯光模拟
        EditText edtTxtLightVoice;
        // 灯光模拟，每个语音间隔
        EditText edtTxtSimulationLightTimeout;
        // 灯光模拟，时间间隔：单位：秒
        EditText edtTxtSimulationLightInterval;
        // 白天灯光模拟
        CheckBox chkSimulationsLightOnDay;
        // 夜间灯光模拟
        CheckBox chkSimulationsLightOnNight;
        #endregion

        #region 起步
        EditText edtTxtStartVoice;
        //
        EditText edtTxtStartEndVoice;
        // 起步项目距离，单位：米
        EditText edtTxtStartStopCheckForwardDistance;
        // 起步时间（单位：秒）
        EditText edtTxtStartTimeout;
        //起步喇叭检测
        CheckBox chkStartLoudSpeakerCheck;
        // 起步检测白考左转向灯
        CheckBox chkIsCheckStartLight;
        // 起步夜间检测远近光交替
        CheckBox chkStartLowAndHighBeamInNight;
        // 起步闯动检测
        CheckBox chkStartShockEnable;
        // 不松手刹最小时间（单位：秒）
        EditText edtTxtStartReleaseHandbrakeTimeout;
        // 起步时发动机最高转速（配置0，不评判）
        EditText edtTxtStartEngineRpm;
        // 起步闯动值
        EditText edtTxtStartShockValue;
        #endregion

        #region 靠边停车
        // 起步闯动次数
        EditText edtTxtStartShockCount;
        // 靠边停车项目语音
        EditText edtTxtPullOverVoice;
        // 靠边停车-最大行驶距离（米）
        EditText edtTxtPullOverMaxDrivingDistance;
        // 靠边停车-停车后未拉手刹超时（单位：秒）
        EditText edtTxtPullOverHandbrakeTimeout;
        //靠边停车之后 开光车门时间
        EditText edtTxtPullOverDoorTimeout;
        // 靠边停车-检测转向灯
        CheckBox chkPullOverTurnLightCheck;
        // 靠边停车-下车前开启警报灯
        CheckBox chkPullOverOpenCautionBeforeGetOff;
        // 靠边停车-停车后松开安全带
        CheckBox chkPullOverOpenSafetyBeltBeforeGetOff;
        // 停车空挡检测
        CheckBox chkPullOverNeutralCheck;
        // 靠边停车-停车后发动机熄火检测
        CheckBox chkPullOverStopEngineBeforeGetOff;
        // 靠边停车-下车前是否关闭大灯
        CheckBox chkPullOverCloseLowBeamBeforeGetOff;
        // 靠边停车-结束后自动触发结束考试
        CheckBox chkPulloverEndAutoTriggerStopExam;
        //停车标志手刹
        RadioButton radStopFlagHandBreak;
        //停车标志车停
        RadioButton radStopFlagCarStop;
        RadioButton radPullOverCautionLight;
        RadioButton radPullOverLowBeamCheck;
        RadioButton radPullOverEngineExtinction;
        RadioButton radPullOverSafetyBelt;
        RadioButton radPullOverOpenCloseDoor;
        RadioButton radPullOverHandbrake;
        #endregion

        #region 直线行驶
        //直线行驶开始语音
        EditText edtTxtStraightDrivingVoice;
        //直线行驶结束语音
        EditText edtTxtStraightDrivingEndVoice;
        //直线行驶项目距离
        EditText edtTxtStraightDrivingDistance;
        //直线行驶角度
        EditText edtTxtStraightDrivingMaxOffsetAngle;
        //直线行驶最高速度
        EditText edtTxtStraightDrivingSpeedMaxLimit;
        //直线行驶最低速度
        EditText edtTxtStraightDrivingSpeedMinLimit;
        //直线行驶达到一次速度
        EditText edtTxtStraightDrivingReachSpeed;
        //直线行驶准备距离
        EditText edtTxtStraightDrivingPrepareDistance;
        #endregion

        #region 人行横道
        // 人行横道项目语音
        EditText edtTxtPedestrianCrossingVoice;
        // 人行横道项目结束语音
        EditText edtTxtPedestrianCrossingEndVoice;
        // 人行横道项目距离
        EditText edtTxtPedestrianCrossingDistance;
        // 人行横道速度限制
        EditText edtTxtPedestrianCrossingSpeedLimit;
        // 人行横道要求踩刹车速度限制
        EditText edtTxtPedestrianCrossingBrakeSpeedUp;
        // 人行横道必须踩刹车
        CheckBox chkPedestrianCrossingBrakeRequire;
        // 人行横道远近光交替灯光检测
        CheckBox chkPedestrianCrossingLightCheck;
        // 人行横道白考喇叭检测
        CheckBox chkPedestrianCrossingLoudSpeakerDayCheck;
        // 人行横道夜考喇叭检测
        #endregion


        #region 公交车站
        CheckBox chkPedestrianCrossingLoudSpeakerNightCheck;
        // 公交车站项目语音
        EditText edtTxtBusAreaVoice;
        // 公交区域结束语音
        EditText edtTxtBusAreaEndVoice;
        // 公交车站项目距离
        EditText edtTxtBusAreaDistance;
        // 公交车站速度限制
        EditText edtTxtBusAreaSpeedLimit;
        // 公交车站要求踩刹车速度限制
        EditText edtTxtBusAreaBrakeSpeedUp;
        // 公交车站必须踩刹车
        CheckBox chkBusAreaBrakeRequire;
        // 公交车站远近光交替
        CheckBox chkBusAreaLightCheck;
        // 公交车站白考喇叭检测
        CheckBox chkBusAreaLoudSpeakerDayCheck;
        // 公交车站夜考喇叭检测
        CheckBox chkBusAreaLoudSpeakerNightCheck;
        #endregion

        #region 学校区域
        // 学校区域项目语音
        EditText edtTxtSchoolAreaVoice;
        // 学习区域结束语音
        EditText edtTxtSchoolAreaEndVoice;
        // 学校区域项目距离
        EditText edtTxtSchoolAreaDistance;
        // 学校区域速度限制
        EditText edtTxtSchoolAreaSpeedLimit;
        // 学校区域要求踩刹车速度限制
        EditText edtTxtSchoolAreaBrakeSpeedUp;
        #endregion

        #region 路口直行
        // 路口直行项目语音
        EditText edtTxtStraightThroughIntersectionVoice;
        // 路口直行项目结束语音
        EditText edtTxtStraightThroughIntersectionEndVoice;
        // 路口直行项目距离
        EditText edtTxtStraightThroughIntersectionDistance;
        // 路口直行速度限制
        EditText edtTxtStraightThroughIntersectionSpeedLimit;
        // 路口直行要求踩刹车速度限制
        EditText edtTxtStraightThroughIntersectionBrakeSpeedUp;
        // 路口直行必须踩刹车
        CheckBox chkStraightThroughIntersectionBrakeRequire;
        // 路口直行夜考远近光交替
        CheckBox chkStraightThroughIntersectionLightCheck;
        // 路口直行白考喇叭检测
        CheckBox chkStraightThroughIntersectionLoudSpeakerDayCheck;
        // 路口直行夜考喇叭检测
        CheckBox chkStraightThroughIntersectionLoudSpeakerNightCheck;
        #endregion

        #region 路口左转

        // 路口左转项目语音
        EditText edtTxtTurnLeftVoice;
        // 路口右转项目语音
        EditText edtTxtTurnLeftEndVoice;
        // 路口左转项目距离
        EditText edtTxtTurnLeftDistance;
        // 路口左转速度限制
        EditText edtTxtTurnLeftSpeedLimit;
        // 路口左转要求踩刹车速度限制
        EditText edtTxtTurnLeftBrakeSpeedUp;
        // 路口左转必须踩刹车
        CheckBox chkTurnLeftBrakeRequire;
        //路口左转专项灯检测
        CheckBox chkTurnLeftLightCheck;
        // 路口左转白考喇叭检测
        CheckBox chkTurnLeftLoudSpeakerDayCheck;
        // 路口左转夜考喇叭检测
        CheckBox chkTurnLeftLoudSpeakerNightCheck;
        #endregion

        #region 路口右转
        // 路口右转项目语音
        EditText edtTxtTurnRightVoice;
        // 路口右转结束语音
        EditText edtTxtTurnRightEndVoice;
        // 路口右转项目距离
        EditText edtTxtTurnRightDistance;
        // 路口右转速度限制
        EditText edtTxtTurnRightSpeedLimit;
        // 路口右转要求踩刹车速度限制
        EditText edtTxtTurnRightBrakeSpeedUp;
        // 路口右转必须踩刹车
        CheckBox chkTurnRightBrakeRequire;
        //路口右转专项灯检测
        CheckBox chkTurnRightLightCheck;
        // 路口右转白考喇叭检测
        CheckBox chkTurnRightLoudSpeakerDayCheck;
        // 路口右转夜考喇叭检测
        CheckBox chkTurnRightLoudSpeakerNightCheck;
        #endregion

        #region 掉头
        // 掉头项目语音 
        EditText edtTxtTurnRoundVoice;
        // 掉头项目结束语音 
        EditText edtTxtTurnRoundEndVoice;
        // 掉头所用最大距离限制（单位：米）
        EditText edtTxtTurnRoundMaxDistance;
        // 掉头转向角度差确认开始掉头（单位：度）
        EditText edtTxtTurnRoundStartAngleDiff;
        // 掉头结束掉头转向角度差（单位：度）
        EditText edtTxtTurnRoundEndAngleDiff;
        // 掉头必踩刹车
        CheckBox chkTurnRoundBrakeRequired;
        // 掉头夜间远近光交替检查
        CheckBox chkTurnRoundLightCheck;
        // 掉头白考喇叭检测 
        CheckBox chkTurnRoundLoudSpeakerDayCheck;
        // 掉头夜考喇叭检测 
        CheckBox chkTurnRoundLoudSpeakerNightCheck;
        #endregion

        #region 会车
        // 会车语音
        EditText edtTxtMeetingVoice;
        // 会车结束语音
        EditText edtTxtMeetingEndVoice;
        // 会车会车距离
        EditText edtTxtMeetingDrivingDistance;
        // 会车速度限制
        EditText edtTxtMeetingSlowSpeedInKmh;
        // 会车向右避让角度
        EditText edtTxtMeetingAngle;
        // 会车刹车
        CheckBox chkMeetingCheckBrake;
        // 会车禁止远光
        CheckBox chkMeetingForbidHighBeamCheck;
        #endregion

        #region 急弯坡路
        // 急坡弯路项目语音
        EditText edtTxtSharpTurnVoice;
        // 急弯破路结束语音
        EditText edtTxtSharpTurnEndVoice;
        // 急坡弯路项目距离
        EditText edtTxtSharpTurnDistance;
        // 速度限制
        EditText edtTxtSharpTurnSpeedLimit;
        // 必须踩刹车
        CheckBox chkSharpTurnBrake;
        // 急坡弯路夜间远近光交替检测
        CheckBox chkSharpTurnLightCheck;
        // 急坡弯路白考喇叭
        CheckBox chkSharpTurnLoudspeakerInDay;
        // 急坡弯路夜考喇叭
        CheckBox chkSharpTurnLoudspeakerInNight;
        #endregion

        #region 变更车道
        // 变更车道项目语音
        EditText edtTxtChangeLanesVoice;
        // 变更结束车道
        EditText edtTxtChangeLanesEndVoice;
        // 变道最大距离（单位：米）
        EditText edtTxtChangeLanesMaxDistance;
        // 变道超时时间（单位：秒）
        EditText edtTxtChangeLanesTimeout;
        // 变道转向角度
        EditText edtTxtChangeLanesAngle;
        // 变更车道夜间远近光交替
        CheckBox chkChangeLanesLowAndHighBeamCheck;
        // 变更车道灯光检测
        CheckBox chkChangeLanesLightCheck;
        // 变道准备距离
        EditText edtTxtChangeLanesPrepareDistance;
        #endregion

        #region 环岛
        // 环岛项目语音
        EditText edtTxtRoundaboutVoice;
        // 环岛项目结束语音
        EditText edtTxtRoundaboutEndVoice;
        // 环岛项目距离
        EditText edtTxtRoundaboutDistance;
        // 环岛默认环岛灯光检测
        CheckBox chkRoundaboutLightCheck;
        #endregion

        #region 超车
        // 超车语音
        EditText edtTxtOvertakeVoice;
        // 超车结束语音
        EditText edtTxtOvertakeEndVoice;
        // 超车最大距离（单位：米）
        EditText edtTxtOvertakeMaxDistance;
        // 超车超时时间（单位：秒）
        EditText edtTxtOvertakeTimeout;
        // 变道转向角度
        EditText edtTxtOvertakeChangeLanesAngle;
        // 超车最低速度
        EditText edtTxtOvertakeLowestSpeed;
        // 超车速度限制
        EditText edtTxtOvertakeSpeedLimit;
        // 超车速度范围
        EditText edtTxtOvertakeSpeedRange;
        // 超车是否返回原车道
        EditText edtTxtOvertakeBackToOriginalLaneVoice;
        // 超车是否返回原车道
        EditText edtTxtOvertakeBackToOriginalLaneDistance;

        //超车准备距离
        EditText edtTxtOverTakePrepareDistance;
        // 超车夜间远近光交替
        CheckBox chkOvertakeLowAndHighBeamCheck;
        // 超车转向灯灯光检测
        CheckBox chkOvertakeLightCheck;
        // 超车白考喇叭检测
        CheckBox chkOvertakeLoudSpeakerDayCheck;
        // 超车夜考喇叭检测
        CheckBox chkOvertakeLoudSpeakerNightCheck;
        // 超车是否返回原车道
        CheckBox chkOvertakeBackToOriginalLane;
        #endregion

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

        #region 综合评判
        CheckBox chkBreakVoice;
        EditText edtTxtExamSuccess;
        EditText edtTxtExamFailed;
        #endregion
        //综合评判

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ItemSetting);

            InitControl();
            initHeader();
            ActivityName = this.GetString(Resource.String.ItemSettingStr);
            setMyTitle(ActivityName);
            InitSetting();
        }


        public string GetItemVoice(string ItemCode, bool IsStart = true)
        {

            var ExamItem = dataService.AllExamItems.Where(s => s.ItemCode == ItemCode).FirstOrDefault();


            if (ExamItem == null)
            {
                return string.Empty;
            }
            var VoiceText = IsStart ? ExamItem.VoiceText : ExamItem.EndVoiceText;

            return VoiceText;
        }

        public override void InitSetting()
        {
            #region 项目语音
            edtTxtPrepareDrivingVoice.Text = GetItemVoice(ExamItemCodes.PrepareDriving, true);
            edtTxtLightVoice.Text = GetItemVoice(ExamItemCodes.Light, true);

            edtTxtStartVoice.Text = GetItemVoice(ExamItemCodes.Start, true);
            edtTxtStartEndVoice.Text = GetItemVoice(ExamItemCodes.Start, false);
            edtTxtPullOverVoice.Text = GetItemVoice(ExamItemCodes.PullOver, true);

            edtTxtStraightDrivingVoice.Text = GetItemVoice(ExamItemCodes.StraightDriving, true);
            edtTxtStraightDrivingEndVoice.Text = GetItemVoice(ExamItemCodes.StraightDriving, false);

            edtTxtPedestrianCrossingVoice.Text = GetItemVoice(ExamItemCodes.PedestrianCrossing, true);
            edtTxtPedestrianCrossingEndVoice.Text = GetItemVoice(ExamItemCodes.PedestrianCrossing, false);


            edtTxtBusAreaVoice.Text = GetItemVoice(ExamItemCodes.BusArea, true);
            edtTxtBusAreaEndVoice.Text = GetItemVoice(ExamItemCodes.BusArea, false);
            edtTxtSchoolAreaVoice.Text = GetItemVoice(ExamItemCodes.SchoolArea, true);
            edtTxtSchoolAreaEndVoice.Text = GetItemVoice(ExamItemCodes.SchoolArea, false);
            edtTxtStraightThroughIntersectionVoice.Text = GetItemVoice(ExamItemCodes.StraightThrough, true);
            edtTxtStraightThroughIntersectionEndVoice.Text = GetItemVoice(ExamItemCodes.StraightThrough, false);
            edtTxtTurnLeftVoice.Text = GetItemVoice(ExamItemCodes.TurnLeft, true);
            edtTxtTurnLeftEndVoice.Text = GetItemVoice(ExamItemCodes.TurnLeft, false);

            edtTxtTurnRightVoice.Text = GetItemVoice(ExamItemCodes.TurnRight, true);
            edtTxtTurnRightEndVoice.Text = GetItemVoice(ExamItemCodes.TurnRight, false);

            edtTxtTurnRoundVoice.Text = GetItemVoice(ExamItemCodes.TurnRound, true);
            edtTxtTurnRoundEndVoice.Text = GetItemVoice(ExamItemCodes.TurnRound, false);

            edtTxtMeetingVoice.Text = GetItemVoice(ExamItemCodes.Meeting, true);
            edtTxtMeetingEndVoice.Text = GetItemVoice(ExamItemCodes.Meeting, false);

            edtTxtSharpTurnVoice.Text = GetItemVoice(ExamItemCodes.SharpTurn, true);
            edtTxtSharpTurnEndVoice.Text = GetItemVoice(ExamItemCodes.SharpTurn, false);

            edtTxtChangeLanesVoice.Text = GetItemVoice(ExamItemCodes.ChangeLanes, true);
            edtTxtChangeLanesEndVoice.Text = GetItemVoice(ExamItemCodes.ChangeLanes, false);

            edtTxtRoundaboutVoice.Text = GetItemVoice(ExamItemCodes.Roundabout, true); ;
            edtTxtRoundaboutEndVoice.Text = GetItemVoice(ExamItemCodes.Roundabout, false); ;

            edtTxtOvertakeVoice.Text = GetItemVoice(ExamItemCodes.Overtaking, true); ;
            edtTxtOvertakeEndVoice.Text = GetItemVoice(ExamItemCodes.Overtaking, false); ;

            edtTxtModifiedGearVoice.Text = GetItemVoice(ExamItemCodes.ModifiedGear, true); ;
            edtTxtModifiedGearEndVoice.Text = GetItemVoice(ExamItemCodes.ModifiedGear, false); ;
            #endregion

            #region 上车准备
            edtTxtAroundCarTimeout.Text = Settings.AroundCarTimeout.ToString();
            radSafeblet.Checked = Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.SafeBelt;
            radDoor.Checked = Settings.PrepareDrivingEndFlag == Infrastructure.PrepareDrivingEndFlag.Door;
            chkAroundCarEnable.Checked = Settings.AroundCarEnable;
            chkPrepareDrivingEnable.Checked = Settings.PrepareDrivingEnable;
            chkAroundCarVoice.Checked = Settings.AroundCarVoiceEnable;
            chkPrepareDriving3TouchVoice.Checked = Settings.PrepareDriving3TouchVoice;
            chkPrepareDrivingTailStockEnable.Checked = Settings.PrepareDrivingTailStockEnable;
            chkPrepareDrivingHeadStockEnable.Checked = Settings.PrepareDrivingHeadStockEnable;
            #endregion

            #region 灯光模拟
            edtTxtSimulationLightTimeout.Text = Settings.SimulationLightTimeout.ToString();
            edtTxtSimulationLightInterval.Text = Settings.SimulationLightInterval.ToString();
            chkSimulationsLightOnDay.Checked = Settings.SimulationsLightOnDay;
            chkSimulationsLightOnNight.Checked = Settings.SimulationsLightOnNight;
            #endregion

            #region 起步

            edtTxtStartStopCheckForwardDistance.Text = Settings.StartStopCheckForwardDistance.ToString();
            edtTxtStartTimeout.Text = Settings.StartTimeout.ToString();
            chkIsCheckStartLight.Checked = Settings.IsCheckStartLight;
            chkStartLowAndHighBeamInNight.Checked = Settings.StartLowAndHighBeamInNight;
            chkStartShockEnable.Checked = Settings.StartShockEnable;
            edtTxtStartReleaseHandbrakeTimeout.Text = Settings.StartReleaseHandbrakeTimeout.ToString();
            edtTxtStartEngineRpm.Text = Settings.StartEngineRpm.ToString();
            edtTxtStartShockValue.Text = Settings.StartShockValue.ToString();
            edtTxtStartShockCount.Text = Settings.StartShockCount.ToString();
            chkStartLoudSpeakerCheck.Checked = Settings.VehicleStartingLoudSpeakerDayCheck;

            #endregion

            #region 靠边停车
            edtTxtPullOverMaxDrivingDistance.Text = Settings.PullOverMaxDrivingDistance.ToString();
            edtTxtPullOverHandbrakeTimeout.Text = Settings.PullOverHandbrakeTimeout.ToString();
            edtTxtPullOverDoorTimeout.Text = Settings.PullOverDoorTimeout.ToString();
            chkPullOverTurnLightCheck.Checked = Settings.PullOverTurnLightCheck;
            chkPullOverOpenCautionBeforeGetOff.Checked = Settings.PullOverOpenCautionBeforeGetOff;
            chkPullOverOpenSafetyBeltBeforeGetOff.Checked = Settings.PullOverOpenSafetyBeltBeforeGetOff;
            chkPullOverNeutralCheck.Checked = Settings.PullOverNeutralCheck;
            chkPullOverStopEngineBeforeGetOff.Checked = Settings.PullOverStopEngineBeforeGetOff;
            chkPullOverCloseLowBeamBeforeGetOff.Checked = Settings.PullOverCloseLowBeamBeforeGetOff;
            chkPulloverEndAutoTriggerStopExam.Checked = Settings.PulloverEndAutoTriggerStopExam;

            radStopFlagHandBreak.Checked = Settings.PullOverMark == PullOverMark.Handbrake;
            radStopFlagCarStop.Checked = Settings.PullOverMark == PullOverMark.CarStop;

            radPullOverCautionLight.Checked = Settings.PullOverEndMark == PullOverEndMark.CautionLightCheck;
            radPullOverEngineExtinction.Checked = Settings.PullOverEndMark == PullOverEndMark.EngineExtinctionCheck;
            radPullOverHandbrake.Checked = Settings.PullOverEndMark == PullOverEndMark.Handbrake;
            radPullOverLowBeamCheck.Checked = Settings.PullOverEndMark == PullOverEndMark.LowBeamCheck;
            radPullOverOpenCloseDoor.Checked = Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck;
            radPullOverSafetyBelt.Checked = Settings.PullOverEndMark == PullOverEndMark.SafetyBeltCheck;


            #endregion

            #region 直线行驶


            edtTxtStraightDrivingMaxOffsetAngle.Text = Settings.StraightDrivingMaxOffsetAngle.ToString();
            edtTxtStraightDrivingDistance.Text = Settings.StraightDrivingDistance.ToString();
            edtTxtStraightDrivingPrepareDistance.Text = Settings.StraightDrivingPrepareDistance.ToString();
            edtTxtStraightDrivingReachSpeed.Text = Settings.StraightDrivingReachSpeed.ToString();
            edtTxtStraightDrivingSpeedMaxLimit.Text = Settings.StraightDrivingSpeedMaxLimit.ToString();
            edtTxtStraightDrivingSpeedMinLimit.Text = Settings.StraightDrivingSpeedMinLimit.ToString();

            #endregion

            #region 人行横道
            edtTxtPedestrianCrossingDistance.Text = Settings.PedestrianCrossingDistance.ToString();
            edtTxtPedestrianCrossingSpeedLimit.Text = Settings.PedestrianCrossingSpeedLimit.ToString();
            edtTxtPedestrianCrossingBrakeSpeedUp.Text = Settings.PedestrianCrossingBrakeSpeedUp.ToString();
            chkPedestrianCrossingBrakeRequire.Checked = Settings.PedestrianCrossingBrakeRequire;
            chkPedestrianCrossingLightCheck.Checked = Settings.PedestrianCrossingLightCheck;
            chkPedestrianCrossingLoudSpeakerDayCheck.Checked = Settings.PedestrianCrossingLoudSpeakerDayCheck;
            chkPedestrianCrossingLoudSpeakerNightCheck.Checked = Settings.PedestrianCrossingLoudSpeakerNightCheck;
            #endregion

            #region 公交汽车
            edtTxtBusAreaDistance.Text = Settings.BusAreaDistance.ToString();
            edtTxtBusAreaSpeedLimit.Text = Settings.BusAreaSpeedLimit.ToString();
            edtTxtBusAreaBrakeSpeedUp.Text = Settings.BusAreaBrakeSpeedUp.ToString();
            chkBusAreaBrakeRequire.Checked = Settings.BusAreaBrakeRequire;
            chkBusAreaLightCheck.Checked = Settings.BusAreaLightCheck;
            chkBusAreaLoudSpeakerDayCheck.Checked = Settings.BusAreaLoudSpeakerDayCheck;
            chkBusAreaLoudSpeakerNightCheck.Checked = Settings.BusAreaLoudSpeakerNightCheck;
            #endregion

            #region 学校区域
            edtTxtSchoolAreaDistance.Text = Settings.SchoolAreaDistance.ToString();
            edtTxtSchoolAreaSpeedLimit.Text = Settings.SchoolAreaSpeedLimit.ToString();
            edtTxtSchoolAreaBrakeSpeedUp.Text = Settings.SchoolAreaBrakeSpeedUp.ToString();
            #endregion

            #region 路口直行
            edtTxtStraightThroughIntersectionDistance.Text = Settings.StraightThroughIntersectionDistance.ToString();
            edtTxtStraightThroughIntersectionSpeedLimit.Text = Settings.StraightThroughIntersectionSpeedLimit.ToString();
            edtTxtStraightThroughIntersectionBrakeSpeedUp.Text = Settings.StraightThroughIntersectionBrakeSpeedUp.ToString();
            chkStraightThroughIntersectionBrakeRequire.Checked = Settings.StraightThroughIntersectionBrakeRequire;
            chkStraightThroughIntersectionLightCheck.Checked = Settings.StraightThroughIntersectionLightCheck;
            chkStraightThroughIntersectionLoudSpeakerDayCheck.Checked = Settings.StraightThroughIntersectionLoudSpeakerDayCheck;
            chkStraightThroughIntersectionLoudSpeakerNightCheck.Checked = Settings.StraightThroughIntersectionLoudSpeakerNightCheck;
            #endregion


            #region 路口左转
            edtTxtTurnLeftDistance.Text = Settings.TurnLeftDistance.ToString();
            edtTxtTurnLeftSpeedLimit.Text = Settings.TurnLeftSpeedLimit.ToString();
            edtTxtTurnLeftBrakeSpeedUp.Text = Settings.TurnLeftBrakeSpeedUp.ToString();
            chkTurnLeftBrakeRequire.Checked = Settings.TurnLeftBrakeRequire;
            chkTurnLeftLightCheck.Checked = Settings.TurnLeftLightCheck;
            chkTurnLeftLoudSpeakerDayCheck.Checked = Settings.TurnLeftLoudSpeakerDayCheck;
            chkTurnLeftLoudSpeakerNightCheck.Checked = Settings.TurnLeftLoudSpeakerNightCheck;

            #endregion

            #region 路口右转
            edtTxtTurnRightDistance.Text = Settings.TurnRightDistance.ToString();
            edtTxtTurnRightSpeedLimit.Text = Settings.TurnRightSpeedLimit.ToString();
            edtTxtTurnRightBrakeSpeedUp.Text = Settings.TurnRightBrakeSpeedUp.ToString();
            chkTurnRightBrakeRequire.Checked = Settings.TurnRightBrakeRequire;

            chkTurnRightLightCheck.Checked = Settings.TurnRightLightCheck;
            chkTurnRightLoudSpeakerDayCheck.Checked = Settings.TurnRightLoudSpeakerDayCheck;
            chkTurnRightLoudSpeakerNightCheck.Checked = Settings.TurnRightLoudSpeakerNightCheck;
            #endregion


            #region 掉头
            edtTxtTurnRoundMaxDistance.Text = Settings.TurnRoundMaxDistance.ToString();
            edtTxtTurnRoundStartAngleDiff.Text = Settings.TurnRoundStartAngleDiff.ToString();
            edtTxtTurnRoundEndAngleDiff.Text = Settings.TurnRoundEndAngleDiff.ToString();
            chkTurnRoundBrakeRequired.Checked = Settings.TurnRoundBrakeRequired;
            chkTurnRoundLightCheck.Checked = Settings.TurnRoundLightCheck;
            chkTurnRoundLoudSpeakerDayCheck.Checked = Settings.TurnRoundLoudSpeakerDayCheck;
            chkTurnRoundLoudSpeakerNightCheck.Checked = Settings.TurnRoundLoudSpeakerNightCheck;

            #endregion

            #region 会车
            edtTxtMeetingDrivingDistance.Text = Settings.MeetingDrivingDistance.ToString();
            edtTxtMeetingSlowSpeedInKmh.Text = Settings.MeetingSlowSpeedInKmh.ToString();
            edtTxtMeetingAngle.Text = Settings.MeetingAngle.ToString();
            chkMeetingCheckBrake.Checked = Settings.MeetingCheckBrake;
            chkMeetingForbidHighBeamCheck.Checked = Settings.MeetingForbidHighBeamCheck;
            #endregion

            #region 急弯破路
            edtTxtSharpTurnDistance.Text = Settings.SharpTurnDistance.ToString();
            edtTxtSharpTurnSpeedLimit.Text = Settings.SharpTurnSpeedLimit.ToString();
            chkSharpTurnBrake.Checked = Settings.SharpTurnBrake;
            chkSharpTurnLightCheck.Checked = Settings.SharpTurnLightCheck;
            chkSharpTurnLoudspeakerInDay.Checked = Settings.SharpTurnLoudspeakerInDay;
            chkSharpTurnLoudspeakerInNight.Checked = Settings.SharpTurnLoudspeakerInNight;
            #endregion

            #region 变道
            edtTxtChangeLanesMaxDistance.Text = Settings.ChangeLanesMaxDistance.ToString();
            edtTxtChangeLanesTimeout.Text = Settings.ChangeLanesTimeout.ToString();
            edtTxtChangeLanesAngle.Text = Settings.ChangeLanesAngle.ToString();
            chkChangeLanesLowAndHighBeamCheck.Checked = Settings.ChangeLanesLowAndHighBeamCheck;
            chkChangeLanesLightCheck.Checked = Settings.ChangeLanesLightCheck;
            edtTxtChangeLanesPrepareDistance.Text = Settings.ChangeLanesPrepareDistance.ToString();
            #endregion

            #region 环岛
            edtTxtRoundaboutDistance.Text = Settings.RoundaboutDistance.ToString();
            chkRoundaboutLightCheck.Checked = Settings.RoundaboutLightCheck;
            #endregion

            #region 超车
            edtTxtOvertakeMaxDistance.Text = Settings.OvertakeMaxDistance.ToString();
            edtTxtOvertakeTimeout.Text = Settings.OvertakeTimeout.ToString();
            edtTxtOvertakeChangeLanesAngle.Text = Settings.OvertakeChangeLanesAngle.ToString();
            edtTxtOvertakeLowestSpeed.Text = Settings.OvertakeLowestSpeed.ToString();
            edtTxtOvertakeSpeedLimit.Text = Settings.OvertakeSpeedLimit.ToString();
            edtTxtOvertakeSpeedRange.Text = Settings.OvertakeSpeedRange.ToString();
            edtTxtOvertakeBackToOriginalLaneVoice.Text = Settings.OvertakeBackToOriginalLaneVocie.ToString();
            edtTxtOvertakeBackToOriginalLaneDistance.Text = Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance.ToString();
            chkOvertakeLowAndHighBeamCheck.Checked = Settings.OvertakeLowAndHighBeamCheck;
            chkOvertakeLightCheck.Checked = Settings.OvertakeLightCheck;
            chkOvertakeLoudSpeakerDayCheck.Checked = Settings.OvertakeLoudSpeakerDayCheck;
            chkOvertakeLoudSpeakerNightCheck.Checked = Settings.OvertakeLoudSpeakerNightCheck;
            chkOvertakeBackToOriginalLane.Checked = Settings.OvertakeBackToOriginalLane;

            edtTxtOverTakePrepareDistance.Text = Settings.OvertakePrepareDistance.ToString();
            #endregion


            #region 加减档位
            edtTxtModifiedGearDrivingDistance.Text = Settings.ModifiedGearDrivingDistance.ToString();
            edtTxtModifiedGearTimeout.Text = Settings.ModifiedGearTimeout.ToString();
            edtTxtModifiedGearIgnoreSeconds.Text = Settings.ModifiedGearIgnoreSeconds.ToString();
            chkModifiedGearIsPlayGearVoice.Checked = Settings.ModifiedGearIsPlayGearVoice;
            chkModifiedGearIsPlayActionVoice.Checked = Settings.ModifiedGearIsPlayActionVoice;

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

            chkBreakVoice.Checked = Settings.CommonExamItemsBreakVoice;
            edtTxtExamFailed.Text = Settings.CommonExamItemExamFailVoice;
            edtTxtExamSuccess.Text = Settings.CommonExamItemExamSuccessVoice;
            ////1-2,2-3,3-4,4-5,5-4,4-3,3-2,2-1

            if (string.IsNullOrEmpty(Settings.ModifiedGearFlowVoice))
            {
                Settings.ModifiedGearFlowVoice = DefaultGlobalSettings.DefaultModifiedGearFlowVoice;
                // Logger.Error("ItemSetting"+ Settings.ModifiedGearFlowVoice);
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
            #region 上车准备
            chkAroundCarEnable = FindViewById<CheckBox>(Resource.Id.chkAroundCarEnable);
            edtTxtPrepareDrivingVoice = FindViewById<EditText>(Resource.Id.edtTxtPrepareDrivingVoice);
            edtTxtAroundCarTimeout = FindViewById<EditText>(Resource.Id.edtTxtAroundCarTimeout);
            radDoor = FindViewById<RadioButton>(Resource.Id.radDoor);
            radSafeblet = FindViewById<RadioButton>(Resource.Id.radSafeblet);
            chkAroundCarEnable = FindViewById<CheckBox>(Resource.Id.chkAroundCarEnable);
            chkPrepareDrivingEnable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingEnable);
            chkAroundCarVoice = FindViewById<CheckBox>(Resource.Id.chkAroundCarVoice);
            chkPrepareDriving3TouchVoice = FindViewById<CheckBox>(Resource.Id.chkPrepareDriving3TouchVoice);
            chkPrepareDrivingTailStockEnable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingTailStockEnable);
            chkPrepareDrivingHeadStockEnable = FindViewById<CheckBox>(Resource.Id.chkPrepareDrivingHeadStockEnable);
            #endregion

            #region 灯光模拟
            edtTxtLightVoice = FindViewById<EditText>(Resource.Id.edtTxtLightVoice);
            edtTxtSimulationLightTimeout = FindViewById<EditText>(Resource.Id.edtTxtSimulationLightTimeout);
            edtTxtSimulationLightInterval = FindViewById<EditText>(Resource.Id.edtTxtSimulationLightInterval);
            chkSimulationsLightOnDay = FindViewById<CheckBox>(Resource.Id.chkSimulationsLightOnDay);
            chkSimulationsLightOnNight = FindViewById<CheckBox>(Resource.Id.chkSimulationsLightOnNight);
            #endregion

            #region 起步
            edtTxtStartVoice = FindViewById<EditText>(Resource.Id.edtTxtStartVoice);
            edtTxtStartEndVoice = FindViewById<EditText>(Resource.Id.edtTxtStartEndVoice);

            edtTxtStartStopCheckForwardDistance = FindViewById<EditText>(Resource.Id.edtTxtStartStopCheckForwardDistance);
            edtTxtStartTimeout = FindViewById<EditText>(Resource.Id.edtTxtStartTimeout);
            chkIsCheckStartLight = FindViewById<CheckBox>(Resource.Id.chkIsCheckStartLight);
            chkStartLoudSpeakerCheck = FindViewById<CheckBox>(Resource.Id.chkStartLoudSpeakerCheck);
            chkStartLowAndHighBeamInNight = FindViewById<CheckBox>(Resource.Id.chkStartLowAndHighBeamInNight);
            chkStartShockEnable = FindViewById<CheckBox>(Resource.Id.chkStartShockEnable);
            edtTxtStartReleaseHandbrakeTimeout = FindViewById<EditText>(Resource.Id.edtTxtStartReleaseHandbrakeTimeout);
            edtTxtStartEngineRpm = FindViewById<EditText>(Resource.Id.edtTxtStartEngineRpm);
            edtTxtStartShockValue = FindViewById<EditText>(Resource.Id.edtTxtStartShockValue);
            edtTxtStartShockCount = FindViewById<EditText>(Resource.Id.edtTxtStartShockCount);
            #endregion

            #region 靠边停车
            edtTxtPullOverVoice = FindViewById<EditText>(Resource.Id.edtTxtPullOverVoice);
            edtTxtPullOverDoorTimeout = FindViewById<EditText>(Resource.Id.edtTxtPullOverDoorTimeout);
            edtTxtPullOverMaxDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtPullOverMaxDrivingDistance);
            edtTxtPullOverHandbrakeTimeout = FindViewById<EditText>(Resource.Id.edtTxtPullOverHandbrakeTimeout);
            chkPullOverTurnLightCheck = FindViewById<CheckBox>(Resource.Id.chkPullOverTurnLightCheck);
            chkPullOverOpenCautionBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverOpenCautionBeforeGetOff);
            chkPullOverOpenSafetyBeltBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverOpenSafetyBeltBeforeGetOff);
            chkPullOverNeutralCheck = FindViewById<CheckBox>(Resource.Id.chkPullOverNeutralCheck);
            chkPullOverStopEngineBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverStopEngineBeforeGetOff);
            chkPullOverCloseLowBeamBeforeGetOff = FindViewById<CheckBox>(Resource.Id.chkPullOverCloseLowBeamBeforeGetOff);
            chkPulloverEndAutoTriggerStopExam = FindViewById<CheckBox>(Resource.Id.chkPulloverEndAutoTriggerStopExam);
            radStopFlagHandBreak = FindViewById<RadioButton>(Resource.Id.radStopFlagHandBreak);
            radStopFlagCarStop = FindViewById<RadioButton>(Resource.Id.radStopFlagCarStop);
            radPullOverCautionLight = FindViewById<RadioButton>(Resource.Id.radPullOverCautionLight);
            radPullOverLowBeamCheck = FindViewById<RadioButton>(Resource.Id.radPullOverLowBeamCheck);
            radPullOverEngineExtinction = FindViewById<RadioButton>(Resource.Id.radPullOverEngineExtinction);
            radPullOverSafetyBelt = FindViewById<RadioButton>(Resource.Id.radPullOverSafetyBelt);
            radPullOverOpenCloseDoor = FindViewById<RadioButton>(Resource.Id.radPullOverOpenCloseDoor);
            radPullOverHandbrake = FindViewById<RadioButton>(Resource.Id.radPullOverHandbrake);
            #endregion


            #region 人行横道
            edtTxtPedestrianCrossingVoice = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingVoice);
            edtTxtPedestrianCrossingEndVoice = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingEndVoice);
            edtTxtPedestrianCrossingDistance = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingDistance);
            edtTxtPedestrianCrossingSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingSpeedLimit);
            edtTxtPedestrianCrossingBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtPedestrianCrossingBrakeSpeedUp);
            chkPedestrianCrossingBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingBrakeRequire);
            chkPedestrianCrossingLightCheck = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingLightCheck);
            chkPedestrianCrossingLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingLoudSpeakerDayCheck);
            chkPedestrianCrossingLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkPedestrianCrossingLoudSpeakerNightCheck);
            #endregion

            #region 直线行驶
            edtTxtStraightDrivingVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingVoice);
            edtTxtStraightDrivingEndVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingEndVoice);
            edtTxtStraightDrivingMaxOffsetAngle = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingMaxOffsetAngle);
            edtTxtStraightDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingDistance);
            edtTxtStraightDrivingSpeedMaxLimit = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingSpeedMaxLimit);
            edtTxtStraightDrivingSpeedMinLimit = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingSpeedMinLimit);
            edtTxtStraightDrivingReachSpeed = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingReachSpeed);
            edtTxtStraightDrivingPrepareDistance = FindViewById<EditText>(Resource.Id.edtTxtStraightDrivingPrepareDistance);
            #endregion



            #region 公交汽车
            edtTxtBusAreaVoice = FindViewById<EditText>(Resource.Id.edtTxtBusAreaVoice);
            edtTxtBusAreaEndVoice = FindViewById<EditText>(Resource.Id.edtTxtBusAreaEndVoice);
            edtTxtBusAreaDistance = FindViewById<EditText>(Resource.Id.edtTxtBusAreaDistance);
            edtTxtBusAreaSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtBusAreaSpeedLimit);
            edtTxtBusAreaBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtBusAreaBrakeSpeedUp);
            chkBusAreaBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkBusAreaBrakeRequire);
            chkBusAreaLightCheck = FindViewById<CheckBox>(Resource.Id.chkBusAreaLightCheck);
            chkBusAreaLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkBusAreaLoudSpeakerDayCheck);
            chkBusAreaLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkBusAreaLoudSpeakerNightCheck);
            #endregion

            #region 学校区域
            edtTxtSchoolAreaVoice = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaVoice);
            edtTxtSchoolAreaEndVoice = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaEndVoice);
            edtTxtSchoolAreaDistance = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaDistance);
            edtTxtSchoolAreaSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaSpeedLimit);
            edtTxtSchoolAreaBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtSchoolAreaBrakeSpeedUp);
            #endregion

            #region 路口直行
            edtTxtStraightThroughIntersectionVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionVoice);
            edtTxtStraightThroughIntersectionEndVoice = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionEndVoice);
            edtTxtStraightThroughIntersectionDistance = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionDistance);
            edtTxtStraightThroughIntersectionSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionSpeedLimit);
            edtTxtStraightThroughIntersectionBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtStraightThroughIntersectionBrakeSpeedUp);
            chkStraightThroughIntersectionBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionBrakeRequire);
            chkStraightThroughIntersectionLightCheck = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionLightCheck);
            chkStraightThroughIntersectionLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionLoudSpeakerDayCheck);
            chkStraightThroughIntersectionLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkStraightThroughIntersectionLoudSpeakerNightCheck);
            #endregion

            #region 左转
            edtTxtTurnLeftVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftVoice);
            edtTxtTurnLeftEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftEndVoice);
            edtTxtTurnLeftDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftDistance);
            edtTxtTurnLeftSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftSpeedLimit);
            edtTxtTurnLeftBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtTurnLeftBrakeSpeedUp);
            chkTurnLeftBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkTurnLeftBrakeRequire);
            chkTurnLeftLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLightCheck);
            chkTurnLeftLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLoudSpeakerDayCheck);
            chkTurnLeftLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnLeftLoudSpeakerNightCheck);
            #endregion

            #region 右转
            edtTxtTurnRightVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRightVoice);
            edtTxtTurnRightEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRightEndVoice);
            edtTxtTurnRightDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnRightDistance);
            edtTxtTurnRightSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtTurnRightSpeedLimit);
            edtTxtTurnRightBrakeSpeedUp = FindViewById<EditText>(Resource.Id.edtTxtTurnRightBrakeSpeedUp);
            chkTurnRightBrakeRequire = FindViewById<CheckBox>(Resource.Id.chkTurnRightBrakeRequire);
            chkTurnRightLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRightLightCheck);
            chkTurnRightLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRightLoudSpeakerDayCheck);
            chkTurnRightLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRightLoudSpeakerNightCheck);
            #endregion

            #region 掉头
            edtTxtTurnRoundVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundVoice);
            edtTxtTurnRoundEndVoice = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundEndVoice);
            edtTxtTurnRoundMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundMaxDistance);
            edtTxtTurnRoundStartAngleDiff = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundStartAngleDiff);
            edtTxtTurnRoundEndAngleDiff = FindViewById<EditText>(Resource.Id.edtTxtTurnRoundEndAngleDiff);
            chkTurnRoundBrakeRequired = FindViewById<CheckBox>(Resource.Id.chkTurnRoundBrakeRequired);
            chkTurnRoundLightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLightCheck);
            chkTurnRoundLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLoudSpeakerDayCheck);
            chkTurnRoundLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkTurnRoundLoudSpeakerNightCheck);
            #endregion

            #region 会车
            edtTxtMeetingVoice = FindViewById<EditText>(Resource.Id.edtTxtMeetingVoice);
            edtTxtMeetingEndVoice = FindViewById<EditText>(Resource.Id.edtTxtMeetingEndVoice);
            edtTxtMeetingDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtMeetingDrivingDistance);
            edtTxtMeetingSlowSpeedInKmh = FindViewById<EditText>(Resource.Id.edtTxtMeetingSlowSpeedInKmh);
            edtTxtMeetingAngle = FindViewById<EditText>(Resource.Id.edtTxtMeetingAngle);
            chkMeetingCheckBrake = FindViewById<CheckBox>(Resource.Id.chkMeetingCheckBrake);
            chkMeetingForbidHighBeamCheck = FindViewById<CheckBox>(Resource.Id.chkMeetingForbidHighBeamCheck);
            #endregion

            #region 急弯坡路
            edtTxtSharpTurnVoice = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnVoice);
            edtTxtSharpTurnEndVoice = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnEndVoice);
            edtTxtSharpTurnDistance = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnDistance);
            edtTxtSharpTurnSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtSharpTurnSpeedLimit);
            chkSharpTurnBrake = FindViewById<CheckBox>(Resource.Id.chkSharpTurnBrake);
            chkSharpTurnLightCheck = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLightCheck);
            chkSharpTurnLoudspeakerInDay = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLoudspeakerInDay);
            chkSharpTurnLoudspeakerInNight = FindViewById<CheckBox>(Resource.Id.chkSharpTurnLoudspeakerInNight);
            #endregion

            #region 变更车道
            edtTxtChangeLanesVoice = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesVoice);
            edtTxtChangeLanesEndVoice = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesEndVoice);
            edtTxtChangeLanesMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesMaxDistance);
            edtTxtChangeLanesTimeout = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesTimeout);
            edtTxtChangeLanesAngle = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesAngle);
            chkChangeLanesLowAndHighBeamCheck = FindViewById<CheckBox>(Resource.Id.chkChangeLanesLowAndHighBeamCheck);
            chkChangeLanesLightCheck = FindViewById<CheckBox>(Resource.Id.chkChangeLanesLightCheck);
            edtTxtChangeLanesPrepareDistance = FindViewById<EditText>(Resource.Id.edtTxtChangeLanesPrepareDistance);
            #endregion

            #region 环岛
            edtTxtRoundaboutVoice = FindViewById<EditText>(Resource.Id.edtTxtRoundaboutVoice);
            edtTxtRoundaboutEndVoice = FindViewById<EditText>(Resource.Id.edtTxtRoundaboutEndVoice);
            edtTxtRoundaboutDistance = FindViewById<EditText>(Resource.Id.edtTxtRoundaboutDistance);
            chkRoundaboutLightCheck = FindViewById<CheckBox>(Resource.Id.chkRoundaboutLightCheck);
            #endregion

            #region 超车
            edtTxtOvertakeVoice = FindViewById<EditText>(Resource.Id.edtTxtOvertakeVoice);
            edtTxtOvertakeEndVoice = FindViewById<EditText>(Resource.Id.edtTxtOvertakeEndVoice);
            edtTxtOvertakeMaxDistance = FindViewById<EditText>(Resource.Id.edtTxtOvertakeMaxDistance);
            edtTxtOvertakeTimeout = FindViewById<EditText>(Resource.Id.edtTxtOvertakeTimeout);
            edtTxtOvertakeChangeLanesAngle = FindViewById<EditText>(Resource.Id.edtTxtOvertakeChangeLanesAngle);
            edtTxtOvertakeLowestSpeed = FindViewById<EditText>(Resource.Id.edtTxtOvertakeLowestSpeed);
            edtTxtOvertakeSpeedLimit = FindViewById<EditText>(Resource.Id.edtTxtOvertakeSpeedLimit);
            edtTxtOvertakeSpeedRange = FindViewById<EditText>(Resource.Id.edtTxtOvertakeSpeedRange);
            edtTxtOverTakePrepareDistance = FindViewById<EditText>(Resource.Id.edtTxtOverTakePrepareDistance);
            edtTxtOvertakeBackToOriginalLaneVoice = FindViewById<EditText>(Resource.Id.edtTxtOvertakeBackToOriginalLaneVoice);
            edtTxtOvertakeBackToOriginalLaneDistance = FindViewById<EditText>(Resource.Id.edtTxtOvertakeBackToOriginalLaneDistance);
            chkOvertakeLowAndHighBeamCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLowAndHighBeamCheck);
            chkOvertakeLightCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLightCheck);
            chkOvertakeLoudSpeakerDayCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLoudSpeakerDayCheck);
            chkOvertakeLoudSpeakerNightCheck = FindViewById<CheckBox>(Resource.Id.chkOvertakeLoudSpeakerNightCheck);
            chkOvertakeBackToOriginalLane = FindViewById<CheckBox>(Resource.Id.chkOvertakeBackToOriginalLane);
            #endregion

            #region 加减档


            edtTxtModifiedGearVoice = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearVoice);
            edtTxtModifiedGearEndVoice = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearEndVoice);
            edtTxtModifiedGearDrivingDistance = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearDrivingDistance);
            edtTxtModifiedGearTimeout = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearTimeout);
            edtTxtModifiedGearIgnoreSeconds = FindViewById<EditText>(Resource.Id.edtTxtModifiedGearIgnoreSeconds);
            chkModifiedGearIsPlayGearVoice = FindViewById<CheckBox>(Resource.Id.chkModifiedGearIsPlayGearVoice);
            chkModifiedGearIsPlayActionVoice = FindViewById<CheckBox>(Resource.Id.chkModifiedGearIsPlayActionVoice);
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

            #region 综合品公安


            //综合评判
            chkBreakVoice = FindViewById<CheckBox>(Resource.Id.chkBreakVoice);
            edtTxtExamSuccess = FindViewById<EditText>(Resource.Id.edtTxtExamSuccess);
            edtTxtExamFailed = FindViewById<EditText>(Resource.Id.edtTxtExamFailed);
            #endregion
        }


        public override void UpdateSettings()
        {
            //需要把项目语音更新进入数据库

            //其实我觉得我只需要一个KevValue 就可以了
            //key,Value

            try
            {
                List<ExamItem> lstExamItem = new List<ExamItem>();
                ExamItem examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.PrepareDriving;
                examItem.VoiceText = edtTxtPrepareDrivingVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.Light;
                examItem.VoiceText = edtTxtLightVoice.Text;
                lstExamItem.Add(examItem);


                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.Start;
                examItem.VoiceText = edtTxtStartVoice.Text;
                examItem.EndVoiceText = edtTxtStartEndVoice.Text;
                lstExamItem.Add(examItem);




                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.PullOver;
                examItem.VoiceText = edtTxtPullOverVoice.Text;
                lstExamItem.Add(examItem);



                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.StraightDriving;
                examItem.VoiceText = edtTxtStraightDrivingVoice.Text;
                examItem.EndVoiceText = edtTxtStraightDrivingEndVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.PedestrianCrossing;
                examItem.VoiceText = edtTxtPedestrianCrossingVoice.Text;
                examItem.EndVoiceText = edtTxtPedestrianCrossingEndVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.BusArea;
                examItem.VoiceText = edtTxtBusAreaVoice.Text;
                examItem.EndVoiceText = edtTxtBusAreaEndVoice.Text;
                lstExamItem.Add(examItem);


                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.SchoolArea;
                examItem.VoiceText = edtTxtSchoolAreaVoice.Text;
                examItem.EndVoiceText = edtTxtSchoolAreaEndVoice.Text;
                lstExamItem.Add(examItem);



                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.StraightThrough;
                examItem.VoiceText = edtTxtStraightThroughIntersectionVoice.Text;
                examItem.EndVoiceText = edtTxtStraightThroughIntersectionEndVoice.Text;
                lstExamItem.Add(examItem);


                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.TurnLeft;
                examItem.VoiceText = edtTxtTurnLeftVoice.Text;
                examItem.EndVoiceText = edtTxtTurnLeftEndVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.TurnRight;
                examItem.VoiceText = edtTxtTurnRightVoice.Text;
                examItem.EndVoiceText = edtTxtTurnRightEndVoice.Text;
                lstExamItem.Add(examItem);


                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.TurnRound;
                examItem.VoiceText = edtTxtTurnRoundVoice.Text;
                examItem.EndVoiceText = edtTxtTurnRoundEndVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.Meeting;
                examItem.VoiceText = edtTxtMeetingVoice.Text;
                examItem.EndVoiceText = edtTxtMeetingEndVoice.Text;
                lstExamItem.Add(examItem);


                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.SharpTurn;
                examItem.VoiceText = edtTxtSharpTurnVoice.Text;
                examItem.EndVoiceText = edtTxtSharpTurnEndVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.ChangeLanes;
                examItem.VoiceText = edtTxtChangeLanesVoice.Text;
                examItem.EndVoiceText = edtTxtChangeLanesEndVoice.Text;
                lstExamItem.Add(examItem);


                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.Roundabout;
                examItem.VoiceText = edtTxtRoundaboutVoice.Text;
                examItem.EndVoiceText = edtTxtRoundaboutEndVoice.Text;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.Overtaking;
                examItem.VoiceText = edtTxtOvertakeVoice.Text; ;
                examItem.EndVoiceText = edtTxtOvertakeEndVoice.Text; ;
                lstExamItem.Add(examItem);

                examItem = new ExamItem();
                examItem.ItemCode = ExamItemCodes.ModifiedGear;
                examItem.VoiceText = edtTxtModifiedGearVoice.Text;
                examItem.EndVoiceText = edtTxtModifiedGearEndVoice.Text;
                lstExamItem.Add(examItem);

                dataService.UpdateExamItemsVoice(lstExamItem);



                #region 上车准备
                Settings.AroundCarTimeout = Convert.ToInt32(edtTxtAroundCarTimeout.Text);

                Settings.AroundCarEnable = chkAroundCarEnable.Checked;
                Settings.PrepareDrivingEnable = chkPrepareDrivingEnable.Checked;
                Settings.AroundCarVoiceEnable = chkAroundCarVoice.Checked;
                Settings.PrepareDriving3TouchVoice = chkPrepareDriving3TouchVoice.Checked;
                Settings.PrepareDrivingTailStockEnable = chkPrepareDrivingTailStockEnable.Checked;
                Settings.PrepareDrivingHeadStockEnable = chkPrepareDrivingHeadStockEnable.Checked;

                if (radDoor.Checked)
                {
                    Settings.PrepareDrivingEndFlag = PrepareDrivingEndFlag.Door;
                }
                else if (radSafeblet.Checked)
                {
                    Settings.PrepareDrivingEndFlag = PrepareDrivingEndFlag.SafeBelt;
                }
                #endregion


                #region 灯光模拟
                Settings.SimulationLightTimeout = Convert.ToInt32(edtTxtSimulationLightTimeout.Text);
                Settings.SimulationLightInterval = Convert.ToDouble(edtTxtSimulationLightInterval.Text);
                Settings.SimulationsLightOnDay = chkSimulationsLightOnDay.Checked;
                Settings.SimulationsLightOnNight = chkSimulationsLightOnNight.Checked;
                #endregion


                #region 起步
                Settings.StartStopCheckForwardDistance = Convert.ToDouble(edtTxtStartStopCheckForwardDistance.Text);
                Settings.StartTimeout = Convert.ToInt32(edtTxtStartTimeout.Text);
                Settings.IsCheckStartLight = chkIsCheckStartLight.Checked;
                //白考核夜考是一样的
                Settings.VehicleStartingLoudSpeakerDayCheck = chkStartLoudSpeakerCheck.Checked;
                Settings.VehicleStartingLoudSpeakerNightCheck = chkStartLoudSpeakerCheck.Checked;

                Settings.StartLowAndHighBeamInNight = chkStartLowAndHighBeamInNight.Checked;
                Settings.StartShockEnable = chkStartShockEnable.Checked;
                Settings.StartReleaseHandbrakeTimeout = Convert.ToInt32(edtTxtStartReleaseHandbrakeTimeout.Text);
                Settings.StartEngineRpm = Convert.ToInt32(edtTxtStartEngineRpm.Text);
                Settings.StartShockValue = Convert.ToDouble(edtTxtStartShockValue.Text);
                Settings.StartShockCount = Convert.ToDouble(edtTxtStartShockCount.Text);
                #endregion

                #region 靠边停车
                Settings.PullOverMaxDrivingDistance = Convert.ToInt32(edtTxtPullOverMaxDrivingDistance.Text);
                Settings.PullOverHandbrakeTimeout = Convert.ToInt32(edtTxtPullOverHandbrakeTimeout.Text);
                Settings.PullOverDoorTimeout = Convert.ToInt32(edtTxtPullOverDoorTimeout.Text);
                Settings.PullOverTurnLightCheck = chkPullOverTurnLightCheck.Checked;
                Settings.PullOverOpenCautionBeforeGetOff = chkPullOverOpenCautionBeforeGetOff.Checked;
                Settings.PullOverOpenSafetyBeltBeforeGetOff = chkPullOverOpenSafetyBeltBeforeGetOff.Checked;
                Settings.PullOverNeutralCheck = chkPullOverNeutralCheck.Checked;
                Settings.PullOverStopEngineBeforeGetOff = chkPullOverStopEngineBeforeGetOff.Checked;
                Settings.PullOverCloseLowBeamBeforeGetOff = chkPullOverCloseLowBeamBeforeGetOff.Checked;
                Settings.PulloverEndAutoTriggerStopExam = chkPulloverEndAutoTriggerStopExam.Checked;

                if (radPullOverCautionLight.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.CautionLightCheck;
                }
                else if (radPullOverEngineExtinction.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.EngineExtinctionCheck;
                }
                else if (radPullOverHandbrake.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.Handbrake;
                }
                else if (radPullOverLowBeamCheck.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.LowBeamCheck;
                }
                else if (radPullOverOpenCloseDoor.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.OpenCloseDoorCheck;
                }
                else if (radPullOverSafetyBelt.Checked)
                {
                    Settings.PullOverEndMark = PullOverEndMark.SafetyBeltCheck;
                }
                #endregion

                #region 直线行驶
                Settings.StraightDrivingDistance = Convert.ToDouble(edtTxtStraightDrivingDistance.Text);
                Settings.StraightDrivingMaxOffsetAngle = Convert.ToDouble(edtTxtStraightDrivingMaxOffsetAngle.Text);
                Settings.StraightDrivingPrepareDistance = Convert.ToInt32(edtTxtStraightDrivingPrepareDistance.Text);
                Settings.StraightDrivingReachSpeed = Convert.ToInt32(edtTxtStraightDrivingReachSpeed.Text);
                Settings.StraightDrivingSpeedMaxLimit = Convert.ToInt32(edtTxtStraightDrivingSpeedMaxLimit.Text);
                Settings.StraightDrivingSpeedMinLimit = Convert.ToInt32(edtTxtStraightDrivingSpeedMinLimit.Text);
                #endregion






                #region 人行横道
                Settings.PedestrianCrossingDistance = Convert.ToInt32(edtTxtPedestrianCrossingDistance.Text);
                Settings.PedestrianCrossingSpeedLimit = Convert.ToInt32(edtTxtPedestrianCrossingSpeedLimit.Text);
                Settings.PedestrianCrossingBrakeSpeedUp = Convert.ToInt32(edtTxtPedestrianCrossingBrakeSpeedUp.Text);
                Settings.PedestrianCrossingBrakeRequire = chkPedestrianCrossingBrakeRequire.Checked;
                Settings.PedestrianCrossingLightCheck = chkPedestrianCrossingLightCheck.Checked;
                Settings.PedestrianCrossingLoudSpeakerDayCheck = chkPedestrianCrossingLoudSpeakerDayCheck.Checked;
                Settings.PedestrianCrossingLoudSpeakerNightCheck = chkPedestrianCrossingLoudSpeakerNightCheck.Checked;
                #endregion


                #region 公交汽车
                Settings.BusAreaDistance = Convert.ToInt32(edtTxtBusAreaDistance.Text);
                Settings.BusAreaSpeedLimit = Convert.ToInt32(edtTxtBusAreaSpeedLimit.Text);
                Settings.BusAreaBrakeSpeedUp = Convert.ToInt32(edtTxtBusAreaBrakeSpeedUp.Text);
                Settings.BusAreaBrakeRequire = chkBusAreaBrakeRequire.Checked;
                Settings.BusAreaLightCheck = chkBusAreaLightCheck.Checked;
                Settings.BusAreaLoudSpeakerDayCheck = chkBusAreaLoudSpeakerDayCheck.Checked;
                Settings.BusAreaLoudSpeakerNightCheck = chkBusAreaLoudSpeakerNightCheck.Checked;
                #endregion


                #region 学校区域
                Settings.SchoolAreaDistance = Convert.ToInt32(edtTxtSchoolAreaDistance.Text);
                Settings.SchoolAreaSpeedLimit = Convert.ToInt32(edtTxtSchoolAreaSpeedLimit.Text);
                Settings.SchoolAreaBrakeSpeedUp = Convert.ToInt32(edtTxtSchoolAreaBrakeSpeedUp.Text);
                #endregion


                #region 路口直行
                Settings.StraightThroughIntersectionDistance = Convert.ToInt32(edtTxtStraightThroughIntersectionDistance.Text);
                Settings.StraightThroughIntersectionSpeedLimit = Convert.ToInt32(edtTxtStraightThroughIntersectionSpeedLimit.Text);
                Settings.StraightThroughIntersectionBrakeSpeedUp = Convert.ToInt32(edtTxtStraightThroughIntersectionBrakeSpeedUp.Text);
                Settings.StraightThroughIntersectionBrakeRequire = chkStraightThroughIntersectionBrakeRequire.Checked;
                Settings.StraightThroughIntersectionLightCheck = chkStraightThroughIntersectionLightCheck.Checked;
                Settings.StraightThroughIntersectionLoudSpeakerDayCheck = chkStraightThroughIntersectionLoudSpeakerDayCheck.Checked;
                Settings.StraightThroughIntersectionLoudSpeakerNightCheck = chkStraightThroughIntersectionLoudSpeakerNightCheck.Checked;
                #endregion


                #region 路口左转
                Settings.TurnLeftDistance = Convert.ToInt32(edtTxtTurnLeftDistance.Text);
                Settings.TurnLeftSpeedLimit = Convert.ToInt32(edtTxtTurnLeftSpeedLimit.Text);
                Settings.TurnLeftBrakeSpeedUp = Convert.ToInt32(edtTxtTurnLeftBrakeSpeedUp.Text);
                Settings.TurnLeftBrakeRequire = chkTurnLeftBrakeRequire.Checked;
                Settings.TurnLeftLightCheck = chkTurnLeftLightCheck.Checked;
                Settings.TurnLeftLoudSpeakerDayCheck = chkTurnLeftLoudSpeakerDayCheck.Checked;
                Settings.TurnLeftLoudSpeakerNightCheck = chkTurnLeftLoudSpeakerNightCheck.Checked;
                #endregion


                #region 路口右转
                Settings.TurnRightDistance = Convert.ToInt32(edtTxtTurnRightDistance.Text);
                Settings.TurnRightSpeedLimit = Convert.ToInt32(edtTxtTurnRightSpeedLimit.Text);
                Settings.TurnRightBrakeSpeedUp = Convert.ToInt32(edtTxtTurnRightBrakeSpeedUp.Text);
                Settings.TurnRightBrakeRequire = chkTurnRightBrakeRequire.Checked;
                Settings.TurnRightLightCheck = chkTurnRightLightCheck.Checked;
                Settings.TurnRightLoudSpeakerDayCheck = chkTurnRightLoudSpeakerDayCheck.Checked;
                Settings.TurnRightLoudSpeakerNightCheck = chkTurnRightLoudSpeakerNightCheck.Checked;
                #endregion


                #region 掉头


                Settings.TurnRoundMaxDistance = Convert.ToInt32(edtTxtTurnRoundMaxDistance.Text);
                Settings.TurnRoundStartAngleDiff = Convert.ToInt32(edtTxtTurnRoundStartAngleDiff.Text);
                Settings.TurnRoundEndAngleDiff = Convert.ToInt32(edtTxtTurnRoundEndAngleDiff.Text);
                Settings.TurnRoundBrakeRequired = chkTurnRoundBrakeRequired.Checked;
                Settings.TurnRoundLightCheck = chkTurnRoundLightCheck.Checked;
                Settings.TurnRoundLoudSpeakerDayCheck = chkTurnRoundLoudSpeakerDayCheck.Checked;
                Settings.TurnRoundLoudSpeakerNightCheck = chkTurnRoundLoudSpeakerNightCheck.Checked;
                #endregion

                #region 会车


                Settings.MeetingDrivingDistance = Convert.ToDouble(edtTxtMeetingDrivingDistance.Text);
                Settings.MeetingSlowSpeedInKmh = Convert.ToInt32(edtTxtMeetingSlowSpeedInKmh.Text);
                Settings.MeetingAngle = Convert.ToDouble(edtTxtMeetingAngle.Text);
                Settings.MeetingCheckBrake = chkMeetingCheckBrake.Checked;
                Settings.MeetingForbidHighBeamCheck = chkMeetingForbidHighBeamCheck.Checked;
                #endregion

                #region 急弯坡路


                Settings.SharpTurnDistance = Convert.ToInt32(edtTxtSharpTurnDistance.Text);
                Settings.SharpTurnSpeedLimit = Convert.ToInt32(edtTxtSharpTurnSpeedLimit.Text);
                Settings.SharpTurnBrake = chkSharpTurnBrake.Checked;
                Settings.SharpTurnLightCheck = chkSharpTurnLightCheck.Checked;
                Settings.SharpTurnLoudspeakerInDay = chkSharpTurnLoudspeakerInDay.Checked;
                Settings.SharpTurnLoudspeakerInNight = chkSharpTurnLoudspeakerInNight.Checked;
                #endregion


                #region 变更车道


                Settings.ChangeLanesMaxDistance = Convert.ToInt32(edtTxtChangeLanesMaxDistance.Text);
                Settings.ChangeLanesTimeout = Convert.ToInt32(edtTxtChangeLanesTimeout.Text);
                Settings.ChangeLanesAngle = Convert.ToDouble(edtTxtChangeLanesAngle.Text);
                Settings.ChangeLanesLowAndHighBeamCheck = chkChangeLanesLowAndHighBeamCheck.Checked;
                Settings.ChangeLanesLightCheck = chkChangeLanesLightCheck.Checked;
                Settings.ChangeLanesPrepareDistance = Convert.ToInt32(edtTxtChangeLanesPrepareDistance.Text);
                #endregion


                #region 环岛



                Settings.RoundaboutDistance = Convert.ToInt32(edtTxtRoundaboutDistance.Text);
                Settings.RoundaboutLightCheck = chkRoundaboutLightCheck.Checked;
                #endregion

                #region 超车



                Settings.OvertakeMaxDistance = Convert.ToInt32(edtTxtOvertakeMaxDistance.Text);
                Settings.OvertakeTimeout = Convert.ToInt32(edtTxtOvertakeTimeout.Text);
                Settings.OvertakeChangeLanesAngle = Convert.ToDouble(edtTxtOvertakeChangeLanesAngle.Text);
                Settings.OvertakeLowestSpeed = Convert.ToInt32(edtTxtOvertakeLowestSpeed.Text);
                Settings.OvertakeSpeedLimit = Convert.ToInt32(edtTxtOvertakeSpeedLimit.Text);
                Settings.OvertakeSpeedRange = Convert.ToInt32(edtTxtOvertakeSpeedRange.Text);

                Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance = Convert.ToInt32(edtTxtOvertakeBackToOriginalLaneDistance.Text);
                Settings.OvertakeBackToOriginalLaneVocie = edtTxtOvertakeBackToOriginalLaneVoice.Text;
                Settings.OvertakeLowAndHighBeamCheck = chkOvertakeLowAndHighBeamCheck.Checked;
                Settings.OvertakeLightCheck = chkOvertakeLightCheck.Checked;
                Settings.OvertakeLoudSpeakerDayCheck = chkOvertakeLoudSpeakerDayCheck.Checked;
                Settings.OvertakeLoudSpeakerNightCheck = chkOvertakeLoudSpeakerNightCheck.Checked;
                Settings.OvertakeBackToOriginalLane = chkOvertakeBackToOriginalLane.Checked;
                Settings.OvertakePrepareDistance = Convert.ToInt32(edtTxtOverTakePrepareDistance.Text);
                #endregion

                #region 加减档



                Settings.ModifiedGearDrivingDistance = Convert.ToInt32(edtTxtModifiedGearDrivingDistance.Text);
                Settings.ModifiedGearTimeout = Convert.ToDouble(edtTxtModifiedGearTimeout.Text);
                Settings.ModifiedGearIgnoreSeconds = Convert.ToDouble(edtTxtModifiedGearIgnoreSeconds.Text);
                Settings.ModifiedGearIsPlayGearVoice = chkModifiedGearIsPlayGearVoice.Checked;
                Settings.ModifiedGearIsPlayActionVoice = chkModifiedGearIsPlayActionVoice.Checked;



                //1-2,2-3,3-4,4-5
                //1-2,2-3,3-4,4-5,5-4,4-3,3-2,2-1
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

                #region 综合评判
                Settings.CommonExamItemExamFailVoice = edtTxtExamFailed.Text;
                Settings.CommonExamItemExamSuccessVoice = edtTxtExamSuccess.Text;
                Settings.CommonExamItemsBreakVoice = chkBreakVoice.Checked;
                #endregion

                #region listSetting


                List<Setting> lstSetting = new List<Setting>
                {
                    #region 上车准备
 new Setting { Key ="AroundCarTimeout", Value = Settings.AroundCarTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="AroundCarEnable", Value = Settings.AroundCarEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingEnable", Value = Settings.PrepareDrivingEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="AroundCarVoiceEnable", Value = Settings.AroundCarVoiceEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDriving3TouchVoice", Value = Settings.PrepareDriving3TouchVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingTailStockEnable", Value = Settings.PrepareDrivingTailStockEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingHeadStockEnable", Value = Settings.PrepareDrivingHeadStockEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PrepareDrivingEndFlag", Value = Settings.PrepareDrivingEndFlag.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 灯光模拟
new Setting { Key ="SimulationLightTimeout", Value = Settings.SimulationLightTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SimulationLightInterval", Value = Settings.SimulationLightInterval.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SimulationsLightOnDay", Value = Settings.SimulationsLightOnDay.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SimulationsLightOnNight", Value = Settings.SimulationsLightOnNight.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 起步


new Setting { Key ="StartStopCheckForwardDistance", Value = Settings.StartStopCheckForwardDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartTimeout", Value = Settings.StartTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="IsCheckStartLight", Value = Settings.IsCheckStartLight.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="VehicleStartingLoudSpeakerDayCheck", Value = Settings.VehicleStartingLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="VehicleStartingLoudSpeakerNightCheck", Value = Settings.VehicleStartingLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartLowAndHighBeamInNight", Value = Settings.StartLowAndHighBeamInNight.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartShockEnable", Value = Settings.StartShockEnable.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartReleaseHandbrakeTimeout", Value = Settings.StartReleaseHandbrakeTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartEngineRpm", Value = Settings.StartEngineRpm.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartShockValue", Value = Settings.StartShockValue.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StartShockCount", Value = Settings.StartShockCount.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 靠边停车
   new Setting { Key ="PullOverMaxDrivingDistance", Value = Settings.PullOverMaxDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverHandbrakeTimeout", Value = Settings.PullOverHandbrakeTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverDoorTimeout", Value = Settings.PullOverDoorTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverTurnLightCheck", Value = Settings.PullOverTurnLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverOpenCautionBeforeGetOff", Value = Settings.PullOverOpenCautionBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverOpenSafetyBeltBeforeGetOff", Value = Settings.PullOverOpenSafetyBeltBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverNeutralCheck", Value = Settings.PullOverNeutralCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverStopEngineBeforeGetOff", Value = Settings.PullOverStopEngineBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverCloseLowBeamBeforeGetOff", Value = Settings.PullOverCloseLowBeamBeforeGetOff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PulloverEndAutoTriggerStopExam", Value = Settings.PulloverEndAutoTriggerStopExam.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PullOverEndMark", Value = Settings.PullOverEndMark.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 直线行驶


new Setting { Key ="StraightDrivingDistance", Value = Settings.StraightDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingMaxOffsetAngle", Value = Settings.StraightDrivingMaxOffsetAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingPrepareDistance", Value = Settings.StraightDrivingPrepareDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingReachSpeed", Value = Settings.StraightDrivingReachSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingSpeedMaxLimit", Value = Settings.StraightDrivingSpeedMaxLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightDrivingSpeedMinLimit", Value = Settings.StraightDrivingSpeedMinLimit.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 人行横道
 new Setting { Key ="PedestrianCrossingDistance", Value = Settings.PedestrianCrossingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingSpeedLimit", Value = Settings.PedestrianCrossingSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingBrakeSpeedUp", Value = Settings.PedestrianCrossingBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingBrakeRequire", Value = Settings.PedestrianCrossingBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingLightCheck", Value = Settings.PedestrianCrossingLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingLoudSpeakerDayCheck", Value = Settings.PedestrianCrossingLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="PedestrianCrossingLoudSpeakerNightCheck", Value = Settings.PedestrianCrossingLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 公交汽车
new Setting { Key ="BusAreaDistance", Value = Settings.BusAreaDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaSpeedLimit", Value = Settings.BusAreaSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaBrakeSpeedUp", Value = Settings.BusAreaBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaBrakeRequire", Value = Settings.BusAreaBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaLightCheck", Value = Settings.BusAreaLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaLoudSpeakerDayCheck", Value = Settings.BusAreaLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="BusAreaLoudSpeakerNightCheck", Value = Settings.BusAreaLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 学校区域
   new Setting { Key ="SchoolAreaDistance", Value = Settings.SchoolAreaDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SchoolAreaSpeedLimit", Value = Settings.SchoolAreaSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SchoolAreaBrakeSpeedUp", Value = Settings.SchoolAreaBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 路口直行
new Setting { Key ="StraightThroughIntersectionDistance", Value = Settings.StraightThroughIntersectionDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionSpeedLimit", Value = Settings.StraightThroughIntersectionSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionBrakeSpeedUp", Value = Settings.StraightThroughIntersectionBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionBrakeRequire", Value = Settings.StraightThroughIntersectionBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionLightCheck", Value = Settings.StraightThroughIntersectionLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionLoudSpeakerDayCheck", Value = Settings.StraightThroughIntersectionLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="StraightThroughIntersectionLoudSpeakerNightCheck", Value = Settings.StraightThroughIntersectionLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 左转


                    new Setting { Key ="TurnLeftDistance", Value = Settings.TurnLeftDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftSpeedLimit", Value = Settings.TurnLeftSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftBrakeSpeedUp", Value = Settings.TurnLeftBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftBrakeRequire", Value = Settings.TurnLeftBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLightCheck", Value = Settings.TurnLeftLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLoudSpeakerDayCheck", Value = Settings.TurnLeftLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnLeftLoudSpeakerNightCheck", Value = Settings.TurnLeftLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 右转


                    new Setting { Key ="TurnRightDistance", Value = Settings.TurnRightDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightSpeedLimit", Value = Settings.TurnRightSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightBrakeSpeedUp", Value = Settings.TurnRightBrakeSpeedUp.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightBrakeRequire", Value = Settings.TurnRightBrakeRequire.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightLightCheck", Value = Settings.TurnRightLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightLoudSpeakerDayCheck", Value = Settings.TurnRightLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRightLoudSpeakerNightCheck", Value = Settings.TurnRightLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 掉头


new Setting { Key ="TurnRoundMaxDistance", Value = Settings.TurnRoundMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundStartAngleDiff", Value = Settings.TurnRoundStartAngleDiff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundEndAngleDiff", Value = Settings.TurnRoundEndAngleDiff.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundBrakeRequired", Value = Settings.TurnRoundBrakeRequired.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLightCheck", Value = Settings.TurnRoundLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLoudSpeakerDayCheck", Value = Settings.TurnRoundLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="TurnRoundLoudSpeakerNightCheck", Value = Settings.TurnRoundLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 会车


  new Setting { Key ="MeetingDrivingDistance", Value = Settings.MeetingDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MeetingSlowSpeedInKmh", Value = Settings.MeetingSlowSpeedInKmh.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MeetingAngle", Value = Settings.MeetingAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MeetingCheckBrake", Value = Settings.MeetingCheckBrake.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="MeetingForbidHighBeamCheck", Value = Settings.MeetingForbidHighBeamCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 急弯破路
new Setting { Key ="SharpTurnDistance", Value = Settings.SharpTurnDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnSpeedLimit", Value = Settings.SharpTurnSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnBrake", Value = Settings.SharpTurnBrake.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnLightCheck", Value = Settings.SharpTurnLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnLoudspeakerInDay", Value = Settings.SharpTurnLoudspeakerInDay.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="SharpTurnLoudspeakerInNight", Value = Settings.SharpTurnLoudspeakerInNight.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 变更车道

	
                    new Setting { Key ="ChangeLanesMaxDistance", Value = Settings.ChangeLanesMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesTimeout", Value = Settings.ChangeLanesTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesAngle", Value = Settings.ChangeLanesAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesLowAndHighBeamCheck", Value = Settings.ChangeLanesLowAndHighBeamCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesLightCheck", Value = Settings.ChangeLanesLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ChangeLanesPrepareDistance", Value = Settings.ChangeLanesPrepareDistance.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 环岛
new Setting { Key ="RoundaboutDistance", Value = Settings.RoundaboutDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="RoundaboutLightCheck", Value = Settings.RoundaboutLightCheck.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 超车
new Setting { Key ="OvertakeMaxDistance", Value = Settings.OvertakeMaxDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeTimeout", Value = Settings.OvertakeTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeChangeLanesAngle", Value = Settings.OvertakeChangeLanesAngle.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLowestSpeed", Value = Settings.OvertakeLowestSpeed.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeSpeedLimit", Value = Settings.OvertakeSpeedLimit.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeSpeedRange", Value = Settings.OvertakeSpeedRange.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance", Value = Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeBackToOriginalLaneVocie", Value = Settings.OvertakeBackToOriginalLaneVocie.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLowAndHighBeamCheck", Value = Settings.OvertakeLowAndHighBeamCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLightCheck", Value = Settings.OvertakeLightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLoudSpeakerDayCheck", Value = Settings.OvertakeLoudSpeakerDayCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeLoudSpeakerNightCheck", Value = Settings.OvertakeLoudSpeakerNightCheck.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakeBackToOriginalLane", Value = Settings.OvertakeBackToOriginalLane.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="OvertakePrepareDistance", Value = Settings.OvertakePrepareDistance.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 加减档
new Setting { Key ="ModifiedGearDrivingDistance", Value = Settings.ModifiedGearDrivingDistance.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ModifiedGearTimeout", Value = Settings.ModifiedGearTimeout.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ModifiedGearIgnoreSeconds", Value = Settings.ModifiedGearIgnoreSeconds.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ModifiedGearIsPlayGearVoice", Value = Settings.ModifiedGearIsPlayGearVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ModifiedGearIsPlayActionVoice", Value = Settings.ModifiedGearIsPlayActionVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ModifiedGearFlowVoice", Value = Settings.ModifiedGearFlowVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="ModifiedGearGearFlow", Value = Settings.ModifiedGearGearFlow.ToString(), GroupName = "GlobalSettings" },
                    #endregion
                    #region 综合评判
new Setting { Key ="CommonExamItemExamFailVoice", Value = Settings.CommonExamItemExamFailVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemExamSuccessVoice", Value = Settings.CommonExamItemExamSuccessVoice.ToString(), GroupName = "GlobalSettings" },
new Setting { Key ="CommonExamItemsBreakVoice", Value = Settings.CommonExamItemsBreakVoice.ToString(), GroupName = "GlobalSettings" },
	                 #endregion

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
    }
}