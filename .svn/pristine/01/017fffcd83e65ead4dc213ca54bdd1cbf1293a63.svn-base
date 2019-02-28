using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Collections.Specialized;
using System;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{
    /// <summary>
    /// 超车
    /// 1,在规定的距离和时间超车
    /// 2,速度限制
    /// 3,超车灯光检测
    /// 4,是否在规则的速度内超车
    /// 5，有返回原车道
    /// 6，停车不评判
    /// 7.新添加功能 超车变更车道如果角度设置未0 不进行角度评判
    /// 
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
        //达到一次速度，3秒12个信号
        private PriorityQueue<double> _reachSpeedSet = new PriorityQueue<double>();
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
            //检测是否超速

            //检测打错灯，一进入就打右转
            //if (OvertakeStepState == OvertakeStep.None &&  !IsCheckedOvertakeLeftLight)
            //{
            //    //超车，左转向灯,或者右灯判错
            //    if (signalInfo.Sensor.RightIndicatorLight)
            //    {
            //        IsCheckedOvertakeLeftLight = true;
            //  CheckRule(true, DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
            //    }
            //}

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
                        //if (signalInfo.Distance - StartChangingLanesDistance.Value < 10)
                        //{
                        //    return;
                        //}

                        //设置开始变道时的距离
                        OvertakeStepState = OvertakeStep.StartChangeLanesToLeft;
                        //动方向就要评判那个额喇叭
                        if (!IsLoudSpeakerCheck)
                        {
                            if (Settings.OvertakeLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night)
                            {
                                BreakRule(DeductionRuleCodes.RC30212);
                            }
                            if (Settings.OvertakeLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day)
                            {
                                BreakRule(DeductionRuleCodes.RC30212);
                            }
                        }

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
                }
                else if (OvertakeStepState == OvertakeStep.StartChangeLanesToLeft)
                {
                    //当开始变道后向前行驶15米则认为变道成功
                    if (signalInfo.Distance - StartChangingLanesDistance >= OvertakeChangingLanesSuccessOrBackToOriginalLanceDistance &&
                        signalInfo.BearingAngle.IsValidAngle())
                    {
                        //路径
                        //
                        OvertakeStepState = OvertakeStep.EndChangeLanesToLeft;
                        StartChangeLanesAngle = signalInfo.BearingAngle;

                        //如果设置了不需要返回原车道

                        if (!Settings.OvertakeBackToOriginalLane)
                        {
                            OvertakeStepState = OvertakeStep.EndChangeLanesToRight;
                        }
                        else
                        {
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
                                BreakRule("302071");
                            }
                            else
                            {
                                if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                                {
                                    BreakRule("302081");
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
                        //只有泸州版本才这样
                        //todo:之所以不分出来 是觉得其实全国所有的版本都可以这样做
                        BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020506);
                        IsCheckedOvertakeLeftLight = true;
                    }
                }
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
            //判断车辆是否返回原车道
            if (Settings.OvertakeChangeLanesAngle > 0)
            {
                //if (OvertakeStepState == OvertakeStep.EndChangeLanesToLeft)
                //{
                //    BreakRule(DeductionRuleCodes.RC41408);
                //}
                if (!IsSuccess)
                {
                    CheckRule(true, DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010302);
                }

            }
            //如果角度设置为0 只需要检测转向灯
            if (Settings.OvertakeChangeLanesAngle == 0)
            {

                //TODO:海南海口版本特殊要求 其他地方不适合
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
                //判断是否返回原车到    //检测右转
                if (Settings.OvertakeBackToOriginalLane)
                {
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


            //检查喇叭
           

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
