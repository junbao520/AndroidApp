using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.ExamItems.GuLin
{
    /// <summary>
    /// 1,掉头前未发出掉头信号
    /// 2,掉头生命周期检测
    /// 3,缓踩刹车
    /// 4,在规定的时间内完成掉头
    /// 5,停车不检测角度
    /// 特殊规则：第二次掉头只播报语音
    /// </summary>
    public class TurnRound : ExamItemBase
    {
        protected TurnRoundStep StepState { get; set; }

        protected bool IsCheckedTurnRoundLight { get; set; }
        protected bool IsCheckedLowAndHighBeam { get; set; }

        protected bool IsLoudSpeakerCheck { get; set; }
        //#endregion


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.TurnRoundVoice;
            MaxDistance = Settings.TurnRoundMaxDistance;

            //如果不是第一次掉头
            if (Constants.IsSecondTurnRound)
            {
                MaxDistance = 3;
                //VoiceExamItem = false;
                //EndVoiceExamItem = false;
                //播放语音点
                //第二次掉头重复播放语音
                //Speaker.PlayAudioAsync("前方请选择合适地点掉头");
            }
        }

        ///初始化变量
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //初始化方向角
            if (signalInfo.BearingAngle.IsValidAngle())
            {
                StepState = TurnRoundStep.None;
                StartAngle = signalInfo.BearingAngle;
                StartDistance = signalInfo.Distance;
                return base.InitExamParms(signalInfo);
            }
            return false;
        }

        protected override void OnDrivingOverDistance()
        {
            if (Constants.IsSecondTurnRound)
            {
                StopCore();
                return;
            }
            //超过距离
            BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010305);
            StopCore();
        }

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!InitializedExamParms)
                return;
            if (Constants.IsSecondTurnRound)
            {
                return;
            }
            //喇叭检测
            if (signalInfo.Sensor.Loudspeaker)
                IsLoudSpeakerCheck = true;

            //检测远近光交替
            if (Settings.TurnRoundLightCheck && !IsCheckedLowAndHighBeam)
                IsCheckedLowAndHighBeam = signalInfo.Sensor.HighBeam;

            //检测右转
            if (Settings.TurnRoundErrorLight && !IsCheckedTurnRoundLight)
            {
                if (signalInfo.Sensor.RightIndicatorLight)
                {
                    IsCheckedTurnRoundLight = true;
                    CheckRule(true, DeductionRuleCodes.RC41503);
                }
            }

            ///停车不检测角度
            if (signalInfo.CarState == CarState.Stop)
                return;

            if (StepState == TurnRoundStep.None)
            {
                //检测方向角是否开始调头
                //Gps角度其实也是可以通过一些方法滤波的
                if (signalInfo.BearingAngle.IsValidAngle() && StartAngle.IsValidAngle() &&
                    !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.TurnRoundStartAngleDiff))
                {
                    StepState = TurnRoundStep.StartTurnRound;
                }
            }
            else if (StepState == TurnRoundStep.StartTurnRound)
            {
                //开始掉头检测灯光
                if (!IsCheckedTurnRoundLight)
                {
                    IsCheckedTurnRoundLight = true;
                    if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight))
                    {
                        CheckRule(true, DeductionRuleCodes.RC41503);
                    }
                    else
                    {
                        var isOk = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                        if (!isOk)
                            BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020607);
                    }
                    ////
                    //if (CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                    //{
                    //    BreakRule(DeductionRuleCodes.RC30206);
                    //}
                }

                //检测是否掉头完毕
                if (signalInfo.BearingAngle.IsValidAngle() &&
                    StartAngle.IsValidAngle() &&
                    !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.TurnRoundEndAngleDiff))
                {
                    if (Settings.TurnRoundBrakeRequired &&
                        !CarSignalSet.Query(StartTime).Any(x => x.Sensor.Brake))
                        BreakRule(DeductionRuleCodes.RC41505);
                    StepState = TurnRoundStep.EndTurnRound;
                }
            }
            else
            {
                StopCore();
            }

            base.ExecuteCore(signalInfo);
        }

        protected override void StopCore()
        {

            //第二次掉头部评判
            if (Constants.IsSecondTurnRound)
            {
                base.StopCore();
                return;
            }
            //第一次完成后 第二次快速完成
            Constants.IsSecondTurnRound = true;
            //远近光交替
            if (Settings.TurnRoundLightCheck &&
            !IsCheckedLowAndHighBeam &&
            Context.ExamTimeMode == ExamTimeMode.Night)
            {
                BreakRule(DeductionRuleCodes.RC41603);
            }

            //白天喇叭
            if (!IsLoudSpeakerCheck &&
                Settings.TurnRoundLoudSpeakerDayCheck &&
                Context.ExamTimeMode == ExamTimeMode.Day)
            {
                BreakRule(DeductionRuleCodes.RC30212);
            }

            //夜间喇叭
            if (!IsLoudSpeakerCheck &&
                Settings.TurnRoundLoudSpeakerNightCheck &&
                Context.ExamTimeMode == ExamTimeMode.Night)
            {
                BreakRule(DeductionRuleCodes.RC30212);
            }

            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.TurnRound; }
        }
    }

    public enum TurnRoundStep
    {
        None = 0,
        StartTurnRound = 1,
        EndTurnRound = 2
    }
}
