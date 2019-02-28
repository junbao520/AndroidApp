using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.HaiKou.ExamItems
{
    /// <summary>
    /// 1,检测车门未完全关闭起步
    /// 2,起步时致使发动机转速过高
    /// 3,不按考试员指令驾驶,在规定的时间起步
    /// 4,起步前不使用或错误使用转向灯
    /// 5,不能合理使用喇叭
    /// 6,夜考检查起步前灯光
    /// 7，夜间起步语音不一样
    /// 8,起步结束才检测喇叭
    /// 9.打了右转向灯进行评判
    /// </summary>
    public class VehicleStarting : ExamItemBase
    {
        protected bool isBrokenStartEngineRpmRule = false;
        protected IAdvancedCarSignal AdvancedCarSignal { get; set; }

        protected DateTime StartMovingTime { get; set; }
        private bool IsCheckReleaseHandbrake = false;
        private DateTime? StartCheckReleaseHandbrake { get; set; }
        protected override void StartCore(ExamItemExecutionContext context, CancellationToken token)
        {
            Logger.InfoFormat("起步开始");
            base.StartCore(context, token);
        }

        protected override void StopCore()
        {
            //检测喇叭
            if ((Settings.VehicleStartingLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day) ||
                (Settings.VehicleStartingLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night))
            {
                if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.Loudspeaker))
                {
                    BreakRule(DeductionRuleCodes.RC40208);
                }
            }
            //Speaker.PlayAudioAsync("sanya/itemEnd40200.wav", SpeechPriority.Highest);
            Logger.InfoFormat("起步结束");
            base.StopCore();
        }

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            IsFirstCarMoving = true;
            VoiceExamItem = false;

            
            MaxDistance = Settings.StartStopCheckForwardDistance;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.StartTimeout);
        }

        protected override void OnDrivingTimeout()
        {
            BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010301);
            StopCore();
        }

        protected bool IsFirstCarMoving { get; set; }
        //是否播报过警报灯规则
        protected bool IsCautionLightSpeaked = false;
        protected DateTime? startMovingCarTime { get; set; }


        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            
            //进行语音播报
            return base.InitExamParms(signalInfo);
        }

   
        private DateTime? _startTime { get; set; }
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (Settings.StartEngineRpm > 0 && signalInfo.EngineRpm > Settings.StartEngineRpm && !isBrokenStartEngineRpmRule)
            {
                isBrokenStartEngineRpmRule = true;
                BreakRule(DeductionRuleCodes.RC40210);
            }
            if (!_startTime.HasValue)
                _startTime = DateTime.Now;
            if ((DateTime.Now - _startTime.Value).TotalSeconds < 3)
            {
                if (signalInfo.Sensor.LeftIndicatorLight)
                {
                    CheckRule(true, DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020501);
                }
            }
            if (signalInfo.CarState != CarState.Moving)
                return;

            //检测起步警报灯延时两秒
            if (startMovingCarTime != null && (DateTime.Now - startMovingCarTime.Value).TotalSeconds > 2 &&
                Settings.IsCheckStartLightOnNight && Context.ExamTimeMode == ExamTimeMode.Night &&
               !IsCautionLightSpeaked && CarSignalSet.Query(StartMovingTime).Any(d => d.Sensor.CautionLight))
            {
                IsCautionLightSpeaked = true;
                BreakRule(DeductionRuleCodes.RC41601, DeductionRuleCodes.SRC4160105);
            }

            if (IsFirstCarMoving)
            {
                if (!startMovingCarTime.HasValue)
                {
                    StartMovingTime = DateTime.Now;
                    startMovingCarTime = DateTime.Now;
                    return;
                }

                ////座椅起步开始前验证三模
                //if (!CarSignalSet.Query(Context.ExamContext.StartExamTime).Any(d => d.Sensor.Seats))
                //{
                //    BreakRule(DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021101);
                //}
                //if (!CarSignalSet.Query(Context.ExamContext.StartExamTime).Any(d => d.Sensor.ExteriorMirror))
                //{
                //    BreakRule(DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021102);
                //}
                //if (!CarSignalSet.Query(Context.ExamContext.StartExamTime).Any(d => d.Sensor.InnerMirror))
                //{
                //    BreakRule(DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021103);
                //}


                IsFirstCarMoving = false;
                //起步左转向灯检查
                if (Settings.IsCheckStartLight)
                {
                    //白天只对左转向灯进行检测
                    //if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight))
                    //打了右转向灯进行评判
                    if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) < Constants.ErrorSignalCount || CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                    {
                        CheckRule(true, DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020501);
                    }
                    else
                    {
                        //是否提前3秒打转向灯进行检测
                        var isTurnLight = AdvancedCarSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime,
                            Settings.TurnLightAheadOfTime);
                        if (!isTurnLight)
                            BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020601);
                    }
                }

                //夜间灯光检查
                if (Settings.IsCheckStartLightOnNight && Context.ExamTimeMode == ExamTimeMode.Night && !IsCautionLightSpeaked)
                {
                    //夜间起步灯光检测，打近光，关危险警报灯光
                    if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LowBeam) < Constants.ErrorSignalCount ||
                        CarSignalSet.Query(StartMovingTime).Any(d => d.Sensor.HighBeam))
                    {
                        IsCautionLightSpeaked = true;
                        BreakRule(DeductionRuleCodes.RC41601, DeductionRuleCodes.SRC4160105);
                    }


                }

                //夜间远近光交替
                if (Settings.StartLowAndHighBeamInNight && Context.ExamTimeMode == ExamTimeMode.Night)
                {
                    if (!AdvancedCarSignal.CheckHighBeam(StartTime, 1))
                    {
                        BreakRule(DeductionRuleCodes.RC41603);
                    }
                }
                //检测是否车门未关闭起步
                if (signalInfo.Sensor.Door)
                    BreakRule(DeductionRuleCodes.RC40202);
            }
            //检测手刹，综合评判里面检测
            //CheckHandbrake(signalInfo);
            //Logger.InfoFormat("StartEngineRpm:{0}", signalInfo.Sensor.EngineRpm);
            //Logger.InfoFormat("Settings:StartEngineRpm:{0}", Settings.StartEngineRpm);
            //检测发动机转速


            //当车辆停止的时候且向前运行一段距离后，停止检测
            if (signalInfo.CarState == CarState.Stop && (signalInfo.Distance - StartDistance) > 0)
            {
                StopCore();
            }
        }
        private void CheckHandbrake(CarSignalInfo signalInfo)
        {
            if (Settings.StartReleaseHandbrakeTimeout <= 0)
                return;

            if (signalInfo.CarState == CarState.Stop)
            {
                IsCheckReleaseHandbrake = false;
                StartCheckReleaseHandbrake = null;
                return;
            }

            if (signalInfo.Sensor.Handbrake && !StartCheckReleaseHandbrake.HasValue)
            {
                StartCheckReleaseHandbrake = DateTime.Now;
                return;
            }
            //未在规定时间内完成拉起手刹

            if (!IsCheckReleaseHandbrake && StartCheckReleaseHandbrake.HasValue)
            {
                if (!signalInfo.Sensor.Handbrake)
                {
                    IsCheckReleaseHandbrake = true;
                    StartCheckReleaseHandbrake = null;
                    BreakRule(DeductionRuleCodes.RC40214);
                }

                if ((DateTime.Now - StartCheckReleaseHandbrake.Value).TotalSeconds > Settings.StartReleaseHandbrakeTimeout)
                {
                    IsCheckReleaseHandbrake = true;
                    StartCheckReleaseHandbrake = null;
                    BreakRule(DeductionRuleCodes.RC40205);
                }

            }
        }
        public override string ItemCode
        {
            get { return ExamItemCodes.Start; }
        }
    }
}
