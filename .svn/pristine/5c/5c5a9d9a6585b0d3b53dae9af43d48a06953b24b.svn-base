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
using Java.Util;
using Android.Content;
using System.Globalization;
using TwoPole.Chameleon3.Infrastructure.Messages;
using Android.OS;
using Android.Util;
using Java.Lang.Reflect;
using CN.Wch.Ch34xuartdriver;
using Android.Hardware.Usb;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Android_serialport_api;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    /// <summary>
    /// 通过USB转串口读取数据 串口打开在最外面进行Open 这个类只负责读取数据
    /// </summary>
    public class USBSerialCarSignalSeed:ISensorCarSignalSeed
    {
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";
        public const int MaxTimeoutCount = 5;
        public const int ReadUSBBufferCount = 512;
        public byte[] buffer = new byte[ReadUSBBufferCount];
        public bool IsOpen = false;
        public double Angle = 0;
        //每一次读取的数据都存储在这个变量里面
        public string[] rawItems;
        CH34xUARTDriver myDriver;
        protected CancellationTokenSource CancellationSource { get; private set; }
        protected AutoResetEvent Event { get; private set; }

        public int TimeoutCount { get; set; }

        protected ICarSignalParser SensorParser { get; set; }
        public Action<CarSignalInfo> CarSignalRecevied { get; set; }

        protected IMessenger Messenger { get; private set; }

        protected ICarSignalReceviedHandler SignalHandler { get; set; }

        protected GlobalSettings Settings { get; private set; }

        protected ILog Logger { get; set; }
        public USBSerialCarSignalSeed(IMessenger Messenger,ILog log, ICarSignalReceviedHandler carSignalReceviedHandler,IDataService dataService)
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
            myDriver = usbDriver;
            IsOpen = true;
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
            while (true)
            {
                try
                {
                    rawItems = ReadToCommand();
                    if (rawItems.Length==0)
                    {
                        continue;
                    }
                    if (rawItems.Where(s => s.Contains("?")).Count() >= 1)
                    {
                        Logger.Error("rawItems contains ?", string.Join("@@@", rawItems));
                        continue;
                    }
                    var lines = rawItems.SelectMany(SpiltCommand).ToArray();
                    if (lines.Length > 2)
                    {
                        //信号解析类 如果信号解析结果和上一个结果不一样则丢掉
                        //最夸张得可以通过连续N个信号的对比进行处理
                        var carSignalInfo = SensorParser.Parse(lines);
                        //直接对无效信号进行过滤处理！
                        if (!carSignalInfo.IsSensorValid)
                        {
                            Logger.Error("carSignalInfo.IsSensorValid", string.Join("@@@", rawItems));
                            continue;
                        }
                        SignalHandler.Execute(carSignalInfo);
                        if (Settings.AngleSource == AngleSource.Gyroscope)
                        {
                            carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                            if (carSignalInfo.BearingAngle!=180)
                            {
                                Angle = carSignalInfo.BearingAngle;
                            }
                            else
                            {
                                carSignalInfo.BearingAngle = Angle;
                            }
                        }
                        else 
                        {
                            if (carSignalInfo.Gps!=null)
                            {
                                carSignalInfo.BearingAngle = carSignalInfo.Gps.AngleDegrees;
                                //TODO:其实在这点处理不合适
                                if (carSignalInfo.BearingAngle!=0)
                                {
                                    Angle = carSignalInfo.BearingAngle;
                                }
                                else
                                {
                                    carSignalInfo.BearingAngle = Angle;
                                }
                            }
                            else
                            {
                                Logger.Info("carSignalInfo Gps is null");
                            }

                        }
                        //把原始信号记录器
                        carSignalInfo.commands = rawItems;
                        OnCarSignalRecevied(carSignalInfo);
                        LogCommands(rawItems);
                    }
                
                }
                catch (Exception exp)
                {
                    Logger.ErrorFormat("USBSerialStartCorte发生异常,原因:{0} 原始信号{1}",exp.Message, string.Join("@@@", rawItems));
                    //Logger.Error("USBSerialStartCorte",exp.Message);
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
            //读取到车载信号发送消息
            if (CarSignalRecevied != null)
                CarSignalRecevied(carSingal);
            //如果信号无效则不要进行发送消息
            if (!carSingal.IsSensorValid)
            {
                Logger.Error("OnCarSignalRecevied IsSensorValid", string.Join("|", carSingal.commands));
                return;
            }
            Messenger.Send(new CarSignalReceivedMessage(carSingal));

        }

        private void EndConnection(IAsyncResult asyncResult)
        {

        }
        public string ReadLine()
        {
            byte[] buffer2 = new byte[1024];
            byte[] buf = new byte[1];
            int count2 = 0;
            while (count2 < 1024)
            {
                
                int n = myDriver.ReadData(buf, 1);
                if (n == 0)
                {
                    System.Threading.Thread.Sleep(10);
                }
                else if (buf[0] == '\n')
                {
                    if (count2 > 0 && buffer2[count2-1] == '\r')
                    {
                        count2 -= 1;
                    }
                    if (count2 == 0)
                        return string.Empty;
                    else
                        return System.Text.ASCIIEncoding.ASCII.GetString(buffer2, 0, count2);
                }
                else
                {
                  buffer2[count2++] = buf[0];
                }
            }
            return string.Empty;
        }
        public string[] ReadToCommand()
        {
            var lines = new List<string>();
            while (IsOpen)
            {
              
                var line = ReadLine();

                //表示有乱码 直接去掉
                //去除乱码情况
                if (!string.IsNullOrEmpty(line) && line.Length >= 10 )
                {
                    lines.Add(line);
                }
                var cmd = line.GetCommand();
                //读取出来的信号包含乱码的去掉
                if (cmd != null && cmd.Equals(SensorCommand,StringComparison.OrdinalIgnoreCase))
                    break;

            }
            return lines.ToArray();
        }

       
    }
}