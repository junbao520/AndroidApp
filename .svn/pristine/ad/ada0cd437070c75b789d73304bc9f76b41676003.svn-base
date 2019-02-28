using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    /// <summary>
    /// 启动考试项目的消息
    /// </summary>
    public class ExamItemStartMessage : MessageBase
    {
        public ExamItemStartMessage(string examItemCode, ExamItemStartMode mode, MapPoint point, IDictionary<string, object> properties = null)
        {
            this.ExamItemCode = examItemCode;
            this.Source = mode;
            this.Properties = properties;
            this.MapPoint = point;
        }

        public MapPoint MapPoint { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// 触发开始考试的原来
        /// </summary>
        public ExamItemStartMode Source { get; private set; }

        /// <summary>
        /// 考试项目
        /// </summary>
        public string ExamItemCode { get; private set; }
    }

    [Description("启动模式")]
    public enum ExamItemStartMode
    {
        /// <summary>
        /// 由考试逻辑自动触发；比如变道，基于GPS高精度在变换车道的情况下
        /// </summary>
        [Description("自动")]
        Auto,
        /// <summary>
        /// 由地图中的点位自动触发
        /// </summary>
        [Description("地图")]
        Map,
        /// <summary>
        /// 人工方式触发
        /// </summary>
        [Description("人工")]
        Manual
    }
}
