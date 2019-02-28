using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class NameValueCollectionExtensions
    {
        public static NameValueCollection ToValues(this IEnumerable<Setting> settings)
        {
            var values = new NameValueCollection();
            if (settings != null)
            {
                foreach (var setting in settings)
                {
                    values[setting.Key] = setting.Value;
                }
            }
            return values;
        }
    }
}
