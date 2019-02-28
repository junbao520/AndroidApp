using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IAdvancedCarSignal
    {
        ICarSignalSet CarSignalSet { get; }

        CarSignalInfo CurrentSignal { get; }

        /// <summary>
        /// 获取从历史时刻到现在远光次数
        /// </summary>
        /// <param name="historyTime">历史时刻</param>
        /// <returns></returns>
        int GetHighBeamChangedNumber(DateTime historyTime);

        /// <summary>
        /// 安全带是否改变
        /// </summary>
        /// <param name="minCount">安全带改变的最低数量（防止偶尔出现信号采集失效）</param>
        /// <returns></returns>
        bool GetSafetyBeltChanged(int minCount);

        /// <summary>
        /// 检查历史时刻内远光次数
        /// </summary>
        /// <param name="historyTime">历史时刻</param>
        /// <param name="number">远光次数</param>
        /// <returns></returns>
        bool CheckHighBeam(DateTime historyTime, int number = 1);

        /// <summary>
        /// 查询的历史时刻
        /// </summary>
        /// <param name="historyTime">历史时刻</param>
        /// <returns></returns>
        IEnumerable<GearChangedState> GetGearChangedStates(DateTime historyTime);

        /// <summary>
        /// 获取灯光改变的属性名称
        /// </summary>
        /// <param name="oldSignal">老的信号</param>
        /// <param name="newSignal">新的传感器信号</param>
        /// <returns></returns>
        IList<string> GetLightChangedProperties(CarSignalInfo oldSignal, CarSignalInfo newSignal);

        /// <summary>
        /// 查找历史时刻到现在信号的时间段；
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="historyTime"></param>
        /// <returns></returns>
        //IEnumerable<TimeSpan> GetPeriods(Func<CarSignalInfo, bool> filter, DateTime historyTime);

        /// <summary>
        /// 检测操作提前多少秒
        /// </summary>
        /// <param name="filter">操作</param>
        /// <param name="historyTime">历史时刻到现在</param>
        /// <param name="seconds">提前多少秒</param>
        /// <returns></returns>
        bool CheckOperationAheadSeconds(Func<CarSignalInfo, bool> filter, DateTime historyTime, double seconds);

        /// <summary>
        /// 2次挂档不进
        /// </summary>
        /// <param name="historyTime"></param>
        /// <returns></returns>
        bool CheckDoubleChangingGearFailed(DateTime historyTime);
    }
}
