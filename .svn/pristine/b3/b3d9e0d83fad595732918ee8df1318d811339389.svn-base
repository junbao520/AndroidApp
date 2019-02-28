using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class AdvancedCarSignal : IAdvancedCarSignal
    {
        public ICarSignalSet CarSignalSet { get; private set; }

        public CarSignalInfo CurrentSignal { get { return CarSignalSet.Current; } }

        public AdvancedCarSignal(ICarSignalSet carSignalSet)
        {
           // Logger = LogManager.GetLogger(this.GetType());
            CarSignalSet = carSignalSet;
        }

        /// <summary>
        /// 在规定的时间内两次挂档不进
        /// </summary>
        /// <param name="historyTime">规定的时间时间</param>
        /// <returns></returns>
        public bool CheckDoubleChangingGearFailed(DateTime historyTime)
        {
            var carSensors = CarSignalSet.Query(historyTime);
            var carSensorsSections = ParseNotNeutralItems(carSensors).Take(2).ToArray();
            if (carSensorsSections.Length < 2)
                return false;

            //如果在2次空挡之间检测到没有档位，则评定挂档不进
            var result = carSensorsSections.SelectMany(x => x).All(x => x.Sensor.Gear == Gear.Neutral);
            return result;
        }

        private static IEnumerable<List<CarSignalInfo>> ParseNotNeutralItems(IEnumerable<CarSignalInfo> carSensorInfos)
        {
            var section = new List<CarSignalInfo>();
            foreach (var signal in carSensorInfos)
            {
                if (!signal.Sensor.IsNeutral)
                {
                    section.Add(signal);
                }
                else if (section.Count > 0)
                {
                    yield return section;
                    section = new List<CarSignalInfo>();
                }
            }
            if (section.Count > 0)
                yield return section;
        }

        #region HighBeam

        public bool GetSafetyBeltChanged(int minCount)
        {
            if (CarSignalSet.Count <= 1)
                return false;

            var currentSafetyBelt = CarSignalSet.Current.Sensor.SafetyBelt;
            var currentCount = CarSignalSet.TakeWhile(x => x.Sensor.SafetyBelt == currentSafetyBelt).Count();
            if (currentCount < minCount)
                return false;

            var previousCount = CarSignalSet.Skip(currentCount).TakeWhile(x => x.Sensor.SafetyBelt == !currentSafetyBelt).Count();
            return (previousCount >= minCount);
        }

        /// <summary>
        /// 检查历史时间内连续远光灯次数
        /// </summary>
        /// <param name="historyTime">历史时刻</param>
        /// <param name="number">是否打灯</param>
        /// <returns></returns>
        public bool CheckHighBeam(DateTime historyTime, int number)
        {
            var highBeamNumbers = QueryHighBeamChangedNumbers(historyTime);
            var result = highBeamNumbers.Count() >= number;
            return result;
        }

        public int GetHighBeamChangedNumber(DateTime historyTime)
        {
            var highBeamNumbers = QueryHighBeamChangedNumbers(historyTime);
            var number = highBeamNumbers.Count();
            return number;
        }

        /// <summary>
        /// 查询远光变换的次数
        /// </summary>
        /// <param name="historyTime">历史时刻</param>
        /// <returns></returns>
        private IEnumerable<int> QueryHighBeamChangedNumbers(DateTime historyTime)
        {
            var items = CarSignalSet.Query(historyTime);
            if (items.Length == 0)
                return Enumerable.Empty<int>();

            var highBeamCounts = ParseHighBeamCounts(items).ToArray();
            //Logger.DebugFormat("时间段：{0:HH:mm:ss.ff}-{1:HH:mm:ss.ff}，远光变换次数：{2}, {3}",
            //    DateTime.Now, historyTime,
            //    highBeamCounts.Length, highBeamCounts);
            return highBeamCounts;
        }

        private static IEnumerable<int> ParseHighBeamCounts(IEnumerable<CarSignalInfo> carSensorInfos)
        {
            var highBeamCount = 0;
            foreach (var carSensor in carSensorInfos)
            {
                if (carSensor.Sensor.HighBeam)
                {
                    highBeamCount++;
                }
                else if (highBeamCount > 0)
                {
                    yield return highBeamCount;
                    highBeamCount = 0;
                }
            }
            if (highBeamCount > 0)
                yield return highBeamCount;
        }


        private int CountHighBeam(IEnumerable<CarSensorInfo> items)
        {
            var count = 0;
            var currentHighBeam = false;
            foreach (var carSensor in items)
            {
                if (carSensor.HighBeam && currentHighBeam == false)
                    count++;

                currentHighBeam = carSensor.HighBeam;
            }

            //Logger.DebugFormat("{0:yyyyMMdd HH:mm:ss.ff} - 远光次数: {1}", DateTime.Now, count);
            return count;
        }

        private static IEnumerable<List<CarSensorInfo>> ParseLowBeamItems(IEnumerable<CarSensorInfo> carSensorInfos)
        {
            var lowBeamItems = new List<CarSensorInfo>();
            foreach (var carSensor in carSensorInfos)
            {
                if (carSensor.LowBeam)
                {
                    lowBeamItems.Add(carSensor);
                }
                else if (lowBeamItems.Count > 0)
                {
                    yield return lowBeamItems;
                    lowBeamItems = new List<CarSensorInfo>();
                }
            }
            if (lowBeamItems.Count > 0)
                yield return lowBeamItems;
        }
        #endregion

        public IEnumerable<TimeSpan> GetPeriods(Func<CarSignalInfo, bool> filter, DateTime historyTime)
        {
            DateTime? lastRecordTime = null;
            foreach (var carSensorInfo in CarSignalSet.TakeWhile(x => x.RecordTime >= historyTime))
            {
                var result = filter(carSensorInfo);
                if (result)
                {
                    if (!lastRecordTime.HasValue)
                        lastRecordTime = DateTime.Now;

                    continue;
                }

                if (lastRecordTime.HasValue)
                {
                    var period = (lastRecordTime.Value - carSensorInfo.RecordTime);
                    lastRecordTime = null;
                    yield return period;
                }
            }
        }

        /// <summary>
        /// 检测操作提前多少秒
        /// </summary>
        /// <param name="filter">操作</param>
        /// <param name="historyTime">历史时刻到现在</param>
        /// <param name="seconds">提前多少秒</param>
        /// <returns></returns>
        public bool CheckOperationAheadSeconds(Func<CarSignalInfo, bool> filter, DateTime historyTime, double seconds)
        {
            ///当设置小于等于0时直接验证成功。
            if (seconds <= 0)
                return true;
            var items = CarSignalSet.Query(historyTime);
            var firstItem = items.FirstOrDefault(filter);
            if (firstItem == null)
                return false;
            var lastItem = items.LastOrDefault(filter);
            if (lastItem == null)
                return false;
            if (firstItem == lastItem)
                return false;

            var isOk = (firstItem.RecordTime - lastItem.RecordTime).TotalSeconds >= seconds;
            return isOk;
        }

        /// <summary>
        /// 获取灯光改变的属性名称
        /// </summary>
        /// <param name="oldSignal">老的信号</param>
        /// <param name="newSignal">新的传感器信号</param>
        /// <returns></returns>
        public IList<string> GetLightChangedProperties(CarSignalInfo oldSignal, CarSignalInfo newSignal)
        {
            var oldSensor = oldSignal.Sensor;
            var newSensor = newSignal.Sensor;

            List<string> propertyNames = new List<string>();
            if (oldSensor == null)
                return propertyNames;

            //灯光被改变
            if (oldSensor.CautionLight != newSensor.CautionLight && newSensor.CautionLight)
                propertyNames.Add("CautionLight");
            if (oldSensor.LeftIndicatorLight != newSensor.LeftIndicatorLight && newSensor.LeftIndicatorLight)
                propertyNames.Add("LeftIndicatorLight");
            if (oldSensor.RightIndicatorLight != newSensor.RightIndicatorLight && newSensor.RightIndicatorLight)
                propertyNames.Add("RightIndicatorLight");
            if (oldSensor.FogLight != newSensor.FogLight && newSensor.FogLight)
                propertyNames.Add("FogLight");
            if (oldSensor.HighBeam != newSensor.HighBeam && newSensor.HighBeam)
                propertyNames.Add("HighBeam");
            if (oldSensor.LowBeam != newSensor.LowBeam && newSensor.LowBeam)
                propertyNames.Add("LowBeam");
            if (oldSensor.OutlineLight != newSensor.OutlineLight && newSensor.OutlineLight)
                propertyNames.Add("OutlineLight");

            return propertyNames;
        }

        public IEnumerable<GearChangedState> GetGearChangedStates(DateTime historyTime)
        {
            Gear? g = null;
            DateTime? lastGearTime = null;
            DateTime? firstGearTime = null;
            foreach (var carSensorInfo in CarSignalSet.Query(historyTime))
            {
                if (!g.HasValue)
                {
                    g = carSensorInfo.Sensor.Gear;
                    lastGearTime = carSensorInfo.RecordTime;
                    continue;
                }

                if (g == carSensorInfo.Sensor.Gear)
                {
                    firstGearTime = carSensorInfo.RecordTime;
                }
                else
                {
                    //档位超过规定值
                    var gearState = new GearChangedState(g.Value, firstGearTime.GetValueOrDefault(lastGearTime.Value), lastGearTime.Value);
                    yield return gearState;

                    //启动新的值；
                    g = carSensorInfo.Sensor.Gear;
                    lastGearTime = carSensorInfo.RecordTime;
                    firstGearTime = null;
                }
            }

            if (g.HasValue)
            {
                var gearState = new GearChangedState(g.Value, firstGearTime.GetValueOrDefault(lastGearTime.Value), lastGearTime.Value);
                yield return gearState;
            }
        }
    }
}
