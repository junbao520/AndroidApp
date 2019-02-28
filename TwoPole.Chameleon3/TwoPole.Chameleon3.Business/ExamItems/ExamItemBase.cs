using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;
using System.IO;
using System.Diagnostics;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    public abstract class ExamItemBase : DisposableBase, IExamItem, IProvider
    {
        private ExamItemState _examItemState = ExamItemState.None;
        protected IAdvancedCarSignal AdvancedSignal { get; private set; }

        #region 属性
        protected ICarSignalSet CarSignalSet
        {
            get { return Singleton.GetCarSignalSet; }
        }
        /// <summary>
        /// 获取所有的考试规则
        /// </summary>
        public IList<IRule> Rules { get;set; }
        public IList<IRule> TempRules { get;  set; }
        public IList<IRule> ActivedRules { get; protected set; }

        protected ILog Logger { get; set; }
        protected IExamScore ExamScore { get; private set; }
        protected IMessenger Messenger { get; private set; }

        protected ISpeaker Speaker { get; private set; }

        public ExamItemExecutionContext Context { get; set; }

        public IDictionary<object, object> Properties { get; private set; }


        //todo://初始 静态变量不属于每一个具体项目,故同事触发多个项目可能存在问题
        //可以考虑结束的时候重新初始化距离。
        /// <summary>
        /// 开始的里程  
        /// </summary>
        public  double StartDistance = -1;
        /// <summary>
        /// 开始的航向角
        /// </summary>
        public double StartAngle { get; protected set; }

          /// <summary>
         /// 开始的时间
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary>
        /// 项目的距离
        /// </summary>
        public double? MaxDistance { get; protected set; }
        /// <summary>
        /// 考试的最大时间
        /// </summary>
        public TimeSpan MaxElapsedTime { get; protected set; }

        public MapPoint TriggerPoint { get; protected set; }
        public GlobalSettings Settings { get; private set; }

        protected bool InitializedExamParms { get; set; }

        protected bool IsEndStartCore=false;

        
        #endregion

        #region Properties

        protected bool TryGetProperty(object key, out object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return Properties.TryGetValue(key, out value);
        }
        protected TProperty GetProperty<TProperty>()
        {
            object v = null;
            TryGetProperty(typeof(TProperty), out v);
            return (v != null) ? (TProperty)v : default(TProperty);
        }

        protected void PutProperty<TProperty>(TProperty property)
        {
            Properties[typeof(TProperty)] = property;
        }

        #endregion

        protected ExamItemBase()
        {
            ExamScore = Singleton.GetExamScore;
            Speaker = Singleton.GetSpeaker;
            Settings = Singleton.GetDataService.GetSettings();
            Messenger = Singleton.GetMessager;
            AdvancedSignal = Singleton.GetAdvancedCarSignal;
            Logger = Singleton.GetLogManager;
            Properties = new Dictionary<object, object>();
            InitializedExamParms = false;
            //初始化项目结束语音操作
        }

        protected void OnStateChanged(ExamItemState old, ExamItemState newValue)
        {
            Logger.InfoFormat("项目{0}状态改变：New，{1}-Old，{2}", this.Name, newValue, old);
            var message = new ExamItemStateChangedMessage(newValue, old, this);
            Messenger.Send(message);
        }

       //拉手刹 退空挡 松安全带 开光车门  发动机不需要熄火
       //拉起手 刹 

        #region ICarSignalDependency

        public int Order { get; protected set; }
        /// <summary>
        /// 当前行驶的距离
        /// </summary>
        protected double CurrentDistance { get; set; }
        /// <summary>
        /// 执行考试项目
        /// </summary>
        /// <param name="signalInfo"></param>
        public void Execute(CarSignalInfo signalInfo)
        {
           
            var result = BeforeExecute(signalInfo);
            if (!result)
                return;
            CurrentDistance = signalInfo.Distance;
            try
            {
                StaticClass.StaticDistance = signalInfo.Distance;
                StaticClass.StaticAngle = signalInfo.BearingAngle;
            }
            catch (Exception ex)
            {
                Logger.Error("ExamItemBaseExecute", ex.Message);
            }
         
            if (!InitializedExamParms)
                InitializedExamParms = InitExamParms(signalInfo);

            ExecuteCore(signalInfo);

            if (IsEndStartCore)
            {
               // Logger.DebugFormat("IsEndStartCore");
                AfterExecute(signalInfo);
            }
        }

        #endregion

        #region 子类重载
        /// <summary>
        /// 执行前做一些检查工作
        /// 当前状态为正在执行，并且信号有效
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected virtual bool BeforeExecute(CarSignalInfo signalInfo)
        {
            //todo:修复项目一开始就结束的情况，原因赋值开始距离(StartCore)和检测超距(OnCarSignalReceived)是异步的两个线程，
            //造成检测超距时，StartDistance还没被赋值(默认为0),一下就超距了
            if (StartDistance < 1)
            {
                StartTime = DateTime.Now;
                //每个项目只触发一次
                StartDistance = signalInfo.Distance < 1 ? 1 : signalInfo.Distance;
            }
            //return this.State == ExamItemState.Progressing && signalInfo.IsValid;
            return this.State == ExamItemState.Progressing && signalInfo.Sensor != null && signalInfo.IsSensorValid;
        }

        protected virtual bool InitExamParms(CarSignalInfo signalInfo)
        {
            return true;
        }

        protected virtual bool ValidTimeout(CarSignalInfo signalInfo)
        {
            if (TimeSpan.Zero != MaxElapsedTime &&
                DateTime.Now - StartTime > MaxElapsedTime)
            {
                Logger.DebugFormat("项目：{0}超过时：{1}", this.Name, MaxElapsedTime);
                return false;
            }
            return true;
        }

        protected virtual bool ValidDistance(CarSignalInfo signalInfo)
        {
            //if (StartDistance == -1 || StartDistance == 0)
            //{
            //    var currentSignal = CarSignalSet.Query(DateTime.Now - TimeSpan.FromSeconds(12)).FirstOrDefault(s => s.Distance != 0);
            //    if (currentSignal != null)
            //    {
            //        StartDistance = currentSignal.Distance;
            //        StartAngle = currentSignal.BearingAngle;
            //    }
            //    Logger.Debug("ValidDistacnce", StartDistance.ToString());
            //}
            //if (StartDistance==0||StartDistance==-1)
            //{
            //    MaxElapsedTime = new TimeSpan(0, 0, 10);
            //    //重新设置时间
            //    Logger.Debug(Name, "Valid Distance ExamItemBase MaxElapsedTime Reset");
            //    return true;
            //}
            //其实我是需要测试是否还有播报项目立即结束
            //如果距离小于等于10米则直接重新设置距离默认100米
            //如果距离不行就设置时间
            //Todo:这个东西需要了解系统底层的一些框架架构呀
            //Todo:
            if (MaxDistance.HasValue && MaxDistance > 0 && signalInfo.Distance - StartDistance > MaxDistance)
            {
                Logger.DebugFormat("项目：{0}超过里程：{1}；{2}-{3}", this.Name, MaxDistance, StartDistance, signalInfo.Distance);
                return false;
            }
         
            return true;
        }

        /// <summary>
        /// 执行任务的核心方法
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ExecuteCore(CarSignalInfo signalInfo)
        {
      
            var finishedRules = new List<IRule>();
            if (Rules != null)
            {
                foreach (var rule in Rules)
                {
                    var result = rule.Check(signalInfo);
                    switch (result)
                    {
                        case RuleExecutionResult.Continue:
                            break;
                        case RuleExecutionResult.Break:
                            return;
                        case RuleExecutionResult.Finish:
                            finishedRules.Add(rule);
                            break;
                        case RuleExecutionResult.FinishExamItem:
                            this.StopAsync().Wait();
                            return;
                    }
                }
            }
           
            //移除所有完成的规则类；
            Rules.RemoveRange(finishedRules);
        }

        /// <summary>
        /// 执行后的一些清理工作
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void AfterExecute(CarSignalInfo signalInfo)
        {
            var isContinue = ValidDistance(signalInfo);
            if (!isContinue)
            {
                OnDrivingOverDistance();
                return;
            }

            isContinue = ValidTimeout(signalInfo);
            if (!isContinue)
            {
                OnDrivingTimeout();
            }
        }

        protected virtual void OnDrivingOverDistance()
        {
            StopCore();
        }

        protected virtual void OnDrivingTimeout()
        {
            StopCore();
        }

        protected virtual void RegisterMessages(IMessenger messenger)
        {
        }

        protected virtual IRule[] GetActiveRules(ExamItemExecutionContext context)
        {
            if (Rules == null)
                return new IRule[0];

            IEnumerable<IRule> query = Rules;
            //分组过滤
            if (!string.IsNullOrEmpty(context.ExamGroup))
                query = Rules.Where(x => string.IsNullOrEmpty(x.Group) || x.Group == context.ExamGroup);

            //匹配夜间、白天模式
            var isNight = context.ExamTimeMode == ExamTimeMode.Night;
            query = query.Where(x => x.TimeMode == RuleTimeMode.Both ||
                                     isNight == (x.TimeMode == RuleTimeMode.Night));
            return query.ToArray();
        }

        protected virtual void StartCore(ExamItemExecutionContext context, CancellationToken token)
        {
            IsEndStartCore = false;
            Logger.DebugFormat("启动项目：{0}", this.Name);
            Context = context;
            StartTime = DateTime.Now;
            State = ExamItemState.Progressing;
            try
            {
                var currentSignal = CarSignalSet.Current;
                TriggerPoint = context.TriggerPoint;
                if (currentSignal != null)
                {
                    StartDistance = currentSignal.Distance;
                    StartAngle = currentSignal.BearingAngle;
                    Logger.DebugFormat("Current 考试项目：{0} 里程 :{1}", this.Name, StartDistance);
                }
                else
                {
                    Logger.Debug("CurrentSignal is null");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("StartCore", ex.Message);
            }
            if (StartDistance==0)
            {
                StartDistance = StaticClass.StaticDistance;
                StartAngle = StaticClass.StaticAngle;
                Logger.DebugFormat("Static考试项目：{0} 里程 :{1}", this.Name, StartDistance);
            }
            //综合评判开始项目没有语音播报
            if (ItemCode!=ExamItemCodes.CommonExamItem)
            {
                if (VoiceFile.Trim()=="滴")
                {
                    Speaker.SpeakBreakeVoice();
                }
                else
                {
                    Speaker.PlayAudioAsync(VoiceFile, SpeechPriority.High);
                }
   
                Logger.Debug(VoiceFile, "Playuccess");
            }
         
            IsEndStartCore = true;
        }

        protected virtual void StopCore()
        {   try
            {
                Speaker.PlayAudioAsync(EndVoiceFile,SpeechPriority.High);
                State = ExamItemState.Finished;
            }
            catch (Exception exp)
            {
                State = ExamItemState.Finished;
                Logger.Error(exp, ItemCode);
                //发生一场了 也要结束考试项目
            }            
        }
        #endregion

        #region IExamItem

        /// <summary>
        /// 考试项目名称
        /// 考试
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 考试项目编号
        /// </summary>
        public virtual string ItemCode { get; set; }
        /// <summary>
        /// 考试项目语音文本
        /// </summary>
        public string VoiceText { get; set; }
        /// <summary>
        /// 考试项目语音文件
        /// </summary>
        public virtual string VoiceFile { get; set; }

        /// <summary>
        /// 考试项目结束语音文件
        /// </summary>
        public virtual string EndVoiceFile { get; set; }
        /// <summary>
        /// 是否语音播报考试项目
        /// </summary>
        protected virtual bool VoiceExamItem { get; set; }

        /// <summary>
        /// 是否语音播报考试项目结束语音
        /// </summary>
        protected virtual bool EndVoiceExamItem { get; set; }
        /// <summary>
        /// 考试项目状态
        /// </summary>
        public ExamItemState State
        {
            get { return _examItemState; }
            private set
            {
                if (_examItemState != value)
                {
                    var old = _examItemState;
                    _examItemState = value;
                    OnStateChanged(old, value);
                }
            }
        }

        public virtual void SetRules(IEnumerable<IRule> rules)
        {
            Rules = rules.OrderBy(x => x.Order).ToList();
            Rules.ForEach(x => x.ExamItem = this);


            TempRules = rules.OrderBy(x => x.Order).ToList();
            TempRules.ForEach(x => x.ExamItem = this);
        }

        public virtual Task StartAsync(ExamItemExecutionContext context, CancellationToken token)
        {
            return Task.Run(() =>
            {
                ActivedRules = GetActiveRules(context);
                RegisterMessages(Messenger);
                StartCore(context, token);
            }, token);
        }

        /// <summary>
        /// 结束、终止考试
        /// </summary>
        public Task StopAsync()
        {
            //20151216,李，结束考试时不播报项目结束语
            VoiceExamItem = false;
            EndVoiceExamItem = false;

            return Task.Run(() => StopCore());
        }
        #endregion

        #region IProvider

        public virtual void Init(NameValueCollection settings)
        {
            //解决占位符语音内容无法替换问题；
            //this.Name = settings.GetValue("name", this.Name ?? string.Empty);
            //this.TextToSpeach = settings.GetValue("textToSpeach", this.TextToSpeach ?? string.Empty);
            //this.EndTextToSpeach = settings.GetValue("endTextToSpeach", this.EndTextToSpeach ?? string.Empty);
            //this.ItemCode = settings.GetValue("itemCode", this.ItemCode ?? string.Empty);
           // this.VoiceExamItem = settings.GetBooleanValue("VoiceExamItem", true);
            //设置EndVoiceExamItem的值
          
        }
        #endregion

        #region DisposableBase

        protected override void Free(bool disposing)
        {
            if (Rules != null)
            {
                foreach (var rule in Rules)
                {
                    var disposable = rule as IDisposable;
                    if (disposable != null)
                    {
                        try
                        {
                            disposable.Dispose();
                        }
                        catch (Exception exp)
                        {
                            Logger.ErrorFormat("释放Rule {0} 发生异常，原因：{1}", rule.Name, exp, exp);
                        }
                    }
                }
                if (!this.Rules.IsReadOnly)
                    this.Rules.Clear();
            }
            this.Rules = new IRule[0];
            Messenger.Unregister(this);
        }
        #endregion

        #region Broken Rule

        protected virtual void BreakRule(string ruleCode, string subRuleCode = null)
        {
            ExamScore.BreakRule(ItemCode, Name, ruleCode, subRuleCode);
        }

        public virtual bool CheckRule(Func<bool> checker, string ruleCode, string subRuleCode = null)
        {
            var result = checker();
            return CheckRule(result, ruleCode, subRuleCode);
        }

        public virtual bool CheckRule(bool isBroken, string ruleCode, string subRuleCode = null)
        {
            bool state = true;
            if (isBroken)
            {
                if (!_brokenRuleStateMap.ContainsKey(ruleCode))
                {
                    _brokenRuleStateMap.Add(ruleCode, false);
                    BreakRule(ruleCode, subRuleCode);
                    return false;
                }
                _brokenRuleStateMap.TryGetValue(ruleCode, out state);
                if (state)
                {
                    _brokenRuleStateMap[ruleCode] = false;
                    //验证规则失败，抛送触犯规则消息
                    BreakRule(ruleCode, subRuleCode);
                }
            }
            else
            {
                _brokenRuleStateMap.TryGetValue(ruleCode, out state);
                if (true != state)
                {
                    _brokenRuleStateMap[ruleCode] = true;
                }
            }

            return isBroken;
        }

        protected void ClearBrokenRuleState()
        {
            _brokenRuleStateMap.Clear();
        }


        private readonly IDictionary<string, bool> _brokenRuleStateMap = new Dictionary<string, bool>();
        #endregion

        #region 通用功能

        #region  达到一次速度（如，35码，4档，3秒）
        protected bool IsReachedGlobalLowestSpeed = false;
        protected DateTime? IniTime { get; set; }
        private bool hasGear = false;

        /// <summary>
        /// 达到一次速度（4档，3秒）
        /// </summary>
        protected void CheckOverOnceSpeed(CarSignalInfo signalInfo)
        {
            //GlobalLowestGear
            if (!IsReachedGlobalLowestSpeed)
            {
                if (signalInfo.SpeedInKmh > Settings.GlobalLowestSpeed && !IniTime.HasValue)
                    IniTime = DateTime.Now;
                if (signalInfo.SpeedInKmh < Settings.GlobalLowestSpeed)
                    IniTime = null;
                //有档位要求
                if (Settings.GlobalLowestGear > 0)
                {
                    if (signalInfo.Sensor.Gear == (Gear)Settings.GlobalLowestGear)
                        hasGear = true;

                    if (IniTime.HasValue && hasGear &&
                        (DateTime.Now - IniTime.Value).TotalSeconds > Settings.GlobalLowestSpeedHoldTimeSeconds)
                    {
                        IsReachedGlobalLowestSpeed = true;
                        //无单独加减档项目，达到40码，3秒，4档则认为加减档完成，用于改变颜色
                       Messenger.Send(new ModifyGearOverMessage() { PassedItemCode = ExamItemCodes.ModifiedGear });
                    }
                }
                else
                {
                    if (IniTime.HasValue &&
                        (DateTime.Now - IniTime.Value).TotalSeconds > Settings.GlobalLowestSpeedHoldTimeSeconds)
                    {
                        IsReachedGlobalLowestSpeed = true;
                        //无单独加减档项目，达到40码，3秒，4档则认为加减档完成，用于改变颜色
                        Messenger.Send(new ModifyGearOverMessage() { PassedItemCode = ExamItemCodes.ModifiedGear });
                    }
                }
            }
        }


        protected bool brakeVoiceSpeaked = false;
        protected virtual void CheckBrakeVoice(CarSignalInfo signalInfo)
        {
            if (!Settings.CommonExamItemsBreakVoice)
            {
                return;
            }
            if (signalInfo.Sensor.Brake && !brakeVoiceSpeaked)
            {
                brakeVoiceSpeaked = true;
                Speaker.SpeakBreakeVoice();
                //   Speaker.PlayAudioAsync("咚");
            }
            if (!signalInfo.Sensor.Brake)
            {
                brakeVoiceSpeaked = false;
            }
        }

        #endregion
        #endregion
    }
}
