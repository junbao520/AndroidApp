using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;


namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 环岛
    /// </summary>
    public class Roundabout : ExamItemBase
    {
        #region 检测规则
        //1，检测右转向灯
        #endregion


        #region 参数配置
        protected string TurnRoundNumber { get; set; }
        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            MaxDistance = Settings.RoundaboutDistance;
        }

        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //TurnRoundNumber = Context.Properties["Road"].ToString();
            StartTime = DateTime.Now;
            //if (Settings.RoundaboutVoice)
                //Speaker.PlayAudioAsync(RoundaboutVoiceFile, Infrastructure.Speech.SpeechPriority.Highest);

            return base.InitExamParms(signalInfo);
        }
 

        protected override void StopCore()
        {
            //只有第一个出口检测转向灯
            if (Settings.RoundaboutLightCheck)
            {
                if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.RightIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC40212);
                }
            }
            base.StopCore();
        }


        public override string ItemCode
        {
            get { return ExamItemCodes.Roundabout; }
        }

        public string RoundaboutVoiceFile
        {
            get
            {
                var voiceFile = "hd1.wav";
                switch (TurnRoundNumber)
                {
                    case "一":
                        voiceFile = "hd1.wav";
                        break;
                    case "二":
                        voiceFile = "hd2.wav";
                        break;
                    case "三":
                        voiceFile = "hd3.wav";
                        break;
                }
                return voiceFile;
            }
        }

        protected override bool VoiceExamItem
        {
            get
            {
                return false;
            }
            set
            {
                base.VoiceExamItem = value;
            }
        }
    }
}
