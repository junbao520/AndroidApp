using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class SimulationLightsRuleMessage : MessageBase
    {
        public SimulationLightsRuleMessage(ILightRule ruleInfo)
        {
            RuleInfo = ruleInfo;
        }

        public ILightRule RuleInfo { get; private set; }
    }
}
