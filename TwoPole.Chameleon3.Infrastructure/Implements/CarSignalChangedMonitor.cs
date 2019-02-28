using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class CarSignalChangedMonitor : ICarSignalDependency
    {
        public int Order { get { return 200; } }
        protected ILog Logger { get; set; }
        protected IMessenger Messenger { get; private set; }

        protected ICarSignalSet CarSignalSet { get; private set; }

        protected CarSignalInfo LastSignal { get; private set; }


        protected IDataService dataService;

        private bool IsTriggerPullOver = false;

        public CarSignalChangedMonitor(IMessenger messenger, ICarSignalSet carSignalSet,ILog log,IDataService dataService)
        {
            Messenger = messenger;
            this.Logger = log;
            this.dataService = dataService;
            CarSignalSet = carSignalSet;
        }

        public void Execute(CarSignalInfo signalInfo)
        {
            if (LastSignal == null)
            {
                LastSignal = signalInfo;
                return;
            }

            if (!signalInfo.IsSensorValid)
                return;

            try
            {
                HandleMessages(signalInfo);
            }
            catch (Exception exp)
            {
               Logger.ErrorFormat("执行信号改变命令出现异常，原因：{0}", exp, exp);
            }
            finally
            {             
                LastSignal = signalInfo;
            }
        }

        private void HandleMessages(CarSignalInfo signalInfo)
        {
            //对无效的信号进行过滤
            if (signalInfo == null || LastSignal == null)
                return;
            var sensorInfo = signalInfo.Sensor;
            var lastSensorInfo = LastSignal.Sensor;

            //手刹
            if (lastSensorInfo.Handbrake != sensorInfo.Handbrake)
                Messenger.Send(new HandBreakChangedMessage(sensorInfo.Handbrake)  { SignalInfo = signalInfo });

            //喇叭
            if (lastSensorInfo.Loudspeaker != sensorInfo.Loudspeaker)
                Messenger.Send(new LoudspeakerChangedMessage(sensorInfo.Loudspeaker) { SignalInfo = signalInfo });

            //发动机 消息机制
            if (lastSensorInfo.Engine != sensorInfo.Engine)
            {
                Messenger.Send(new EngineChangedMessage(sensorInfo.Engine) { SignalInfo = signalInfo });
                if (sensorInfo.Engine)
                    Messenger.Send(new EngineStartMessage());
                else
                {
                    //打印出原始信号
                   // Logger.Error("SendEngineStopMessage", string.Join(",", signalInfo.commands));
                    Messenger.Send(new EngineStopMessage());
                    
                }
            }

            //指纹仪
            if(lastSensorInfo.Fingerprint!=sensorInfo.Fingerprint)
            {
                if(sensorInfo.Fingerprint)
                    Messenger.Send(new FingerprintMessage());
            }
            if (lastSensorInfo.SafetyBelt!=sensorInfo.SafetyBelt)
            {
                if (sensorInfo.SafetyBelt)
                {
                    Messenger.Send(new SafetyBeltMessage());
                }
            }
       
            //车门
            if (lastSensorInfo.Door != sensorInfo.Door)
            {
                Messenger.Send(new DoorChangedMessage(sensorInfo.Door) { SignalInfo = signalInfo });
                if (sensorInfo.Door)
                    Messenger.Send(new OpenDoorMessage());
                else
                    Messenger.Send(new CloseDoorMessage());
            }

            //档位变换
            if (lastSensorInfo.Gear != sensorInfo.Gear)
                Messenger.Send(new SwitchGearMessage(sensorInfo.Gear, lastSensorInfo.Gear) { SignalInfo = signalInfo });

            //刹车
            if (lastSensorInfo.Brake != sensorInfo.Brake)
                Messenger.Send(new BrakingChangedMessage(sensorInfo.Brake) { SignalInfo = signalInfo });

            //车辆状态
            if(signalInfo.CarState != LastSignal.CarState)
                Messenger.Send(new CarStateChangedMessage(signalInfo.CarState, LastSignal.CarState) { SignalInfo = signalInfo });

            //是否空挡
            if(lastSensorInfo.IsNeutral != sensorInfo.IsNeutral)
                Messenger.Send(new IsNeutralChangedMessage(sensorInfo.IsNeutral) { SignalInfo = signalInfo });

            //if (dataService.GetSettings().EndExamByDistance&&signalInfo.Distance>= dataService.GetSettings().ExamDistance&&IsTriggerPullOver==false)
            //{
            //    //发送靠边停车消息，系统触发靠边停车
            //    Messenger.Send(new PullOverTriggerMessage());
            //    IsTriggerPullOver = true;
            //}

            //判断一下 是否自动触发靠边停车
        }
    }
}
