using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class PrepareDrivingFinishedMessage : MessageBase
    {
        public PrepareDrivingFinishedMessage()
        {
            this.IsJudge = true;
        }
        public PrepareDrivingFinishedMessage(bool IsJudge)
        {
            this.IsJudge = IsJudge;
        }
        public bool IsJudge { get; set; }
    }
}
