using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Domain;
using Android.Util;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class DefaultCarSignalProcessor : ICarSignalProcessor
    {
        public ICarSignalSet CarSignalSet { get; private set; }
        public GpsAngleCalculator AngleCalculator { get; private set; }
        public MileageUpdater MileageUpdator { get; private set; }
        protected IMessenger Messenger { get; private set; }

        public ExamContext ExamContext { get; private set; }
        public GlobalSettings Settings { get; private set; }

        protected ILog Logger { get; set; }

        private Queue<CarSignalInfo> CarSignalQuque;

        public DefaultCarSignalProcessor(ICarSignalSet signalSet, IDataService dataService, IMessenger messenger,ILog log)
        {
            Order = 1;
            CarSignalSet = signalSet;
            Messenger = messenger;
            Settings = dataService.GetSettings();
            Logger = log;
            CarSignalQuque = new Queue<CarSignalInfo>();
            AngleCalculator = new GpsAngleCalculator(signalSet);
            MileageUpdator = new MileageUpdater(messenger, Settings.ParkingValueKmh);
            Messenger.Register<ExamStartMessage>(this, OnExamStart);
        }

        private void OnExamStart(ExamStartMessage message)
        {
            MileageUpdator.Reset();
            ExamContext = message.ExamContext;
        }

        public int Order { get; set; }


        //********************,*********************//
        public virtual void Execute(CarSignalInfo signalInfo)
        {
            try
            {
                //TODO:记录最近10个值对角度进行处理
                if (CarSignalQuque.Count>=10)
                {
                    CarSignalQuque.Dequeue();
                }
                CarSignalQuque.Enqueue(signalInfo);
                //警告：顺序相当重要，请勿随意改动，否则引起严重问题；
                //处理信号是否有效
                ProcessIsValid(signalInfo);
                //计算考试耗时
                ProcessUsedTime(signalInfo);
              
                //自定义计算航向角 屏蔽自定义计算航向角
                // ProcessAngle(signalInfo);
                //信号滤波 目前只针对角度
                //TODO:目前看来还是有Bug,需要通过模拟测试和实际测试才可以使用
                //SensorWaveFiltering(signalInfo);
                //处理速度，从OBD读取速度
                ProcessSpeed(signalInfo);
                //处理车辆停车状态，评定车辆速度是否低于设置值
                ProcessCarState(signalInfo);
                //处理车辆考试里程，通过gps来核算，当车辆停止后，不运算
                ProcessDistance(signalInfo);
                //处理发动机引擎，
                //TODO:转速比放大缩小，android版本没有接脉冲的情况 
                ProcessEngineRpm(signalInfo);
                //处理速度
                ProcessSpeedMode(signalInfo);
                //处理车辆发动机是否熄火；
                ProcessEngine(signalInfo);
                //处理发动机转速与速度的比值，齿速比
                ProcessEngineRatio(signalInfo);
                //处理安全带信号，2017-01-18 新增
                ProcessSafeBelt(signalInfo);
                //处理手刹信号，2017-07-28 新增
                ProcessHandbrake(signalInfo);

                //Logger.Error(Settings.GearSource.ToString());
                if (Settings.GearSource == GearSource.GearDisplay)
                {
                    ProcessGearWithGearDisplay(signalInfo);
                }
                else if (Settings.GearSource == GearSource.OBD)
                {
                    //如果档位来源是Obd 
                    signalInfo.Sensor.Gear = signalInfo.Sensor.OBDGear;
                } 
                else
                {
                    
                    //根据齿速比计算档位
                    ProcessGear(signalInfo);
                    //处理是否空挡的信号源,默认从离合这里读取
                    //TODO:转速比 这里不用离合处理
                    //如果转速比
                    //如果选的转速比
                    
                    if (signalInfo.Sensor.IsNeutral)
                    {
                       // Logger.Error("空挡");
                        signalInfo.Sensor.Gear = Gear.Neutral;
                    }
                    //处理转速比 踩下离合就是空挡
                    if (signalInfo.Sensor.Clutch)
                    {
                        signalInfo.Sensor.Gear = Gear.Neutral;
                    }
                }
             

            }
            catch (Exception ex)
            {
                //捕获signal=null的情况
                if (signalInfo != null)
                    signalInfo.IsSensorValid = false;
                Logger.Error("SensorException", string.Join(",",signalInfo.commands));
                Logger.ErrorFormat("信号处理异常：{0}", ex);
            }
        }

        //TODO:信号滤波,目前只对  速度,转速,角度 三个值进行滤波
        //滤波算法 中位值滤波算法 取出最近的N个值，去掉最大值和最小值 然后求平均值
        //对信号进行滤波,//
        protected void SensorWaveFiltering(CarSignalInfo signalInfo)
        {
            //GetEngineRpmFiltering(signalInfo);
            //GetSpeedFiltering(signalInfo);
            GetAngleFiltering(signalInfo);
        }
        protected void GetEngineRpmFiltering(CarSignalInfo signalInfo)
        {
            //开始100个信号不会处理，避免信号不稳定情况！
            if (CarSignalSet.Count < 100)
            {
                signalInfo.SpeedInKmh = signalInfo.SpeedInKmh;
                return;
            }
            Logger.Error("GetEngineRpmFiltering Count>100");
            var SignalInfo = CarSignalSet.Skip(CarSignalSet.Count - 10).Take(10).Select(s => s.EngineRpm).ToArray();
            Logger.Error("SignalInfoSensor", string.Join(",", SignalInfo));


            //var SignalInfo = CarSignalSet.Select(s => s.SpeedInKmh).Take(10).ToArray();
            signalInfo.EngineRpm = GetFilterVale(SignalInfo);
        }
        protected void GetSpeedFiltering(CarSignalInfo signalInfo)
        {
            //开始100个信号不会处理，避免信号不稳定情况！
            if (CarSignalSet.Count < 100)
            {
                signalInfo.SpeedInKmh = signalInfo.SpeedInKmh;
                return;
            }
            var SignalInfo = CarSignalSet.Skip(CarSignalSet.Count - 10).Take(10).Select(s => s.SpeedInKmh).ToArray();
            //var SignalInfo = CarSignalSet.Select(s => s.SpeedInKmh).Take(10).ToArray();
            signalInfo.SpeedInKmh = GetFilterVale(SignalInfo);
        }
        protected void GetAngleFiltering(CarSignalInfo signalInfo)
        {
            //开始100个信号不会处理，避免信号不稳定情况！
            if (CarSignalQuque.Count<10)
            {
                signalInfo.BearingAngle = signalInfo.BearingAngle;
                return;
            }
            Logger.Error("CarSignalQuque Count==10");
            //var SignalInfo = CarSignalSet.Skip(CarSignalSet.Count - 10).Take(10).Select(s=>s.BearingAngle).ToArray();
            var SignalInfo = CarSignalQuque.Select(s => s.BearingAngle).ToArray();
            signalInfo.BearingAngle = GetFilterVale(SignalInfo);
        }

        public double GetFilterVale(double[] data)
        {
            Logger.Error("GetFilterVale", string.Join(",", data));
            int Length = data.Length;

            double MaxData = data.Max();
            double MinData = data.Min();
            double SumData = data.Sum();
            double AvgData= (SumData - MaxData - MinData) / (Length - 2);
            Logger.Error("MaxData", MaxData.ToString());
            Logger.Error("MinData", MinData.ToString());
            Logger.Error("AvgData", AvgData.ToString());
            Logger.Error("SumData", SumData.ToString());
            Logger.Error("Length", Length.ToString());
            return AvgData;
               
        }
        public int GetFilterVale(int[] data)
        {
            int Length = data.Length;
            int MaxData = data.Max();
         
            int MinData = data.Min();
            int SumData = data.Sum();
            int AvgData = (SumData - MaxData - MinData) / (Length - 2);
            return AvgData;

        }


        /// <summary>
        /// 处理空挡传感器
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessIsNeutral(CarSignalInfo signalInfo)
        {
            signalInfo.Sensor.IsNeutral = signalInfo.Sensor.Clutch;
        }

        protected virtual void ProcessEngineRatio(CarSignalInfo signalInfo)
        {
            if (signalInfo.EngineRpm <= 0 || signalInfo.SpeedInKmh <= 0)
                signalInfo.EngineRatio = 0;
            else
            {
                signalInfo.EngineRatio = Convert.ToInt32(signalInfo.EngineRpm / signalInfo.SpeedInKmh);
            }

        }

        protected virtual void ProcessGear(CarSignalInfo signalInfo)
        {
            //进行防闪烁处理
            signalInfo.Sensor.Gear = ParseGear(signalInfo);
        }


        /// <summary>
        /// 档位处理档显
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessGearWithGearDisplay(CarSignalInfo signalInfo)
        {
            //20151217，李，忽略空挡取反
            signalInfo.Sensor.IsNeutral = false;
            ///档显通过高低信息进行判断档位

            //现在要加上去和自定义信号线  //
            //空挡按下就是强制空挡 
            //如果是空挡
            if (!signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && !signalInfo.Sensor.GearDisplayD4)
            {
                //20151217，李，档显时，空档这个属性要读取挡显的值
                signalInfo.Sensor.IsNeutral = true;
                signalInfo.Sensor.Gear = Gear.Neutral;
            }
            // 1 0 0 0  //  0 1 1 1
            else if (signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && !signalInfo.Sensor.GearDisplayD4)
            {
                signalInfo.Sensor.Gear = Gear.One;
            }
            else if (!signalInfo.Sensor.GearDisplayD1 && signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && !signalInfo.Sensor.GearDisplayD4)
            {
                signalInfo.Sensor.Gear = Gear.Two;
            }
            else if (!signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && signalInfo.Sensor.GearDisplayD3 && !signalInfo.Sensor.GearDisplayD4)
            {
                signalInfo.Sensor.Gear = Gear.Three;
            }
            else if (!signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && signalInfo.Sensor.GearDisplayD4)
            {
                signalInfo.Sensor.Gear = Gear.Four;
            }
            else if (signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && signalInfo.Sensor.GearDisplayD4)
            {
                signalInfo.Sensor.Gear = Gear.Five;
            }
            else if (signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && signalInfo.Sensor.GearDisplayD4)
            {
                signalInfo.Sensor.Gear = Gear.Reverse;
            }
        }



        protected virtual void ProcessGearWithPullLine(CarSignalInfo signalInfo)
        {

            signalInfo.Sensor.IsNeutral = false;


            if (!signalInfo.Sensor.PullLineD1 && !signalInfo.Sensor.PullLineD2 && !signalInfo.Sensor.PullLineD3 && !signalInfo.Sensor.PullLineD4)
            {
                signalInfo.Sensor.IsNeutral = true;
                signalInfo.Sensor.Gear = Gear.Neutral;
            }
            else if (signalInfo.Sensor.PullLineD1 && signalInfo.Sensor.PullLineD2 && !signalInfo.Sensor.PullLineD3 && !signalInfo.Sensor.PullLineD4)
            {
                signalInfo.Sensor.Gear = Gear.One;
            }
            else if (!signalInfo.Sensor.PullLineD1 && signalInfo.Sensor.PullLineD2 && !signalInfo.Sensor.PullLineD3 && signalInfo.Sensor.PullLineD4)
            {
                signalInfo.Sensor.Gear = Gear.Two;
            }
            else if (signalInfo.Sensor.PullLineD1 && !signalInfo.Sensor.PullLineD2 && !signalInfo.Sensor.PullLineD3 && !signalInfo.Sensor.PullLineD4)
            {
                signalInfo.Sensor.Gear = Gear.Three;
            }
            else if (!signalInfo.Sensor.PullLineD1 && !signalInfo.Sensor.PullLineD2 && !signalInfo.Sensor.PullLineD3 && signalInfo.Sensor.PullLineD4)
            {
                signalInfo.Sensor.Gear = Gear.Four;
            }
            else if (signalInfo.Sensor.PullLineD1 && !signalInfo.Sensor.PullLineD2 && signalInfo.Sensor.PullLineD3 && !signalInfo.Sensor.PullLineD4)
            {
                signalInfo.Sensor.Gear = Gear.Five;
            }
        }
        protected virtual void ProcessIsValid(CarSignalInfo signalInfo)
        {
            if (signalInfo.Gps==null)
            {
                signalInfo.IsGpsValid = false;
                Logger.Error("ProcessIsValid:"+string.Join(",",signalInfo.commands));
                return;
            }
            //有卫星数量
            signalInfo.IsGpsValid = signalInfo.Gps.FixedSatelliteCount > 0 && signalInfo.Gps.LatitudeDegrees > 0 && signalInfo.Gps.LongitudeDegrees > 0;

            //对于异常速度进行处理
            if (signalInfo.Sensor.SpeedInKmh >= 0 && signalInfo.Sensor.SpeedInKmh <= Constants.InvalidSpeedLimit)
            {
                signalInfo.IsSensorValid = true;
                return;
            }
            //对于异常转速比进行处理

            //对最近2秒钟的信号进行判断分析，如果持续无效信息则返回
            var queryTime = DateTime.Now.AddSeconds(-2);
            var allInvalid = CarSignalSet.TakeWhile(x => x.RecordTime > queryTime).All(x => x.Sensor.SpeedInKmh < 0 && x.Sensor.SpeedInKmh > Constants.InvalidSpeedLimit);
            signalInfo.IsSensorValid = !allInvalid;

        }

        protected virtual void ProcessCarState(CarSignalInfo signalInfo)
        {
            if (signalInfo.SpeedInKmh >= Settings.ParkingValueKmh)
            {
                signalInfo.CarState = CarState.Moving;
                return;
            }
            var carSignal = CarSignalSet.FirstOrDefault();
            if (carSignal == null)
            {
                signalInfo.CarState = CarState.Stop;
                return;
            }

            //跳过速度小于ParkingValue并且记录时间大于延迟停车时间
            var hasMovingCarSignal = CarSignalSet.QueryCachedSeconds(Settings.ParkingDelaySeconds).Any(x => x.SpeedInKmh > Settings.ParkingValueKmh);
            signalInfo.CarState = hasMovingCarSignal ? CarState.Moving : CarState.Stop;
        }

        protected virtual void ProcessDistance(CarSignalInfo signalInfo)
        {
            if (ExamContext == null)
                return;

            if (ExamContext.IsExaming)
            {
                MileageUpdator.Execute(signalInfo);
                signalInfo.Distance = MileageUpdator.Distance;
                ExamContext.TravlledDistance = signalInfo.Distance;
            }
            else
            {
                //todo:经常起步不能结束，原因，一点开始考试会先给距离附上上一次结束考试时的MileageUpdator.Distance，
                //然后再重置MileageUpdator.Distance=0,所以造成不能结束问题
                MileageUpdator.Reset();

                // Logger.Error("ProcessDistanceError","ExamContextIsNotExaming");
                signalInfo.Distance = 0;
            }
        }

        /// <summary>
        /// 处理使用的时间
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessUsedTime(CarSignalInfo signalInfo)
        {
            if (ExamContext == null)
                return;

            if (ExamContext.IsExaming)
            {
                signalInfo.UsedTime = DateTime.Now - ExamContext.StartExamTime;
            }
            else
            {
                var lastSignal = CarSignalSet.FirstOrDefault();
                if (lastSignal != null)
                {
                    signalInfo.UsedTime = lastSignal.UsedTime;
                }
            }
            ExamContext.UsedTime = signalInfo.UsedTime;
        }

        protected virtual void ProcessAngle(CarSignalInfo signalInfo)
        {
            //if (signalInfo.IsGpsValid&&signalInfo.Gps!=null)
            //{
            //  signalInfo.BearingAngle = AngleCalculator.CalculateAngle(signalInfo.Gps);
            //}
            if (Settings.AngleSource==AngleSource.GPS)
            {
                if (signalInfo.IsGpsValid && signalInfo.Sensor != null && signalInfo.Sensor.Heading > 0)
                {
                    signalInfo.BearingAngle = AngleCalculator.CalculateAngle(signalInfo.Gps);
                }
            }
           
         
        }

        protected virtual void ProcessEngineRpm(CarSignalInfo signalInfo)
        {
            switch (Settings.EngineRpmRatioMode)
            {
                case EngineRpmRatioMode.ZoomIn:
                    signalInfo.EngineRpm = Convert.ToInt32(signalInfo.Sensor.EngineRpm * Settings.EngineRpmRatio);
                    break;
                case EngineRpmRatioMode.ZoomOut:
                    signalInfo.EngineRpm = Convert.ToInt32((double)signalInfo.Sensor.EngineRpm / Settings.EngineRpmRatio);
                    break;
                default:
                    signalInfo.EngineRpm = signalInfo.Sensor.EngineRpm;
                    break;
            }

            //当发动机转速异常时进行过滤
            if (signalInfo.EngineRpm > 5000)
            {
                signalInfo.IsSensorValid = false;
                signalInfo.EngineRpm = 0;
            }

        }

        protected virtual void ProcessSpeedMode(CarSignalInfo signalInfo)
        {//处理速度，20160328
            switch (Settings.SpeedKhmMode)
            {
                case SpeedKhmMode.ZoomIn:
                    signalInfo.Sensor.SpeedInKmh = Convert.ToInt32(signalInfo.Sensor.SpeedInKmh * Settings.MultiSpeed);
                    break;
                case SpeedKhmMode.ZoomOut:
                    signalInfo.Sensor.SpeedInKmh = Convert.ToInt32(signalInfo.Sensor.SpeedInKmh / Settings.MultiSpeed);
                    break;
                default:
                    break;
            }
        }

        private static int AmoutCount = 100;
        //记录安全带 
        private Queue<bool> SafetyBeltQueue = new Queue<bool>(AmoutCount);
        //记录手刹
        private Queue<bool> HandBrakeQueue = new Queue<bool>(AmoutCount);
        protected virtual void ProcessSafeBelt(CarSignalInfo signalInfo)
        {
            SafetyBeltQueue.Enqueue(signalInfo.Sensor.SafetyBelt);
            if (SafetyBeltQueue.Count >= AmoutCount)
            {
                SafetyBeltQueue.Dequeue();
            }

            if (signalInfo.Sensor.SafetyBelt)
                return;
            //安全带,进行配置延时
            //
            if (SafetyBeltQueue.Count > 10)
            {
                int delayCount = (int)Settings.CommonExamItemsSareBeltTimeOut*5;
                if (SafetyBeltQueue.Count < delayCount)
                {
                    delayCount = SafetyBeltQueue.Count - 3;
                }

                signalInfo.Sensor.SafetyBelt = SafetyBeltQueue.Reverse().Take(delayCount).Any(x => x == true);

            }

        }

        protected virtual void ProcessHandbrake(CarSignalInfo signalInfo)
        {
           // signalInfo.Sensor.Handbrake
            HandBrakeQueue.Enqueue(signalInfo.Sensor.Handbrake);
            if (HandBrakeQueue.Count >= AmoutCount)
            {
                HandBrakeQueue.Dequeue();
            }
            //如果手刹没有拉起不管
            //手刹没有拉起是false
            if (!signalInfo.Sensor.Handbrake)
                return;
            //首杀,进行配置延时
            //
            if (HandBrakeQueue.Count > 10)
            {
                int delayCount = 1 * 5;
                if (HandBrakeQueue.Count < delayCount)
                {
                    delayCount = HandBrakeQueue.Count - 3;
                }

                signalInfo.Sensor.Handbrake = HandBrakeQueue.Reverse().Take(delayCount).Any(x => x == true);

            }

        }
        protected virtual void ProcessEngine(CarSignalInfo signalInfo)
        {

            //如果发动机线的配置不是默认值则 就进行处理发动机线的函数
            if (Settings.EngineAddress != -1)
            {
                ProcessEngineLine(signalInfo);
                return;
            }

            //如果信号无效，用最后一次有效的发动机状态，不能过滤-1的情况
            if (signalInfo.Sensor == null || !signalInfo.IsSensorValid)
            {
                if (signalInfo.Sensor == null)
                    Logger.WarnFormat("ProcessEngine函数，处理发动机状态时sensor==null");

                var lastSignal = CarSignalSet.FirstOrDefault(d => d.IsSensorValid && d.Sensor != null);
                if (lastSignal != null)
                {
                    signalInfo.Sensor.Engine = lastSignal.Sensor.Engine;
                }
                else
                {
                }
                //处理新版0BD熄火不掉转速
                if (signalInfo.Sensor.Engine==false)
                {
                    signalInfo.EngineRpm = 0;
                }
                return;
            }


            if (signalInfo.EngineRpm > Settings.EngineStopRmp)
            {
                signalInfo.Sensor.Engine = true;
                return;
            }

            //按照连续的信号个数，基本相当于2秒,这个还是需要配置可以进行调节，发动机熄火时间。
            if (CarSignalSet.Count > 10)
            {
                int delayCount =(int)Settings.CommonExamItemsEngineTimeOut*5;
                if (CarSignalSet.Count < delayCount)
                {
                    delayCount = CarSignalSet.Count - 3;
                }
                signalInfo.Sensor.Engine = CarSignalSet.Where(x => x.IsSensorValid).Take(delayCount).Any(x => x.EngineRpm > Settings.EngineStopRmp);

                //处理新版OBD，钥匙关了后转速20秒不掉问题,强制设0
                if (signalInfo.Sensor.Engine == false)
                    signalInfo.EngineRpm = 0;

                return;
            }
            //为什么不用条件编译？或者通过配置？不然很容易忘记那个版本需要修改什么；
            //todo:重庆复盛网口版是过滤15个信号，其它地方5个 //徐州是10
            //5条记录任意一条，防止转速闪烁 
            signalInfo.Sensor.Engine = CarSignalSet.Where(x => x.IsSensorValid).Take(5).Any(x => x.EngineRpm > Settings.EngineStopRmp);

            //处理新版0BD熄火不掉转速
            if (signalInfo.Sensor.Engine == false)
            {
                signalInfo.EngineRpm = 0;
            }

            //表示需要进行处理发动机信号 ,如果两个同时无效 才认为发动机熄火,只要其中一个有信号就认为发动机没有熄火！
        }
        private Queue<bool> recordEngineInfo = new Queue<bool>(10);
        /// <summary>
        /// 处理发动机 外接发动机线的情况
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessEngineLine(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor == null || !signalInfo.IsSensorValid)
            {
                if (signalInfo.Sensor == null)
                    Logger.WarnFormat("ProcessEngine函数，处理发动机状态时sensor==null");



                var lastSignal = CarSignalSet.FirstOrDefault(d => d.IsSensorValid && d.Sensor != null);
                if (lastSignal != null)
                {
                    Logger.WarnFormat("ProcessEngine函数，处理发动机状态时使用上一次数据：{0}-{1}，当前发动机转速：{2}-{3}", lastSignal.Sensor.Engine, lastSignal.EngineRpm, signalInfo.Sensor.Engine, signalInfo.EngineRpm);
                    signalInfo.Sensor.Engine = lastSignal.Sensor.Engine;
                }
                else
                {
                    //signalInfo.Sensor.Engine = true;
                }
                return;
            }

            //发动机信号线为高电平或者转速大于10，都认为发动机处理正常状态；否则，发动机视为熄火状态；发动机信号线延迟0.5秒，转速线延迟0.5秒 
            recordEngineInfo.Enqueue(signalInfo.Sensor.Engine);
            if (recordEngineInfo.Count > 10)
            {
                recordEngineInfo.Dequeue();
            }
            signalInfo.Sensor.Engine = recordEngineInfo.Any(x => x == true) ||
                                      signalInfo.EngineRpm > Constants.EngineRpmLimit ||
                                      CarSignalSet.Take(5).Where(x => x.IsSensorValid).Any(x => x.EngineRpm > Constants.EngineRpmLimit)
                                      || signalInfo.Sensor.SpecialEngine;
            //signalInfo.Sensor.Engine = signalInfo.Sensor.Engine ||
            //                           signalInfo.EngineRpm > Constants.EngineRpmLimit ||
            //                           CarSignalSet.Take(5).Where(x => x.IsSensorValid).Any(x => x.EngineRpm > Constants.EngineRpmLimit);
            if (!signalInfo.Sensor.Engine)
            {
                Logger.DebugFormat("发动机熄火，当前转速：{0}", signalInfo.EngineRpm);
            }
        }


        /// <summary>
        /// 当发动机熄火时OBD速度熄时太高过滤
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessSpeed(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor == null)
            {
                signalInfo.SpeedInKmh = 0;
                Logger.WarnFormat("ProcessSpeed函数，处理速度时sensor==null");
                return;
            }
            signalInfo.SpeedInKmh = signalInfo.Sensor.SpeedInKmh > 200 ? 0 : signalInfo.Sensor.SpeedInKmh;
        }

        private Gear ParseGear(CarSignalInfo signalInfo)
        {
            var ratio = signalInfo.EngineRatio;
            if (ratio <= 0)
                return Gear.Neutral;

            if (ratio >= Settings.GearOneMinRatio && ratio <= Settings.GearOneMaxRatio)
            {
                return Gear.One;
            }
            if (ratio >= Settings.GearTwoMinRatio && ratio <= Settings.GearTwoMaxRatio)
            {
                return Gear.Two;
            }
            if (ratio >= Settings.GearThreeMinRatio && ratio <= Settings.GearThreeMaxRatio)
            {
                return Gear.Three;
            }
            if (ratio >= Settings.GearFourMinRatio && ratio <= Settings.GearFourMaxRatio)
            {
                return Gear.Four;
            }
            if (ratio >= Settings.GearFiveMinRatio && ratio <= Settings.GearFiveMaxRatio)
            {
                return Gear.Five;
            }
            return Gear.Neutral;
        }
    }
}