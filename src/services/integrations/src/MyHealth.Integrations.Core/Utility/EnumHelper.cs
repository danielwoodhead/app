using System;
using System.Collections.Generic;

namespace MyHealth.Integrations.Core.Utility
{
    public static class EnumHelper
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
