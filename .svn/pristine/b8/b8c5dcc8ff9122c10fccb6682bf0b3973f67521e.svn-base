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
    public class UDPCarSignalSeedMessage : ISensorCarSignalSeed
    {
        public const string DefaultWifiName = "yib-13948694";
        public const string DefaultWifiPassword = "20140501";
        public const int DefaultWifiPort = 4000;
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";
        public const int MaxTimeoutCount = 5;
        public const int ReadBufferCount = 512;
        public byte[] Readbuffer = new byte[ReadBufferCount];
        protected CancellationTokenSource CancellationSource { get; private set; }
        protected AutoResetEvent Event { get; private set; }

        protected char[] Spearator = System.Environment.NewLine.ToCharArray();

        public int TimeoutCount { get; set; }

        public bool IsThreadDisable = false;
        protected ICarSignalParser SensorParser { get; set; }

        DatagramSocket datagramSocket;
        DatagramPacket datagramPacket; 
        public Action<CarSignalInfo> CarSignalRecevied { get; set; }

        protected IMessenger Messenger { get; private set; }

        protected ICarSignalReceviedHandler SignalHandler { get; set; }

        protected GlobalSettings Settings { get; private set; }

        protected ILog Logger { get; set; }

        public UDPCarSignalSeedMessage(IMessenger Messenger, ILog log, ICarSignalReceviedHandler carSignalReceviedHandler, IDataService dataService)
        {
            SensorParser = new CarSignalParserV4();
            this.Messenger = Messenger;
            this.SignalHandler = carSignalReceviedHandler;
            this.Logger = log;
            Settings = dataService.GetSettings();
            RegisterMessages(Messenger);
        }

        protected void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<CarSensorMessage>(this, OnCarSensorMessageReceive);
        }
        public void OnCarSensorMessageReceive(CarSensorMessage carSensorMessage)
        {
            try
            {
                var rawItems = carSensorMessage.Sensor;
                var lines = rawItems.SelectMany(SpiltCommand).ToArray();
                if (lines.Length>2)
                {
                    var carSignalInfo = SensorParser.Parse(lines);
                    SignalHandler.Execute(carSignalInfo);
                    if(Settings.AngleSource == AngleSource.Gyroscope)
                    {
                        carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                    }
                    OnCarSignalRecevied(carSignalInfo);
                    LogCommands(rawItems);
                }
            }
            catch (Exception exp)
            {
                Logger.Error("OnCarSensorMessageReceive:" + exp.Message);
            }
        }

        public void Init(NameValueCollection settings)
        {
        }
    
        public bool InitAsync(CH34xUARTDriver usbDriver=null, Connections serialDriver=null)
        {
            datagramSocket = new DatagramSocket(DefaultWifiPort);
            datagramSocket.Broadcast = true;
            //ReadBuffer  其实这个数据可以缩小，让每次读取的数据量变小
            datagramPacket = new DatagramPacket(Readbuffer, Readbuffer.Length);
            return true;
        }
        public void StartAsync()
        {
          Thread thread = new Thread((new ThreadStart(StartCore)));
          thread.Start();
        }

        public IEnumerable<string> SpiltCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                yield break;

            var s = command.First().ToString(CultureInfo.InvariantCulture);
            foreach (var c in command.Skip(1))
            {
                if (c == '$')
                {
                    yield return s;
                    s = c.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    s += c;
                }
            }
            if (s.Length > 0)
                yield return s;
        }
       /// <summary>
       /// 不在采用消息机制转发数据直接解析数据.
       /// </summary>
        public void StartCore()
        {
            while (!IsThreadDisable)
            {
                var rawItems = ReadToCommand();
                if (rawItems.Length!=0)
                {
                    Messenger.Send(new CarSensorMessage(rawItems));
                }
               
            }
        }
        protected virtual void LogCommands(IEnumerable<string> commands)
        {
            //进行一个日志采集
            if (Settings.GpsLogEnable)
            {
                Logger.WriteSensorLog(commands);
            }
        }
        protected virtual void OnCarSignalRecevied(CarSignalInfo carSingal)
        {
            try
            {
                if (CarSignalRecevied != null)
                    CarSignalRecevied(carSingal);
                Messenger.Send(new CarSignalReceivedMessage(carSingal));
            }
            catch (Exception ex)
            {
                Logger.Error("OnCarSignalRecevied", ex.Message);
            }
        }
        /// <summary>
        /// 如果输出的是两条Debug 是否会有信号问题
        /// </summary>
        /// <returns></returns>
        public string[] ReadToCommand()
        {
            try
            {
                datagramSocket.Receive(datagramPacket);
                string strMsg = new Java.Lang.String(datagramPacket.GetData()).Trim();
                string[] msg = strMsg.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                return msg;
            }
            catch (System.Exception ex)
            {
                Log.Error("UDPERROR", ex.Message);
            }
            return new string[0];
        }
        public string[] ReadTestCommand()
        {
            string[] msg = new string[5];
            msg[0] = "$GNRMC,065948.20,A,2932.22840,N,10632.06767,E,24.986,216.34,220117,,,D*45";
            msg[1] = "$GNGGA,065948.20,2932.22840,N,10632.06767,E,2,09,0.76,185.4,M,-26.5,M,,0000*69";
            msg[2] = "$GYRO,acc:0.34 -0.00 0.93,gyro:0.00 0.00 0.00,angle:-0.30 -20.13 -87.84;";
            msg[3] = "$OBD_SENSOR,band:1,throttlePosition:27,gear:4,angle:6538.50,0110000000000001;";
            msg[4] = "$SENSOR,count:30084,time:6047000,v:v1.3,rpm:0.00,speed:0.00,obd_rpm:1478,obd_speed:50,voltage:0.00,a0:1,a1:0,000000000000000000000000;";
            return msg;
        }

     
    }
}