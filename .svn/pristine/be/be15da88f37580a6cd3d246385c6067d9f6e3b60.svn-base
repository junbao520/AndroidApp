using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Devices;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.Modules
{

    //TODO:也可以考虑加入依赖注入
    public abstract class ModuleBase : DisposableBase, IModule
    {
        public bool Initialized { get; private set; }
        protected IMessenger Messenger { get; private set; }
        public ICarSignalSeed CarSignalSeed { get; private set; }
        public GlobalSettings Settings { get; private set; }
        protected ISpeaker Speaker { get; private set; }
        protected IDataService DataService { get; private set; }

        protected ModuleBase()
        {
            Messenger = Singleton.GetMessager;
            DataService = Singleton.GetDataService;
            Settings = DataService.GetSettings();
            Speaker = Singleton.GetSpeaker;
        }

        #region Init
        public virtual async Task InitAsync(ExamInitializationContext context)
        {
            InitCore(context);
            InitCompleted();
        }

        protected virtual void InitCore(ExamInitializationContext context)
        {
        }

        protected virtual void InitCompleted()
        {
            Initialized = true;
            Messenger.Send(new ApplicationInitializedMessage());
        }
        #endregion



        public abstract Task StartAsync(ExamContext context);

        public abstract Task StopAsync(bool isClose=false);

        #region Regist Services
     
        #endregion


        #region IDispose
        protected override void Free(bool disposing)
        {
            if (!disposing)
                return;

            try
            {
                //ChildContext.Dispose();
            }
            catch (Exception ex)
            {
                //Logger.Error(ex.StackTrace);
            }
            finally
            {
                //ChildLocator = null;
              //  Locator.SetCurrentLocatorProvider(() => Locator.Root);
            }
        }
        #endregion
    }
}
