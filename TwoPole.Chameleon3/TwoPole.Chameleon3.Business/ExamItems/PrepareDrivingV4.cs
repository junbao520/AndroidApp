using System;
using TwoPole.Chameleon3.Infrastructure;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 4.0 版本绕车一周采用按钮形式 
    /// 1，插安全带结束上车准备项目 王涛 2016-04-11
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


        protected override void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<PrepareDrivingFinishedMessage>(this, OnPrepareDrivingFinished);
            base.RegisterMessages(messenger);
        }
        private void OnPrepareDrivingFinished(PrepareDrivingFinishedMessage message)
        {

            bool IsBroken = false;
            //手动触发
            if (Settings.PrepareDrivingEndFlag==PrepareDrivingEndFlag.ManualTrigger)
            {
                //如果设置为不评判
                if (message.IsJudge==false)
                {
                    StopCore();
                    return;
                }
                if (Settings.PrepareDrivingHeadStockEnable)
                {
                    if (!ArrivedHeadstock)
                    {
                        IsBroken = true;
                        BreakRule(DeductionRuleCodes.RC40101);
                    }
                }
                if (Settings.PrepareDrivingTailStockEnable&&!IsBroken)
                {
                    if (!ArrivedTailstock)
                    {
                        IsBroken = true;
                        BreakRule(DeductionRuleCodes.RC40101);
                    }
                }
                StopCore();
            }
    
            return;
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
                //绕车一周语音
                Speaker.PlayAudioAsync("请绕车一周检查车辆外观及安全情况", SpeechPriority.Normal);
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

        
                //Logger.Error("PrepareDrivingEndFlae:Enter" + Settings.PrepareDrivingEndFlag.ToString());
                if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.EngineAndSafeBelt)
                {
                    if (signalInfo.Sensor.Engine)
                    {
                        if (signalInfo.Sensor.SafetyBelt)
                        {
                            if (!ArrivedHeadstock || !ArrivedTailstock)
                            {
                                BreakRule(DeductionRuleCodes.RC40101);
                            }
                            if (Settings.CommonExamItemsCheckHandBreake)
                            {
                                //如果没有拉手刹
                                if (!signalInfo.Sensor.Handbrake)
                                {
                                    BreakRule(DeductionRuleCodes.RC30103);
                                }
                            }
                            StopCore();
                            return;
                        }
                    }
                }

                else if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.SafeBelt)
                {
                    if (signalInfo.Sensor.SafetyBelt)
                    {
                        if (!ArrivedHeadstock || !ArrivedTailstock)
                        {
                            BreakRule(DeductionRuleCodes.RC40101);
                        }
                        //客户特殊规则
                        //客户特殊规则
                        //客户特殊规则要求拉手刹
                        if (Settings.CommonExamItemsCheckHandBreake)
                        {
                            //如果没有拉手刹
                            if (!signalInfo.Sensor.Handbrake)
                            {
                                BreakRule(DeductionRuleCodes.RC30103);
                            }
                        }
                        StopCore();
                        return;
                    }
                }

                else if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.Door)
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

                //todo:难道是信号不稳定
                else if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.Brake)
                {
                    if (signalInfo.Sensor.Brake)
                    {
                    
                        if (Settings.AroundCarEnable && (!ArrivedHeadstock || !ArrivedTailstock))
                        {
                            CheckRule(true, DeductionRuleCodes.RC40101);
                        }

                        StopCore();
                        return;
                    }
                }
            if (Settings.AroundCarEnable)
            {

                //还有安全带加发动机的情况

                if (ArrivedHeadstock && ArrivedTailstock)
                {
                    return;
                }

                var arrivedHead = signalInfo.Sensor.ArrivedHeadstock;
                var arrivedTail = signalInfo.Sensor.ArrivedTailstock;

                if (!Settings.PrepareDrivingTailStockEnable)
                {
                    //判断学员是否经过车尾 //
                    //
                    if (!ArrivedTailstock && arrivedTail && ArrivedHeadstock)
                    {
                        ArrivedTailstock = true;
                        Thread.Sleep(4000);
                        Speaker.PlayAudioAsync(Settings.PrepareDrivingTailstockVoice, SpeechPriority.Normal);
                        return;
                    }
                }
                else
                {
                    if (!ArrivedTailstock && arrivedTail)
                    {
                        ArrivedTailstock = true;
                        //学员正经过车尾
                        //延时3秒钟播放语音 
                        //Speaker.PlayAudioAsync("学员正经过车尾", SpeechPriority.Normal);
                        //通过车尾语音
                        Speaker.PlayAudioAsync(Settings.PrepareDrivingTailstockVoice, SpeechPriority.Normal);

                        //绕车顺序启用
                        if (!ArrivedHeadstock && Settings.PrepareDrivingOrder)
                        {
                            BreakRule(DeductionRuleCodes.RC40101);
                        }
                        return;
                    }
                }
                if (Settings.PrepareDrivingHeadStockEnable)
                {

                    if (!ArrivedHeadstock && arrivedHead)
                    {
                        ArrivedHeadstock = true;
                        //学员正经过车头
                        //Speaker.PlayAudioAsync("学员正经过车头", SpeechPriority.Normal);
                        Speaker.PlayAudioAsync(Settings.PrepareDrivingHeadstockVoice, SpeechPriority.Normal);
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
