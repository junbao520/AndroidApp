using System;
using TwoPole.Chameleon3.Infrastructure;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace TwoPole.Chameleon3.Business.ExamItems.YunFu
{
    public enum AroundCarOrder : byte
    {
        W1 = 1,
        W2 = 2,
        W3 = 3,
        W4 = 4
    }

    /// <summary>
    /// 4.0 版本绕车一周采用按钮形式 
    /// 1，插安全带结束上车准备项目 王涛 2016-04-11
    /// 2, Zork:2018-08-30 绕车一周新增2个探头（移植工业）W1=>车尾 左后，W2=》车尾2 右后，W3=》车头2 右前，W4=》 车头 左前
    /// 3，鲍君:添加了车头2 车尾2 对应的信号处理  
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

        private List<AroundCarOrder> ExpectedOrders = new List<AroundCarOrder>();
        private List<AroundCarOrder> ActualOrders = new List<AroundCarOrder>();

        protected bool InitDoorState { get; set; }

        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.AroundCarVoiceEnable;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.AroundCarTimeout);
            if (Settings.PrepareDrivingEnable)
            {
                //修改默认顺序 车尾 车尾2 车头2 车头
                if (Settings.PrepareDrivingTailStockEnable)
                    ExpectedOrders.Add(AroundCarOrder.W1);
                if (Settings.PrepareDrivingTailStock2Enable)
                    ExpectedOrders.Add(AroundCarOrder.W2);
               
                if (Settings.PrepareDrivingHeadStock2Enable)
                    ExpectedOrders.Add(AroundCarOrder.W3);
                if (Settings.PrepareDrivingHeadStockEnable)
                    ExpectedOrders.Add(AroundCarOrder.W4);
            }
        }

        protected override void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<PrepareDrivingFinishedMessage>(this, OnPrepareDrivingFinished);
            base.RegisterMessages(messenger);
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

        protected override void OnDrivingTimeout()
        {
            //超时且没有经过车头车尾
            if (Settings.AroundCarEnable && !CheckAroundCarOrders())
            {
                CheckRule(true, DeductionRuleCodes.RC40101);
            }
            base.OnDrivingTimeout();
        }

        private void OnPrepareDrivingFinished(PrepareDrivingFinishedMessage message)
        {
            //手动触发
            if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.ManualTrigger)
            {
                StopCore();
            }

            return;
        }

        protected override void StopCore()
        {
            //如果绕车一周顺序没有启用了？
            //打印出绕车顺序
            string actualOrders = string.Empty;
            string exceptOrders = string.Empty;
            foreach (var item in ActualOrders)
            {
                actualOrders += item.ToString() + ",";
            }
            foreach (var item in ExpectedOrders)
            {
                exceptOrders += item.ToString()+",";
            }
            //打印出日志

            if (Settings.AroundCarEnable && !CheckAroundCarOrders())
            {
                CheckRule(true, DeductionRuleCodes.RC40101);
            }
            //绕车一周顺序没有启用
            //if (Settings.AroundCarEnable&&!CheckAroundCarNoOrders())
            //{
            //    CheckRule(true, DeductionRuleCodes.RC40101);
            //}
            base.StopCore();
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
                if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.EngineAndSafeBelt)
                {
                    if (signalInfo.Sensor.Engine)
                    {
                        if (signalInfo.Sensor.SafetyBelt)
                        {
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
                if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.SafeBelt)
                {
                    if (signalInfo.Sensor.SafetyBelt)
                    {
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
                if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.Door)
                {
                    if (signalInfo.Sensor.Door != InitDoorState && signalInfo.Sensor.Door)
                    {
                        InitDoorState = signalInfo.Sensor.Door;
                        StopCore();
                        return;
                    }
                    else if (signalInfo.Sensor.Door != InitDoorState && !signalInfo.Sensor.Door)
                    {
                        StopCore();
                        return;
                    }
                }

                var sensor = signalInfo.Sensor;
                var ok = false;
                //读取最后一个信号
                //难道是信号持续来？明白
                var last = ActualOrders.LastOrDefault();
                if (Settings.PrepareDrivingTailStockEnable && sensor.ArrivedTailstock)
                {
                    if (last != AroundCarOrder.W1 && !ActualOrders.Contains(AroundCarOrder.W1))
                    {
                        Speaker.PlayAudioAsync(Settings.PrepareDrivingTailstockVoice);
                        ok = AddOrder(AroundCarOrder.W1);
                    }
                       
                    //if (!ok) StopCore();
                }
                if (Settings.PrepareDrivingTailStock2Enable && sensor.ArrivedTailstock2)
                {
                    if (last != AroundCarOrder.W2 && !ActualOrders.Contains(AroundCarOrder.W2))
                    { 
                    Speaker.PlayAudioAsync(Settings.PrepareDrivingTailstock2Voice);
                    ok = AddOrder(AroundCarOrder.W2);
                    }
                    //if (!ok) StopCore();
                    // return;
                }
                if (Settings.PrepareDrivingHeadStock2Enable && sensor.ArrivedHeadstock2)
                {
                    if (last != AroundCarOrder.W3 && !ActualOrders.Contains(AroundCarOrder.W3))
                        {
                        Speaker.PlayAudioAsync(Settings.PrepareDrivingHeadstock2Voice);
                        ok = AddOrder(AroundCarOrder.W3);
                       }
                        
                    //if (!ok) StopCore();
                    //return;
                }
                if (Settings.PrepareDrivingHeadStockEnable && sensor.ArrivedHeadstock)
                {
                    if (last != AroundCarOrder.W4 && !ActualOrders.Contains(AroundCarOrder.W4))
                    {
                        Speaker.PlayAudioAsync(Settings.PrepareDrivingHeadstockVoice);
                        ok = AddOrder(AroundCarOrder.W4);
                    }
                }
            }

            base.ExecuteCore(signalInfo);
        }

        private bool AddOrder(AroundCarOrder order)
        {
            //明白了 信号会持续来 需要进行过滤掉重复的
            //去掉重复的 这样才对
            if (!ActualOrders.Contains(order))
            {
                ActualOrders.Add(order);
            }
            
            return true;
            //对比2个队列是否一致
            //for(var i = 0; i < ActualOrders.Count; i++)
            //{
            //    if(ExpectedOrders[i] != ActualOrders[i])
            //    {
            //        return false;
            //    }
            //}
            //return true;
        }
        //private bool IsPrepareDrivingSuccess()
        //{
            
        //}

        private bool CheckAroundCarOrders()
        {
            //未按规定顺序绕车一周
            if (ActualOrders.Count<ExpectedOrders.Count)
                return false;
            //对比2个队列是否一致
            for (var i = 0; i < ExpectedOrders.Count; i++)
            {
                if (ExpectedOrders[i] != ActualOrders[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 绕车一周无顺序
        /// </summary>
        /// <returns></returns>
        public bool CheckAroundCarNoOrders()
        {
            //if (Settings.PrepareDrivingTailStockEnable)
            //    if (true)
            //    {

            //    }
            //    ExpectedOrders.Add(AroundCarOrder.W1);
            //if (Settings.PrepareDrivingTailStock2Enable)
            //    ExpectedOrders.Add(AroundCarOrder.W2);
            //if (Settings.PrepareDrivingHeadStock2Enable)
            //    ExpectedOrders.Add(AroundCarOrder.W3);
            //if (Settings.PrepareDrivingHeadStockEnable)
            //    ExpectedOrders.Add(AroundCarOrder.W4);
            //就是一个包含关系了
            var NotThrough= ExpectedOrders.Where(s => ActualOrders.Contains(s)==false);
            //等于0 就是成功的 不等于0 就是失败的
            return NotThrough.Count() == 0;
          
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PrepareDriving; }
        }
    }
}
