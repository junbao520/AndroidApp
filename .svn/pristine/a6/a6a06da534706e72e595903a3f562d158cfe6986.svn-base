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
    public class TurnRight : SlowSpeed
    {

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.TurnRightVoice;
            MaxDistance = Settings.TurnRightDistance;
            SlowSpeedLimit = Settings.TurnRightSpeedLimit;
            OverSpeedMustBrake = Settings.TurnRightBrakeSpeedUp;
            CheckLowAndHighBeam = Settings.TurnRightLightCheck;
            CheckBrakeRequired = Settings.TurnRightBrakeRequire;
            CheckLoudSpeakerInDay = Settings.TurnRightLoudSpeakerDayCheck;
            CheckLoudSpeakerInNight = Settings.TurnRightLoudSpeakerNightCheck;
            PrepareDistance = Settings.TurnRightPrepareD;
            //播放语音点
            if (Constants.IsExamMode_Luzhou && Constants.IsFirstTurnRight)
            {
                VoiceFile = string.Empty;
                MaxDistance = 5;
                VoiceExamItem = false;
                EndVoiceExamItem = false;
                //播放语音点
                Speaker.PlayAudioAsync("前方路口右转", SpeechPriority.Normal);
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

        private bool hasRightIndicator = false;

        private bool hasLeftIndicator = false;
        //踩过刹车
        protected bool braked = false;

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.RightIndicatorLight)
                hasRightIndicator = true;

            if (signalInfo.Sensor.LeftIndicatorLight)
            {
                hasLeftIndicator = true;

            }
            if (signalInfo.Sensor.Brake)
                braked = true;

            //if (signalInfo.Sensor.RightIndicatorLight)
            //{
            //    Constants.IsHaveRightLight = true;
            //}

            base.ExecuteCore(signalInfo);
            //如果必须踩刹车
            if (CheckBrakeRequired && _hasLoosenBrake&& Constants.IsFirstTurnRight==false)
            {
                CheckBrakeVoice(signalInfo);
            }

        }
        protected override bool CheckSpeedLimit()
        {
            //进入项目后没得松开刹车动作再踩，则不算踩刹车
            if (Settings.CheckBrakeMustInitem)
            {
                if (!_hasLoosenBrake)
                    return false;
            }
            //判断下如果是泸州


            //必踩刹车
            if (CheckBrakeRequired)
            {
                var hasBrake = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Brake);
                Logger.InfoFormat("{0}-{1}-{2}-必踩刹车-{3}", Name, StartDistance, StartTime, hasBrake);
                if (!hasBrake && !braked)
                    return false;
            }
            // 速度限制
            if (SlowSpeedLimit > 0)
            {
                var isOverspeed = CarSignalSet.Current.SpeedInKmh > SlowSpeedLimit;
                Logger.InfoFormat("{0}-{1}-{2}-速度限制-{3}:{4}", Name, StartDistance, StartTime, isOverspeed, SlowSpeedLimit);
                if (isOverspeed)
                    return false;
            }
            // 该速度以上必踩刹车
            if (OverSpeedMustBrake > 0)
            {
                var isOverspeed = CarSignalSet.Current.SpeedInKmh > OverSpeedMustBrake;
                Logger.InfoFormat("{0}-{1}-{2}-该速度以上必踩刹车-{3}:{4}", Name, StartDistance, StartTime, isOverspeed, OverSpeedMustBrake);
                if (isOverspeed)
                {
                    var hasBrake = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Brake);
                    if (!hasBrake && !braked)
                        return false;
                }
            }
            return true;
        }

        protected override void StopCore()
        {
            //播放语音点
            if (Constants.IsExamMode_Luzhou && Constants.IsFirstTurnRight)
            {
                base.StopImmediately();
            }
            else
            {
                //第二次进入，则为true
                Constants.IsFirstTurnRight = true;
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
            if (checkState == IndicatorCheckState.MidCheckAngle && Settings.TurnRightAngle < 0.5)
                return;
            if (indicatorChecked)
                return;

            if (checkState == IndicatorCheckState.MidCheckAngle)
            {
                //设置了变道角度后
                if (CarSignalSet.Current.BearingAngle.IsValidAngle() &&
                    StartAngle.IsValidAngle() &&
                    !GeoHelper.IsBetweenDiffAngle(CarSignalSet.Current.BearingAngle, StartAngle, Settings.TurnRightAngle))
                {
                    CheckRight();
                }
            }
            else if (checkState == IndicatorCheckState.StopCheck)
            {

                CheckRight();
            }

        }

        protected void CheckRight()
        {
            indicatorChecked = true;
            //
            if (hasLeftIndicator&&CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) >= Constants.ErrorSignalCount)
            {
                BreakRule(DeductionRuleCodes.RC30205);
            }
            else
            {


                if (!hasRightIndicator && CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) < Constants.ErrorSignalCount)
                {
                    //是否打转向灯
                    //
                    BreakRule(DeductionRuleCodes.RC30205);
                    //是否
                }

                else
                {
                    //检测时间
                    var isOk = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                    if (!isOk)
                    {
                        BreakRule(DeductionRuleCodes.RC30206);
                    }
                }
            }
        }

        protected override string SpeedLimitRuleCode
        {
            get { return DeductionRuleCodes.RC40901; }
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.TurnRight; }
        }
    }
}
