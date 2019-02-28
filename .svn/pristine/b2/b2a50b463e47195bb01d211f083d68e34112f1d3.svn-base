using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure
{
    public abstract class ExamScoreBase : DisposableBase, IExamScore
    {
        private int _score = 100;
        private readonly IList<BrokenRuleInfo> _breakRules = new List<BrokenRuleInfo>();

        public bool VoiceBrokenRule { get; set; }

        public bool ContinueExamIfFailed { get; set; }
        public virtual int Score
        {
            get {
                return _score;
            }
            protected set { _score = value; }
        }
        public virtual bool Failed { get { return false; } }

        protected ILog Logger { get; set; }
        public ISpeaker Speaker { get; private set; }
        public IMessenger Messenger { get; protected set; }
        public IEnumerable<BrokenRuleInfo> BrokenRules { get { return _breakRules; } }
        public IDataService DataService { get; private set; }
        protected GlobalSettings Settings { get; private set; }

        protected ExamScoreBase(
            ISpeaker speaker,
            IMessenger messenger,
            IDataService dataService)
        {
            Messenger = messenger;
            Speaker = speaker;
            DataService = dataService;
            Settings = dataService.GetSettings();
            Logger = Singleton.GetLogManager;
            VoiceBrokenRule = Settings.VoiceBrokenRule;
        }

        protected override void Free(bool disposing)
        {
        }

        public  virtual void AddScore(int Score)
        {

        }
        public virtual void Init(NameValueCollection settings)
        {
        }

        //添加自定义扣分规则
        public void BreakRule(string examItemCode, string examItemName,
            string ruleCode, string subRuleCode = null, string message = null,int DeductedScores=0,string DeductedVoiceFile="")
        {
            var deductionRule = DataService.GetDeductionRule(ruleCode, subRuleCode);
            if (deductionRule == null)
            {
               Logger.WarnFormat("扣分规则：{0}-{1} 在数据库中不存在", ruleCode, subRuleCode);
                return;
            }
         
            deductionRule.DeductedScores = DeductedScores == 0 ? deductionRule.DeductedScores : DeductedScores;
            deductionRule.VoiceFile = DeductedVoiceFile == string.Empty ? deductionRule.VoiceFile : DeductedVoiceFile;
            deductionRule.ExamItemName = deductionRule.VoiceFile;
            var brokenRuleInfo = new BrokenRuleInfo(deductionRule)
            {
                ExamItemCode = examItemCode,
                ExamItemName = examItemName,
                Message = message,

            };
            //自定义扣分规则
            _breakRules.Add(brokenRuleInfo);
            Score -= brokenRuleInfo.DeductedScores;
            Logger.DebugFormat("项目：{0}，扣分代码：{1}，扣分：{2}，{3}", examItemName, brokenRuleInfo.RuleCode, brokenRuleInfo.DeductedScores, brokenRuleInfo.RuleDescription);
            var brokenRulemessage = new BrokenRuleMessage(brokenRuleInfo);
            Messenger.Send(brokenRulemessage);

            if (VoiceBrokenRule)
                SpeakBrokenRule(brokenRuleInfo);
        }





        private void SpeakBrokenRule(BrokenRuleInfo ruleInfo)
        {
            if (!string.IsNullOrEmpty(ruleInfo.DeductionRule.VoiceFile))
            {
                //Todo：lz
                if (Settings.PlayFail && ruleInfo.DeductionRule.DeductedScores == 100)
                {
                    Speaker.PlayAudioAsync(string.Format("{0}，不合格", ruleInfo.DeductionRule.VoiceFile, ruleInfo.DeductionRule.DeductedScores));
                }
                else
                {
                    Speaker.PlayAudioAsync(string.Format("{0}，扣{1}分", ruleInfo.DeductionRule.VoiceFile, ruleInfo.DeductionRule.DeductedScores));
                }
            }
              
        }

        public virtual void Reset()
        {
            Score = 100;
            _breakRules.Clear();
        }
    }
}
