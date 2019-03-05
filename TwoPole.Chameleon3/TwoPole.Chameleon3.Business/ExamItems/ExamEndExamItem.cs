using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 考试结束直接触发消息考试结束
    /// </summary>
    public class ExamEndExamItem : ExamItemBase
    {
        #region 检测规则
        //1，检测右转向灯
        #endregion


        #region 参数配置
        protected string TurnRoundNumber { get; set; }
        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            //最大考试距离
            MaxDistance = 3;
        }

        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //TurnRoundNumber = Context.Properties["Road"].ToString();
            StartTime = DateTime.Now;
            //if (Settings.RoundaboutVoice)
                //Speaker.PlayAudioAsync(RoundaboutVoiceFile, Infrastructure.Speech.SpeechPriority.Highest);

            return base.InitExamParms(signalInfo);
        }
    
 

        protected override void StopCore()
        {
            //发送考试结束消息
            Messenger.Send(new ExamFinishingMessage(false));

            base.StopCore();
        }


        public override string ItemCode
        {
            get { return ExamItemCodes.ExamEndVoice; }
        }

       
        protected override bool VoiceExamItem
        {
            get
            {
                return false;
            }
            set
            {
                base.VoiceExamItem = value;
            }
        }
    }
}
