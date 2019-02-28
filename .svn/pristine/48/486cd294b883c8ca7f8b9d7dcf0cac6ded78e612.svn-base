using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Collections.Specialized;
using System;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 变更车道：左、右转向灯都可以
    /// </summary>
    internal class ChangeLanes : ExamItemBase
    {
        #region 规则检测
        /// <summary>
        /// 1，变更车道前打转向灯
        /// 2，在规定的距离和时间变道
        /// 3，速度限制
        /// 4, 项目距离结束才进行评判
        /// </summary>

        #endregion

        #region 私有变量

        private ChangeLanesStep StepState = ChangeLanesStep.None;

        private double StartChangeLanesAngle { get; set; }

        private DateTime StartChangeLanesTime { get; set; }

        /// <summary>
        /// 当开始转向时时间
        /// </summary>
        private DateTime ChangeLanesStartTime { get; set; }

        private double StartChangeLanesDistance { get; set; }

        private bool IsSuccess = false;

        #endregion


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.ChangeLanesVoice;
            //AdvancedSignal = Singleton.GetAdvancedCarSignal;
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
                //StopCore();
            }
        }

        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            IsSuccess = false;
            //航向角无效，不记录
            if (!signalInfo.BearingAngle.IsValidAngle())
                return false;

            StepState = ChangeLanesStep.None;
            StartChangeLanesAngle = signalInfo.BearingAngle;
            StartChangeLanesTime = DateTime.Now;
            StartChangeLanesDistance = signalInfo.Distance;
            Constants.ChangeLaneDistance = 0;
            return base.InitExamParms(signalInfo);
        }

        /// <summary>
        /// 变道超时
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsTimeOut()
        {
            var changeLanesTimeout = Settings.ChangeLanesTimeout;
            return changeLanesTimeout > 0 && (DateTime.Now - StartChangeLanesTime).TotalSeconds > changeLanesTimeout;
        }
        /// <summary>
        /// 变道超距离
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected virtual bool IsOverDistance(CarSignalInfo signalInfo)
        {
            var changeLanesMaxDistance = Settings.ChangeLanesMaxDistance;
            return changeLanesMaxDistance > 0 && (signalInfo.Distance - Constants.ChangeLaneDistance) > changeLanesMaxDistance;
        }

        private bool isOverPrepareDistance = false;
        private double? ChangeStartDistance { get; set; }
        private bool IsSecondVoice = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (Constants.ChangeLaneDistance == 0)
            {
                Constants.ChangeLaneDistance=signalInfo.Distance;
            }
            if (stopDelayDistance.HasValue)
            {
                if ((signalInfo.Distance - stopDelayDistance.Value) > 10)
                {
                    StopCore();
                }
            }
            if (signalInfo.CarState == CarState.Stop)
                return;
            //播报第2次语音
            if (IsSecondVoice == false && !string.IsNullOrEmpty(Settings.ChangeLanesSecondVoice) && Settings.ChangeLanesPrepareDistance > 1)
            {
                IsSecondVoice = true;
                Speaker.PlayAudioAsync(Settings.ChangeLanesSecondVoice, SpeechPriority.Normal);
            }
            //过滤10m
            if (!isOverPrepareDistance)
            {
                if (signalInfo.Distance - StartChangeLanesDistance < Settings.ChangeLanesPrepareDistance)
                {
                    return;
                }
                else
                {
                    isOverPrepareDistance = true;
                    StartChangeLanesAngle = signalInfo.BearingAngle;
                    return;
                }
            }
            //不要超时避免误判
            //不要吵
            //if (IsTimeOut())
            //{
            //    if (!IsSuccess)
            //    {
            //        BreakRule(DeductionRuleCodes.RC40503);
            //    }

            //    StopCore();
            //    return;
            //}

            //变更车道超距
            if (IsOverDistance(signalInfo))
            {
                if (!IsSuccess)
                {
                    BreakRule(DeductionRuleCodes.RC40503);
                }
                StopCore();
                return;
            }
            Logger.InfoFormat("变道流程状态： {0}", StepState.ToString());
            //Logger.Error("变道流程状态"+StepState.ToString()+"ChangeLanes" + signalInfo.BearingAngle.ToString()+ "StartChangeLanesAngle:"+ StartChangeLanesAngle.ToString()+ "Settings.ChangeLanesAngle:" + Settings.ChangeLanesAngle.ToString());
            if (Settings.ChangeLanesAngle > 0)
            {
                if (StepState == ChangeLanesStep.None)
                {
                    //变道摆正10米后才进行下面的检测(转向灯评判太快，车还没摆正)
                    if (ChangeStartDistance.HasValue)
                    {
                        if (signalInfo.Distance - ChangeStartDistance.Value > Settings.ChangeLanesPrepareDistance)
                        {
                            StepState = ChangeLanesStep.StartChangeLanes;
                        }
                    }
                    //判断车辆是否在进行变道
                    if (signalInfo.BearingAngle.IsValidAngle() &&
                        StartChangeLanesAngle.IsValidAngle() &&
                        !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartChangeLanesAngle, Settings.ChangeLanesAngle))
                    {
                        StepState = ChangeLanesStep.StartChangeLanes;
                        ChangeStartDistance = signalInfo.Distance;
                        ChangeLanesStartTime = DateTime.Now;
                    }
                }
                else if (StepState == ChangeLanesStep.StartChangeLanes)
                {
                    StepState = ChangeLanesStep.EndChangeLanes;
                    if (Settings.ChangeLanesLightCheck)
                    {
                        //只能左转
                        if (Settings.ChangelineDirect.Equals(1))
                        {
                            //是否打转向灯
                            var hasTurnLight = CarSignalSet.Query(StartChangeLanesTime).Any(d => d.Sensor.LeftIndicatorLight);
                            var hasErrorLight = CarSignalSet.Query(StartChangeLanesTime).Any(d => d.Sensor.RightIndicatorLight);
                            if (!hasTurnLight || hasErrorLight)
                            {
                                BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
                            }
                            else
                            {
                                //转向灯是否超过3秒
                                var hasIndicatorLight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                                if (hasIndicatorLight)
                                    return;
                                BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020603);
                            }
                        }//只能右转
                        else if (Settings.ChangelineDirect.Equals(2))
                        {
                            //是否打转向灯
                            var hasTurnLight = CarSignalSet.Query(StartChangeLanesTime).Any(d => d.Sensor.RightIndicatorLight);
                            var hasErrorLight = CarSignalSet.Query(StartChangeLanesTime).Any(d => d.Sensor.LeftIndicatorLight);
                            if (!hasTurnLight || hasErrorLight)
                            {
                                BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
                            }
                            else
                            {
                                //转向灯是否超过3秒
                                var hasIndicatorLight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                                if (hasIndicatorLight)
                                    return;
                                BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020603);
                            }
                        }
                        else
                        {
                            //是否打转向灯
                            var hasTurnLight = CarSignalSet.Query(StartChangeLanesTime).Any(d => d.Sensor.LeftIndicatorLight || d.Sensor.RightIndicatorLight);
                            if (!hasTurnLight)
                            {
                                BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
                            }
                            else
                            {
                                //转向灯是否超过3秒
                                var hasIndicatorLight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight || x.Sensor.RightIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                                if (hasIndicatorLight)
                                    return;
                                BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020603);
                            }
                        }
                    }
                }
                else
                {
                    IsSuccess = true;
                    //完成变道，结束该项目
                    if (Settings.ChkChangeLanesEndFlag && !stopDelayDistance.HasValue)
                        stopDelayDistance = signalInfo.Distance;
                    return;
                }
            }
            else if (Settings.ChangeLanesAngle == 0)
            {
                IsSuccess = true;
            }

            base.ExecuteCore(signalInfo);
        }

        private double? stopDelayDistance { get; set; }

        protected override void StopCore()
        {

            //最后判断是否有打转向灯
            if (Settings.ChangeLanesAngle == 0)
            {

                //如果没打左转
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
            //夜考双闪
            if (Settings.ChangeLanesLowAndHighBeamCheck && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                if (!AdvancedSignal.CheckHighBeam(StartChangeLanesTime))
                {
                    BreakRule(DeductionRuleCodes.RC41603);
                }
            }
            Logger.InfoFormat("变道结束  当前流程状态;{0} 设定角度：{1} 开始角度：{2} 结束角度：{3}", StepState.ToString(), Settings.ChangeLanesAngle, StartChangeLanesAngle, CarSignalSet.Current.BearingAngle);

            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.ChangeLanes; }
        }

    }

    public enum ChangeLanesStep
    {
        None = 0,
        StartChangeLanes = 1,
        EndChangeLanes = 2
    }
}
