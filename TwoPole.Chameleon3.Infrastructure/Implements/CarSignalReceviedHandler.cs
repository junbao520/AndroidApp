using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class CarSignalReceviedHandler : ICarSignalReceviedHandler
    {
        protected ILog Logger { get; set; }
       // protected IServiceLocator ServiceLocator { get; private set; }
        protected IMessenger Messenger { get; private set; }
        protected ICarSignalSet CarSignalSet { get; private set; }
        protected ICarSignalProcessor CarSignalProcessors { get; private set; }

        protected ICarSignalDependency CarSignalMonitor { get; private set; }
        public CarSignalReceviedHandler(ILog log,ICarSignalProcessor carSignalProcessor,ICarSignalDependency carSignalDependency,IMessenger messenger,ICarSignalSet carSignalSet)
        {
            this.Logger = log;
            //TODO:根据李兴亮描述这点依赖注入 没有注入进来
            this.CarSignalProcessors = carSignalProcessor??Singleton.GetCarSignalProcessor;
            this.CarSignalMonitor =carSignalDependency;
            this.Messenger = messenger;
            this.CarSignalSet = carSignalSet;
        }

        public void Execute(CarSignalInfo signalInfo)
        {
            //加入队列
            try
            {
                CarSignalProcessors.Execute(signalInfo);
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("执行任务 {0} 发生异常，原因： {1}", "CarSignalProcessors", exp.Message);
            }
            try
            {
                CarSignalMonitor.Execute(signalInfo);
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("处理任务 {0} 发生异常，原因： {1}", "CarSignalMonitor", exp.Message);
            }
            //加入队列
            CarSignalSet.Enqueue(signalInfo);
        }
    }
}
