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
    public class ExamManager_luzhou : ExamManager
    {
        public ExamManager_luzhou(
            IProviderFactory providerFactory,
            ISpeaker speaker,
            IMessenger messenger,
            IExamScore examScore,
            IDataService dataService,
            IGpsPointSearcher pointSearcher,
            ILog logger
            ) : base(
             providerFactory,
             speaker,
             messenger,
             examScore,
             dataService,
             pointSearcher,
             //这个有一个Logger导致依赖注入不成功
             logger
            )
        {

        }


        protected override void InitMapItem()
        {
            base.InitMapItem();
            Constants.IsFirstTurnRight = false;
            Constants.IsFirstTurnLeft = false;
            Constants.IsExamMode_Luzhou = false;
        }
        protected override void VoiceStartExam()
        {
            Speaker.PlayAudioAsync("新收到考试信息，白天模式，请等待身份确认，做好考试准备后踩刹车获取考试指令", SpeechPriority.High);
        }


        string hgVoice = "考试结束，成绩合格";
        string bhgVoice = "考试结束，成绩不合格，您的扣分项目是";
        string afterVoice = "";

        /// <summary>
        /// 语音播报结束考试
        /// </summary>
        protected override void VoiceEndExam()
        {

            if (ExamScore.Score < 90)
            {
                Speaker.PlayAudioAsync(bhgVoice);
            }
            else
            {
                Speaker.PlayAudioAsync(hgVoice);
            }
            if (ExamScore.Score < 90)
            {
                foreach (var rule in Context.Rules)
                {
                    if (Settings.PlayFail && rule.DeductionRule.DeductedScores == 100)
                    {
                        Speaker.PlayAudioAsync(string.Format("{0}，不合格", rule.DeductionRule.VoiceFile, rule.DeductionRule.DeductedScores));
                    }
                    else
                    {
                        Speaker.PlayAudioAsync(string.Format("{0}，扣{1}分", rule.DeductionRule.VoiceFile, rule.DeductionRule.DeductedScores));
                    }
                }
                Speaker.PlayAudioAsync(afterVoice);
            }




        }
        //泸州特殊项目过滤完成了就不在自动触发
        protected override async Task MatchMapPointsAsync(CarSignalInfo signalInfo)
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
                        Constants.IsExamMode_Luzhou = true;
                        if (item.MapPointType != MapPointType.TurnLeft && item.MapPointType != MapPointType.TurnRight)
                        {
                            //只报一次，除了左，右转
                            if (!FinishedPointList.Contains(item.MapPointType))
                                FinishedPointList.Add(item.MapPointType);
                            else
                            {
                                //播报过的项目，直接返回
                                return;
                            }
                        }
                        //////左转，右转，直行路口不过滤
                        ////if (FinishedPointList.Contains(item.MapPointType))
                        ////{
                        ////    //泸州过滤掉所有考试项目
                        ////    //if (item.ItemCode != ExamItemCodes.TurnLeft && item.ItemCode != ExamItemCodes.TurnRight &&
                        ////    //    item.ItemCode != ExamItemCodes.StraightThrough && item.ItemCode != ExamItemCodes.TurnRound
                        ////    //    && item.ItemCode != ExamItemCodes.SchoolArea && item.ItemCode != ExamItemCodes.BusArea && item.ItemCode != ExamItemCodes.PedestrianCrossing && item.ItemCode != ExamItemCodes.PedestrianCrossingHasPeople)
                        ////    {
                        ////        Logger.Info("跳过项目:" + item.ItemCode);
                        ////        continue;
                        ////    }
                        ////}

                        ////if (!FinishedPointList.Contains(item.MapPointType) && item.MapPointType != MapPointType.PullOver)
                        ////{
                        ////    FinishedPointList.Add(item.MapPointType);
                        ////    Logger.Info(item.MapPointType.ToString() + ":" + FinishedPointList.Count.ToString());
                        ////}
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
