using System.Threading;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Collections.Generic;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 灯光模拟
    /// </summary>
    public class SimulationLightsBak : ExamItemBase, ILightExamItem
    {
        protected ILightRule CurrentLightRule { get; set; }

        protected IEnumerable<ILightRule> ActivedLightRules
        {
            get
            {
                return ActivedRules.Cast<ILightRule>();
            }
        }

        private int currentLightRuleIndex = -1;

        public virtual string GetRandomGroup(ExamItemExecutionContext context)
        {
            //var num = (new Random()).Next(0, Groups.Length * 1000);
            //var index = num % Groups.Length;
            //return Groups.ElementAt(index)

            Logger.DebugFormat("模拟灯光:随机分组开始");
            var randomName= GetRandomLightGroup();
            Logger.InfoFormat("模拟灯光:随机分组结束：分组：{0}", randomName);
            return randomName;
        }

        /// <summary>
        /// 从灯光模拟中随机抽取一组
        /// </summary>
        /// <returns></returns>
        private static List<string> triggerExamGroups = new List<string>();
        private static List<string> _tempExamGroups = new List<string>();
        public string GetRandomLightGroup()
        {
            var _group = Groups;

            //进行初始化重置临时列表
            if (_tempExamGroups.Count <= 0)
            {
                foreach (var item in _group)
                {
                    _tempExamGroups.Add(item);
                }
            }

            //进行初始化重置触发项目
            if (triggerExamGroups.Count == _group.Length)
                triggerExamGroups.Clear();


            Random r = new Random();
            int _index = r.Next(0, _tempExamGroups.Count - 1);
            while (triggerExamGroups.Contains(_tempExamGroups[_index]))
            {
                _index = r.Next(0, _tempExamGroups.Count - 1);
            }

            var _examItem = _tempExamGroups[_index];
            triggerExamGroups.Add(_examItem);
            _tempExamGroups.Remove(_examItem);

            return _examItem;
        }

        #region 重写父类方法
        public override Task StartAsync(ExamItemExecutionContext context, CancellationToken token)
        {
            return Task.Run(() =>
            {
                Logger.DebugFormat("模拟灯光：考试开始： {0}", context.ExamGroup);
                if (string.IsNullOrEmpty(context.ExamGroup))
                    context.ExamGroup = GetRandomGroup(context);
                ActivedRules = GetActiveRules(context);
                ClearBrokenRuleState();
                ResetRules();
                //RegisterMessages(Messenger);
                currentLightRuleIndex = 0;
                SetCurrentLightRule(0);
                StartCore(context, token);
                Logger.Debug("StartCore");
            }, token);
        }

        
        protected override IRule[] GetActiveRules(ExamItemExecutionContext context)
        {
            var dataService = Singleton.GetDataService;

            var query = from a in dataService.AllLightExamItems.First(x => x.GroupName == context.ExamGroup).LightRules.Split(',')
                        let b = Convert.ToInt32(a)
                        join c in TempRules.OfType<ILightRule>() on b equals c.Id
                        select (IRule)c;
            var rules = query.ToArray();
           
            return rules;
        }

        #endregion

        protected virtual void ResetRules()
        {
            currentLightRuleIndex = -1;
            foreach (var lightRule in ActivedLightRules)
            {
                lightRule.Reset();
            }
        }


        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            ///检测灯光是否全部是关闭的
            ///

            if (signalInfo.Sensor.HighBeam||
                signalInfo.Sensor.LowBeam||
                signalInfo.Sensor.OutlineLight||
                signalInfo.Sensor.FogLight||
                signalInfo.Sensor.CautionLight||
                signalInfo.Sensor.LeftIndicatorLight||
                signalInfo.Sensor.RightIndicatorLight)
            {

                Logger.InfoFormat("未关闭所有灯光开始灯光模拟");
            }
            return base.InitExamParms(signalInfo);
        }
        /// <summary>
        /// 信号有效即可；GPS无信号也能进行灯光模拟；
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected override bool BeforeExecute(CarSignalInfo signalInfo)
        {
            return State == ExamItemState.Progressing && signalInfo.Sensor != null;
        }

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (CurrentLightRule == null)
            {
                StopCore();
                return;
            }

            var result = CurrentLightRule.Check(signalInfo);
            switch (result)
            {
                case RuleExecutionResult.Continue:
                    return;
                case RuleExecutionResult.Break:
                case RuleExecutionResult.Finish:
                    ////获取下一个项目
                    //var next = ActivedRules.OfType<ILightRule>().SkipWhile(x => x != CurrentLightRule).Skip(1).FirstOrDefault();
                    //SetCurrentLightRule(next);
                    //获取下一个灯光规则
                    currentLightRuleIndex++;
                    SetCurrentLightRule(currentLightRuleIndex);
                    return;
                case RuleExecutionResult.FinishExamItem:
                    break;
            }
        }

        private void SetCurrentLightRule(ILightRule lightRule, bool isFirstRule = false)
        {
                //进行灯光规则重置
                 CurrentLightRule = lightRule;           
                if (CurrentLightRule!=null)
                {
                    CurrentLightRule.Reset();
                    CurrentLightRule.IsFirstRule = isFirstRule;
                }
        }

        private void SetCurrentLightRule(int index)
        {
            //这里面就是直接从预先加载好的灯光规则里面取出来
            //这点其实也可以直接马上进行加载
            var rules = ActivedRules.OfType<ILightRule>().ToArray();
            if (currentLightRuleIndex >= 0)
            {
                if (rules.Count() > index)
                {
                    CurrentLightRule = rules.ToArray()[index];
                    CurrentLightRule.Reset();
                    Logger.InfoFormat("灯光模拟：设置规则：{0}",CurrentLightRule.VoiceFile);
                    return;
                }

            }
            CurrentLightRule = null;
        }

        public override bool CheckRule(Func<bool> checker, string ruleCode, string subRuleCode = null)
        {
            base.BreakRule(ruleCode, subRuleCode);
            return true;
        }

        public override bool CheckRule(bool isBroken, string ruleCode, string subRuleCode = null)
        {
            base.BreakRule(ruleCode, subRuleCode);
            return true;
        }


        public override string ItemCode
        {
            get { return ExamItemCodes.Light; }
        }

        private string[] _groups = null;
        public string[] Groups
        {
            get
            {
                if (_groups == null)
                {
                    var dataService = Singleton.GetDataService;
                    _groups = dataService.AllLightExamItems.Select(x => x.GroupName).Distinct().ToArray();
                }
                return _groups;
            }
        }

        public string EndVoiceText { get; private set; }
    }
}
