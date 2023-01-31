using System.Collections;
using System.Linq;

namespace OneBot.Core.Util;

internal static class PropertyAccessor
{
    internal static object? Get(object data, string key)
    {
        var type = data.GetType();
        var properties = type.GetProperties();
        var prop = properties.FirstOrDefault(s => s?.Name == key && s.CanRead, null);
        if (prop != null)
        {
            return prop.GetValue(data);
        }

        var dictionaryInterface = type.GetInterfaces().Where(s => s == typeof(IDictionary)).ToList();
        if (dictionaryInterface.Count > 0)
        {
            return ((IDictionary)data)[key];
        }
        return default;
    }
}
