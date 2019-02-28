using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 变道的消息
    /// </summary>
    public class LaneNumberChangedMessage : MessageBase
    {
        public LaneNumberChangedMessage(
            int currentLaneNumber,
            int lastLaneNumber,
            DateTime beginChangeLaneTime)
        {
            this.CurrentLaneNumber = currentLaneNumber;
            this.LastLaneNumber = lastLaneNumber;
            this.BeginTime = beginChangeLaneTime;
            this.EndTime = DateTime.Now;
            this.Direction = currentLaneNumber > lastLaneNumber ? TurnDirection.Left : TurnDirection.Right;
        }

        /// <summary>
        /// 车辆当前行驶的车道
        /// </summary>
        public int CurrentLaneNumber { get; private set; }

        /// <summary>
        /// 变道当时车辆行驶的车道
        /// </summary>
        public int LastLaneNumber { get; private set; }

        /// <summary>
        /// 开始变道的时间
        /// </summary>
        public DateTime BeginTime { get; private set; }

        /// <summary>
        /// 变道结束的时间
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// 转向的方向
        /// </summary>
        public TurnDirection Direction { get; private set; }

        public override string ToString()
        {
            return string.Format("当前车道-{0}，" +
                                 "上一个车道-{1}，" +
                                 "变道起止时间：{2:yyyyMMdd HH:mm:ss.f}--{3:yyyyMMdd HH:mm:ss.f}",
                this.CurrentLaneNumber,
                this.LastLaneNumber,
                this.BeginTime,
                this.EndTime);
        }
    }
}
