using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class ExamManager_chengduduolun : ExamManager
    {


        public ExamManager_chengduduolun
            (IProviderFactory providerFactory, ISpeaker speaker, IMessenger messenger, IExamScore examScore, IDataService dataService, IGpsPointSearcher pointSearcher, ILog logger)
            : base(providerFactory, speaker, messenger, examScore, dataService, pointSearcher, logger)
        {

        }



        /// <summary>
        /// 语音播报结束考试
        /// </summary>
        protected override void VoiceEndExam()
        {

            Speaker.PlayAudioAsync(Settings.CommonExamEndExamVoice, SpeechPriority.High);
            if (ExamScore.Score < 100)
            {
                //Context.Rules 没有清零
                foreach (var rule in Context.Rules)
                {
                    Speaker.PlayAudioAsync(string.Format("{0}，扣{1}分", rule.DeductionRule.VoiceFile, rule.DeductionRule.DeductedScores));
                }

            }
            if (ExamScore.Score < 90)
            {

                if (!string.IsNullOrEmpty(Settings.CommonExamItemExamFailVoice))
                {
                    Speaker.PlayAudioAsync(Settings.CommonExamItemExamFailVoice);
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Settings.CommonExamItemExamSuccessVoice))
                {
                    Speaker.PlayAudioAsync(Settings.CommonExamItemExamSuccessVoice);
                }
            }
        }
    }
}
