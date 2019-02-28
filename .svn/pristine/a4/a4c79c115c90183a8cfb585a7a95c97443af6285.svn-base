//using System.Collections.Specialized;
//using System.Threading;
//using Chameleon.Car.Infrastructure;
//using Chameleon.Car.Infrastructure.Gps;
//using Chameleon.Car.Infrastructure.Messages;
//using DCommon;
//using GalaSoft.MvvmLight.Messaging;

//namespace Chameleon.Bussiness.ExamItems
//{
//    public abstract class PreVoiceExamItem : ExamItemBase
//    {
//        protected int VoiceForwardDistance { get; set; }

//        protected IGpsDistance GpsDistance { get; set; }
//        protected double? VoiceStartDistance { get; set; }

//        public PreVoiceExamItem(
//            IGpsDistance gpsDistance,
//            ISpeaker speaker, 
//            IMessenger messenger)
//            : base(speaker, messenger)
//        {
//            GpsDistance = gpsDistance;
//        }

//        public override void Init(NameValueCollection settings)
//        {
//            base.Init(settings);
//            VoiceForwardDistance = settings.GetIntValue("VoiceForwardDistance",
//                DefaultGlobalSettings.DefaultVoiceForwardDistance);
//        }

//        /// <summary>
//        /// 开始考试项目，但不激活考试规则
//        /// </summary>
//        /// <param name="context"></param>
//        /// <param name="token"></param>
//        protected override void StartCore(ExamItemExecutionContext context, CancellationToken token)
//        {
//            if (context.Source == TriggerSource.Manual ||
//                context.Source == TriggerSource.Auto)
//            {
//                VoiceStartDistance = GpsDistance.Distance;
//            }
//            else if (context.Source == TriggerSource.Map)
//            {
//                Messenger.Register<CurrentMapPointChangedMessage>(this, OnMapPointChanged);
//            }
//            SpeakStartVoice();
//            Active();
//        }

//        protected virtual void SpeakStartVoice()
//        {
//            if (VoiceExamItem)
//                Speaker.SpeakAsync(TextToSpeach);
//        }

//        protected virtual void OnMapPointChanged(CurrentMapPointChangedMessage message)
//        {
//            if (MapPointFilter(message))
//            {
//                Messenger.Unregister<CurrentMapPointChangedMessage>(this, OnMapPointChanged);
//                StartRules();
//            }
//        }

//        protected abstract bool MapPointFilter(CurrentMapPointChangedMessage message);

//        public override void GpsReceived(GpsInfo gpsInfo)
//        {
//            base.GpsReceived(gpsInfo);

//            //距离改变
//            if (VoiceStartDistance.HasValue &&
//                GpsDistance.Distance - VoiceStartDistance.Value > VoiceForwardDistance)
//            {
//                StartRules();
//                //不要重复执行
//                VoiceStartDistance = null;
//            }
//        }
//    }
//}
