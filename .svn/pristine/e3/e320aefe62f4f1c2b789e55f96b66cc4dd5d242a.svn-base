using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Diagnostics;
using System.Windows;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Devices;
using TwoPole.Chameleon3.Infrastructure.Devices.CarSignal;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Services;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Android.Content;
using Android_serialport_api;
using Java.IO;
using Autofac;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using System.Collections.Generic;

namespace TwoPole.Chameleon3
{
    public class Bootstrapper
    {

        public void Initialize()
        {
        
        }
        //其实可以考虑
        public static void InitializeSignalAsyncTest(IMessenger messenger, Context context,string DBName)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(MasterControlBoxVersion.SimulatedData.ToString());
            signalSeedTask.InitAsync();
            signalSeedTask.StartAsync();
        }


        public static void InitializeSignalAsyncBluetooth(IMessenger messenger, Context context,string DBName)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(MasterControlBoxVersion.Bluetooth.ToString());
            var connected = signalSeedTask.InitAsync();
            if (connected)
                signalSeedTask.StartAsync();
        }


        public static void InitializeSignalAsyncUSBSerial(IMessenger messenger, Context context,string DBName)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(MasterControlBoxVersion.USB.ToString());
            signalSeedTask.InitAsync(MyApp.driver);
            signalSeedTask.StartAsync();

        }
        public static void InitializeSignalAsyncSerial(IMessenger messenger, Context context, string DBName)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(MasterControlBoxVersion.Serial.ToString());
            Connections conn = new Connections();
            //TODO:可以考虑串口和波特率可选
            conn.InitSerial(new File("/dev/ttyS0"), 115200, 1);
            signalSeedTask.InitAsync(null,conn);
            signalSeedTask.StartAsync();
        }

        public static void InitializeSignalAsyncSerial2(IMessenger messenger, Context context, string DBName)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(MasterControlBoxVersion.Serial.ToString());
            Connections conn = new Connections();
            //TODO:可以考虑串口和波特率可选
            conn.InitSerial(new File("/dev/ttyS5"), 57600, 1);
            signalSeedTask.InitAsync(null, conn);
            signalSeedTask.StartAsync();
        }

        /// <summary>
        /// 初始化信号
        /// </summary>
        public static void InititalizeSignalAsyc(MasterControlBoxVersion version)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(version.ToString());
            bool Result = false;
            switch (version)
            {
                case MasterControlBoxVersion.SimulatedData:
                case MasterControlBoxVersion.WifiTcp:
                case MasterControlBoxVersion.WifiUdp:
                   Result= signalSeedTask.InitAsync();
                    break;
                case MasterControlBoxVersion.USB:
                    Result = signalSeedTask.InitAsync(MyApp.driver);
                    break;
                case MasterControlBoxVersion.Serial:
                    Connections conn = new Connections();
                    //TODO:可以考虑串口和波特率可选
                    conn.InitSerial(new File("/dev/ttyS0"), 115200, 1);
                    Result = signalSeedTask.InitAsync(null,conn);
                    break;
                default:
                    Result = signalSeedTask.InitAsync(MyApp.driver);
                    break;
            }
            if (Result)
            {
                signalSeedTask.StartAsync();
            }
            else
            {
                Singleton.GetSpeaker.SpeakAsync("初始化系统失败");
            }
      
     
        }

    
  
        public static void InitializeSignalUDPAsync(IMessenger messenger, Context context, string DBName)
        {
            var signalSeedTask = Singleton.GetContainer().ResolveNamed<ISensorCarSignalSeed>(MasterControlBoxVersion.WifiUdp.ToString());
            signalSeedTask.InitAsync();
             signalSeedTask.StartAsync();
        }
  
        public static void InitializeServices(Context context,string DBName,bool IsPlayActionVoice=false,bool IsUseIoc=true)
        {

            Singleton.InitSpeaker(context, IsPlayActionVoice);
            if (IsUseIoc)
            {
                Singleton.IsUseIoc = IsUseIoc;
                InitializeServicesIoc(context, DBName, IsPlayActionVoice);
                return;
            }
            //Singleton.InitSpeaker(context,IsPlayActionVoice);
            Singleton.InitDataService(context,DBName);
        }

        public static void InitializeServicesIoc(Context context, string DBName, bool IsPlayActionVoice = false)
        {
    
            var builder = Singleton.GetContainerBuilder();
            //TODO:使用依赖注入这个框架非常好，有利于系统的稳定性以及解决各种偶发性的Bug
            builder.RegisterType<DefaultCarSignalProcessor>().As<ICarSignalProcessor>().SingleInstance();
            builder.RegisterType<Messenger>().As<IMessenger>().SingleInstance();
            builder.RegisterType<CarSignalChangedMonitor>().As<ICarSignalDependency>().SingleInstance(); ;
            builder.RegisterType<CarSignalReceviedHandler>().As<ICarSignalReceviedHandler>().SingleInstance();
            builder.RegisterType<DefaultProviderFactory>().As<IProviderFactory>().SingleInstance(); 
            builder.RegisterType<AdvancedCarSignal>().As<IAdvancedCarSignal>().SingleInstance(); 
            builder.RegisterType<GpsPointSearcher>().As<IGpsPointSearcher>().SingleInstance(); 
            builder.RegisterType<CarSignalSet>().As<ICarSignalSet>().SingleInstance(); 
            builder.RegisterType<ExamScore>().As<IExamScore>().SingleInstance();
            //TODO:这点还可以根据不同的版本注入不同的ExamManager Log类已经修改可以依赖注入
            //builder.RegisterType<ExamManager>().As<IExamManager>().SingleInstance();

            //todo:版本切换，泸县使用单独的ExamManager,(其他版本请切换到上一个manager)
            //使用DBName进行判断时，不起作用
            //todo:使用中文判断很奇怪
            if (DataBase.VersionNumber.Contains("泸县"))
               builder.RegisterType<ExamManager_luxian>().As<IExamManager>().SingleInstance();
            else if (DataBase.VersionNumber.Contains("泸州"))
                builder.RegisterType<ExamManager_luzhou>().As<IExamManager>().SingleInstance();
            //else if (DataBase.VersionNumber.Contains("成都多伦"))
            //    builder.RegisterType<ExamManager_chengduduolun>().As<IExamManager>().SingleInstance();
            else if (DataBase.VersionNumber.Contains("佛山") || DataBase.VersionNumber.Contains("云浮")||DataBase.VersionNumber.Contains("浮云"))
                builder.RegisterType<ExamManager_foshan>().As<IExamManager>().SingleInstance();
            else
                builder.RegisterType<ExamManager>().As<IExamManager>().SingleInstance();
            builder.RegisterType<SpeechManagerBak>().As<ISpeaker>().WithParameters(
               new List<NamedParameter> {
                new NamedParameter("context", context.ApplicationContext),
                new NamedParameter("IsPlayActionVoice", IsPlayActionVoice)
                  }
               ).SingleInstance();

            builder.RegisterType<DataService>().As<IDataService>().WithParameters(
               new List<NamedParameter> {
                new NamedParameter("context", context.ApplicationContext),
                new NamedParameter("DataBaseName", DBName)
                  }
               ).SingleInstance();
            builder.RegisterType<LogManager>().As<ILog>().SingleInstance();
            builder.RegisterType<USBSerialCarSignalSeed>().As<ISensorCarSignalSeed>().Named<ISensorCarSignalSeed>(MasterControlBoxVersion.USB.ToString()).SingleInstance();
            builder.RegisterType<UDPCarSignalSeed>().As<ISensorCarSignalSeed>().Named<ISensorCarSignalSeed>(MasterControlBoxVersion.WifiUdp.ToString()).SingleInstance();
            builder.RegisterType<SerialCarSignalSeed>().As<ISensorCarSignalSeed>().Named<ISensorCarSignalSeed>(MasterControlBoxVersion.Serial.ToString()).SingleInstance();
            builder.RegisterType<BluetoothCarSignalSeed>().As<ISensorCarSignalSeed>().Named<ISensorCarSignalSeed>(MasterControlBoxVersion.Bluetooth.ToString()).SingleInstance();
            builder.RegisterType<TestCarSignalSeed>().As<ISensorCarSignalSeed>().Named<ISensorCarSignalSeed>(MasterControlBoxVersion.SimulatedData.ToString()).SingleInstance();
            //测试的不注入一个接口如果被多次注入会以最后一次为准
            // builder.RegisterType<TestCarSignalSeed>().As<ISerialSignalSeed>().SingleInstance();
            //通过依赖注入注入不同的信号处理类
            //
            //builder.RegisterType<LogManager>().As<ILog>().WithParameter(
            //    new NamedParameter("IsLogEnable",Singleton.GetContainer().Resolve<IDataService>().GetSettings().GpsLogEnable)
            // ).SingleInstance();
            //For Example
            //ExamManager = Singleton.GetContainer().Resolve<IExamManager>();
            //SignalSet = Singleton.GetContainer().Resolve<ICarSignalSet>();
        }

    }
}

