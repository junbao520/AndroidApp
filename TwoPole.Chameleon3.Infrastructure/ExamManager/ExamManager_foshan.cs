﻿using System.Collections.Generic;
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
    public class ExamManager_foshan :ExamManager
    {

        public ExamManager_foshan
       (IProviderFactory providerFactory, ISpeaker speaker, IMessenger messenger, IExamScore examScore, IDataService dataService, IGpsPointSearcher pointSearcher, ILog logger)
       : base(providerFactory, speaker, messenger, examScore, dataService, pointSearcher, logger)
        {

        }
        protected override void InitMapItem()
        {
            try
            {
                //if (!Settings.PullOverStartFlage)
                //    return;
                if (Context == null)
                    return;
                if (!Context.IsExaming)
                    return;
                //清空项目
                FinishedPointList = new List<MapPointType>();
                InitPointList = new List<MapPointType>();
                var temMap = Context.Map.MapPoints;
                //有选择地图
                if (temMap.Count() > 1)
                {
                    foreach (var mapPoint in temMap)
                    {
                        //所有的都加入进去,除开语音点
                        if (!mapPoint.Name.Contains("点")||mapPoint.Name.Contains("考试结束"))
                        {
                            InitPointList.Add(mapPoint.PointType);
                            Logger.Info("@@InitMapPoint本次加入的项目名：:" +mapPoint.Name);
                        }

                        
                    }
                }
                var str = string.Join(",", InitPointList);
                Logger.Info("@@InitMapPoint项目个数：:" + InitPointList.Count);
            }
            catch (Exception ex)
            {
                Logger.Error("InitMap" + ex.Message);
            }
        }
        protected override void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            base.OnExamItemStateChanged(message);
            //totod:经过张仪测试 这个是可以的！ 但是左福川测试 这个不可以 原因不明
           
            if (InitPointList.Count > 0 && FinishedPointList.Count >= InitPointList.Count)
            {
                //综合评判考试完成也会结束
                if (message.ExamItem.State==ExamItemState.Finished&&message.ExamItem.ItemCode!=ExamItemCodes.CommonExamItem)
                {
                    Logger.Info("@@完成所有项目直接触发考试结束");
                    Messenger.Send(new ExamFinishingMessage(false));
                }
            }
        }

        //记录正在考或刚考完的项目,只过滤一个项目（即不能连着打两个相同的项目，中间必须间隔一个其他项目）
        private Queue<MapPointType> examingItem = new Queue<MapPointType>(1);

        protected override async Task MatchMapPointsAsync(CarSignalInfo signalInfo)
        {
            //只是一个条件
            if (Settings.EndExamByDistance && IsTriggerPullOver == false)
            {
                if (signalInfo.Distance >= Settings.ExamDistance)
                {
                    IsTriggerPullOver = true;
                    Logger.Info("@@里程达到三公里自动触发考试项目");
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

               
                //其实最简单还是要在这里处理
                if (item != null)
                {
                    //排除语音点和起步
                    if (InitPointList.Count > 0 && !item.ItemName.Contains("点"))
                    {
                        if (!examingItem.Any(x => x.Equals(item.MapPointType)) && item.MapPointType!= MapPointType.VehicleStarting)
                        {
                            examingItem.Enqueue(item.MapPointType);
                            FinishedPointList.Add(item.MapPointType);
                            Logger.Info("@@加入队列项目：{0}", item.ItemName);
                        }
                        if (examingItem.Count > 1)
                            examingItem.Dequeue();


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



    }
}
