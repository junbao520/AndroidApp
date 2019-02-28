using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System.Collections.Specialized;
using System;
using System.Linq;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Business.ExamItems;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.ExamItems
{
    /// <summary>
    /// 1,掉头前未发出掉头信号
    /// 2,掉头生命周期检测
    /// 3,缓踩刹车
    /// 4,在规定的时间内完成掉头
    /// 5,距离结束才结束项目
    /// </summary>
    public class TurnRound : ExamItemBase
    {
        protected TurnRoundStep StepState { get; set; }

        protected bool IsCheckedTurnRoundLight { get; set; }
        protected bool IsCheckedLowAndHighBeam { get; set; }

        protected bool IsLoudSpeakerCheck { get; set; }
        //#endregion
       

        private bool IsSuccess = false;
       

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.TurnRoundVoice;
            MaxDistance = Settings.TurnRoundMaxDistance;
        }

        ///初始化变量
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            IsSuccess = false;
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
            //超过距离

            if (!IsSuccess)
            {
                BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010305);
            }
 
            StopCore();
        }

        //是否已经播报乱打转向灯
        private bool isSpeakIndicator = false;
        //是否已经打过左转灯，掉头打完左转向，在打右转向不扣分，20160323
        private bool IsOpenLeftIndicatorLight = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!InitializedExamParms)
                return;

            //乱打灯不扣分，20160323，李
            if (!IsOpenLeftIndicatorLight && signalInfo.Sensor.LeftIndicatorLight)
            {
                IsOpenLeftIndicatorLight = true;
            }
            //掉头打完左转向，在打右转向不扣分，20160323
            if (!isSpeakIndicator && !IsOpenLeftIndicatorLight &&
                CarSignalSet.Query(StartTime).Count(d => d.Sensor.RightIndicatorLight) >= Constants.ErrorSignalCount)
            {
                isSpeakIndicator = true;
                BreakRule(DeductionRuleCodes.RC40212);
            }

            //喇叭检测
            if (signalInfo.Sensor.Loudspeaker)
                IsLoudSpeakerCheck = true;

            //检测远近光交替
            if (Settings.TurnRoundLightCheck && !IsCheckedLowAndHighBeam)
                IsCheckedLowAndHighBeam = signalInfo.Sensor.HighBeam;

            if (StepState == TurnRoundStep.None)
            {
                //检测方向角是否开始调头
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
                        BreakRule(DeductionRuleCodes.RC41503);
                    }
                    else
                    {
                        //打了右转向灯进行评判
                        if (!IsOpenLeftIndicatorLight && CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                        {
                            BreakRule(DeductionRuleCodes.RC30205);
                            return;
                        }

                        var isOk = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                        if (!isOk)
                            BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020607);
                    }
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
                IsSuccess = true;
                return;
                //StopCore();
            }

            base.ExecuteCore(signalInfo);
        }

        protected override void StopCore()
        {
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
}
