using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Providers
{
    public interface ISettingService
    {
        TSettings GetSettings<TSettings>()
            where TSettings : class, ISettings, new();

        void SaveSettings<TSettings>(TSettings settings)
            where TSettings : class, ISettings, new();
    }
}
