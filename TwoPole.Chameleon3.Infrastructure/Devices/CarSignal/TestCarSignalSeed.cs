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
using TwoPole.Chameleon3.Infrastructure.Instance;
using Android_serialport_api;
using CN.Wch.Ch34xuartdriver;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class TestCarSignalSeed : ISensorCarSignalSeed
    {
        public const string DefaultBluetoothName = "yikaoxin";
        public const string DefaultBluetoothPassword = "20140501";
        // public const string DefaultBluetoothAddress = "00:15:83:35:6C:71";
        public const string DefaultBluetoothAddress = "98:D3:37:00:89:87";
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";
        public const int MaxTimeoutCount = 5;
        public const int ReadBluetoothBufferCount = 512;
        public int ReadCount = 0;
        public int MaxCount = 0;
        public string[] Datas;
        public byte[] buffer = new byte[ReadBluetoothBufferCount];
        protected CancellationTokenSource CancellationSource { get; private set; }
        protected AutoResetEvent Event { get; private set; }

        public int TimeoutCount { get; set; }
        public string BluetoothName { get; private set; }
        public string BluetoothPassword { get; private set; }

        protected ICarSignalParser SensorParser { get; set; }

        protected ICarSignalReceviedHandler SignalHandler { get; set; }


        //蓝牙连接Socket
        BluetoothSocket mSocket;
        public Action<CarSignalInfo> CarSignalRecevied { get; set; }

        protected IMessenger Messenger { get; private set; }

        protected ILog Logger { get; private set; }

        public TestCarSignalSeed(IMessenger Messenger)
        {
            SensorParser = new CarSignalParserV4();
            this.Messenger = Messenger;
            this.SignalHandler = Singleton.GetSignalHandler;
            Logger = Singleton.GetLogManager;
        }
        public void Init(NameValueCollection settings)
        {
        }
        public bool InitAsync(CH34xUARTDriver usbDriver = null, Connections serialDriver = null)
        {
            InitData();
            return true;
        }
        public void StartAsync()
        {
            //开启一个线程来单独进行信号处理
            //Thread thread = new Thread((new ThreadStart(StartCore)));
            //thread.Start();
          // 1秒钟2条信号
            System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(StartCore), null, 1000, 500);

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
        public void StartCore(object a)
        {
            try
            {
                var rawItems = ReadCommands().ToArray();

                if (rawItems.Count() == 0)
                {
                    return;
                }

                var lines = rawItems.SelectMany(SpiltCommand).ToArray();
                if (lines.Length > 2)
                {
                    var carSignalInfo = SensorParser.Parse(lines);
                    SignalHandler.Execute(carSignalInfo);
                    //TOOD:由于模拟Gps数据没有数据,所以给一个默认20的速度！
                    carSignalInfo.Sensor.SpeedInKmh = 20;
                    carSignalInfo.SpeedInKmh = 20;
                    carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                    OnCarSignalRecevied(carSignalInfo);
                }
                if (lines.Length <= 2)
                {

                }
            }
            catch (Exception exp)
            {
                string ex = exp.Message;
            }
        }
        protected virtual void OnCarSignalRecevied(CarSignalInfo carSingal)
        {
            //读取到车载信号发送消息
            if (CarSignalRecevied != null)
                CarSignalRecevied(carSingal);

            carSingal.Sensor.ArrivedHeadstock = true;
            carSingal.Sensor.ArrivedTailstock = true;
            //发送消息
            Messenger.Send(new CarSignalReceivedMessage(carSingal));

        }
        /// <summary>
        /// 读取蓝牙数据
        /// </summary>
        /// <param name="socket"></param>




        private void EndConnection(IAsyncResult asyncResult)
        {

        }

        public void InitData()
        {
            Datas = File.ReadAllLines(Android.OS.Environment.ExternalStorageDirectory+"/"+"Simulation.ini");
            MaxCount = Datas.Count();
            Logger.ErrorFormat("InitData Count:{0}", MaxCount);
           
        }

        public string TestReadLine()
        {
            //循环输出固定的科目三信号

            if (ReadCount >= MaxCount - 1)
            {
                ReadCount = 0;
            }
            return Datas[ReadCount++];
        }
        public string ReadLine(BluetoothSocket socket)
        {
            byte[] ReadBuffer = new byte[1024];
            byte[] Temp;

            byte[] buffer = new byte[1];
            int Count = 0;
            while (true)
            {
                socket.InputStream.Read(buffer, 0, 1);
                string ReadChar = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                //多读了一个
                if (ReadChar == "\n" || ReadChar == "\r")
                {
                    break;
                }
                ReadBuffer[Count] = buffer[0];
                Count++;
            }
            Temp = new byte[Count];
            for (int i = 0; i < Count; i++)
            {
                Temp[i] = ReadBuffer[i];
            }
            string ReadMsg = System.Text.ASCIIEncoding.ASCII.GetString(Temp);
            if (Count == 0)
            {
                return string.Empty;
            }
            return ReadMsg;

        }

        public string[] TestReadToCommand(string targetCmd)
        {
            var lines = new List<string>();
            while (true)
            {
                var line = TestReadLine();
                if (!string.IsNullOrEmpty(line) && line.Length >= 10)
                {
                    lines.Add(line);
                }
                var cmd = line.GetCommand();
                if (cmd != null && cmd.Equals(targetCmd, StringComparison.OrdinalIgnoreCase))
                    break;

            }
            return lines.ToArray();
        }
        public string[] ReadToCommand(BluetoothSocket socket, string targetCmd)
        {
            var lines = new List<string>();
            while (mSocket.InputStream.CanRead)
            {
                var line = ReadLine(mSocket);
                if (!string.IsNullOrEmpty(line) && line.Length >= 10)
                {
                    lines.Add(line);
                }
                var cmd = line.GetCommand();
                if (cmd != null && cmd.Equals(targetCmd, StringComparison.OrdinalIgnoreCase))
                    break;

            }
            return lines.ToArray();
        }
        public IEnumerable<string> ReadCommands()
        {
            try
            {
                //   mSocket = ConnectAsync(DefaultBluetoothAddress); SensorCommand
                var items = TestReadToCommand(SensorCommand);
                return items;
            }
            catch (Exception ex)
            {
                // mSocket = ConnectAsync(DefaultBluetoothAddress);
                Log.Error("BluetootConnectError", ex.Message);
                string Exception = ex.Message;
                return Enumerable.Empty<string>();
            }
        }

      
    }
}