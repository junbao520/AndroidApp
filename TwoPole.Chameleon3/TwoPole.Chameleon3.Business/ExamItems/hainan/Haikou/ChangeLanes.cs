using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Collections.Specialized;
using System;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.HaiKou.ExamItems
{
    /// <summary>
    /// 变更车道：不能往右变道
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

        //protected IAdvancedCarSignal AdvancedSignal { get; private set; }

        private bool IsSuccess = false;

        #endregion

      
        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.ChangeLanesVoice;
            //AdvancedSignal = Singleton.GetAdvancedCarSignal;
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

        //protected override void OnDrivingOverDistance()
        //{
        //    if (!IsSuccess)
        //    {
        //        BreakRule(DeductionRuleCodes.RC40503);
        //    }
        //    StopCore();
        //}

        //protected override void OnDrivingTimeout()
        //{
        //    if (!IsSuccess)
        //    {
        //        BreakRule(DeductionRuleCodes.RC40503);
        //    }
        //    StopCore();
        //}

        /// <summary>
        /// 变道超距离
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected virtual bool IsOverDistance(CarSignalInfo signalInfo)
        {
            var changeLanesMaxDistance = Settings.ChangeLanesMaxDistance;
            return changeLanesMaxDistance > 0 && (signalInfo.Distance - StartChangeLanesDistance) > changeLanesMaxDistance;
        }

        private bool isOverPrepareDistance = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            ///停车不检测角度
            if (signalInfo.CarState == CarState.Stop)
                return;
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
            //变更车道超时
            if (IsTimeOut())
            {
                if (!IsSuccess)
                {
                    BreakRule(DeductionRuleCodes.RC40503);
                }

                StopCore();
                return;
            }

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

            if (IsLightChecked == false && IsSuccess==false)
            {
                if (signalInfo.Sensor.RightIndicatorLight)
                {
                    IsLightChecked = true;
                    CheckRule(true, DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
                }
            }

            Logger.InfoFormat("变道流程状态： {0}", StepState.ToString());

            if (Settings.ChangeLanesAngle>0)
            {
                if (StepState == ChangeLanesStep.None)
                {
                    //判断车辆是否在进行变道
                    if (signalInfo.BearingAngle.IsValidAngle() &&
                        StartChangeLanesAngle.IsValidAngle() &&
                        !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartChangeLanesAngle, Settings.ChangeLanesAngle))
                    {
                        StepState = ChangeLanesStep.StartChangeLanes;
                        ChangeLanesStartTime = DateTime.Now;
                    }
                }
                else if (StepState == ChangeLanesStep.StartChangeLanes)
                {
                    StepState = ChangeLanesStep.EndChangeLanes;
                    if (Settings.ChangeLanesLightCheck && IsLightChecked==false)
                    {
                        IsLightChecked = true;
                        //是否打转向灯
                        var hasTurnLight = CarSignalSet.Query(StartChangeLanesTime).Any(d => d.Sensor.LeftIndicatorLight);
                        
                        if (!hasTurnLight)
                        {
                            CheckRule(true,DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
                        }
                        else
                        {
                            //转向灯是否超过3秒
                            var hasIndicatorLight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime);
                            if (hasIndicatorLight)
                                return;

                            BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020603);
                        }
                    }
                }
                else
                {
                    IsSuccess = true;
                    //完成变道，结束该项目
                    return;
                    //StopCore();
                }
            }
            else if(Settings.ChangeLanesAngle==0)
            {
                IsSuccess = true;
            }

            base.ExecuteCore(signalInfo);
        }


        private bool IsLightChecked = false;
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

                //


            }
            //夜考双闪
            if (Settings.ChangeLanesLowAndHighBeamCheck && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                if (!AdvancedSignal.CheckHighBeam(StartChangeLanesTime))
                {
                    BreakRule(DeductionRuleCodes.RC41603);
                }
            }
            Logger.InfoFormat("变道结束  当前流程状态;{0} 设定角度：{1} 开始角度：{2} 结束角度：{3}", StepState.ToString(),Settings.ChangeLanesAngle, StartChangeLanesAngle, CarSignalSet.Current.BearingAngle);
      
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
