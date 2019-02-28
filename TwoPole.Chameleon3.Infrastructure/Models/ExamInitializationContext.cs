using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Map;

namespace TwoPole.Chameleon3.Infrastructure
{
    public sealed class ExamInitializationContext
    {
        public static readonly ExamInitializationContext Empty = new ExamInitializationContext();

        public IMapSet MapSet { get; private set; }

        private ExamInitializationContext() { }

        public ExamInitializationContext(IMapSet map)
        {
            MapSet = map;
        }
    }
}
