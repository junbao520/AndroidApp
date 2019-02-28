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
    /// 右转, 减速项目
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
            if (checkState == IndicatorCheckState.MidCheckAngle && Settings.TurnRightAngle < 0.5)
                return;
            if (indicatorChecked)
                return;

            if (checkState == IndicatorCheckState.MidCheckAngle)
            {
                //设置了变道角度后
                if (CurrentAngle.IsValidAngle() &&
                    StartAngle.IsValidAngle() &&
                    !GeoHelper.IsBetweenDiffAngle(CurrentAngle, StartAngle, Settings.TurnRightAngle))
                {
                    if(Settings.TurnRightEndFlag)
                       stopDelayDistance = CurrentDistance;
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
            if (Settings.TurnRightErrorLight && CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) >= Constants.ErrorSignalCount)
            {
                BreakRule(DeductionRuleCodes.RC30205);
            }
            else
            {


                if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) < Constants.ErrorSignalCount)
                {
                    //是否打转向灯
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
