using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Providers;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IExamScore : IProvider
    {
        bool VoiceBrokenRule { get; set; }

        bool ContinueExamIfFailed { get; set; }
        int Score { get; }

        /// <summary>
        /// 是否考试失败
        /// </summary>
        bool Failed { get; }

        IEnumerable<BrokenRuleInfo> BrokenRules { get; }

        ///// <summary>
        ///// 触犯规则
        ///// </summary>
        ///// <param name="ruleInfo"></param>
        ////void BreakRule(BrokenRuleInfo ruleInfo);

        void BreakRule(string examItemCode, string examItemName, string ruleCode, string subRuleCode = null,string message = null,int DeductedScores=0,string DeductedVoiceFile="");

        void AddScore(int Score);


        void Reset();
    }
}
