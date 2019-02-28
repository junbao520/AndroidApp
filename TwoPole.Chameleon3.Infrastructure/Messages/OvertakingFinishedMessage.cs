using System;
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 超车结束的消息
    /// </summary>
    public class OvertakingFinishedMessage : MessageBase
    {
        public OvertakingFinishedMessage(int currentLaneNumber, int overtakingLaneNumber,
            DateTime beginOvertakingTime)
        {

            this.CurrentLaneNumber = currentLaneNumber;
            this.OvertakingLaneNumber = overtakingLaneNumber;
            this.BeginTime = beginOvertakingTime;
            this.EndTime = DateTime.Now;
        }

        /// <summary>
        /// 车辆当前行驶的车道
        /// </summary>
        public int CurrentLaneNumber { get; private set; }

        /// <summary>
        /// 变道当时车辆行驶的车道
        /// </summary>
        public int OvertakingLaneNumber { get; private set; }

        /// <summary>
        /// 开始变道的时间
        /// </summary>
        public DateTime BeginTime { get; private set; }

        /// <summary>
        /// 变道结束的时间
        /// </summary>
        public DateTime EndTime { get; private set; }

        public override string ToString()
        {
            return string.Format("当前车道-{0}，" +
                                 "借道车道-{1}，" +
                                 "超车起止时间：{2:yyyyMMdd HH:mm:ss.f}--{3:yyyyMMdd HH:mm:ss.f}",
                this.CurrentLaneNumber,
                this.OvertakingLaneNumber,
                this.BeginTime,
                this.EndTime);
        }
    }
}
