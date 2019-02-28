using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.Rules
{
    
    public abstract class RuleBase : DisposableBase, IRule
    {
        protected CarSignalInfo CurrentSignal
        {
            get { return CarSignalSet.Current; }
        }

        protected GlobalSettings Settings { get; private set; }

        #region Services
        protected IMessenger Messenger { get; private set; }
        protected ISpeaker Speaker { get; private set; }
        protected ICarSignalSet CarSignalSet
        {
            get { return Singleton.GetCarSignalSet; }
        }
    
        #endregion

        protected RuleBase()
        {
            Messenger = Singleton.GetMessager;
            Speaker = Singleton.GetSpeaker;
            Settings = Singleton.GetDataService.GetSettings();
        }

        #region IProvider
        public virtual void Init(NameValueCollection settings)
        {
            //this.Name = settings.GetValue("Name", this.Name ?? string.Empty);
            //this.Order = settings.GetIntValue("Order", this.Order);
            //this.Group = settings.GetValue("Group", this.Group ?? string.Empty);
            //this.RuleCode = settings.GetValue("RuleCode", RuleCode);
            //this.SubRuleCode = settings.GetValue("SubRuleCode", SubRuleCode);
            //this.TimeMode = (RuleTimeMode) settings.GetIntValue("TimeMode", (int) RuleTimeMode.Both);
        }
        #endregion

        protected virtual void RegisterMessages(IMessenger messenger)
        {
        }
        
        #region IRule
        public virtual string RuleCode { get; set; }
        public virtual string SubRuleCode { get; private set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 规则序号
        /// </summary>
        public virtual int Order { get; private set; }
        /// <summary>
        /// 规则执行的模式
        /// </summary>
        public virtual RuleTimeMode TimeMode { get;  set; }
        /// <summary>
        /// 规则分组
        /// </summary>
        public virtual string Group { get; set; }
        /// <summary>
        /// 规则所属考试项目
        /// </summary>
        public IExamItem ExamItem { get; set; }

        public virtual RuleExecutionResult Check(CarSignalInfo carSignal)
        {
            return RuleExecutionResult.Continue;
        }

        public virtual void BreakRule()
        {
            ExamItem.CheckRule(true, RuleCode, SubRuleCode);
        }

        public virtual void BreakRule(bool isBroken)
        {
            ExamItem.CheckRule(isBroken, RuleCode, SubRuleCode);
        }
        #endregion

        protected override void Free(bool disposing)
        {
            ExamItem = null;
            Messenger.Unregister(this);
        }
    }
}
