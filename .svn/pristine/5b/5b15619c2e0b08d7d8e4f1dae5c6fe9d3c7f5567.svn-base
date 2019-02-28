using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class CarSignalSet : ICarSignalSet
    {
        /// <summary>
        /// 缓存20分钟
        /// </summary>
        private const int DefaultCachedSeconds = 20 * 60;
        private readonly Queue<CarSignalInfo> _signalInfoNodes;
        public CarSignalSet()
        {
            SyncRoot = new object();
            _signalInfoNodes = new Queue<CarSignalInfo>();
        }

        #region IGpsDependency
        public int Order { get { return 10; } }

        public void Enqueue(CarSignalInfo signal)
        {
            lock (SyncRoot)
            {
                _signalInfoNodes.Enqueue(signal);
                RemoveOldGps();
            }
        }
        #endregion

        private void RemoveOldGps()
        {
            //保留一条记录，防止读取Current失败；
            while (_signalInfoNodes.Count > 1 &&
                (DateTime.Now - _signalInfoNodes.Peek().RecordTime).TotalSeconds > CachedSeconds)
            {
                _signalInfoNodes.Dequeue();
            }
        }

        #region IGpsSet

        private int _cachedSeconds = DefaultCachedSeconds;

        public int CachedSeconds
        {
            get { return _cachedSeconds; }
            set { _cachedSeconds = value; }
        }

        public CarSignalInfo Current
        {
            get
            {
                lock (SyncRoot)
                {
                    return _signalInfoNodes.Count > 0 ? _signalInfoNodes.Last() : null;
                }
            }
        }

        public CarSignalInfo[] Query(DateTime cachedDateTime)
        {
            lock (SyncRoot)
            {
                var items = this.TakeWhile(x => x.RecordTime >= cachedDateTime&&x.IsSensorValid).ToArray();
                return items;
            }
        }

        public CarSignalInfo[] Query(DateTime startTime, DateTime endTime)
        {
            if(startTime >= endTime)
                throw new ArgumentException("开始时间必须小于结束时间");

            lock (SyncRoot)
            {
                //1.跳过大于结束时间的
                //2.获取大于其实时间的
                var items = this.SkipWhile(x => x.RecordTime > endTime).TakeWhile(x => x.RecordTime >= startTime&&x.IsSensorValid).ToArray();
                //var items = this.Where(x => x.RecordTime <= endTime && x.RecordTime >= startTime).ToArray();
                return items;
            }
        }

        public void Clear()
        {
            lock (SyncRoot)
            {
                this._signalInfoNodes.Clear();
            }
        }

        #endregion

        #region IEnumerable

        public IEnumerator<CarSignalInfo> GetEnumerator()
        {
            return _signalInfoNodes.Reverse().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICollection
        public void CopyTo(Array array, int index)
        {
            ((ICollection)_signalInfoNodes).CopyTo(array, index);
        }

        public int Count
        {
            get
            {
                return _signalInfoNodes.Count;
            }
        }

        public object SyncRoot { get; private set; }
        public bool IsSynchronized { get { return true; } }
        #endregion
    }
}
