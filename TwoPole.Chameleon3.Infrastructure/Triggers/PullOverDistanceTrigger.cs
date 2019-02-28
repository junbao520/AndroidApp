using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    /// <summary>
    /// 靠边停车距离触发器
    /// </summary>
    public class PullOverDistanceTrigger : DistanceTrigger
    {
        public PullOverDistanceTrigger(IMessenger messenger)
            : base(messenger)
        {
        }

        /// <summary>
        /// 触发后自动结束
        /// </summary>
        /// <param name="currentDistance"></param>
        public override void Run(double currentDistance)
        {
            Task.Run(() => Messenger.Send(new ExamItemStartMessage(ExamItemCodes.PullOver, ExamItemStartMode.Auto,
                new MapPoint(Coordinate.Empty, 0, string.Empty, MapPointType.PullOver))));
            base.Stop();
        }
    }

    /// <summary>
    /// 靠边停车，由考试距离触发
    /// </summary>
    public class PullOverExamDistanceTrigger : PullOverDistanceTrigger
    {
        protected GlobalSettings Settings { get; set; }

        public PullOverExamDistanceTrigger(IDataService dataService, IMessenger messenger)
            : base(messenger)
        {
            Settings = dataService.GetSettings();
        }

        /// <summary>
        /// 启用靠边停车
        /// </summary>
        /// <returns></returns>
        protected override bool ValidParameters()
        {
            return Settings.PullOverAutoTrigger;
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
