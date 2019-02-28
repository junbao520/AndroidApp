using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using TwoPole.Chameleon3.Infrastructure.Messages;
namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    /// <summary>
    /// 车载信号读取器v4
    /// gps、obd以及车载信号
    /// </summary>
    public abstract class CarSignalSeedV4 : DisposableBase, ICarSignalSeed, IProvider
    {
        public const string SensorCommand = "$SENSOR";
        public const string DebugCommand = "$DEBUG";

        public IProviderFactory ProviderFactory { get; set; }
        public ICarSignalReceviedHandler SignalHandler { get; private set; }
        protected ICarSignalParser SensorParser { get; private set; }
        protected ILog Logger { get; private set; }
        protected IMessenger Messenger { get; private set; }
        public Action<CarSignalInfo> CarSignalRecevied { get; set; }
        protected CancellationTokenSource CancellationSource { get; private set; }
        protected AutoResetEvent Event { get; private set; }
        protected GlobalSettings Settings { get; private set; }
        protected abstract bool Connected { get; }
        private StreamWriter _logWriter = null;

        protected void CarSignalParserV4()
        {

        }

        /// <summary>
        ///初始化
        /// </summary>
        /// <returns></returns>
        public virtual void InitAsync()
        {
      
        }

        public virtual Task StartAsync()
        {
            CancellationSource = new CancellationTokenSource();
            Event = new AutoResetEvent(false);
            return Task.Run(() =>
            {
                Event.Reset();
                while (!CancellationSource.IsCancellationRequested)
                {
                    StartCore();
                }
                Event.Set();
            }, CancellationSource.Token);
        }

        /// <summary>
        /// 是否是内联陀螺仪
        /// </summary>
        protected bool? IsInnerGyro { get; private set; }

        /// <summary>
        /// 检测陀螺仪的来源
        /// </summary>
        private bool DetectGyroSource()
        {
            //1. 去掉第一次数据有可能不完整
            if (Settings.AngleSource==AngleSource.Gyroscope)
            {
                ReadCommands().ToArray();
                var rawItems = ReadCommands().ToArray();
                var isInnerGyro = rawItems.Any(x => x.StartsWith("$GYRO"));
                return isInnerGyro;
            }
            else 
            {
                return false;
            }
           
          
        }

        protected virtual void StartCore()
        {
            try
            {
                //if (!IsInnerGyro.HasValue)
                //{
                //    IsInnerGyro = DetectGyroSource();
                //    //Logger.InfoFormat("信号中是否包含陀螺仪信息：{0}", IsInnerGyro);
                //}

                var rawItems = ReadCommands().ToArray();
                var lines = rawItems.SelectMany(SpiltCommand).ToArray();
           
                if (lines.Length > 2)
                {
                    var carSignalInfo = SensorParser.Parse(lines);
                    SignalHandler.Execute(carSignalInfo);
                    //方向角
                    if (Settings.AngleSource == AngleSource.Gyroscope|| Settings.AngleSource == AngleSource.ExternalGyroscope)
                    {
                        if (IsInnerGyro.Value)
                        {
                            carSignalInfo.BearingAngle = carSignalInfo.AngleZ + 180;
                        }
                        else
                        {
                            ProcessCarDirection(carSignalInfo);
                        }
                    }
                    OnCarSignalRecevied(carSignalInfo);
                }
                //if (lines.Length <= 2 || lines.Any(x => x.StartsWith("$D")))
                if (lines.Length <= 2)
                {
                    //Logger.WarnFormat("信号异常：{0}", string.Join(Environment.NewLine, rawItems));
                }

                LogCommands(rawItems);
            }
            catch (Exception exp)
            {
                //Logger.ErrorFormat("读取数据发生异常，原因：{0}", exp, exp);
            }
        }


        /// <summary>
        /// 处理方向角
        /// </summary>
        /// <param name="carSingal"></param>
        protected virtual void ProcessCarDirection(CarSignalInfo carSingal)
        {

        }

        protected abstract IEnumerable<string> ReadCommands();

        protected virtual IEnumerable<string> SpiltCommand(string command)
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

        public virtual async Task StopAsync()
        {
            if (CancellationSource != null && CancellationSource.Token.CanBeCanceled)
            {
                CancellationSource.Cancel(true);
            }
            if (Event != null)
            {
                await Task.Run(() =>
                {
                    Event.WaitOne(1000, false);
                    Event.Dispose();
                    Event = null;
                });
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

        public virtual void Init(NameValueCollection settings)
        {
        }

        protected override void Free(bool disposing)
        {
            if (CancellationSource != null)
                CancellationSource.Cancel();

            if (Event != null)
            {
                Event.WaitOne(200, false);
                Event.Dispose();
                Event = null;
            }
            ReleaseLogWriter();
        }

        /// <summary>
        /// 启用GPS或者传感器信号，写入日志
        /// </summary>
        /// <param name="commands"></param>
        protected virtual void LogCommands(IEnumerable<string> commands)
        {
            if (Settings.GpsLogEnable || Settings.CarSignalLogEnable)
            {
                foreach (var line in commands)
                {
                    LogWriter.WriteLine(line);
                }
            }
        }

        private StreamWriter LogWriter
        {
            get
            {
                if (_logWriter == null)
                {
                    if (!Directory.Exists(DefaultGlobalSettings.DefaultGpsLogFolder))
                        Directory.CreateDirectory(DefaultGlobalSettings.DefaultGpsLogFolder);

                    var filename = string.Format("sensor-{0:yyyyMMddHHmmss}.txt", DateTime.Now);
                    var fullname = Path.Combine(DefaultGlobalSettings.DefaultGpsLogFolder, filename);
                    _logWriter = File.CreateText(fullname);
                }
                return _logWriter;
            }
        }

        protected void ReleaseLogWriter()
        {
            //if (_logWriter != null && _logWriter.BaseStream != null)
            //{
            //    _logWriter.Dispose();
            //    _logWriter = null;
            //}
        }
    }
}
