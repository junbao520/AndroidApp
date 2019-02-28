using System;
using System.Collections;
using System.Collections.Generic;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface ICarSignalSet : IEnumerable<CarSignalInfo>, ICollection
    {
        //int CachedSeconds { get; set; }

        CarSignalInfo Current { get; }

        /// <summary>
        /// 查询历史时刻到现在的数据
        /// 最近的传感器排在前面
        /// </summary>
        /// <param name="cachedDateTime">历史时刻</param>
        /// <returns></returns>
        CarSignalInfo[] Query(DateTime cachedDateTime);

        CarSignalInfo[] Query(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="carSignal"></param>
        void Enqueue(CarSignalInfo carSignal);

        void Clear();
    }
}
