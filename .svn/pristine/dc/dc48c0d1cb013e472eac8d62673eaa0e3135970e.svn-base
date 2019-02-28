using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using TwoPole.Chameleon3.Infrastructure.Map;
using GalaSoft.MvvmLight;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class ExamContext : ObservableObject, IDisposable
    {
        private ExamTimeMode _examTimeMode;
        private ExamMode _examMode;
        public IExamScore Score { get; private set; }
        private DateTime _startExamTime = DateTime.Now;
        private DateTime? _endExamTime;
        private TimeSpan _usedTime;
        private double _examDistance = 3000;
        private string _examGroup;
        private string _examLine = string.Empty;
        private double _travlledDistance;
        private bool _examSimulationLight;

        /// <summary>
        /// 是否启用灯光模拟
        /// </summary>
        public bool ExamSimulationLight
        {
            get { return _examSimulationLight; }
            set { base.Set(() => ExamSimulationLight, ref _examSimulationLight, value); }
        }

        public double TravlledDistance
        {
            get { return _travlledDistance; }
            set { base.Set(() => TravlledDistance, ref _travlledDistance, value); }
        }

        //扣分规则都是存在Rules里面的需要移除
        public ObservableCollection<BrokenRuleInfo> Rules { get; private set; }
        public ObservableCollection<ExamItemStateInfo> ExamItemStates { get; private set; }
        /// <summary>
        /// 考试模式，白天或者夜晚
        /// </summary>
        public ExamTimeMode ExamTimeMode
        {
            get { return _examTimeMode; }
            set { base.Set(() => ExamTimeMode, ref _examTimeMode, value); }
        }
        /// <summary>
        /// 考试模式，训练或者考试模式
        /// </summary>
        public ExamMode ExamMode
        {
            get { return _examMode; }
            set { base.Set(() => ExamMode, ref _examMode, value); }
        }
        /// <summary>
        /// 开始考试的时间
        /// </summary>
        public DateTime StartExamTime
        {
            get { return _startExamTime; }
            set { base.Set(() => StartExamTime, ref _startExamTime, value); }
        }
        /// <summary>
        /// 考试里程
        /// </summary>
        public double ExamDistance
        {
            get { return _examDistance; }
            set { base.Set(() => ExamDistance, ref _examDistance, value); }
        }
        /// <summary>
        /// 考试分组模式
        /// </summary>
        public string ExamGroup
        {
            get { return _examGroup; }
            set { base.Set(() => ExamGroup, ref _examGroup, value); }
        }
        /// <summary>
        /// 考试耗费的时间
        /// </summary>
        public TimeSpan UsedTime
        {
            get { return _usedTime; }
            set { base.Set(() => UsedTime, ref _usedTime, value); }
        }
        /// <summary>
        /// 结束考试的时间
        /// </summary>
        public DateTime? EndExamTime
        {
            get { return _endExamTime; }
            set { base.Set(() => EndExamTime, ref _endExamTime, value); }
        }
        /// <summary>
        /// 考试当前的分数
        /// </summary>
        public int ExamScore
        {
            get { return Score.Score; }
            //set { base.Set(() => _ExamScore.Score, ref _examScore, value); }
        }

        public string ExamLine
        {
            get { return _examLine; }
            set { base.Set(() => ExamLine, ref _examLine, value); }
        }
        /// <summary>
        /// 地图线路
        /// </summary>
        public IMapSet Map { get; set; }

        //
        public bool IsExaming { get { return !EndExamTime.HasValue; } }

        public ExamContext()
        {
            Rules = new ObservableCollection<BrokenRuleInfo>();
            ExamItemStates = new ObservableCollection<ExamItemStateInfo>();
            Score = Singleton.GetExamScore;
        }

        public ExamContext(ExamTimeMode timeMode, IMapSet map, double examDistance)
            : this()
        {
            ExamTimeMode = timeMode;
            Map = map;
            ExamDistance = examDistance;
        }

        public void StartExam()
        {
            Rules.Clear();
            StartExamTime = DateTime.Now;
            Score.Reset();
            EndExamTime = null;
            //重置考试状态
            foreach (var stateInfo in ExamItemStates)
            {
                stateInfo.State = ExamItemState.None;
                stateInfo.Result = ExamItemResult.None;
            }
        }

        public void StopExam()
        {
            EndExamTime = DateTime.Now;
        }

        public void Dispose()
        {
            Rules.Clear();
            GC.SuppressFinalize(this);
        }

        ~ExamContext()
        {
            Dispose();
        }
    }
}
