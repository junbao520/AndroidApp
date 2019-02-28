using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace TwoPole.Chameleon3.Infrastructure
{
    /// <summary>
    /// 考试项目状态信息
    /// </summary>
    public class ExamItemStateInfo : ObservableObject
    {
        private string _itemCode;
        private string _itemName;
        private string _iconUrl;
        private DateTime _startTime = DateTime.Now;
        private DateTime? _endTime;
        private ExamItemResult _result;
        private ExamItemState _state;

        /// <summary>
        /// 考试项目编码
        /// </summary>
        public string ItemCode
        {
            get { return _itemCode; }
            set { base.Set(() => ItemCode, ref _itemCode, value); }
        }
        /// <summary>
        /// 考试项目名称
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set { base.Set(() => ItemName, ref _itemName, value); }
        }
        /// <summary>
        /// 项目的图片信息
        /// </summary>
        public string IconUrl
        {
            get { return _iconUrl; }
            set { base.Set(() => IconUrl, ref _iconUrl, value); }
        }
        /// <summary>
        /// 开始考试的时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set { base.Set(() => StartTime, ref _startTime, value); }
        }
        /// <summary>
        /// 结束考试的时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { base.Set(() => EndTime, ref _endTime, value); }
        }
        /// <summary>
        /// 结束考试的时间
        /// </summary>
        public ExamItemResult Result
        {
            get { return _result; }
            set { base.Set(() => Result, ref _result, value); }
        }
        /// <summary>
        /// 考试状态
        /// </summary>
        public ExamItemState State
        {
            get { return _state; }
            set { base.Set(() => State, ref _state, value); }
        }
    }
}
