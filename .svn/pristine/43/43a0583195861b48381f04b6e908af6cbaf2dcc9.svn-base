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
    public class ExamManager_luxian : ExamManager
    {


        public ExamManager_luxian(
            IProviderFactory providerFactory,
            ISpeaker speaker,
            IMessenger messenger,
            IExamScore examScore,
            IDataService dataService,
            IGpsPointSearcher pointSearcher,
            //这个有一个Logger导致依赖注入不成功
            ILog logger
            ):base(
             providerFactory,
             speaker,
             messenger,
             examScore,
             dataService,
             pointSearcher,
            //这个有一个Logger导致依赖注入不成功
             logger
            )
        {
           
        }

        protected override void VoiceStartExam()
        {
            Speaker.PlayAudioAsync("本次考试开始，请走1号考试线路", SpeechPriority.High);
        }


        string hgVoice = "恭喜考试合格，请走1号考试线路";
        string bhgVoice = "考试结束，您的扣分项目是";
        string afterVoice = "未达到分数线，下次继续努力，请走1号考试线路";

        /// <summary>
        /// 语音播报结束考试
        /// </summary>
        protected override void VoiceEndExam()
        {

           
            if (ExamScore.Score < 90)
            {
                Speaker.PlayAudioAsync(bhgVoice);
               
            }
            else
            {

                Speaker.PlayAudioAsync(hgVoice);

                //Logger.Error("ExamScore.Score >= 90 :Score" + ExamScore.Score.ToString() + "," + hgVoice);
            }
            if (ExamScore.Score < 90)
            {
                //合格不播报扣分项，泸县
                //Context.Rules 没有清零
                foreach (var rule in Context.Rules)
                {
                    Speaker.PlayAudioAsync(string.Format("{0}，扣{1}分", rule.DeductionRule.VoiceFile, rule.DeductionRule.DeductedScores));
                }
               // Logger.Error("ExamScore.Score < 100 Score:" + ExamScore.ToString());

                Speaker.PlayAudioAsync(afterVoice);
            }

            
            

        }





    }
}
