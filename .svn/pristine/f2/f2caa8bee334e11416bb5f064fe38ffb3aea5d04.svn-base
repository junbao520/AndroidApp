using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Providers
{
    public class ProviderFactory
    {
      // private static readonly ILog Logger = LogManager.GetLogger(typeof(ProviderFactory));

        public static T CreateProvider<T>(Type type, NameValueCollection settings)
            where T : class
        {
            if (!typeof(IProvider).IsAssignableFrom(type))
                throw new ApplicationException(string.Format("类型 {0} 不正确", type));

            try
            {
                var provider = Activator.CreateInstance(type) as IProvider;
                if (provider != null)
                    provider.Init(settings);
                return (T)provider;
            }
            catch (Exception exp)
            {
               // Logger.ErrorFormat("创建Provider {0} 发生异常", type, exp);
            }
            return null;
        }
    }
}
