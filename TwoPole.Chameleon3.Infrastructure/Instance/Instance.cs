using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoPole.Chameleon3.Infrastructure.Messages;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Services;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using TwoPole.Chameleon3.Infrastructure.Devices.CarSignal;
using Autofac;

namespace TwoPole.Chameleon3.Infrastructure.Instance
{
    /// <summary>
    /// 实例管理类,用单利模式进行
    /// 可以考虑使用依赖注入方式 比较好 IOC 容器
    /// Autofac
    /// </summary>
    public static class Singleton
    {

        static IMessenger messager = null;
        static IGpsPointSearcher gpsPointSearcher = null;
        static IDataService dataService = null;
        static ICarSignalSet carSignalSet = null;
        static IExamScore examScore = null;
        static ISpeaker speaker = null;
        static IAdvancedCarSignal advancedCarSignal = null;
        static IExamManager examManager = null;
        static IProviderFactory providerFactory = null;
        static ITriggerHandler triggerHandler = null;
        static ICarSignalReceviedHandler SignalHandler = null;
        static ILog logger = null;
        static ICarSignalProcessor defaultCarSignalProcessor = null;
        static ICarSignalDependency carSignalDependency = null;
        static ContainerBuilder builder = null;
        static IContainer container = null;

      


        private static readonly object padlock = new object();
        private static readonly object GpsPointSearcherLock = new object();
        private static readonly object DataServiceLock = new object();
        private static readonly object CarSignalSetLock = new object();
        private static readonly object examScoreLock = new object();
        private static readonly object speakerLock = new object();
        private static readonly object advancedCarSignalLock = new object();
        private static readonly object examManagerLock = new object();
        private static readonly object providerFactoryLock = new object();
        private static readonly object triggerHandlerLock = new object();
        private static readonly object SignalHandlerLock = new object();
        private static readonly object loggerLock = new object();
        private static readonly object defaultCarSignalProcessorLock = new object();
        private static readonly object carSignalDependencyLock = new object();

        private static readonly object ContainerLock = new object();
        private static readonly object BuilderLock = new object();
        public static bool IsUseIoc = false;
        public static bool PlayActionVoice = false;
        public static string dbName = string.Empty;




        //IOC 容器还是需要 单利模式来控制
        public static ContainerBuilder GetContainerBuilder()
        {
            if (builder == null)
            {
                lock (BuilderLock)
                {
                    if (builder == null)
                    {
                        builder = new ContainerBuilder();
                    }
                }
            }
            return builder;
        }

        public static IContainer GetContainer()
        {
            if (container == null)
            {
                lock (ContainerLock)
                {
                    if (container == null)
                    {
                        container = GetContainerBuilder().Build();
                    }
                }
            }
            return container;
        }



        public static void InitSpeaker(Context context,bool IsPlayActionVoice=false)
        {
            if (speaker == null)
            {
                lock (speakerLock)
                {
                    if (speaker == null)
                    {
                        PlayActionVoice = IsPlayActionVoice;
                        speaker = new SpeechManagerBak(context.ApplicationContext,IsPlayActionVoice);
                    }
                }
            }
        }
        public static void InitDataService(Context context,string DBName)
        {
            if (dataService == null)
            {
                lock (DataServiceLock)
                {
                    if (dataService == null)
                    {
                        dataService = new DataService(context.ApplicationContext,DBName);
                    }
                }
            }
        }
        public static ISpeaker GetSpeaker
        {
            //可以考虑这点直接使用Ioc 
            //容器
         
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<ISpeaker>();
                }
                if (speaker!=null)
                {
                    return speaker;
                }
                else
                {
                    
                    return speaker;
                }
             
            }
        }
        public static ILog GetLogManager
        {
            get
            {
                if (IsUseIoc)
                {
                    //可以使用的时候注入参数

                    //    var service = scope.Resolve<AnotherService>(
                    //new NamedParameter("id", "service-identifier"),
                    //new TypedParameter(typeof(Guid), Guid.NewGuid()),
                    //new ResolvedParameter(
                    //  (pi, ctx) => pi.ParameterType == typeof(ILog) && pi.Name == "logger",
                    //  (pi, ctx) => LogManager.GetLogger("service")));
                    return GetContainer().Resolve<ILog>();
                }
                if (logger == null)
                {
                    lock (loggerLock)
                    {
                        if (logger == null)
                        {
                           logger = new LogManager(GetDataService);
                        }
                    }
                }
                return logger;
            }
        }

        

        public static ICarSignalProcessor GetCarSignalProcessor
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<ICarSignalProcessor>();
                }
                if (defaultCarSignalProcessor == null)
                {
                    lock (defaultCarSignalProcessorLock)
                    {
                        if (defaultCarSignalProcessor == null)
                        {
                          defaultCarSignalProcessor = new DefaultCarSignalProcessor(GetCarSignalSet, GetDataService, GetMessager,GetLogManager);
                        }
                    }
                }
                return defaultCarSignalProcessor;
            }
        }
        public static ICarSignalDependency GetCarSignalDependency
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<ICarSignalDependency>();
                }
                if (carSignalDependency == null)
                {
                    lock (carSignalDependencyLock)
                    {
                        if (carSignalDependency == null)
                        {
                            carSignalDependency = new CarSignalChangedMonitor(GetMessager, GetCarSignalSet,GetLogManager,GetDataService);
                        }
                    }
                }
                return carSignalDependency;
            }
        }
        
        public static ICarSignalReceviedHandler GetSignalHandler
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<ICarSignalReceviedHandler>();
                }
                if (SignalHandler == null)
                {
                    lock (SignalHandlerLock)
                    {
                        if (SignalHandler == null)
                        {
                            SignalHandler = new CarSignalReceviedHandler(GetLogManager,GetCarSignalProcessor,GetCarSignalDependency,GetMessager,GetCarSignalSet);
                        }
                    }
                }
                return SignalHandler;
            }
        }
        public static ITriggerHandler GetriggerHandler
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<ITriggerHandler>();
                }
                if (triggerHandler == null)
                {
                    lock (triggerHandlerLock)
                    {
                        if (triggerHandler == null)
                        {
                            triggerHandler = new DefaultTriggerHandler(GetProviderFactory, GetMessager);
                        }
                    }
                }
                return triggerHandler;
            }
        }
        public static IProviderFactory GetProviderFactory
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IProviderFactory>();
                }
                if (providerFactory == null)
                {
                    lock (providerFactoryLock)
                    {
                        if (providerFactory == null)
                        {
                            providerFactory = new DefaultProviderFactory(GetDataService,GetLogManager);
                        }
                    }
                }
                return providerFactory;
            }
        }
        public static IAdvancedCarSignal GetAdvancedCarSignal
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IAdvancedCarSignal>();
                }
                if (advancedCarSignal == null)
                {
                    lock (advancedCarSignalLock)
                    {
                        if (advancedCarSignal == null)
                        {
                            advancedCarSignal = new AdvancedCarSignal(GetCarSignalSet);
                        }
                    }
                }
                return advancedCarSignal;
            }
        }
        public static IMessenger GetMessager
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IMessenger>();
                }
                if (messager == null)
                {
                    lock(padlock)
                    {
                        if (messager == null)
                        {
                            messager= new GalaSoft.MvvmLight.Messaging.Messenger();
                        }
                    }
                }
                return messager;
            }
        }

        public static IGpsPointSearcher GetGpsPointSearcher
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IGpsPointSearcher>();
                }
                if (gpsPointSearcher == null)
                {
                    lock (padlock)
                    {
                        if (gpsPointSearcher == null)
                        {
                            gpsPointSearcher = new GpsPointSearcher();
                        }
                    }
                }
                return gpsPointSearcher;
            }
        }

        public static IDataService GetDataService
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IDataService>();
                }
                if (dataService!= null)
                {
                    return dataService;
                }
                else
                {

                    return dataService;
                }

            }
        }

        public static ICarSignalSet GetCarSignalSet
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<ICarSignalSet>();
                }
                if (carSignalSet == null)
                {
                    lock (CarSignalSetLock)
                    {
                        if (carSignalSet == null)
                        {
                            carSignalSet = new CarSignalSet();
                        }
                    }
                }
                return carSignalSet;
            }
        }


        public static IExamScore GetExamScore
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IExamScore>();
                }
                if (examScore== null)
                {
                    lock (examScoreLock)
                    {
                        if (examScore  == null)
                        {
                            examScore =new ExamScore(GetSpeaker,GetMessager,GetDataService);
                        }
                    }
                }
                return examScore;
            }
        }

        public static IExamManager GetExamManager
        {
            get
            {
                if (IsUseIoc)
                {
                    return GetContainer().Resolve<IExamManager>();
                }
                if (examManager == null)
                {
                    lock (examManagerLock)
                    {
                        if (examManager == null)
                        {
                            examManager = new ExamManager(GetProviderFactory,GetSpeaker, GetMessager, GetExamScore, GetDataService, GetGpsPointSearcher,GetLogManager);
                        }
                    }
                }
                return examManager;
            }
        }
    }
}