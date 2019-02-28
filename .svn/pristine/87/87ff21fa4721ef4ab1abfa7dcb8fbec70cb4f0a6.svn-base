using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.ExamItems.YuunFu
{
    /// <summary>
    /// 拱桥项目
    /// </summary>
    public  class ArchBridge : ExamItemBase
    {
     

        protected IAdvancedCarSignal AdvancedSignal { get; private set; }
       
        


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            //获取人行横道项目距离
            MaxDistance = Settings.PedestrianCrossingDistance;
            VoiceExamItem = Settings.PedestrianCrossingVoice;
          
           
        }

        protected override void StopCore()
        {

            //喇叭检测
            var hasLoudSpeaker = CarSignalSet.Query(StartTime).Any(x => x.Sensor.Loudspeaker);
            if (!hasLoudSpeaker)
            {
                BreakRule(DeductionRuleCodes.RC30212);
            }

            //夜间远近光交替
            if (Context.ExamTimeMode == ExamTimeMode.Night)
            {
                var hasLowAndHighBeam = AdvancedSignal.CheckHighBeam(StartTime);
                if (!hasLowAndHighBeam)
                {
                    BreakRule(DeductionRuleCodes.RC41603);
                }
            }

            base.StopCore();
        }

       

        public override string ItemCode
        {
            get { return ExamItemCodes.ArchBridge; }
        }
    }
}
