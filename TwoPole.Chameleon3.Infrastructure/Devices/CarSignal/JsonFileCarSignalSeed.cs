using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure.Devices.Gps;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure.Devices
{
    public class JsonFileCarSignalSeed 
    {
        private bool _isRunning = false;
        protected IProviderFactory ProviderFactory { get; set; }
        public ICarSignalReceviedHandler SignalHandler { get; private set; }
        protected string JsonFilename { get; set; }
        protected Timer Timer;
        protected StreamReader SignalReader;
        /// <summary>
        /// 间隔,单位毫秒
        /// </summary>
        private int ReadInterval { get; set; }
        protected ILog Logger { get; private set; }
        protected IMessenger Messenger { get; private set; }

        public JsonFileCarSignalSeed(
          IMessenger messenger)
        {
            Logger = Singleton.GetLogManager;
            Messenger = messenger;
            SignalHandler = Singleton.GetSignalHandler;
            Messenger.Register<ExamStartMessage>(this, OnExamStart);
    
        }

        private void OnExamStart(ExamStartMessage message)
        {
            try
            {
                SignalReader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception exp)
            {
                string str = exp.Message;
            }
        }

        public Action<CarSignalInfo> CarSignalRecevied { get; set; }

        public void Init()
        {
            //var jsonFile = settings.GetValue("Filename", string.Empty);
            //if (string.IsNullOrEmpty(jsonFile) || !File.Exists(jsonFile))
            //    throw new ArgumentException(string.Format("传感器信号文件{0}不存在", jsonFile));

            //JsonFilename = jsonFile;
            //ReadInterval = settings.GetIntValue("Interval", 200);
            var jsonFile = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath+"//"+"test.json";
            JsonFilename = jsonFile;
            ReadInterval =30;

        }

        /// <summary>
        ///初始化
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitAsync()
        {
            SignalReader = File.OpenText(JsonFilename);
            while (true)
            {
                ReadSignal(true);
                Thread.Sleep(ReadInterval);
            }
       
        }

        private void ReadSignal(object state)
        {
            if (_isRunning)
                return;

            _isRunning = true;
            try
            {
                //重新设置到最开始
                if (SignalReader.BaseStream.Position == SignalReader.BaseStream.Length)
                    SignalReader.BaseStream.Seek(0, SeekOrigin.Begin);

                var line = SignalReader.ReadLine();
                var carSingal = line.FromJson<CarSignalInfo>();
                carSingal.RecordTime = carSingal.Gps.UtcTime = carSingal.Gps.LocalTime = carSingal.Gps.RecordTime = carSingal.Sensor.RecordTime = DateTime.Now;
                if (carSingal != null)
                {
                    //处理信号
                    SignalHandler.Execute(carSingal);
                    OnCarSignalRecevied(carSingal);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("读取车载传感器发生异常：{0}", ex.StackTrace, ex);
            }
            finally
            {
                _isRunning = false;
            }
        }

        public virtual async Task StartAsync()
        {
            SignalReader.BaseStream.Seek(0, SeekOrigin.Begin);
            Timer.Change(ReadInterval, ReadInterval);
        }

        public virtual async Task StopAsync()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        protected virtual void OnCarSignalRecevied(CarSignalInfo carSingal)
        {
            //if (!carSingal.IsSensorValid)
            //{
            //读取到车载信号发送消息
            if (CarSignalRecevied != null)
                CarSignalRecevied(carSingal);
            //发送消息
            Messenger.Send(new CarSignalReceivedMessage(carSingal));

        }

        protected void Free(bool disposing)
        {
            if (SignalReader != null)
            {
                SignalReader.Close();
                SignalReader = null;
            }
            if (Timer != null)
            {
                Timer.Dispose();
                Timer = null;
            }
            Messenger.Unregister(this);
        }
    }
}
