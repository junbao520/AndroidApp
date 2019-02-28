using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 1,检测车门未完全关闭起步
    /// 2,起步时致使发动机转速过高
    /// 3,不按考试员指令驾驶,在规定的时间起步
    /// 4,起步前不使用或错误使用转向灯
    /// 5,不能合理使用喇叭
    /// 6,夜考检查起步前灯光
    /// 7,起步闯动检测（王涛，2015-11-30/40209）
    /// if 1==1 else 2==2 else if 3==3,如果 1==1,2==2,3==3,4==4.
    /// 智能盒子,//Why try catch  //为什么try catch
    /// 健身这个还是挺不错的。  
    ///
    /// 晚上可以吃了晚饭再去
    /// 简单的吃一点不然吃晚饭可能太晚了
    /// 没有什么是不可以的。
    /// 尝试新的解决方案
    /// 
    /// TwoPole.Chameleon3.Business.ExamItems.VehicleStarting,TwoPole.Chameleon3.Business
    /// </summary>
    public class VehicleStarting : ExamItemBase
    {
        protected bool isBrokenStartEngineRpmRule = false;
        protected IAdvancedCarSignal AdvancedCarSignal { get; set; }

        protected DateTime StartMovingTime { get; set; }

        protected bool isBrokenStartShock = false;

        protected bool initG = false;
        private double gx = double.NaN;
        private double gy = double.NaN;
        private double gz = double.NaN;
        private int cd_cnt = 0;
        private bool IsCheckReleaseHandbrake = false;
        private DateTime? StartCheckReleaseHandbrake { get; set; }
        protected override void StartCore(ExamItemExecutionContext context, CancellationToken token)
        {
            Logger.Debug("起步开始");
            base.StartCore(context, token);
        }

        protected override void StopCore()
        {
            Logger.Debug("起步结束");
            base.StopCore();
        }

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            IsFirstCarMoving = true;
            AdvancedCarSignal = Singleton.GetAdvancedCarSignal;
            VoiceExamItem = Settings.StartVoice;
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

        private bool IsMustGearOneSpeaked = false;

        //是否按喇叭
        private bool IsLoudspeaker = false;
        protected DateTime? startMovingCarTime { get; set; }
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
             initG = false;
             gx = double.NaN;
             gy = double.NaN;
             gz = double.NaN;
             cd_cnt = 0;
            return base.InitExamParms(signalInfo);
        }
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            ////初始化加速度
            if (!initG)
            {
                initG = true;
                this.gx = signalInfo.AccelerationX;
                this.gy = signalInfo.AccelerationY;
                this.gz = signalInfo.AccelerationZ;
            }

            StartShockCheck(signalInfo);

            if (Settings.StartEngineRpm > 0 && signalInfo.EngineRpm > Settings.StartEngineRpm && !isBrokenStartEngineRpmRule)
            {
                isBrokenStartEngineRpmRule = true;
                BreakRule(DeductionRuleCodes.RC40210);
            }
            if (signalInfo.CarState != CarState.Moving)
                return;
            //检测起步//起步
            //检测起步
            //检测起步警报灯延时两秒
            if (startMovingCarTime != null && (DateTime.Now - startMovingCarTime.Value).TotalSeconds > 2 &&
                Settings.IsCheckStartLightOnNight && Context.ExamTimeMode == ExamTimeMode.Night &&
               !IsCautionLightSpeaked && CarSignalSet.Query(StartMovingTime).Any(d => d.Sensor.CautionLight))
            {
                IsCautionLightSpeaked = true;
                BreakRule(DeductionRuleCodes.RC41601, DeductionRuleCodes.SRC4160105);
            }
            //todo:始终没有档位检测是不好检测的。
            if (!IsMustGearOneSpeaked && Settings.StartIsMustGearOne && !signalInfo.Sensor.Clutch)
            {
                if (signalInfo.Sensor.Gear == Domain.Gear.Three|| signalInfo.Sensor.Gear == Domain.Gear.Two|| signalInfo.Sensor.Gear == Domain.Gear.Four|| signalInfo.Sensor.Gear == Domain.Gear.Five)
                {
                    IsMustGearOneSpeaked = true;
                    BreakRule(DeductionRuleCodes.RC30137);
                }
         
            }

            if (signalInfo.Sensor.Loudspeaker)
            {
                IsLoudspeaker = true;
            }
            if (IsFirstCarMoving)
            {
                if (!startMovingCarTime.HasValue)
                {
                    StartMovingTime = DateTime.Now;
                    startMovingCarTime = DateTime.Now;
                    return;
                }
                //这样有问题

                IsFirstCarMoving = false;
                //起步左转向灯检查
                if (Settings.IsCheckStartLight)
                {
                    //白天只对左转向灯进行检测
                    if (CarSignalSet.Query(StartTime).Count(d => d.Sensor.LeftIndicatorLight) < Constants.ErrorSignalCount)
                    {
                        BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020501);
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

                //检测喇叭
                if ((Settings.VehicleStartingLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day) ||
                    (Settings.VehicleStartingLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night))
                {
                    //如果没有按喇叭
                    if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.Loudspeaker)&&!IsLoudspeaker)
                    {
                       BreakRule(DeductionRuleCodes.RC40208);
                    }
                }

                //检测是否车门未关闭起步
                if (signalInfo.Sensor.Door)
                    BreakRule(DeductionRuleCodes.RC40202);
            }

            
           
            //TODO:起步项目才会去评判手刹
            CheckHandbrake(signalInfo);
            //当车辆停止的时候且向前运行一段距离后，停止检测
            //当前
            if (signalInfo.CarState == CarState.Stop && (signalInfo.Distance - StartDistance) > 0)
            {
                StopCore();
            }

            //只要超过一定距离起步就结束
        
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
        /// <summary>
        /// 检测起步
        /// </summary>
        private void StartShockCheck(CarSignalInfo signalInfo)
        {
            if (!Settings.StartShockEnable)
                return;

            if(Settings.StartShockValue<=0||Settings.StartShockCount<=0)
               return;

            if (!isBrokenStartShock)
            {
                if (initG && ((Math.Abs(signalInfo.AccelerationX - gx) > Settings.StartShockValue) || Math.Abs(signalInfo.AccelerationY - gy) > Settings.StartShockValue))
                {
                    //Logger.InfoFormat("初始GX值：{0}，当前GX值：{1}。初始GY值：{2}，当前GY值：{3}",gx,signalInfo.AccelerationX,gy,signalInfo.AccelerationY);
                    cd_cnt += 1;
                }
                else
                {
                    cd_cnt = 0;
                }

                if (cd_cnt >= Settings.StartShockCount)
                {
                    isBrokenStartShock = true;
                    BreakRule(DeductionRuleCodes.RC40209);
                }
            }


        }
        public override string ItemCode
        {
            get { return ExamItemCodes.Start; }
        }
    }
}
