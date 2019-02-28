using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System.Net;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.HaiKou.ExamItems
{
    /// <summary>
    /// 只判定车头探头  TwoPole.Chameleon3.Business.Areas.HaiNan.HaiKou.ExamItems.HaiKouPrepareDriving
    /// </summary>
    public class HaiKouPrepareDriving : ExamItemBase
    {
        public CarSensorInfo LastSensor { get; private set; }
        protected int AroundCarTimeout { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected bool IsAttachHeadstock { get; set; }

        public HaiKouPrepareDriving()
        {
            LastSensor = new CarSensorInfo();
            IsAttachHeadstock = false;
        }
        /// <summary>
        /// 检测传感器 海口没有车尾  车头 需要摸两次
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            var isTimeout = (DateTime.Now - StartTime).TotalSeconds > Settings.AroundCarTimeout;
            var FirstArrivedHeadStockTime = DateTime.Now;
            if (isTimeout)
            {
                ValidExtraSensor();
                StopCore();
                return;
            }
            var newExtraSensor = signalInfo.Sensor;
            if (newExtraSensor != null)
            {
                //需要有一个时间间隔 //
                if (!LastSensor.ArrivedHeadstock && newExtraSensor.ArrivedHeadstock)
                {
                    LastSensor.ArrivedHeadstock = true;
                    IsAttachHeadstock = true;
                    Messenger.Send(new ExamStatusMessage("正经过车头", LastSensor));
                    //训练模式才有语音
                    Speaker.PlayAudioAsync("上车一",SpeechPriority.High);
                    //if (Context.ExamMode == ExamMode.Training)
                    //{
                    //    Speaker.PlayAudioAsync("haikou/shangche1.wav", Infrastructure.Speech.SpeechPriority.Highest);
                    //}
                }
                else if (LastSensor.ArrivedHeadstock && !newExtraSensor.ArrivedHeadstock && IsAttachHeadstock)
                {
                    IsAttachHeadstock = false;
                }
                //这样就上车1 和上车2 就只会执行一次
                else if (!LastSensor.ArrivedTailstock && newExtraSensor.ArrivedHeadstock && !IsAttachHeadstock)
                {
                    LastSensor.ArrivedTailstock = true;
                    Messenger.Send(new ExamStatusMessage("第二次经过车头", LastSensor));

                    Speaker.PlayAudioAsync("上车二", SpeechPriority.High);
                    //训练模式才有语音
                    //if (Context.ExamMode == ExamMode.Training)
                    //{
                    //    Speaker.PlayAudioAsync("haikou/shangche2.wav", Infrastructure.Speech.SpeechPriority.Highest);
                    //}
                }
                //海口没有经过车尾
                if (!LastSensor.ExteriorMirror && newExtraSensor.ExteriorMirror)
                {
                    Messenger.Send(new ExamStatusMessage("正在调整观视镜", LastSensor));
                    LastSensor.ExteriorMirror = true;
                    //TODO:最好把这些写死的语音都做成一个键值对配置文件可以修改
                    //这个配置文件可以是一个xml配置文件 不需要存放在数据库
                    Speaker.PlayAudioAsync("观望镜确认",SpeechPriority.High);
                    //if (Context.ExamMode == ExamMode.Training)
                    //{ 
                    //    Speaker.PlayAudioAsync("haikou/guanwangjing.wav", Infrastructure.Speech.SpeechPriority.Highest);
                    //}
                }
                if (!LastSensor.InnerMirror && newExtraSensor.InnerMirror)
                {
                    Messenger.Send(new ExamStatusMessage("正在调整后视镜", LastSensor));
                    LastSensor.InnerMirror = true;
                    Speaker.PlayAudioAsync("内后视镜确认", SpeechPriority.High);
                    //if (Context.ExamMode == ExamMode.Training)
                    //{
                    //    Speaker.PlayAudioAsync("haikou/neihoushijing.wav", Infrastructure.Speech.SpeechPriority.Highest);
                    //}
                }
                if (!LastSensor.Seats && newExtraSensor.Seats)
                {
                    Messenger.Send(new ExamStatusMessage("正在调整座椅", LastSensor));
                    LastSensor.Seats = true;
                    Speaker.PlayAudioAsync("调整座椅确认",SpeechPriority.High);
                    //if (Context.ExamMode == ExamMode.Training)
                    //{
                    //    Speaker.PlayAudioAsync("haikou/zuoyi.wav", Infrastructure.Speech.SpeechPriority.Highest);
                    //}
                }
            }

            base.ExecuteCore(signalInfo);
        }

        protected void ValidExtraSensor()
        {
            LastSensor = LastSensor ?? new CarSensorInfo();
            //座椅
            if (!LastSensor.Seats)
            {
                BreakRule(DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021101);
            }
            if (!LastSensor.ExteriorMirror)
            {
                BreakRule(DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021102);
            }
            if (!LastSensor.InnerMirror)
            {
                BreakRule(DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021103);
            }
            //绕车一周（车头）
            if (!LastSensor.ArrivedHeadstock)
            {
                BreakRule(DeductionRuleCodes.RC40101);
            }
            if (!LastSensor.ArrivedTailstock)
            {
                BreakRule(DeductionRuleCodes.RC40101);
            }
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PrepareDriving; }
        }
    }
}
