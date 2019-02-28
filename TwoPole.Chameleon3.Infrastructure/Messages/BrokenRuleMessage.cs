using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class BrokenRuleMessage : MessageBase
    {
        public BrokenRuleMessage(BrokenRuleInfo ruleInfo)
        {
            RuleInfo = ruleInfo;
        }

        public BrokenRuleInfo RuleInfo { get; private set; }
    }

    public class SpecialBrokenRuleMessage : MessageBase
    {
        public string ruleCode { get; set; }
        public SpecialBrokenRuleMessage(string ruleCode)
        {
            this.ruleCode = ruleCode;
        }
    }

}
