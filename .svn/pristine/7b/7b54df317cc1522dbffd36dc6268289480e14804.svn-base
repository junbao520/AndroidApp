using System;
using TwoPole.Chameleon3.Infrastructure;
using System.Collections.Specialized;

namespace TwoPole.Chameleon3.Business.ExamItems.LongQuan
{
    /// <summary>
    /// 成都龙泉中控客户,上车准备
    /// 成都客户特殊处理，上车准备。
    /// 上车准备结束标志，开光车门。
    /// </summary>
    public class PrepareDrivingV4 : ExamItemBase
    {
        #region 检测项目
        /// <summary>
        /// 1，是否启用项目
        /// 2，检测是否绕车一周检查
        /// 3，绕车一周是否超时
        /// </summary>
        #endregion

        #region 私有变量
        protected bool ArrivedTailstock { get; set; }
        protected bool ArrivedHeadstock { get; set; }
        protected bool InitDoorState { get; set; }

        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.AroundCarVoiceEnable;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.AroundCarTimeout);
        }

        protected override void OnDrivingTimeout()
        {
            //超时且没有经过车头车尾
            if (Settings.AroundCarEnable && (!ArrivedHeadstock || !ArrivedTailstock))
            {
                BreakRule(DeductionRuleCodes.RC40101);
            }
            base.OnDrivingTimeout();
        }

        protected override bool BeforeExecute(CarSignalInfo signalInfo)
        {
            return this.State == ExamItemState.Progressing && signalInfo.Sensor != null;
        }



        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            InitDoorState = signalInfo.Sensor.Door;
            if (Settings.AroundCarVoiceEnable)
            {
                Speaker.PlayAudioAsync("请绕车一周检测车辆外观及安全情况");
            }
            if (!Settings.PrepareDrivingHeadStockEnable)
            {
                ArrivedHeadstock = true;
            }
            if (!Settings.PrepareDrivingTailStockEnable)
            {
                ArrivedTailstock = true;
            }
            return base.InitExamParms(signalInfo);
        }
        /// <summary>
        /// 检测传感器
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (Settings.AroundCarTimeout <= 0)
            {
                StopCore();
                return;
            }

            if (Settings.AroundCarEnable)
            {

                if (Settings.PrepareDrivingEndFlag==PrepareDrivingEndFlag.EngineAndSafeBelt)
                {
                    if (signalInfo.Sensor.Engine)
                    {
                        if (signalInfo.Sensor.SafetyBelt)
                        {
                            if (!ArrivedHeadstock || !ArrivedTailstock)
                            {
                                BreakRule(DeductionRuleCodes.RC40101);
                            }
                            StopCore();
                            return;
                        }
                    }
                }

                if (Settings.PrepareDrivingEndFlag==PrepareDrivingEndFlag.SafeBelt)
                {
                    if (signalInfo.Sensor.SafetyBelt)
                    {
                        if (!ArrivedHeadstock || !ArrivedTailstock)
                        {
                            BreakRule(DeductionRuleCodes.RC40101);
                        }
                        StopCore();
                        return;
                    }
                }

                if (Settings.PrepareDrivingEndFlag==PrepareDrivingEndFlag.Door)
                {
                    if (signalInfo.Sensor.Door != InitDoorState && signalInfo.Sensor.Door)
                    {
                        InitDoorState = signalInfo.Sensor.Door;
                        //开门
                        if (!ArrivedHeadstock || !ArrivedTailstock)
                        {
                            BreakRule(DeductionRuleCodes.RC40101);
                            StopCore();
                            return;
                        }

                    }
                    else if (signalInfo.Sensor.Door != InitDoorState && !signalInfo.Sensor.Door)
                    {
                        //关门
                        if (!ArrivedHeadstock || !ArrivedTailstock)
                        {
                            BreakRule(DeductionRuleCodes.RC40101);
                            StopCore();
                            return;
                        }
                    }

                }

                //还有安全带加发动机的情况

                if (ArrivedHeadstock && ArrivedTailstock)
                {
                    return;
                }

                var arrivedHead = signalInfo.Sensor.ArrivedHeadstock;
                var arrivedTail = signalInfo.Sensor.ArrivedTailstock;

                if (Settings.PrepareDrivingTailStockEnable)
                {
                    if (!ArrivedTailstock && arrivedTail)
                    {
                        ArrivedTailstock = true;
                        //学员正经过车尾
                        Speaker.PlayAudioAsync("学员正在经过车尾");
                        return;
                    }
                }
                if (Settings.PrepareDrivingHeadStockEnable)
                {
                    if (!ArrivedHeadstock && arrivedHead)
                    {
                        ArrivedHeadstock = true;
                        Speaker.PlayAudioAsync("学员正在经过车头");
                        //学员正经过车头
                      
                        return;
                    }
                }
              
                
            }

            base.ExecuteCore(signalInfo);
        }

  
        public override string ItemCode
        {
            get { return ExamItemCodes.PrepareDriving; }
        }


    }
}
