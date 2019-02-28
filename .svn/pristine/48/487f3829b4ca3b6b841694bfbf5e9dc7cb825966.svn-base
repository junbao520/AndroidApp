using System;
using TwoPole.Chameleon3.Infrastructure;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Threading;

namespace TwoPole.Chameleon3.Business.Areas.Chongqin.RongChang.ExamItems
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
                Speaker.PlayAudioAsync("请绕车一周检查车辆外观及安全情况",SpeechPriority.Normal);
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

                if (!Settings.PrepareDrivingTailStockEnable)
                {   
                    //判断学员是否经过车尾 //
                    //
                    if (!ArrivedTailstock && arrivedTail&& ArrivedHeadstock)
                    {
                        ArrivedTailstock = true;
                        //学员正经过车尾
                        //延时3秒钟播放语音 
                        Thread.Sleep(4000);
                        Speaker.PlayAudioAsync("正经过车尾",SpeechPriority.Normal);
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
                        Speaker.PlayAudioAsync("正经过车尾", SpeechPriority.Normal);
                        return;
                    }
                }
                if (Settings.PrepareDrivingHeadStockEnable)
                {    
    
                    if (!ArrivedHeadstock && arrivedHead)
                    {
                        ArrivedHeadstock = true;
                        //学员正经过车头
                        Speaker.PlayAudioAsync("学员正经过车头", SpeechPriority.Normal);
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
