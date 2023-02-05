using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using OneBot.Core.Attributes;
using OneBot.Core.Util;

namespace OneBot.Core.Model.Message;

/// <summary>
/// 消息段
/// </summary>
public class MessageSegment : IMessageSegment
{
    private static readonly ConcurrentDictionary<Type, string> TypeCache = new ConcurrentDictionary<Type, string>();

    public string Type { get; }

    public IReadOnlyDictionary<string, object?> Data { get; }

    public MessageSegment(IReadOnlyDictionary<string, object?> data)
    {
        Type = TypeCache.GetOrAdd(data.GetType(), GeneratedType);
        Data = data;
    }

    public MessageSegment(string type, IReadOnlyDictionary<string, object?> data)
    {
        Type = type;
        Data = data;
    }

    [Obsolete("不保证 data 不丢信息")]
    public MessageSegment(IReadOnlyCollection<KeyValuePair<string, object?>> data)
    {
        Type = TypeCache.GetOrAdd(data.GetType(), GeneratedType);
        Data = data.ToImmutableDictionary();
    }

    [Obsolete("不保证 data 不丢信息")]
    public MessageSegment(string type, IReadOnlyCollection<KeyValuePair<string, object?>> data)
    {
        Type = type;
        Data = data.ToImmutableDictionary();
    }

    private static string GeneratedType(Type start)
    {
        var walker = new AttributeWalker(start);
        var attr = walker.FindFirst<OneBotExtraPropertiesAttribute>(s => s.GetType().IsAssignableTo(typeof(OneBotExtraPropertiesAttribute)))
                   ?? throw new ArgumentException("message segment type not acceptable");
        var s = attr.GetProperties()["type"];
        if (s is string ret)
        {
            return ret;
        }

        throw new ArgumentException("message segment type not acceptable");
    }
}
