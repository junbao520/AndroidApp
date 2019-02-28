using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Business.ExamItems.JingTang
{
    class CustomDeductionRules
    {
        public static List<DeductionRule> lstCustomDeductionRules = new List<DeductionRule>
        {
            new DeductionRule {RuleCode="302051",RuleName="起步前不使用或错误使用转向灯",DeductedScores=100},
            new DeductionRule {RuleCode="302052",RuleName="转向前不使用或错误使用转向灯",DeductedScores=100},
            new DeductionRule {RuleCode="302054",RuleName="变更车道前不使用或错误使用转向灯",DeductedScores=100},
            new DeductionRule {RuleCode="302054",RuleName="超车前不使用或错误使用转向灯",DeductedScores=100},
            new DeductionRule {RuleCode="302055",RuleName="停车前不使用或错误使用转向灯",DeductedScores=100},

            new DeductionRule {RuleCode="302061",RuleName="起步开转向灯少于3秒即转向",DeductedScores=100},
            new DeductionRule {RuleCode="302062",RuleName="转向开转向灯少于3秒即转向",DeductedScores=100},
            new DeductionRule {RuleCode="302063",RuleName="变更车道开转向灯少于3秒即转向",DeductedScores=100},
            new DeductionRule {RuleCode="302064",RuleName="超车开转向灯少于3秒即转向",DeductedScores=100},
            new DeductionRule {RuleCode="302065",RuleName="停车开转向灯少于3秒即转向",DeductedScores=100}



        };
    }
}