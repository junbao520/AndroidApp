//using System.Threading;
//using Chameleon.Car.Infrastructure;
//using Chameleon.Car.Infrastructure.Gps;
//using Chameleon.Car.Infrastructure.Messages;
//using GalaSoft.MvvmLight.Messaging;

//namespace Chameleon.Bussiness.ExamItems
//{
//    public abstract class TransientExamItem : PreVoiceExamItem
//    {
//        protected double? MapTriggerDistance = null;

//        protected TransientExamItem(IGpsDistance gpsDistance, ISpeaker speaker, IMessenger messenger)
//            : base(gpsDistance, speaker, messenger)
//        {
//        }

//        public override void SensorReceived(CarSensorInfo sensorInfo)
//        {
//        }

//        public override void GpsReceived(GpsInfo gpsInfo)
//        {
//            //距离改变，计算行驶距离是否到达
//            if (VoiceStartDistance.HasValue &&
//                GpsDistance.Distance - VoiceStartDistance.Value > VoiceForwardDistance)
//            {
//                StartRules();
//                //不要重复执行
//                VoiceStartDistance = null;
//            }

//            //计算行驶距离是否超过点位触发的距离；
//            if (MapTriggerDistance.HasValue &&
//                GpsDistance.Distance - MapTriggerDistance.Value > base.VoiceForwardDistance * 1.5)
//            {
//                //如果行驶的里程大于语音播报的2倍距离，自动结束；
//                Logger.DebugFormat("项目：{0}，行驶里程大于语音播报的距离，自动结束", Name);
//                StopCore();
//            }
//        }

//        protected override void StartCore(ExamItemExecutionContext context, CancellationToken token)
//        {
//            base.StartCore(context, token);

//            //地图触发项目时的里程
//            if (context.Source == TriggerSource.Map)
//                MapTriggerDistance = GpsDistance.Distance;
//        }

//        /// <summary>
//        /// 只执行一次Rule
//        /// </summary>
//        protected override void StartRules()
//        {
//            base.StartRules();

//            foreach (var rule in GpsDependencyRules)
//            {
//                IGpsSet gpsSet = Resolve<IGpsSet>();
//                rule.GpsReceived(gpsSet.Current);
//            }
//            foreach (var rule in SensorDependencyRules)
//            {
//                ICarSensorSet sensorSet = Resolve<ICarSensorSet>();
//                rule.SensorReceived(sensorSet.Current);
//            }

//            StopCore();
//        }
//    }
//}
