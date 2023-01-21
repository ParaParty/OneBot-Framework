using System;
using System.Collections.Concurrent;
using OneBot.Core.Attributes;
using OneBot.Core.Interface;

namespace OneBot.Core.Util;

public static class MiddlewareUtil
{
    private static readonly ConcurrentDictionary<Type, string> NameCache = new ConcurrentDictionary<Type, string>();

    public static string GetName(this IOneBotMiddleware t)
    {
        return NameCache.GetOrAdd(t.GetType(), s => ParseNameFromAttribute(t) ?? t.GetType().FullName ?? "?");
    }

    private static string? ParseNameFromAttribute(IOneBotMiddleware t)
    {
        var walker = new AttributeWalker(t.GetType());
        var c = walker.FindFirst<OneBotComponentNameAttribute>(s => s.GetType().IsAssignableTo(typeof(OneBotComponentNameAttribute)));
        return c?.Name;
    }
}
