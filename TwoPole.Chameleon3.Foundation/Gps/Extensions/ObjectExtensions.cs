using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    internal static class ObjectExtensions
    {
        public static int ToInt32(this string input, int defaultValue = 0)
        {
            int v;
            if (Int32.TryParse(input, out v))
                return v;
            return defaultValue;
        }
    }
}
