using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.Modules
{
    public class LightModule : ModuleBase
    {
        //这个也可以加入依赖注入
        public LightModule()
        {
            var examItemCreator = Singleton.GetProviderFactory;
            LightExamItem = (ILightExamItem)examItemCreator.CreateExamItem(ExamItemCodes.Light);
        }
        public ILightExamItem LightExamItem { get; private set; }
        protected override void InitCore(ExamInitializationContext context)
        {
            base.InitCore(context);
        }

     
        protected CancellationTokenSource TokenSource { get; private set; }

        public override async Task StartAsync(ExamContext context)
        {
            var signalSet = Singleton.GetCarSignalSet;
            signalSet.Clear();
            var executionContext = new ExamItemExecutionContext(context);
            executionContext.ItemCode = ExamItemCodes.Light;
            Speaker.CancelAllAsync();
            TokenSource = new CancellationTokenSource();
            if (LightExamItem==null)
            {
               LightExamItem = (ILightExamItem)Singleton.GetProviderFactory.CreateExamItem(ExamItemCodes.Light);
            }
            await LightExamItem.StartAsync(executionContext, TokenSource.Token);
        }

        public override async Task StopAsync(bool close=false)
        {
            Speaker.CancelAllAsync();
            if (LightExamItem != null)
            {
                await LightExamItem.StopAsync();
                Speaker.CancelAllAsync();    
            }
        }
    }
}
