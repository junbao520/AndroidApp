using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System.Collections.Specialized; 
using System;
using System.Linq;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Business.ExamItems;

namespace TwoPole.Chameleon3.Business.Areas.Chongqin.Fuling.ExamItems
{
    /// <summary>
    /// 请掉头语音
    /// </summary>
    public class TurnRoundPlease : ExamItemBase
    {

        //#endregion
     

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceFile = "请掉头";
            VoiceExamItem = Settings.TurnRoundVoice;
            MaxDistance = Settings.TurnRoundMaxDistance;
        }

     
        public override string ItemCode
        {
            get { return ExamItemCodes.TurnRoundPlease; }
        }
    }

    
}
