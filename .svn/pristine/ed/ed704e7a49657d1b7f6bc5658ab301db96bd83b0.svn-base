using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using TwoPole.Chameleon3.Infrastructure.Services;
using TwoPole.Chameleon3.Infrastructure.Devices.Gps;
using Android.Bluetooth;
using Java.Util;
using Android.Content;
using System.Globalization;
using TwoPole.Chameleon3.Infrastructure.Messages;
using Android.OS;
using Android.Util;
using Java.Lang.Reflect;
using Android.Net.Wifi;
using Java.Net;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Android_serialport_api;
using CN.Wch.Ch34xuartdriver;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class BinaryCarSignalSeed:ISensorCarSignalSeed
    {
        public const string DefaultWifiName = "yib-13948694";
        public const string DefaultWifiPassword = "20140501";
        public const int DefaultWifiPort = 4000;
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";
        public const int MaxTimeoutCount = 5;
        public const int ReadBufferCount =1024;
        public byte[] Readbuffer = new byte[ReadBufferCount];
        public const byte Begin = 0xAA;
        public int DataEndIndex = 0;
        public int DataBeginIndex = 0;
        protected CancellationTokenSource CancellationSource { get; private set; }
        protected AutoResetEvent Event { get; private set; }

        protected char[] Spearator = System.Environment.NewLine.ToCharArray();

        public int TimeoutCount { get; set; }

        public bool IsThreadDisable = false;
        protected BinaryCarSignalParser SensorParser { get; set; }

        DatagramSocket datagramSocket;
        DatagramPacket datagramPacket;
        public Action<CarSignalInfo> CarSignalRecevied { get; set; }

        protected IMessenger Messenger { get; private set; }

        protected ICarSignalReceviedHandler SignalHandler { get; set; }

        protected GlobalSettings Settings { get; private set; }

        protected ILog Logger { get; set; }

        public BinaryCarSignalSeed(IMessenger Messenger)
        {
            SensorParser = new BinaryCarSignalParser();
            this.Messenger = Messenger;
            this.SignalHandler = Singleton.GetSignalHandler;
            this.Logger = Singleton.GetLogManager;
            Settings = Singleton.GetDataService.GetSettings();
            RegisterMessages(Messenger);
        }

        protected void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<BinaryCarSensorMessage>(this, OnCarSensorMessageReceive);
        }
        public void OnCarSensorMessageReceive(BinaryCarSensorMessage carSensorMessage)
        {
            try
            {
                var Buffer = carSensorMessage.Sensor;
                var carSignalInfo = SensorParser.Parse(Buffer);
                SignalHandler.Execute(carSignalInfo);
                if (Settings.AngleSource == AngleSource.Gyroscope)
                {
                    carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                }
                OnCarSignalRecevied(carSignalInfo);
            }
            catch (Exception exp)
            {
                Logger.Error("OnCarSensorMessageReceive:" + exp.Message);
            }
        }
        public void Init(NameValueCollection settings)
        {
        }
        public void InitAsync()
        {
            datagramSocket = new DatagramSocket(DefaultWifiPort);
            datagramSocket.Broadcast = true;
            //ReadBuffer  其实这个数据可以缩小，让每次读取的数据量变小
            datagramPacket = new DatagramPacket(Readbuffer, Readbuffer.Length);
        }
        public bool InitAsync(CH34xUARTDriver usbDriver=null, Connections serDriver=null)
        {
            try
            {
                datagramSocket = new DatagramSocket(DefaultWifiPort);
                datagramSocket.Broadcast = true;
                //ReadBuffer  其实这个数据可以缩小，让每次读取的数据量变小
                datagramPacket = new DatagramPacket(Readbuffer, Readbuffer.Length);
            }
            catch (Exception ex)
            {
                Logger.Error("InitAsync", ex.Message);
                return false;
            }
            return true;
           
        }
        public void StartAsync()
        {
            Thread thread = new Thread((new ThreadStart(StartCore)));
            thread.Start();
        }
        public void StartCore()
        {
            while (!IsThreadDisable)
            {
                var Buffeer = ReadToCommand();
                Messenger.Send(new BinaryCarSensorMessage(Buffeer));
            }
        }
        protected virtual void LogCommands(IEnumerable<string> commands)
        {
            //if (Settings.GpsLogEnable || Settings.CarSignalLogEnable)
            //{
            //进行一个完整的日志采集
            Logger.WriteSensorLog(commands);
            // }
        }
        protected virtual void OnCarSignalRecevied(CarSignalInfo carSingal)
        {

            //读取到车载信号发送消息
            //考试系统计算机
            if (CarSignalRecevied != null)
                CarSignalRecevied(carSingal);
            Messenger.Send(new CarSignalReceivedMessage(carSingal));


        }
        /// <summary>
        /// 如果输出的是两条Debug 是否会有信号问题
        /// </summary>
        /// <returns></returns>
        public byte[] ReadToCommand()
        {
            try
            {
                datagramSocket.Receive(datagramPacket);
                Readbuffer = datagramPacket.GetData();
                DataBeginIndex = 0;
                while (Readbuffer[DataBeginIndex] != Begin)
                {
                    DataBeginIndex++;
                }
                byte[] buffer = new byte[1024];
                //buffer[0] = Begin;
                int len =0;
                while (len < 1024)
                {
                    var b = (byte)Readbuffer[DataBeginIndex];
                    if (b == '\n')
                    {
                        if (buffer[len - 1] == '\r')
                            len -= 1;
                        return buffer.Take(len).ToArray();
                    }
                    DataBeginIndex++;
                    buffer[len++] = b;
                }
                //需要进行一个数据处理
                return buffer;
            }
            catch (System.Exception ex)
            {
                Log.Error("ReadToCommand", ex.Message);
            }
            return new byte[0];
        }

       
    }
}