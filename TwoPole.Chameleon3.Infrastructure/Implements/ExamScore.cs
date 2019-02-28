using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class ExamScore : ExamScoreBase
    {
        /// <summary>
        /// 成绩合格分数
        /// </summary>
        public int QualifiedScore { get; set; }
        /// <summary>
        /// 如果扣分考试失败是否继续考试，默认可以继续考试
        /// </summary>
       // public bool ContinueExamIfFailed { get; set; }

        private readonly object lockObj = new object(); 

        public ExamScore(ISpeaker speaker, IMessenger messenger, IDataService dataService)
            : base(speaker, messenger, dataService)
        {
            QualifiedScore = 100;
            //初始化的时候
            ContinueExamIfFailed = Settings.ContinueExamIfFailed;
            Messenger = messenger;

            Messenger.Register<ExamStartMessage>(this,OnExamStart);
        }

        public override int Score
        {
            get { return base.Score; }
            protected set
            {
                //是不是锁的原因
                lock (lockObj)
                {
                    if (base.Score != value)
                    {
                        base.Score = value;
                        if (Failed)
                        {
                            //Logger.InfoFormat("考试失败-{0}", value);
                            if (!ContinueExamIfFailed)
                            {
                                //异步发送考试结束消息
                                Task.Run(()=> { Messenger.Send(new ExamFinishingMessage(true)); });
                            }
                        }
                    }
                }

            }
        }

        public override bool Failed
        {
            get { return Score < QualifiedScore; }
        }

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
          //  QualifiedScore = settings.GetIntValue("QualifiedScore", ExamSection.Instance.Basic.QualifiedScore);
        }

        protected override void Free(bool disposing)
        {
            base.Free(disposing);
            Messenger.Unregister(this);
        }

        public override void AddScore(int Score)
        {
            this.Score += Score;
        }
        private void OnExamStart(ExamStartMessage message)
        {
            this.Score = 100;
        }
    }
}
