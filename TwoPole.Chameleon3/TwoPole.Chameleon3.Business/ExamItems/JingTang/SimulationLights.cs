using System.Threading;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Collections.Generic;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Business.Rules.JingTang;

namespace TwoPole.Chameleon3.Business.ExamItems.JingTang
{
    /// <summary>
    /// 成都金堂客户，特殊需求。第一句请开启前照灯，远光、近光都可以
    /// 如果开的近光 判断下一下自动换成开远光的
    /// 如果开的远光 判断一下地洞换成近光
    /// </summary>
    public class SimulationLights : ExamItemBase, ILightExamItem
    {
        protected ILightRule CurrentLightRule { get; set; }


        protected LightRule[] CurrentActiviedRules { get; set; }



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

            Logger.InfoFormat("模拟灯光:随机分组开始");
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
                Logger.InfoFormat("模拟灯光：考试开始： {0}", context.ExamGroup);
                if (string.IsNullOrEmpty(context.ExamGroup))
                    context.ExamGroup = GetRandomGroup(context);
                //模拟灯光考试开始:用的时候在根据规则去一条一条在通过反射去创建
                
                CurrentActiviedRules = GetActiveRulesNew(context);
                ClearBrokenRuleState();
                //ResetRules();
                //RegisterMessages(Messenger);
                currentLightRuleIndex = 0;
                SetCurrentLightRule(0);
                StartCore(context, token);
                Logger.Debug("StartCore");
            }, token);
        }

        
        //这里面有通过join 查询出来，其实我也可以不通过 join 去查询得出IRule
        //这里就直接通过一个联合查询查出来这个对象,不通过反射进行创建
        //Singleton 、、

        protected  LightRule[] GetActiveRulesNew(ExamItemExecutionContext context)
        {
            var dataService = Singleton.GetDataService;

            var query = from a in dataService.AllLightExamItems.First(x => x.GroupName == context.ExamGroup).LightRules.Split(',')
                        let b = Convert.ToInt32(a)
                        join c in  dataService.AllLightRules on b equals c.Id
                        select (LightRule)c;

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

                    if (currentLightRuleIndex == 0)
                    {

                        //其实只需要判断客户当前停留的灯光是在近光还是远光
                        //如果客户开的是近光
                        if (signalInfo.Sensor.HighBeam)
                        {
                            var NextLightRule = CurrentActiviedRules[currentLightRuleIndex + 1];
                            Logger.Info("SimulationLightss HighBeam", NextLightRule.LightRuleType);
                            //表示远光
                            if (NextLightRule.LightRuleType.Contains("HighBeamLightRule"))
                            {
                                currentLightRuleIndex++;
                            }
                        }

                    }
                    else if (signalInfo.Sensor.LowBeam)
                    {
                        //var NextLightRule = CurrentActiviedRules[currentLightRuleIndex + 1];
                        ////表示是近光
                        Logger.Info("SimulationLightss LowBeam");
                        //if (NextLightRule.LightRuleType.Contains("LowBeamLightRule"))
                        //{
                        //    currentLightRuleIndex++;
                        //}
                    }
                    else
                    {
                        Logger.Info("SimulationLightss Default", "Default");
                    }
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
            var DataService = Singleton.GetDataService;
            if (currentLightRuleIndex >= 0)
            {
                
                if (CurrentActiviedRules.Count() > index)
                {
                    //处理下如果是第一条
                    CurrentLightRule = CreateLightRule(CurrentActiviedRules[index]);
                    CurrentLightRule.ExamItem = this;
                    CurrentLightRule.Reset();
                    Logger.InfoFormat("灯光模拟：设置规则：{0}",CurrentLightRule.VoiceFile);
                    return;
                }

            }
            CurrentLightRule = null;
        }
        private ILightRule CreateLightRule(LightRule rule, IEnumerable<Setting> settings = null)
        {
            try
            {
                var type = Type.GetType(rule.LightRuleType);
                //如果是一个
                var v = (ILightRule)Activator.CreateInstance(type, true);
                v.Id = rule.Id;
                v.VoiceFile = rule.VoiceFile;
                v.VoiceText = rule.VoiceText;
                v.RuleCode = rule.ItemCode;
                var provider = v as IProvider;
                var nameValues = settings.ToValues();
                provider.Init(nameValues);
                return v;
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("创建 ILightRule {0}发生异常，原因：{1}", rule.LightRuleType, exp, exp);
                return null;
            }

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
