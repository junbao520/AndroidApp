
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Areas.Chongqin.Fuling.ExamItems
{
    /// <summary>
    /// 超车
    /// 1,在规定的距离和时间超车
    /// 2,速度限制
    /// 3,超车灯光检测
    /// 4,是否在规则的速度内超车
    /// 5，有返回原车道
    /// 6，停车不评判
    /// 改为和通用版一样，20180223，李
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


        private double StartChangingLanesDistance { get; set; }

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
                BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010302);
            }
            StopCore();
        }

        protected override void OnDrivingTimeout()
        {
            if (!IsSuccess)
            {
                BreakRule(DeductionRuleCodes.RC30103, DeductionRuleCodes.SRC3010302);
            }
            StopCore();
        }

        private bool isOverPrepareDistance = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!IsLoudSpeakerCheck && signalInfo.Sensor.Loudspeaker)
                IsLoudSpeakerCheck = true;
            //过滤10m
            if (!isOverPrepareDistance)
            {
                //超车10米后才评判
                if (signalInfo.Distance - StartDistance < Settings.OvertakePrepareDistance)
                {
                    return;
                }
                if (signalInfo.BearingAngle.IsValidAngle())
                {
                    isOverPrepareDistance = true;
                    StartAngle = signalInfo.BearingAngle;
                }
                return;
            }
            //检测是否超速
            if (Settings.OvertakeSpeedLimit > 0 && signalInfo.SpeedInKmh > Settings.OvertakeSpeedLimit)
            {
                Logger.Error("OvertakeSpeedLimitOver" + signalInfo.SpeedInKmh.ToString());
                //CheckRule(true, DeductionRuleCodes.RC30116);
            }

            if (Settings.OvertakeLowestSpeed > 0 && signalInfo.SpeedInKmh < Settings.OvertakeLowestSpeed)
            {
                Logger.Error("OvertakeSpeedLimitLow" + signalInfo.SpeedInKmh.ToString());
                // CheckRule(true, DeductionRuleCodes.RC30116);
            }

            //检测是否低于规定速度

            //Logger.InfoFormat("OverTakeTest {0}", OvertakeStepState.ToString());
            //检测开始超车转向角度
            if (OvertakeStepState == OvertakeStep.None)
            {
                if (signalInfo.BearingAngle.IsValidAngle() &&
                   !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.OvertakeChangeLanesAngle))
                {
                    //设置开始变道时的距离
                    OvertakeStepState = OvertakeStep.StartChangeLanesToLeft;
                    StartChangingLanesDistance = signalInfo.Distance;

                    if (Settings.OvertakeLightCheck && !IsCheckedOvertakeLeftLight)
                    {
                        IsCheckedOvertakeLeftLight = true;
                        //超车，左转向灯
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
                if (!GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartChangeLanesAngle, Settings.OvertakeChangeLanesAngle))
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



            base.ExecuteCore(signalInfo);
        }


        protected override void StopCore()
        {
            //判断车辆是否返回原车道
            if (OvertakeStepState == OvertakeStep.EndChangeLanesToLeft)
            {
                BreakRule(DeductionRuleCodes.RC41408);
            }

            //检查喇叭
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
}
