using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Triggers;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IProviderFactory
    {
      

        IExamItem CreateExamItem(string examItemCode, string ItemVoice = "", string ItemEndVoice = "");

        ITrigger[] CreateTriggers();

   

        ICarSignalProcessor[] CreateSignalProcessors();

        ICarSignalSeed CreateCarSignalSeed();

        void BeforeLoadSimulationLight();
    }
  
}
