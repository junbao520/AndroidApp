using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Collections.Specialized;
using System;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.ExamItems.YunFu
{
    /// <summary>
    ///  Zork:2018-08-30 辛苦了 Zork Thanks
    /// 超车
    /// 1,在规定的距离和时间超车
    /// 2,速度限制
    /// 3,超车灯光检测
    /// 4,是否在规则的速度内超车
    /// 5，有返回原车道
    /// 6，停车不评判
    /// 7. 新添加功能 超车变更车道如果角度设置未0 不进行角度评判
    /// 8 .客户需求 再超车道上面可以打等也可以不打灯 再行车到上面 超出之后必须要返回原车到 鲍君 2018-09-03
    /// 前提：如果动方向就认为再行车道上面 如果不动就认为再超车道上面
    /// </summary>
    public class Overtake : ExamItemBase
    {
        #region 私有变量
        /// <summary>
        /// 开始变道的角度
        /// </summary>
        protected double StartChangeLanesAngle { get; set; }

        private bool IsCheckedOvertakeLeftLight { get; set; }

        private bool IsCheckedOvertakeRightLight { get; set; }


        private bool IsLoudSpeakerCheck = false;


        private double? StartChangingLanesDistance { get; set; }

        /// <summary>
        /// 变更车道步骤状态
        /// </summary>
        protected OvertakeStep OvertakeStepState = OvertakeStep.None;

        /// <summary>
        /// 变更车道距离
        /// </summary>
        //当车辆开始变道到变道结束距离参数
        protected double OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance = 30;

        #endregion

        /// <summary>
        /// 是否已经完成超车动作
        /// </summary>
        private bool IsSuccess = false;

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            MaxDistance = Settings.OvertakeMaxDistance;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.OvertakeTimeout);
            VoiceExamItem = Settings.OvertakeVoice;
            OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance = Settings.OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance;
            //返回原车道的播报距离
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //记录开始超车时的初始角度
            if (signalInfo.BearingAngle.IsValidAngle())
            {
                StartAngle = signalInfo.BearingAngle;
                StartTime = DateTime.Now;
                StartDistance = signalInfo.Distance;

                return base.InitExamParms(signalInfo);
            }
            else
            {
                return false;
            }
        }

        protected override void OnDrivingOverDistance()
        {
            if (!IsSuccess)
            {
                //TODO:客户反馈要扣分 测试没发现 屏蔽掉 影响不大 20180421 鲍君
                // BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010302);
            }
            StopCore();
        }

        protected override void OnDrivingTimeout()
        {
            if (!IsSuccess)
            {
                //TODO:客户反馈要扣分 测试没发现 屏蔽掉 影响不大 20180421 鲍君
                // BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010302);
            }
            StopCore();
        }

        private bool IsCheckOverSpeed = false;

        private bool isOverPrepareDistance = false;

        //默认是在超车道上面 超车道上面不会进行扣分 除非超速
        private bool isInOverTakeLine=true;

        //达到一次速度，3秒12个信号
        //TODO:这个为何没有使用？好像不是我定义的  鲍君 2018-09-03
        private PriorityQueue<double> _reachSpeedSet = new PriorityQueue<double>();
        /// <summary>
        /// 是否需要判断返回原车到（根据角度来判断是否报返回和判右灯）
        /// </summary>
        private bool IsNeedOvertakeBacked = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!IsLoudSpeakerCheck && signalInfo.Sensor.Loudspeaker)
                IsLoudSpeakerCheck = true;

            //过滤10m
            //准备距离0 处理
            if (!isOverPrepareDistance && Settings.OvertakePrepareDistance > 0)
            {
                //超车10米后才评判
                if (signalInfo.Distance - StartDistance < Settings.OvertakePrepareDistance)
                    return;
                if (signalInfo.BearingAngle.IsValidAngle())
                {
                    isOverPrepareDistance = true;
                    StartAngle = signalInfo.BearingAngle;
                }
                return;
            }

            //TODO：以前有部分客户反馈是会2扣分得。原因不明。不知道是不是信号不稳定影响得。有时间可以打开在多次测试 2018.04.22 鲍君
            if (IsCheckOverSpeed == false && ((Settings.OvertakeSpeedLimit > 0 && signalInfo.SpeedInKmh > Settings.OvertakeSpeedLimit) ||
                (Settings.OvertakeLowestSpeed > 0 && signalInfo.SpeedInKmh < Settings.OvertakeLowestSpeed)))
            {
                IsCheckOverSpeed = true;
                CheckRule(true, DeductionRuleCodes.RC30116);
            }

            //达到一次速度
            if (Settings.OvertakeSpeedOnce > 10)
            {
                if (signalInfo.SpeedInKmh > Settings.OvertakeSpeedOnce)
                    _reachSpeedSet.Enqueue(signalInfo.SpeedInKmh);
                if (_reachSpeedSet.Count > 30)
                    _reachSpeedSet.Dequeue();
            }

            //检测是否低于规定速度
            //Logger.InfoFormat("OverTakeTest {0}", OvertakeStepState.ToString());
            //检测开始超车转向角度
            //如果角度大于0
            if (Settings.OvertakeChangeLanesAngle > 0)
            {
                if (OvertakeStepState == OvertakeStep.None)
                {
                    if (signalInfo.BearingAngle.IsValidAngle() &&
                       !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.OvertakeChangeLanesAngle))
                    {
                        //设置开始变道时的距离(由于评判转向灯太快，所以延后大概1秒)
                        if (!StartChangingLanesDistance.HasValue)
                            StartChangingLanesDistance = signalInfo.Distance;

                        //变道摆正10米后才开始评判
                        //TODO:科目三可以配置的 准备距离
                        if (signalInfo.Distance - StartChangingLanesDistance.Value<Settings.OvertakePrepareDistance)
                        {
                            return;
                        }

                        //设置开始变道时的距离
                        OvertakeStepState = OvertakeStep.StartChangeLanesToLeft;
                        //已经达到了角度认为是在行车道上面
                        isInOverTakeLine = false;
                        //这个应该没问题
                        Logger.DebugFormat("超车时在超车道");
                        if (Settings.OvertakeLightCheck && !IsCheckedOvertakeLeftLight)
                        {
                            IsCheckedOvertakeLeftLight = true;
                            //超车，左转向灯,或者右灯判错
                            if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight))
                            {
                                CheckRule(true, DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                            }
                            else
                            {
                                if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                                {
                                    BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                                }
                            }
                        }

                        //判断一下是否有点客户不需要返回原车道

                    }
                    //TODO:这样写是有问题的
                    //TODO:这个是什么意思？为什么写死了？ 
                    //else if(signalInfo.Distance - StartDistance > 20)
                    //{
                    //    OvertakeStepState = OvertakeStep.EndChangeLanesToLeft;
                    //    Logger.DebugFormat("超车时在超车道");
                    //}
                }
                else if (OvertakeStepState == OvertakeStep.StartChangeLanesToLeft)
                {
                    //当开始变道后向前行驶15米则认为变道成功
                    if (signalInfo.Distance - StartChangingLanesDistance >= OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance &&
                        signalInfo.BearingAngle.IsValidAngle())
                    {
                        //路径
                        OvertakeStepState = OvertakeStep.EndChangeLanesToLeft;
                        StartChangeLanesAngle = signalInfo.BearingAngle;

                        //如果设置了不需要返回原车道
                        if (!Settings.OvertakeBackToOriginalLane)
                        {
                            OvertakeStepState = OvertakeStep.EndChangeLanesToRight;
                        }
                        else
                        {
                            IsNeedOvertakeBacked = true;
                            Speaker.PlayAudioAsync(Settings.OvertakeBackToOriginalLaneVocie);
                        }
                    }
                }
                else if (OvertakeStepState == OvertakeStep.EndChangeLanesToLeft)
                {
                    if (signalInfo.BearingAngle.IsValidAngle() && !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartChangeLanesAngle, Settings.OvertakeChangeLanesAngle))
                    {
                        OvertakeStepState = OvertakeStep.EndChangeLanesToRight;
                        if (Settings.OvertakeLightCheck && !IsCheckedOvertakeRightLight)
                        {
                            IsCheckedOvertakeRightLight = true;
                            if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                            {
                                BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                            }
                            else
                            {
                                if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                                {
                                    BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                                }
                            }
                        }
                    }
                }
                else if (OvertakeStepState == OvertakeStep.EndChangeLanesToRight)
                {
                    //结束考试项目
                    //StopCore();
                    IsSuccess = true;
                    return;
                }
            }
            else
            {
                //表示客户其实只关心打灯问题
                //简单的处理 如果打右转灯之前没有打过左转灯则扣分
                if (signalInfo.Sensor.RightIndicatorLight && !IsCheckedOvertakeLeftLight)
                {
                    //如果没有打左转灯就扣分
                    if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                    {
                        //CheckRule(true, DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                        BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                        IsCheckedOvertakeLeftLight = true;
                    }
                }
                //    if (signalInfo.Sensor.RightIndicatorLight)
                //    {
                //        IsCheckedOvertakeLeftLight = true;
                //        CheckRule(tru

                IsSuccess = true;
            }
            
            base.ExecuteCore(signalInfo);
        }

        protected override void StopCore()
        {
            //达到一次速度判断
            if (Settings.OvertakeSpeedOnce > 10 && IsCheckOverSpeed == false)
            {
                IsCheckOverSpeed = true;
                if (_reachSpeedSet.Count < 12)
                    CheckRule(true, DeductionRuleCodes.RC30116); ;

            }

            if (IsNeedOvertakeBacked)
            {
                //判断车辆是否返回原车道 判断下客户是否是在超车道上面
                //如果不是在超车道上面才会进行判断
                if (Settings.OvertakeChangeLanesAngle > 0 && !isInOverTakeLine)
                {

                    if (OvertakeStepState == OvertakeStep.EndChangeLanesToLeft)
                    {
                        BreakRule(DeductionRuleCodes.RC41408);
                    }
                    if (!IsSuccess)
                    {
                        CheckRule(true, DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010302);
                    }

                }
            }
            else
            {
                //如果角度未达到 只需要检测转向灯
               

                   
                    //检测左转 
                    if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight))
                    {
                        BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                    }
                    else
                    {
                        if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                        {
                            BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                        }
                    }
                    ////判断是否返回原车到    //检测右转
                    //if (Settings.OvertakeBackToOriginalLane)
                    //{
                    //    if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                    //    {
                    //        BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                    //    }
                    //    else
                    //    {
                    //        if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                    //        {
                    //            BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                    //        }
                    //    }
                    //}

                
            }

            //检查喇叭
            if (!IsLoudSpeakerCheck)
            {
                if (Settings.OvertakeLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night)
                {
                    BreakRule(DeductionRuleCodes.RC30212);
                }
                else if (Settings.OvertakeLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day)
                {
                    BreakRule(DeductionRuleCodes.RC30212);
                }
            }
            //夜考双闪
            if (Settings.OvertakeLowAndHighBeamCheck && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                if (!AdvancedSignal.CheckHighBeam(StartTime))
                {
                    BreakRule(DeductionRuleCodes.RC41603);
                }
            }
            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.Overtaking; }
        }
    }

    public enum OvertakeStep
    {
        None = 0,
        StartChangeLanesToLeft = 1,
        EndChangeLanesToLeft = 2,
        StartChangeLanesToRight = 3,
        EndChangeLanesToRight = 4,
    }
}
