using System;
using TwoPole.Chameleon3.Infrastructure;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{
    /// <summary>
    /// 特殊，
    /// 
    /// </summary>
    public class PrepareDrivingV4 : TwoPole.Chameleon3.Business.ExamItems.PrepareDrivingV4
    {

        protected override void RegisterMessages(IMessenger messenger)
        {

            if (Settings.NeutralStart)
            {
                //界面配置，启用空挡打火
                messenger.Register<EngineChangedMessage>(this, OnEngineChanged);
            }

            base.RegisterMessages(messenger);

        }
        private void OnEngineChanged(EngineChangedMessage message)
        {
            if (message.NewValue)
            {
                if (!CarSignalSet.Current.Sensor.IsNeutral)
                {
                    BreakRule(DeductionRuleCodes.RC40204);
                }
            }
        }

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
                        //if (!ArrivedHeadstock && Settings.PrepareDrivingOrder)
                        //{
                        //    BreakRule(DeductionRuleCodes.RC40101);
                        //}
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
                        //绕车顺序改为逆时针绕车
                        if (!ArrivedTailstock && Settings.PrepareDrivingOrder)
                        {
                            BreakRule(DeductionRuleCodes.RC40101);
                        }
                        return;
                    }
                }


            }

            base.ExecuteCore(signalInfo);
        }
        protected override void StopCore()
        {
            //结束时，不启动发动机不合格
            //会不会是在

            //todo:不清楚是不是这个报错了 导致没有能够结束掉考试项目 //有可能
            //Logger.Error("PrepareDriving Stop Begin");
            if (!CarSignalSet.Current.Sensor.Engine)
            {
                //Logger.Error("发动机未启动扣分:");
                BreakRule(DeductionRuleCodes.RC30103);
            }
            //Logger.Error("PrepareDriving Stop End");
            base.StopCore();
        }
    }
}
