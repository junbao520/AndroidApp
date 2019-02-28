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
    public class UDPCarSignalSeed : ISensorCarSignalSeed
    {
        public const string DefaultWifiName = "yib-13948694";
        public const string DefaultWifiPassword = "88888888";
        public const int DefaultWifiPort = 4000;
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";
        public const int MaxTimeoutCount = 5;
        public const int ReadBufferCount =1024;
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

        public UDPCarSignalSeed(IMessenger Messenger, ILog log, ICarSignalReceviedHandler carSignalReceviedHandler, IDataService dataService)
        {
            SensorParser = new CarSignalParserV4();
            this.Messenger = Messenger;
            this.SignalHandler = carSignalReceviedHandler;
            this.Logger = log;
            Settings = dataService.GetSettings();
        }
        public void Init(NameValueCollection settings)
        {
        }
        public bool InitAsync(CH34xUARTDriver usbDriver=null, Connections serialDriver=null)
        {
            datagramSocket = new DatagramSocket(DefaultWifiPort);
            datagramSocket.Broadcast = true;
            datagramPacket = new DatagramPacket(Readbuffer, Readbuffer.Length);
            return true;
        }

        public void StartAsync()
        {
            //开启一个线程来单独进行信号处理
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

        
        public void StartCore()
        {
            while (!IsThreadDisable)
            {
                try
                {
                    var rawItems = ReadToCommand();
                    if(rawItems.Length == 0)
                    {
                        continue;
                    }
                    var lines = rawItems.SelectMany(SpiltCommand).ToArray();
                    if (lines.Length > 2)
                    {
                        var carSignalInfo = SensorParser.Parse(lines);
                        SignalHandler.Execute(carSignalInfo);
                        if (Settings.AngleSource == AngleSource.Gyroscope)
                        { 
                         carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                        }
                        OnCarSignalRecevied(carSignalInfo);
                        LogCommands(rawItems);
                    }
                }
                catch (Exception exp)
                {
                    Logger.Error(exp.Message);
                }
            }
        }
        protected virtual void LogCommands(IEnumerable<string> commands)
        {
    
            if (Settings.GpsLogEnable)
              {
                 Logger.WriteSensorLog(commands);
              }
 
        }
        protected virtual void OnCarSignalRecevied(CarSignalInfo carSingal)
        {
            //读取到车载信号发送消息
            if (CarSignalRecevied != null)
                CarSignalRecevied(carSingal);
            //发送消息
            Messenger.Send(new CarSignalReceivedMessage(carSingal));

        }

        //我希望采用消息机制来进行这个信号读取和传送
        //
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
               // Messenger.Send(new BthExceptionMessage());
                Log.Error("UDPERROR", ex.Message);
            }
            return new string[0];
        }

     
    }
}