using System;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class BrokenRuleInfo
    {
        public int Id { get { return DeductionRule.Id; } }
        public DateTime BreakTime { get; internal set; }
        public string ExamItemCode { get; internal set; }
        public string ExamItemName { get; internal set; }
        public string Message { get; internal set; }

        public string RuleCode { get { return DeductionRule.RuleCode; } }
        public string SubRuleCode { get { return DeductionRule.SubRuleCode; } }
        public string RuleName { get { return DeductionRule.RuleName; }  }
        public bool Required { get { return DeductionRule.IsRequired; } }
        public int DeductedScores { get { return DeductionRule.DeductedScores; } }

        public DeductionRule DeductionRule { get; private set; }

        public BrokenRuleInfo(DeductionRule deductionRule)
        {
            if(deductionRule == null)
                throw new ArgumentNullException("deductionRule");

            DeductionRule = deductionRule;
            BreakTime = DateTime.Now;
        }
        public string RuleDescription
        {
            get
            {
                return !string.IsNullOrEmpty(Message) ? Message : RuleName;
            }
        }
    }
}