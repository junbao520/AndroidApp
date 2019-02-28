using System;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Business.Areas.HaiNan.HaiKou.ExamItems
{
    /// <summary>
    /// 车头探头,2个，车尾也装在车头位置
    /// </summary>
    public class PrepareDriving_HaiHe : ExamItemBase
    {
        public CarSensorInfo LastSensor { get; private set; }
        protected int AroundCarTimeout { get; set; }

        public PrepareDriving_HaiHe()
        {
            LastSensor = new CarSensorInfo();
        }

        private bool IsStartEngine = false;

        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.Engine)
            {
                IsStartEngine = true;
            }
            return base.InitExamParms(signalInfo);
        }

        /// <summary>
        /// 检测传感器
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            var isTimeout = (DateTime.Now - StartTime).TotalSeconds > Settings.AroundCarTimeout;
            if (isTimeout)
            {
                ValidExtraSensor();
                StopCore();
                return;
            }

            //当发动机启动后进行结束上车准备。

            //if (IsStartEngine)
            //{
            //    if (signalInfo.Sensor.SafetyBelt)
            //    {
            //        ValidExtraSensor();
            //        StopCore();
            //        return;
            //    }
            //}
            //else {

            if (signalInfo.Sensor.Engine)
            {
                ValidExtraSensor();
                StopCore();
                return;
            }
            //单机训练如果发动机启动的情况下以安全带插上结束考试
            var newExtraSensor = signalInfo.Sensor;
            if (newExtraSensor != null)
            {
                if (!LastSensor.ArrivedHeadstock && newExtraSensor.ArrivedHeadstock)
                {
                    LastSensor.ArrivedHeadstock = true;
                    Messenger.Send(new ExamStatusMessage("正经过车头", LastSensor));
                    //训练模式才有语音
                    //if (Context.ExamMode == ExamMode.Training)
                    //{
                    //    Speaker.PlayAudioAsync("sanya/ct.wav", Infrastructure.Speech.SpeechPriority.Highest);
                    //}
                }
                //海口车尾安装在了车头位置
                if (!LastSensor.ArrivedTailstock && newExtraSensor.ArrivedTailstock)
                {
                    LastSensor.ArrivedTailstock = true;
                    Messenger.Send(new ExamStatusMessage("正经过车头二", LastSensor));

                }

                if (!LastSensor.ExteriorMirror && newExtraSensor.ExteriorMirror)
                {
                    Messenger.Send(new ExamStatusMessage("正在调整后视镜", LastSensor));
                    LastSensor.ExteriorMirror = true;
                }
                if (!LastSensor.InnerMirror && newExtraSensor.InnerMirror)
                {
                    Messenger.Send(new ExamStatusMessage("正在调整后视镜", LastSensor));
                    LastSensor.InnerMirror = true;
                }
                if (!LastSensor.Seats && newExtraSensor.Seats)
                {
                    Messenger.Send(new ExamStatusMessage("正在调整座椅", LastSensor));
                    LastSensor.Seats = true;
                }
            }

            base.ExecuteCore(signalInfo);
        }

        protected void ValidExtraSensor()
        {
            LastSensor = LastSensor ?? new CarSensorInfo();
            //座椅 改在起步开始前验证
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
            if (!LastSensor.ArrivedHeadstock || !LastSensor.ArrivedTailstock)
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
