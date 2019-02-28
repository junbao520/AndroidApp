using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class ExamManager : DisposableBase, IExamManager
    {
        protected ISpeaker Speaker { get; private set; }
        protected IMessenger Messenger { get; private set; }
        protected ILog Logger { get; set; }
        protected GlobalSettings Settings { get; set; }
        public IList<IExamItem> ExamItems { get; private set; }
        protected ExamContext Context { get; private set; }
        public IExamScore ExamScore { get; private set; }
        public IProviderFactory ProviderFactory { get; private set; }
        public ITriggerHandler TriggerHandler { get; private set; }
        public IGpsPointSearcher PointSearcher { get; private set; }
        public IDataService DataService { get; private set; }
        public bool IsTriggerPullOver = false;

        public ExamManager(
            IProviderFactory providerFactory,
            ISpeaker speaker,
            IMessenger messenger,
            IExamScore examScore,
            IDataService dataService,
            IGpsPointSearcher pointSearcher,
            //这个有一个Logger导致依赖注入不成功
            ILog logger
            )
        {
            ProviderFactory = providerFactory;
            Speaker = speaker;
            Messenger = messenger;
            ExamScore = examScore;
            DataService = dataService;
            Logger = logger;
            PointSearcher = pointSearcher;
            Init();
        }
        /// <summary>
        /// 触发过的地位
        /// </summary>
        protected List<MapPointType> FinishedPointList = new List<MapPointType>();
        /// <summary>
        /// 此地图本身点位（不包括左转，右转，直行，灯光，上车准备）
        /// </summary>
        protected List<MapPointType> InitPointList = new List<MapPointType>();


        private void Init()
        {
            //设置是否语音播报考试项目
            ExamItems = new List<IExamItem>(16);
            Settings = DataService.GetSettings();

            RegisterMessages();
        }
        protected virtual void InitMapItem()
        {
            try
            {
                if (!Settings.PullOverStartFlage)
                    return;
                if (Context == null)
                    return;
                //不是考试时，不管
                if (!Context.IsExaming)
                    return;
                //清空项目
                FinishedPointList = new List<MapPointType>();
                InitPointList = new List<MapPointType>();

                //Logger.Error("InitMapItem");
                //新增功能，所有项目只播报一遍，20170707
                var temMap = Context.Map.MapPoints;
                //有选择地图
                if (temMap.Count() > 1)
                {
                    foreach (var mapPoint in temMap)
                    {
                        if (!InitPointList.Contains(mapPoint.PointType) && mapPoint.PointType != MapPointType.PullOver)
                        {
                            //这个只有2 
                            InitPointList.Add(mapPoint.PointType);
                        }
                    }
                }
                Logger.Info("LastMapPoint:"+InitPointList.LastOrDefault());
            }
            catch (Exception ex)
            {

                Logger.Error("InitMap" + ex.Message);
            }
        }
        #region Handle Message

        private void OnBrokenRule(BrokenRuleMessage message)
        {
            Context.Rules.Add(message.RuleInfo);

            var itemState = Context.ExamItemStates.FirstOrDefault(x => x.ItemCode == message.RuleInfo.ExamItemCode);
            if (itemState != null)
            {
                if (message.RuleInfo.Required)
                    itemState.Result = ExamItemResult.Failed;
            }
        }

        protected virtual void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            if (message.ExamItem == null)
                return;

            var examItems = ExamItems.ToArray();
            var itemState = Context.ExamItemStates.FirstOrDefault(x => x.ItemCode == message.ExamItem.ItemCode);
            if (itemState != null)
            {
                if (message.NewState == ExamItemState.Progressing)
                    itemState.Result = ExamItemResult.None;
                itemState.State = message.NewState;
            }
            //KeiWEI
            if (message.NewState == ExamItemState.Finished)
            {
                if (itemState != null)
                {
                    var rules = Context.Rules.Where(x => x.ExamItemCode == itemState.ItemCode).ToArray();
                    if (rules.Length == 0)
                        itemState.Result = ExamItemResult.Perfect;
                    else if (rules.Any(x => x.Required))
                        itemState.Result = ExamItemResult.Failed;
                    else
                        itemState.Result = ExamItemResult.Passed;
                }

                if (examItems.Contains(message.ExamItem))
                {
                    try
                    {
                        lock (ExamItems)
                        {
                            ExamItems.Remove(message.ExamItem);
                        }
                        message.ExamItem.Dispose();
                    }
                    catch (Exception exp)
                    {
                        Logger.ErrorFormat("释放考试项目下{0}发生异常，原因：{1}", message.ExamItem.ItemCode, exp, exp);
                    }
                }
                else
                {
                    Logger.WarnFormat("ExamMananger中不存在考试项目{0}", message.ExamItem);
                }
            }
        }


        protected bool IsRunning { get; private set; }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            var signalInfo = message.CarSignal;
            //灯光模拟等在信号不正常情况下也能够使用；
            //if (Context == null || !Context.IsExaming || !signalInfo.IsValid)
            if (Context == null || !Context.IsExaming)
                return;
            ExecuteExamItems(signalInfo);
            if (IsRunning)
            {
                Logger.InfoFormat("搜索点位忽略，正在执行上个任务");
                return;
            }
            if (signalInfo.IsGpsValid)
            {
                IsRunning = true;
                MatchMapPointsAsync(signalInfo).ContinueWith(x =>
                {
                    IsRunning = false;
                });
            }
        }

        /// <summary>
        /// 记录最近5个点,防止重复播报
        /// </summary>
        protected Queue<MapPoint> _mapTriggerPoints = new Queue<MapPoint>(2);

        protected virtual async Task MatchMapPointsAsync(CarSignalInfo signalInfo)
        {
            //只是一个条件
            if (Settings.PullOverStartFlage == false && Settings.EndExamByDistance && IsTriggerPullOver == false)
            {
                if (signalInfo.Distance >= Settings.ExamDistance)
                {
                    IsTriggerPullOver = true;
                    Logger.Info("里程达到三公里自动触发考试项目");
                    Messenger.Send<PullOverTriggerMessage>(new PullOverTriggerMessage());
                    return;
                }
            }
            //直线行驶路段忽略其它的考试项目
            if (ExamItems.Any(d => d.ItemCode == ExamItemCodes.StraightDriving && d.State != ExamItemState.Finished))
                return;

            var points = PointSearcher.Search(signalInfo);

            //对解除限速进行
            if (points.Any(d => d.PointType == MapPointType.UnfreezeSpeedLimit))
            {
                Messenger.Send(new EndSpeedLimitMessage());
            }
            var query = from p in points
                        where !ExamItems.Any(x => x.State != ExamItemState.Finished && x.TriggerPoint != null && x.TriggerPoint.PointType == p.PointType) &&
                            _mapTriggerPoints.All(x => x.Index != p.Index)
                        select p;

            foreach (var mapPoint in query)
            {
                var context = new ExamItemExecutionContext(Context);

                var item = DataService.AllExamItems.FirstOrDefault(x => x.MapPointType == mapPoint.PointType);
                if (item != null)
                {
                    if (Settings.PullOverStartFlage && InitPointList.Count > 0 && context.ExamMode == ExamMode.Examming)
                    {
                        //左转，右转，直行路口不过滤
                        if (FinishedPointList.Contains(item.MapPointType))
                        {
                            if (item.ItemCode != ExamItemCodes.TurnLeft && item.ItemCode != ExamItemCodes.TurnRight &&
                                item.ItemCode != ExamItemCodes.StraightThrough && item.ItemCode != ExamItemCodes.TurnRound
                                && item.ItemCode != ExamItemCodes.SchoolArea && item.ItemCode != ExamItemCodes.BusArea && item.ItemCode != ExamItemCodes.PedestrianCrossing && item.ItemCode != ExamItemCodes.PedestrianCrossingHasPeople)
                            {
                                Logger.Info("跳过项目:" + item.ItemCode);
                                continue;
                            }
                        }

                        if (!FinishedPointList.Contains(item.MapPointType) && item.MapPointType != MapPointType.PullOver)
                        {
                            FinishedPointList.Add(item.MapPointType);
                            Logger.Info(item.MapPointType.ToString() + ":" + FinishedPointList.Count.ToString());
                        }

                        if (FinishedPointList.Count != InitPointList.Count)
                        {
                            //没达到项目数据，不触发靠边停车
                            if (item.ItemCode == ExamItemCodes.PullOver)
                            {
                                Logger.Info("靠边停车项目跳过:" + item.ItemCode);
                                continue;
                            }
                        }
                        else if (FinishedPointList.Count >= InitPointList.Count)
                        {
                            if (item.ItemCode == ExamItemCodes.PullOver)
                            {
                                //表示已经完成所有的考试项目
                                if (Settings.EndExamByDistance && signalInfo.Distance >= Settings.ExamDistance)
                                {
                                    Logger.Info("里程达到三公里并且完成所有项目触发");
                                }
                                else if (Settings.EndExamByDistance == false)
                                {
                                    Logger.Info("完成所有项目直接触发靠边停车");
                                }
                                else
                                {
                                    Logger.Info("靠边停车项目跳过 里程未达到:" + signalInfo.Distance);
                                    continue;
                                }
                            }
                        }
                    }
                    context.ItemCode = item.ItemCode;
                    ///超变会处理，相应的点位随机触发超车，变道，会车
                    if (item.ItemCode == ExamItemCodes.OvertakeChangeMeeting)
                        context.ItemCode = ExamItemCodes.GetRandomExamItemFromOvertakeChangeMeeting();
                    context.TriggerSource = ExamItemTriggerSource.Map;
                    context.Properties = mapPoint.Properties;
                    context.TriggerPoint = mapPoint;
                    Logger.InfoFormat("当前GPS：{0}-{1}-{2}", signalInfo.Gps.LatitudeDegrees, signalInfo.Gps.LongitudeDegrees, signalInfo.Gps.SpeedInKmh);
                    Logger.InfoFormat("点位GPS：{0}-{1}", mapPoint.Point.Latitude, mapPoint.Point.Longitude);
                    Logger.InfoFormat("相距位置：{0}米", GeoHelper.GetDistance(mapPoint.Point.Longitude, mapPoint.Point.Latitude, signalInfo.Gps.LongitudeDegrees, signalInfo.Gps.LatitudeDegrees));
                    Logger.InfoFormat("点位触发项目：{0}", item.ItemName);

                    await StartItemAsync(context, CancellationToken.None);
                }
                _mapTriggerPoints.Enqueue(mapPoint);
            }
            while (_mapTriggerPoints.Count > 2)
            {
                _mapTriggerPoints.Dequeue();
            }
        }

        private void ExecuteExamItems(CarSignalInfo signalInfo)
        {
            var activedItems = ExamItems.Where(x => x.State == ExamItemState.Progressing).ToArray();
            foreach (var examItem in activedItems.OrderBy(x => x.Order))
            {
                try
                {
                    examItem.Execute(signalInfo);
                }
                catch (Exception exp)
                {
                    Logger.ErrorFormat("接收到信号后执行考试项目{0}发生异常，原因：{1}", examItem.ItemCode, exp, exp);
                }
            }
        }

        private void OnExamItemEnd(ExamItemEndMessage message)
        {
            if (ExamItems != null)
            {
                var items = ExamItems.Where(x => x.ItemCode == message.ExamItemCode).ToArray();
                foreach (var examItem in items)
                {
                    examItem.StopAsync();
                }
            }
        }

        private void OnExamItemStart(ExamItemStartMessage message)
        {
            StartItemAsync(message, message.Properties);
        }

        #endregion

        private void RegisterMessages()
        {
            //处理信号的方式
            Messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            Messenger.Register<BrokenRuleMessage>(this, OnBrokenRule);
            Messenger.Register<ExamItemStateChangedMessage>(this, OnExamItemStateChanged);
            Messenger.Register<ExamItemStartMessage>(this, OnExamItemStart);
            Messenger.Register<ExamItemEndMessage>(this, OnExamItemEnd);
        }

        public async Task StartExamAsync(ExamContext context)
        {
            Speaker.CancelAllAsync();
            StartExam(context);
            PointSearcher.SetMapPoints(context.Map.MapPoints);
        }

        public async Task StopExamAsync(bool close = false)
        {
            StopExam(close);
        }



        #region 语音播报
        /// <summary>
        /// 语音播报开始考试
        /// </summary>
        protected virtual void VoiceStartExam()
        {
            Speaker.PlayAudioAsync(Settings.CommonExamStarExamVoice, SpeechPriority.High);
        }

        /// <summary>
        /// 语音播报结束考试
        /// </summary>
        protected virtual void VoiceEndExam()
        {
            /**********,*************/
            /**********,*************/
            // Logger.Error("Speaker CommonExamEndExamVoice");
            Speaker.PlayAudioAsync(Settings.CommonExamEndExamVoice, SpeechPriority.High);
            //如果没有设置了 不是优先播报那么就先播报考试不合格 再播报扣分 项目
            if (!Settings.FirstPlayExamFailVoice)
            {
                Speaker.PlayAudioAsync(Settings.CommonExamItemExamFailVoice);

                if (ExamScore.Score < 100)
                {
                    //Context.Rules 没有清零
                    foreach (var rule in Context.Rules)
                    {
                        Speaker.PlayAudioAsync(string.Format("{0}，扣{1}分", rule.DeductionRule.VoiceFile, rule.DeductionRule.DeductedScores));
                    }
                }
            }
            if (ExamScore.Score < 90)
            {
                //如果设置了优先播报 那么就先播报考试不合格
                if (Settings.FirstPlayExamFailVoice)
                {
                    if (!string.IsNullOrEmpty(Settings.CommonExamItemExamFailVoice))
                    {
                        Speaker.PlayAudioAsync(Settings.CommonExamItemExamFailVoice);
                    }
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Settings.CommonExamItemExamSuccessVoice))
                {
                    Speaker.PlayAudioAsync(Settings.CommonExamItemExamSuccessVoice);
                }
                // Logger.Error("ExamScore.Score >= 90 :Score" + ExamScore.Score.ToString() + "," + Settings.CommonExamItemExamSuccessVoice);
            }
            if (Settings.FirstPlayExamFailVoice)
            {
                if (ExamScore.Score < 100)
                {
                    //Context.Rules 没有清零
                    foreach (var rule in Context.Rules)
                    {
                        Speaker.PlayAudioAsync(string.Format("{0}，扣{1}分", rule.DeductionRule.VoiceFile, rule.DeductionRule.DeductedScores));
                    }
                }
            }
            //成都地区特殊要求 考试不合格 不在播报扣分语音
            //加入一个配置就可以了



        }
        #endregion

        /// <summary>
        /// 开始考试
        /// </summary>
        private void StartExam(ExamContext context)
        {
            Context = context;
            //每次开始考试都重置靠边停车的触发状态
            IsTriggerPullOver = false;
            Context.StartExam();
            ExamScore.Reset();
            ExamScore.VoiceBrokenRule = Settings.VoiceBrokenRule;
            VoiceStartExam();
            //超车项目语音删除掉！
            //Logger.Error("VoiceStartExam");
            InitMapItem();
            //todo:触发器主要是达到里程自动触发 靠边停车项目 目前暂时取消
            Messenger.Send(new ExamStartMessage(context));

            //InitMapItem();
        }

        /// <summary>
        /// 停止考试
        /// </summary>
        private void StopExam(bool close = false)
        {

            //停止语音播报；
            //TODO:这个需要 持续观察下 敖兄说的有时候报有时候不报这个问题
            //打Error日志只是为了方便客户在不开启日志的情况下也可以
            // Logger.Error("Stop Exam");
            ExamScore.VoiceBrokenRule = false;
            // TriggerHandler.Stop();
            //  Logger.Error("allItemTasks  Stop");
            var allItemTasks = ExamItems.Select(x => x.StopAsync()).ToArray();

            try
            {
                Task.WaitAll(allItemTasks, 1000 * 10);
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("停止考试项目发生异常，原因：{0}", exp);
            }
            //  Logger.Error("allItemTasks  Stop Finish");
            //取消消息机制
            //Messenger.Unregister(this);
            try
            {
                if (!close)
                {
                    VoiceEndExam();
                    Free(true);
                    //TODO:回收资源
                    //Messenger.Send(new ExamFinishedMessage(true, !ExamScore.Failed, Context));
                    Dispose();
                    //进行资源的一个释放

                }
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("VoiceEndExam Error，原因：{0}", exp);
            }

        }

        public async Task<IExamItem> StartItemAsync(ExamItemExecutionContext context, CancellationToken token,string ItemVoice="",string ItemEndVoice="")
        {
            try
            {
                //todo:如果
                var examItem =
                    ExamItems.FirstOrDefault(x => x.ItemCode == context.ItemCode && x.State == ExamItemState.Progressing);
                if (examItem != null)
                {
                    if (!string.IsNullOrEmpty(ItemVoice))
                    {
                        examItem.VoiceFile = ItemVoice;
                    }
                    if (!string.IsNullOrEmpty(ItemEndVoice))
                    {
                        examItem.EndVoiceFile = ItemEndVoice;
                    }
                    return examItem;
                }
                examItem = ProviderFactory.CreateExamItem(context.ItemCode, ItemVoice, ItemEndVoice);
                if (examItem != null)
                {
                    if (!string.IsNullOrEmpty(ItemVoice))
                    {
                        examItem.VoiceFile = ItemVoice;
                    }
                    if (!string.IsNullOrEmpty(ItemEndVoice))
                    {
                        examItem.EndVoiceFile = ItemEndVoice;
                    }
                    
                }
                lock (ExamItems)
                {
                    ExamItems.Add(examItem);
                }
                await examItem.StartAsync(context, token).ContinueWith((task) =>
                {
                    if (task.Exception != null)
                    {
                        Logger.ErrorFormat("运行考试项目{0}发生异常，原因：{1}", context.ItemCode, task.Exception, task.Exception);
                    }
                }, token);
                return examItem;
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("创建考试项目{0}发生异常，原因：{1}", context.ItemCode, exp, exp);
                return null;
            }
        }

        private Task<IExamItem> StartItemAsync(ExamItemStartMessage message, IDictionary<string, object> properties)
        {
            var itemCode = message.ExamItemCode;
            if (Context.ExamItemStates.All(x => x.ItemCode != itemCode))
            {
                Logger.WarnFormat("考试项目：{0}不存在", itemCode);
                return null;
            }
            var context = new ExamItemExecutionContext(Context);
            context.TriggerSource = ExamItemTriggerSource.Map;
            context.Properties = properties;
            context.TriggerPoint = message.MapPoint;
            context.ItemCode = message.ExamItemCode;

            return StartItemAsync(context, CancellationToken.None);
        }

        protected override void Free(bool disposing)
        {
            // Messenger.Unregister(this);
            //释放所有ExamItem资源
            //
            if (ExamItems != null)
            {
                var allItems = ExamItems.ToArray();
                foreach (var examItem in allItems)
                {
                    try
                    {
                        examItem.Dispose();
                    }
                    catch (Exception exp)
                    {
                        Logger.ErrorFormat("释放考试项目 {0}-{1} 资源发生异常，原因：{2}", examItem.Name, examItem.ItemCode, exp, exp);
                    }
                }
                lock (ExamItems)
                {
                    ExamItems.Clear();
                }
                //try
                //{
                //    ProviderFactory.BeforeLoadSimulationLight();
                //}
                //catch (Exception ex)
                //{
                //    Logger.ErrorFormat("异步提前加载灯光出错：{0}", ex.Message);
                //}
            }
        }
    }
}
