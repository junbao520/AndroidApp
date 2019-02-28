using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.Modules
{

    //TODO:可以考虑加入依赖注入
    public class ExamModule : ModuleBase
    {
        public IExamManager ExamManager { get; private set; }
        public ICarSignalSet SignalSet { get; private set; }

        public ILog Logger { get; private set; }

        public ExamModule()
        {
        }

        public  virtual async Task ReadyStartExamAsync(ExamContext context)
        {

            await StartExamItemAutoAsync(context, ExamItemCodes.CommonExamItem);
            await StartExamItemAutoAsync(context, ExamItemCodes.Start);
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
            await StartExamItemAutoAsync(context, ExamItemCodes.CommonExamItem);
            if (!context.IsExaming)
            {
                Logger.Debug("考试结束:context.IsExaming");
                return;
            }
            //上车准备完成之后就启动灯光模拟
       
            //吉首白天夜间都要灯光模拟
            if (context.ExamTimeMode == ExamTimeMode.Day  && Settings.SimulationsLightOnDay
                || context.ExamTimeMode == ExamTimeMode.Night && Settings.SimulationsLightOnNight)
            {
                
                Speaker.CancelAllAsync(false);
                while (SignalSet.Current == null)
                {
                    Thread.Sleep(100);
                }
                if (SignalSet.Current != null && !SignalSet.Current.Sensor.Engine)
                {
                    if (Settings.CommonExambeforeSimionLightStartEngine)
                    {
                        Speaker.PlayAudioAsync(Settings.CommonExambeforeSimionLightStartEngineVoice);
                    }
                }
                while (!(SignalSet.Current != null && SignalSet.Current.Sensor.Engine)&&Settings.CommonExambeforeSimionLightStartEngine)
                {
                   //如果启动要扣分
                  
                    Thread.Sleep(100);
                }

                //打火黑屏延时启动灯光模拟
                if (Settings.CommonExambeforeSimionLightStartEngine && Settings.CommonStartEngineDeleyTime > 0)
                {
                    await Task.Run(() =>
                    {
                        int delaytime= Settings.CommonStartEngineDeleyTime * 1000;
                        Thread.Sleep(delaytime);
                    });
                }

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

            //TODO:可以考虑做成配置 决定综合评判启动的位置
            //TODO:以及是否自动起步也可以考虑做成配置
     
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
            //Logger.Error("停止考试模块");
            Speaker.CancelAllAsync();
            //Logger.Error("停止语音播报");
            await ExamManager.StopExamAsync(isClose);
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

        public Task<IExamItem> StartExamItemManualAsync(ExamContext context, string itemCode, string parameters = null,string ItemVoice = "", string ItemEndVoice = "")
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
            return ExamManager.StartItemAsync(itemContext, CancellationToken.None,ItemVoice,ItemEndVoice);
        }
    }
}
