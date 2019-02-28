using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Business.Modules;

namespace TwoPole.Chameleon3.Business.HaiNan.SanYa.Modules
{
    public class ExamModule : ModuleBase
    {
        public IExamManager ExamManager { get; private set; }
        public ICarSignalSet SignalSet { get; private set; }

        public ILog Logger { get; private set; }

        public ExamModule()
        {
        }

        protected override void InitCore(ExamInitializationContext context)
        {
            base.InitCore(context);
            ExamManager = Singleton.GetExamManager;
            SignalSet = Singleton.GetCarSignalSet;
            Logger = Singleton.GetLogManager;
        }

        protected virtual async Task StartTrainingAsync(ExamContext context)
        {
            Logger.DebugFormat("启动训练模块");
            //2. 通用规则
            await StartExamItemAutoAsync(context, ExamItemCodes.CommonExamItem);
            await StartExamItemAutoAsync(context, ExamItemCodes.Start);
        }

        protected virtual async Task StartExamAsync(ExamContext context)
        {
            if (!context.IsExaming)
                return;
            await StartExamItemAutoAsync(context, ExamItemCodes.CommonExamItem);
            //启用绕车一周 to 在车上进行验证探头
            if (Settings.PrepareDrivingEnable)
            {
                var prepareTask = StartExamItemAutoAsync(context, ExamItemCodes.PrepareDriving);
                await prepareTask;

                await Task.Run(() =>
                {
                    while (context.IsExaming && prepareTask.Result.State != ExamItemState.Finished)
                    {
                        Thread.Sleep(100);
                    }
                });
            }

            if (!context.IsExaming)
            {
                Logger.Debug("考试结束:context.IsExaming");
                return;
            }

            //吉首白天夜间都要灯光模拟
            if (context.ExamTimeMode == ExamTimeMode.Day && Settings.CheckLightingSimulation && Settings.SimulationsLightOnDay
                || context.ExamTimeMode == ExamTimeMode.Night && Settings.CheckLightingSimulation && Settings.SimulationsLightOnNight)
            {

                Speaker.CancelAllAsync(false);
                //等待关闭发动机
                var lightTask = StartExamItemAutoAsync(context, ExamItemCodes.Light);

                await lightTask;
                await Task.Run(() =>
                {
                    while (context.IsExaming && lightTask.Result.State != ExamItemState.Finished)
                    {
                        Thread.Sleep(100);
                    }
                });

            }
            if (!context.IsExaming)
            {
                Logger.Debug("考试结束:context.IsExaming");
                return;
            }

        
            await StartExamItemAutoAsync(context, ExamItemCodes.Start);
        }


        public override async Task StartAsync(ExamContext context)
        {
            //清除历史的信号记录；
            SignalSet.Clear();

            //设置考试里程
            context.ExamDistance = (context.ExamTimeMode == ExamTimeMode.Day)
                ? Settings.ExamDistance
                : Settings.NightDistance;

          
            await ExamManager.StartExamAsync(context);

            if (context.ExamMode == ExamMode.Training)
                await StartTrainingAsync(context);
            else
                await StartExamAsync(context);
        }

        public override async Task StopAsync(bool isClose = false)
        {
            //Logger.DebugFormat("停止考试模块");
            Speaker.CancelAllAsync();
            await ExamManager.StopExamAsync(isClose);
            //TODO:把ExamManager=null，整个考试流程应该就停止了
          
           
        }

        protected Task<IExamItem> StartExamItemAutoAsync(ExamContext context, string itemCode)
        {
            var itemContext = new ExamItemExecutionContext(context);
            itemContext.ItemCode = itemCode;

            itemContext.TriggerSource = ExamItemTriggerSource.Auto;
            var examItem = DataService.AllExamItems.First(x => x.ItemCode == itemCode);

            if (SignalSet.Current != null)
            {
                itemContext.TriggerPoint = new MapPoint(SignalSet.Current.Gps.ToPoint(), -1, examItem.ItemName, examItem.MapPointType);
            }
            Logger.DebugFormat("自动启动项目：{0}-{1}", examItem.ItemName, itemCode);
            return ExamManager.StartItemAsync(itemContext, CancellationToken.None);
        }

        public Task<IExamItem> StartExamItemManualAsync(ExamContext context, string itemCode, string parameters = null)
        {
            Logger.DebugFormat("启动考试项目：{0}", itemCode);
            var itemContext = new ExamItemExecutionContext(context);
            itemContext.ItemCode = itemCode;
            itemContext.TriggerSource = ExamItemTriggerSource.Manual;
            var examItem = DataService.AllExamItems.First(x => x.ItemCode == itemCode);
            if (SignalSet.Current != null)
            {
                itemContext.TriggerPoint = new MapPoint(SignalSet.Current.Gps.ToPoint(), -1, examItem.ItemName, examItem.MapPointType);
            }
            if (!string.IsNullOrEmpty(parameters))
            {
                var query = from a in parameters.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            let b = a.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            select new
                            {
                                key = b[0],
                                value = b[1]
                            };

                if (itemContext.Properties == null)
                    itemContext.Properties = new Dictionary<string, object>();

                foreach (var pair in query)
                {
                    itemContext.Properties[pair.key] = pair.value;
                }
            }
            Logger.DebugFormat("启动考试项目-2：{0}", examItem.ItemName);
            return ExamManager.StartItemAsync(itemContext, CancellationToken.None);
        }
    }
}
