
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    public class ExamEndDistanceTrigger : DistanceTrigger
    {
        protected GlobalSettings Settings { get; set; }
        public ExamEndDistanceTrigger(IDataService dataService,IMessenger messenger)
            : base(messenger)
        {
            Settings = dataService.GetSettings();
        }

        /// <summary>
        /// 触发后自动结束
        /// </summary>
        /// <param name="currentDistance"></param>
        public override void Run(double currentDistance)
        {
            //在没有项目的时候自动结束，如有项目则等到项目结束后再结束所有项目
            //var examManager = ServiceLocator.Current.GetInstance<IExamManager>();
            //var _isInExamItem = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode != ExamItemCodes.CommonExamItem);

            //if (!_isInExamItem)
            //{
            //    Messenger.Send(new ExamFinishingMessage(true));
            //    base.Stop();
            //}
        }

        protected override bool ValidParameters()
        {
            return Settings.EndExamByDistance;
        }

        public override void Start(ExamContext context)
        {
            //重新设置触发距离值
            Distance = context.ExamDistance;
            //Logger.DebugFormat("{0}-重新设置触发的距离值{1}", Name, Distance);
            base.Start(context);
        }
    }
}
