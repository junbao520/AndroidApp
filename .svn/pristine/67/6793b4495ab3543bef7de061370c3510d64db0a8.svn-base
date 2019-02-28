using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{
    /// <summary>
    /// 左转, 减速项目
    /// 路口右转向灯评判
    /// </summary>
    public class TurnLeft : SlowSpeed
    {


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.TurnLeftVoice;
            MaxDistance = Settings.TurnLeftDistance;
            SlowSpeedLimit = Settings.TurnLeftSpeedLimit;
            OverSpeedMustBrake = Settings.TurnLeftBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.TurnLeftLightCheck;
            CheckBrakeRequired = Settings.TurnLeftBrakeRequire;
            CheckLoudSpeakerInDay = Settings.TurnLeftLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.TurnLeftLoudSpeakerNightCheck;
            PrepareDistance = Settings.TurnLeftPrepareD;

            //播放语音点
            if (Constants.IsExamMode_Luzhou && Constants.IsFirstTurnLeft)
            {
                VoiceFile = string.Empty;
                MaxDistance = 5;
                VoiceExamItem = false;
                EndVoiceExamItem = false;
                //播放语音点
                Speaker.PlayAudioAsync("前方路口左转", SpeechPriority.Normal);
                StopImmediately();

            }
        }
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //航向角无效，不记录
            if (!signalInfo.BearingAngle.IsValidAngle())
                return false;

            StartAngle = signalInfo.BearingAngle;


            return base.InitExamParms(signalInfo);
        }
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            //如果必须踩刹车
            base.ExecuteCore(signalInfo);
            if (CheckBrakeRequired && _hasLoosenBrake && Constants.IsFirstTurnLeft == false)
            {
                CheckBrakeVoice(signalInfo);
            }

        }
        protected override void StopCore()
        {
            //播放语音点
            if (Constants.IsExamMode_Luzhou && Constants.IsFirstTurnLeft)
            {
                base.StopImmediately();
            }
            else
            {
                //第二次进入，则为true
                Constants.IsFirstTurnLeft = true;
                //检测转向灯
                LeftIndicatorCheck(IndicatorCheckState.StopCheck);
                base.StopCore();
            }
        }


        /// <summary>
        /// 检测转向灯
        /// </summary>
        protected override void LeftIndicatorCheck(IndicatorCheckState checkState)
        {
            //角度设置0时，不进行角度达到检测转向灯
            if (checkState == IndicatorCheckState.MidCheckAngle && Settings.TurnLeftAngle < 0.5)
                return;
            if (indicatorChecked)
                return;

            if (checkState == IndicatorCheckState.MidCheckAngle)
            {
                //设置了变道角度后
                if (CurrentAngle.IsValidAngle() &&
                    StartAngle.IsValidAngle() &&
                    !GeoHelper.IsBetweenDiffAngle(CurrentAngle, StartAngle, Settings.TurnLeftAngle))
                {
                    CheckLeft();
                }
            }
            else if (checkState == IndicatorCheckState.StopCheck)
            {

                CheckLeft();
            }

        }

        protected void CheckLeft()
        {
            indicatorChecked = true;
            if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) >= Constants.ErrorSignalCount)
            {
                BreakRule(DeductionRuleCodes.RC30205);
            }
            else
            {

                //检测转向灯
                if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) < Constants.ErrorSignalCount)
                {
                    //是否打转向灯
                    BreakRule(DeductionRuleCodes.RC30205);
                }
                else
                {
                    var isOk = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                    if (!isOk)
                    {
                        BreakRule(DeductionRuleCodes.RC30206);
                    }
                }

            }
        }
        protected override string SpeedLimitRuleCode
        {
            get { return DeductionRuleCodes.RC40801; }
        }
        public override string ItemCode
        {
            get { return ExamItemCodes.TurnLeft; }
        }
    }
}
