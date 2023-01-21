using System;
using System.Collections.Generic;

namespace OneBot.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
public class OneBotExtraPropertiesAttribute : Attribute
{
    private readonly Dictionary<string, object> _prop;

    public Dictionary<string, object> GetProperties()
    {
        return new Dictionary<string, object>(_prop);
    }

    public OneBotExtraPropertiesAttribute(params string[] keyValuePairs)
    {
        if (keyValuePairs.Length % 2 == 1)
        {
            throw new ArgumentException("the elements count of keyValuePairs must be even");
        }
        var n = keyValuePairs.Length / 2;

        _prop = new Dictionary<string, object>();
        for (var i = 0; i < n; i++)
        {
            _prop[keyValuePairs[i * 2]] = keyValuePairs[i * 2 + 1];
        }
    }
}