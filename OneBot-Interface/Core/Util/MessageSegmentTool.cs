using System.Collections.Generic;
using JetBrains.Annotations;
using OneBot.Core.Model.Message;

namespace OneBot.Core.Util;

public static class MessageSegmentTool
{
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    public static string GetSegmentType(this IMessageSegment it)
    {
        return it.Type;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    public static T? Get<T>(this IMessageSegment it, string key, T? defaultValue = default)
    {
        IReadOnlyDictionary<string, object?> data = it.Data;
        if (data.TryGetValue(key, out var ret))
        {
            return (T?)ret;
        }
        return defaultValue;
    }  
    
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    public static object? Get(this IMessageSegment it, string key, object? defaultValue = default)
    {
        IReadOnlyDictionary<string, object?> data = it.Data;
        if (data.TryGetValue(key, out var ret))
        {
            return ret;
        }
        return defaultValue;
    }
}
