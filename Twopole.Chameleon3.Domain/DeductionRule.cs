using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoPole.Chameleon3.Domain
{
    public class DeductionRule 
    {
        public int Id { get; set; }
        public string RuleCode { get; set; }

        public string SubRuleCode { get; set; }

        public string RuleName { get; set; }

        public int ExamItemId { get; set; }

        public int DeductedScores { get; set; }

        public bool IsRequired { get; set; }

        public string DeductedReason { get; set; }

        public string VoiceText { get; set; }

        public string VoiceFile { get; set; }

        /// <summary>
        /// 是否自动评判
        /// </summary>
        public bool IsAuto { get; set; }
        public string ItemName { get; set; }
     
        public string ExamItemName { get; set; }

        public string EnabledName { get; set; }

     
    }
}
