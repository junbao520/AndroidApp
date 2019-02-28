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
                //TODO:��¼���10��ֵ�ԽǶȽ��д���
                if (CarSignalQuque.Count>=10)
                {
                    CarSignalQuque.Dequeue();
                }
                CarSignalQuque.Enqueue(signalInfo);
                //���棺˳���൱��Ҫ����������Ķ������������������⣻
                //�����ź��Ƿ���Ч
                ProcessIsValid(signalInfo);
                //���㿼�Ժ�ʱ
                ProcessUsedTime(signalInfo);
              
                //�Զ�����㺽��� �����Զ�����㺽���
                // ProcessAngle(signalInfo);
                //�ź��˲� Ŀǰֻ��ԽǶ�
                //TODO:Ŀǰ����������Bug,��Ҫͨ��ģ����Ժ�ʵ�ʲ��Բſ���ʹ��
                //SensorWaveFiltering(signalInfo);
                //�����ٶȣ���OBD��ȡ�ٶ�
                ProcessSpeed(signalInfo);
                //������ͣ��״̬�����������ٶ��Ƿ��������ֵ
                ProcessCarState(signalInfo);
                //������������̣�ͨ��gps�����㣬������ֹͣ�󣬲�����
                ProcessDistance(signalInfo);
                //�����������棬
                //TODO:ת�ٱȷŴ���С��android�汾û�н��������� 
                ProcessEngineRpm(signalInfo);
                //�����ٶ�
                ProcessSpeedMode(signalInfo);
                //�������������Ƿ�Ϩ��
                ProcessEngine(signalInfo);
                //��������ת�����ٶȵı�ֵ�����ٱ�
                ProcessEngineRatio(signalInfo);
                //����ȫ���źţ�2017-01-18 ����
                ProcessSafeBelt(signalInfo);
                //������ɲ�źţ�2017-07-28 ����
                ProcessHandbrake(signalInfo);

                //Logger.Error(Settings.GearSource.ToString());
                if (Settings.GearSource == GearSource.GearDisplay)
                {
                    ProcessGearWithGearDisplay(signalInfo);
                }
                else if (Settings.GearSource == GearSource.OBD)
                {
                    //�����λ��Դ��Obd 
                    signalInfo.Sensor.Gear = signalInfo.Sensor.OBDGear;
                } 
                else
                {
                    
                    //���ݳ��ٱȼ��㵵λ
                    ProcessGear(signalInfo);
                    //�����Ƿ�յ����ź�Դ,Ĭ�ϴ���������ȡ
                    //TODO:ת�ٱ� ���ﲻ����ϴ���
                    //���ת�ٱ�
                    //���ѡ��ת�ٱ�
                    
                    if (signalInfo.Sensor.IsNeutral)
                    {
                       // Logger.Error("�յ�");
                        signalInfo.Sensor.Gear = Gear.Neutral;
                    }
                    //����ת�ٱ� ������Ͼ��ǿյ�
                    if (signalInfo.Sensor.Clutch)
                    {
                        signalInfo.Sensor.Gear = Gear.Neutral;
                    }
                }
             

            }
            catch (Exception ex)
            {
                //����signal=null�����
                if (signalInfo != null)
                    signalInfo.IsSensorValid = false;
                Logger.Error("SensorException", string.Join(",",signalInfo.commands));
                Logger.ErrorFormat("�źŴ����쳣��{0}", ex);
            }
        }

        //TODO:�ź��˲�,Ŀǰֻ��  �ٶ�,ת��,�Ƕ� ����ֵ�����˲�
        //�˲��㷨 ��λֵ�˲��㷨 ȡ�������N��ֵ��ȥ�����ֵ����Сֵ Ȼ����ƽ��ֵ
        //���źŽ����˲�,//
        protected void SensorWaveFiltering(CarSignalInfo signalInfo)
        {
            //GetEngineRpmFiltering(signalInfo);
            //GetSpeedFiltering(signalInfo);
            GetAngleFiltering(signalInfo);
        }
        protected void GetEngineRpmFiltering(CarSignalInfo signalInfo)
        {
            //��ʼ100���źŲ��ᴦ�������źŲ��ȶ������
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
            //��ʼ100���źŲ��ᴦ�������źŲ��ȶ������
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
            //��ʼ100���źŲ��ᴦ�������źŲ��ȶ������
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
        /// ����յ�������
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
            //���з���˸����
            signalInfo.Sensor.Gear = ParseGear(signalInfo);
        }


        /// <summary>
        /// ��λ������
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessGearWithGearDisplay(CarSignalInfo signalInfo)
        {
            //20151217������Կյ�ȡ��
            signalInfo.Sensor.IsNeutral = false;
            ///����ͨ���ߵ���Ϣ�����жϵ�λ

            //����Ҫ����ȥ���Զ����ź���  //
            //�յ����¾���ǿ�ƿյ� 
            //����ǿյ�
            if (!signalInfo.Sensor.GearDisplayD1 && !signalInfo.Sensor.GearDisplayD2 && !signalInfo.Sensor.GearDisplayD3 && !signalInfo.Sensor.GearDisplayD4)
            {
                //20151217�������ʱ���յ��������Ҫ��ȡ���Ե�ֵ
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
            //����������
            signalInfo.IsGpsValid = signalInfo.Gps.FixedSatelliteCount > 0 && signalInfo.Gps.LatitudeDegrees > 0 && signalInfo.Gps.LongitudeDegrees > 0;

            //�����쳣�ٶȽ��д���
            if (signalInfo.Sensor.SpeedInKmh >= 0 && signalInfo.Sensor.SpeedInKmh <= Constants.InvalidSpeedLimit)
            {
                signalInfo.IsSensorValid = true;
                return;
            }
            //�����쳣ת�ٱȽ��д���

            //�����2���ӵ��źŽ����жϷ��������������Ч��Ϣ�򷵻�
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

            //�����ٶ�С��ParkingValue���Ҽ�¼ʱ������ӳ�ͣ��ʱ��
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
                //todo:�����𲽲��ܽ�����ԭ��һ�㿪ʼ���Ի��ȸ����븽����һ�ν�������ʱ��MileageUpdator.Distance��
                //Ȼ��������MileageUpdator.Distance=0,������ɲ��ܽ�������
                MileageUpdator.Reset();

                // Logger.Error("ProcessDistanceError","ExamContextIsNotExaming");
                signalInfo.Distance = 0;
            }
        }

        /// <summary>
        /// ����ʹ�õ�ʱ��
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

            //��������ת���쳣ʱ���й���
            if (signalInfo.EngineRpm > 5000)
            {
                signalInfo.IsSensorValid = false;
                signalInfo.EngineRpm = 0;
            }

        }

        protected virtual void ProcessSpeedMode(CarSignalInfo signalInfo)
        {//�����ٶȣ�20160328
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
        //��¼��ȫ�� 
        private Queue<bool> SafetyBeltQueue = new Queue<bool>(AmoutCount);
        //��¼��ɲ
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
            //��ȫ��,����������ʱ
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
            //�����ɲû�����𲻹�
            //��ɲû��������false
            if (!signalInfo.Sensor.Handbrake)
                return;
            //��ɱ,����������ʱ
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

            //����������ߵ����ò���Ĭ��ֵ�� �ͽ��д��������ߵĺ���
            if (Settings.EngineAddress != -1)
            {
                ProcessEngineLine(signalInfo);
                return;
            }

            //����ź���Ч�������һ����Ч�ķ�����״̬�����ܹ���-1�����
            if (signalInfo.Sensor == null || !signalInfo.IsSensorValid)
            {
                if (signalInfo.Sensor == null)
                    Logger.WarnFormat("ProcessEngine��������������״̬ʱsensor==null");

                var lastSignal = CarSignalSet.FirstOrDefault(d => d.IsSensorValid && d.Sensor != null);
                if (lastSignal != null)
                {
                    signalInfo.Sensor.Engine = lastSignal.Sensor.Engine;
                }
                else
                {
                }
                //�����°�0BDϨ�𲻵�ת��
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

            //�����������źŸ����������൱��2��,���������Ҫ���ÿ��Խ��е��ڣ�������Ϩ��ʱ�䡣
            if (CarSignalSet.Count > 10)
            {
                int delayCount =(int)Settings.CommonExamItemsEngineTimeOut*5;
                if (CarSignalSet.Count < delayCount)
                {
                    delayCount = CarSignalSet.Count - 3;
                }
                signalInfo.Sensor.Engine = CarSignalSet.Where(x => x.IsSensorValid).Take(delayCount).Any(x => x.EngineRpm > Settings.EngineStopRmp);

                //�����°�OBD��Կ�׹��˺�ת��20�벻������,ǿ����0
                if (signalInfo.Sensor.Engine == false)
                    signalInfo.EngineRpm = 0;

                return;
            }
            //Ϊʲô�����������룿����ͨ�����ã���Ȼ�����������Ǹ��汾��Ҫ�޸�ʲô��
            //todo:���츴ʢ���ڰ��ǹ���15���źţ������ط�5�� //������10
            //5����¼����һ������ֹת����˸ 
            signalInfo.Sensor.Engine = CarSignalSet.Where(x => x.IsSensorValid).Take(5).Any(x => x.EngineRpm > Settings.EngineStopRmp);

            //�����°�0BDϨ�𲻵�ת��
            if (signalInfo.Sensor.Engine == false)
            {
                signalInfo.EngineRpm = 0;
            }

            //��ʾ��Ҫ���д��������ź� ,�������ͬʱ��Ч ����Ϊ������Ϩ��,ֻҪ����һ�����źž���Ϊ������û��Ϩ��
        }
        private Queue<bool> recordEngineInfo = new Queue<bool>(10);
        /// <summary>
        /// �������� ��ӷ������ߵ����
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessEngineLine(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor == null || !signalInfo.IsSensorValid)
            {
                if (signalInfo.Sensor == null)
                    Logger.WarnFormat("ProcessEngine��������������״̬ʱsensor==null");



                var lastSignal = CarSignalSet.FirstOrDefault(d => d.IsSensorValid && d.Sensor != null);
                if (lastSignal != null)
                {
                    Logger.WarnFormat("ProcessEngine��������������״̬ʱʹ����һ�����ݣ�{0}-{1}����ǰ������ת�٣�{2}-{3}", lastSignal.Sensor.Engine, lastSignal.EngineRpm, signalInfo.Sensor.Engine, signalInfo.EngineRpm);
                    signalInfo.Sensor.Engine = lastSignal.Sensor.Engine;
                }
                else
                {
                    //signalInfo.Sensor.Engine = true;
                }
                return;
            }

            //�������ź���Ϊ�ߵ�ƽ����ת�ٴ���10������Ϊ��������������״̬�����򣬷�������ΪϨ��״̬���������ź����ӳ�0.5�룬ת�����ӳ�0.5�� 
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
                Logger.DebugFormat("������Ϩ�𣬵�ǰת�٣�{0}", signalInfo.EngineRpm);
            }
        }


        /// <summary>
        /// ��������Ϩ��ʱOBD�ٶ�Ϩʱ̫�߹���
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void ProcessSpeed(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor == null)
            {
                signalInfo.SpeedInKmh = 0;
                Logger.WarnFormat("ProcessSpeed�����������ٶ�ʱsensor==null");
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