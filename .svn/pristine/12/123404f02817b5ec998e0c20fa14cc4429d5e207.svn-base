using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Providers;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface ILightRule : IRule
    {
        /// <summary>
        /// 编号
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 开始考试时的语音提示
        /// </summary>
        string VoiceText { get; set; }
        /// <summary>
        /// 语音文件
        /// </summary>
        string VoiceFile { get; set; }
        /// <summary>
        /// 扣分规则编号
        /// </summary>
        string RuleCode { get; set; }
        /// <summary>
        /// 是否已播报项目语音
        /// </summary>
        bool IsPlayedVoice { get; }

        /// <summary>
        /// 是否是第一条语音
        /// </summary>
        bool IsFirstRule { get; set; }
        /// <summary>
        /// 重置参数
        /// </summary>
        void Reset();
    }
}
