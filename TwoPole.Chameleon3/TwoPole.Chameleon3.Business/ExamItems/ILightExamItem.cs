
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    public interface ILightExamItem : IExamItem, IProvider
    {
        string[] Groups { get; }

        string EndVoiceText { get; }

        //string GetRandomGroup(ExamItemExecutionContext context);
    }
}