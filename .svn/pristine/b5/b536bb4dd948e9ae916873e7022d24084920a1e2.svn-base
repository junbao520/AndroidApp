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

    //Todo:蓝牙地址保存报错，故暂时使用蓝牙名称这个变量，变量里面保存的值都是一样的
    //Todo:初步怀疑可能是蓝牙问题
    public class BluetoothCarSignalSeed : ISensorCarSignalSeed
    {
        public const string DefaultBluetoothName = "yikaoxin";
        public const string DefaultBluetoothPassword = "20140501";
        public string DefaultBluetoothAddress = "98:D3:37:00:89:4A";
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";
        public const int MaxTimeoutCount = 5;
        public const int ReadBluetoothBufferCount = 512;
        public byte[] buffer = new byte[ReadBluetoothBufferCount];
        protected CancellationTokenSource CancellationSource { get; private set; }
        protected AutoResetEvent Event { get; private set; }

        public int TimeoutCount { get; set; }
        public string BluetoothName { get; private set; }
        public string BluetoothPassword { get; private set; }

        protected ICarSignalParser SensorParser { get; set; }
        //蓝牙连接Socket
        static BluetoothSocket mSocket;
        public Action<CarSignalInfo> CarSignalRecevied { get; set; }

        protected IMessenger Messenger { get; private set; }

        protected ICarSignalReceviedHandler SignalHandler { get; set; }

        protected GlobalSettings Settings { get; private set; }

        protected ILog Logger { get; set; }

        protected Stream inputStream;

        private DateTime lastReadDateTime = DateTime.Now;
        private bool SocketCanRead = true;

        public BluetoothCarSignalSeed(IMessenger Messenger, ICarSignalReceviedHandler carSignalReceviedHandler, ILog log, IDataService dataService)
        {
            SensorParser = new CarSignalParserV4();
            this.Messenger = Messenger;
            this.SignalHandler = carSignalReceviedHandler;
            this.Logger = log;
            this.Settings = dataService.GetSettings();
            DefaultBluetoothAddress = Settings.BluetoothName;
        }


        public void Init(NameValueCollection settings)
        {
        }

        public bool InitAsync(CH34xUARTDriver usbDriver=null, Connections serDriver=null)
        {
            try
            {
                Messenger.Register<CarSensorBthMessage>(this, OnCarSensorBthMessageReceived);
                return ConnectAsync(DefaultBluetoothAddress);
            }
            catch (Exception ex)
            {
                Logger.Error("InitAsyc", ex.Message);
            }
            return true;

        }
        private void OnCarSensorBthMessageReceived(CarSensorBthMessage message)
        {
            var lineslst = new List<string>();
            var line = message.Sensor;
            if (!string.IsNullOrEmpty(line) && line.Length >= 10)
            {
                lineslst.Add(line);
            }
            var cmd = line.GetCommand();
            if (cmd != null && cmd.Equals(SensorCommand, StringComparison.OrdinalIgnoreCase))
                return;

            var lines = lineslst.ToArray().SelectMany(SpiltCommand).ToArray();
            if (lines.Length > 2)
            {
                var carSignalInfo = SensorParser.Parse(lines);
                SignalHandler.Execute(carSignalInfo);
                if (Settings.AngleSource == AngleSource.Gyroscope)
                {
                    carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                }
                OnCarSignalRecevied(carSignalInfo);
            }
        }
        public static List<KeyValuePair<string, string>> GetPairBluetooth()
        {
            var pairedDevices = BluetoothAdapter.DefaultAdapter.BondedDevices;
            List<KeyValuePair<string, string>> lstDevices = new List<KeyValuePair<string, string>>();
            if (pairedDevices.Count > 0)
            {
                foreach (var item in pairedDevices)
                {
                    KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>(item.Name, item.Address);
                    lstDevices.Add(keyValue);
                }
            }
            return lstDevices;
        }
        public void StartAsync()
        {
            //开启一个线程来单独进行信号处理
            Thread thread = new Thread((new ThreadStart(StartCore)));
            thread.Start();
        }
        public bool pair(string strAddr, string strPsw)
        {
            bool result = false;
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            bluetoothAdapter.CancelDiscovery();
            if (!bluetoothAdapter.IsEnabled)
            {
                bluetoothAdapter.Enable();
            }
            //检查蓝牙地址是否有效   
            if (!BluetoothAdapter.CheckBluetoothAddress(strAddr))
            {
                Log.Debug("mylog", "devAdd un effient!");
            }
            BluetoothDevice device = bluetoothAdapter.GetRemoteDevice(strAddr);
            if (device.BondState != Bond.Bonded)
            {
                try
                {
                    Log.Debug("mylog", "NOT BOND_BONDED");
                    ClsUtils.setPin(device.Class, device, strPsw); // 手机和蓝牙采集器配对    
                    ClsUtils.createBond(device.Class, device);
                    //ClsUtils.cancelPairingUserInput(device.Class, device);
                    result = true;
                }
                catch (Exception ex)
                {

                    Log.Debug("mylog", "setPiN failed!");
                }
            }
            else
            {
                try
                {
                    ClsUtils.createBond(device.Class, device);
                    ClsUtils.setPin(device.Class, device, strPsw); // 手机和蓝牙采集器配对    
                    ClsUtils.createBond(device.Class, device);
                    result = true;
                }
                catch (Exception ex)
                {
                    Log.Debug("mylog", "setPiN failed!");
                }
            }
            return result;
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
            try
            {

                while (true)
                {
                    ReadLine();
                }

            }
            catch (Exception exp)
            {
                Logger.Error("BluetoothStartCore", exp.Message);
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
            //发送消息
            Messenger.Send(new CarSignalReceivedMessage(carSingal));

        }
        /// <summary>
        /// 读取蓝牙数据
        /// </summary>
        /// <param name="socket"></param>
        public void ReadBluetoothData(BluetoothSocket socket)
        {
            byte[] buffer = new byte[ReadBluetoothBufferCount];
            while (true)
            {
                try
                {
                    socket.InputStream.Read(buffer, 0, ReadBluetoothBufferCount);
                    string msg = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                    //收到的数据进行处理
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }

        private string uuid = "00001101-0000-1000-8000-00805F9B34FB";
        /// <summary>
        /// 连接蓝牙 
        /// </summary>
        /// <param name="btDev"></param>
        /// <returns></returns>
        private bool ConnectAsync(string DeviceAddress)
        {
            try
            {
                BluetoothDevice device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(DeviceAddress);
                mSocket = device.CreateRfcommSocketToServiceRecord(UUID.FromString(uuid));
                mSocket.Connect();
                inputStream = mSocket.InputStream;
                return true;
            }
            catch (Exception ex)
            {
                //如果发生异常则关闭流
                inputStream.Close();
                Logger.Error("BluetoothConnectAsync", ex.Message + "BluetoothAddress:" + DeviceAddress);
                return false;
            }
        }

        /// <summary>
        /// 蓝牙重连
        /// </summary>
        System.Threading.Timer ConnectTimer;
        //检测时间
        private int period = 1 * 1000;
        private void ResetConnect(object state)
        {
            //如果大于2,则进行重新连接
            if (mSocket != null && (DateTime.Now - lastReadDateTime).TotalSeconds >= 2)
            {
                SocketCanRead = false;
                ConnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
                // mSocket.Close();
                ConnectAsync(DefaultBluetoothAddress);
                ConnectTimer.Change(period, period);
                Logger.Error("BluetoothResetConnect");
                SocketCanRead = true;
            }
        }

        private void EndConnection(IAsyncResult asyncResult)
        {

        }
        private string step = "";
        private string[] ReadLine2()
        {
            try
            {
                byte[] buffer = new byte[1024 * 2];
                step += "1" + '\n';

                if (inputStream.CanRead == false)
                    return null;
                var length = inputStream.Read(buffer, 0, buffer.Length);

                step += "2" + '\n';
                if (length == -1)
                    return null;

                var readline = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, length);
                //Logger.InfoFormat("读取的数据，个数：{0}，数据{1}", length, readline);
                if (readline.Contains('\r'))
                {
                    var array = readline.Replace("\n", "").Split('\r');
                    List<string> list = new List<string>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (i != 0 && i != array.Length - 1)
                        {
                            list.Add(array[i]);

                        }
                    }
                    return list.ToArray();
                }


            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return new string[0];
        }
        /// <summary>
        /// 如果采用消息机制进行处理，不直接进行返回
        /// </summary>
        /// <returns></returns>
        public void ReadLine()
        {
            byte[] buffer2 = new byte[1024];
            int count2 = 0;
            string Result = string.Empty;

            //  inputStream.ReadTimeout = 1000;            
            while (count2 < 1024)
            {
                try
                {
                    int b = inputStream.ReadByte();
                    if (b < 0)
                    {
                        //很有可能是这个延时造成的
                        Thread.Sleep(1);
                        Log.Error("ReadLine", b.ToString());
                    }
                    else if (b == '\n')
                    {
                        if (count2 > 0 && buffer2.Last() == '\r')
                        {
                            count2 -= 1;
                        }
                        if (count2 == 0)
                            Result = string.Empty;
                        else
                            Result = System.Text.ASCIIEncoding.ASCII.GetString(buffer2, 0, count2);
                    }
                    else
                    {
                        buffer2[count2++] = (byte)b;
                    }
                }
                catch (Exception ex)
                {
                    lastReadDateTime = DateTime.Now;
                    Logger.Error("BluetoothRead" + ex.Message);
                    //throw new Exception(ex.Message);
                }

            }
            Messenger.Send(new CarSensorBthMessage(Result));
        }
        public string[] ReadToCommand(BluetoothSocket socket, string targetCmd)
        {
            var lines = new List<string>();
            //while (mSocket.InputStream.CanRead)

            while (mSocket.IsConnected)
            {
                //  var line = ReadLine();
                var line = string.Empty;
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
                // var items = ReadToCommand(mSocket, SensorCommand);

                // return items;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("BluetootConnectError", ex.Message);
                if (!mSocket.IsConnected)
                {
                    mSocket.Close();
                    Logger.Error("蓝牙开始重连。。。" + ex.Message);
                    ConnectAsync(DefaultBluetoothAddress);
                }

                return Enumerable.Empty<string>();
            }
        }


    }
}