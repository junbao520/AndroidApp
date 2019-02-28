using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Business.ExamItems
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
        }
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //航向角无效，不记录
            if (!signalInfo.BearingAngle.IsValidAngle())
                return false;

            StartAngle = signalInfo.BearingAngle;

       
            return base.InitExamParms(signalInfo);
        }

       
       
        protected override void CheckDelayStop(CarSignalInfo signalInfo)
        {
            if (stopDelayDistance.HasValue)
            {
                if ((signalInfo.Distance - stopDelayDistance.Value) > 10)
                {
                    stopDelayDistance = null;
                    StopCore();
                }
            }
        }
        protected override void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<MapItemFinishedMessage>(this, OnMapItemFinished);
            base.RegisterMessages(messenger);
        }
        //消息结束项目
        private void OnMapItemFinished(MapItemFinishedMessage message)
        {
            if (message.FinishedItemCode == ItemCode)
            {
                StopCore();
            }
        }
        protected override void StopCore()
        {
            //检测转向灯
            LeftIndicatorCheck(IndicatorCheckState.StopCheck);
            base.StopCore();
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
                    if (Settings.TurnLeftEndFlag)
                        stopDelayDistance = CurrentDistance;
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
            if (Settings.TurnLeftErrorLight && CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) >= Constants.ErrorSignalCount)
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
