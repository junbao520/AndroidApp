using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class LaneStateChangedMessage : GenericMessage<LaneState>
    {
        public LaneStateChangedMessage(int lastLaneNumber, int currentLaneNumber, LaneState state, int[] laneNumbers)
            : base(state)
        {
            LastLaneNumber = lastLaneNumber;
            CurrentLaneNumber = currentLaneNumber;
            if (laneNumbers == null || laneNumbers.Length == 0)
                throw new ArgumentNullException("laneNumbers");
            LaneNumbers = laneNumbers;
            this.ChangedTime = DateTime.Now;
        }

        /// <summary>
        /// 上一次车辆所在车道
        /// </summary>
        public int LastLaneNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前车辆中点（GPS基准点）所在车道
        /// </summary>
        public int CurrentLaneNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前车辆所在车道
        /// </summary>
        public int[] LaneNumbers
        {
            get;
            private set;
        }

        /// <summary>
        /// 车辆压线运行状态
        /// </summary>
        public LaneState LaneState
        {
            get { return base.Content; }
        }

        public DateTime ChangedTime { get; private set; }
    }
}
