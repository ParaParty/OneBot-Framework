using System;
using System.Collections.Concurrent;
using OneBot.Core.Attributes;
using OneBot.Core.Util;

namespace OneBot.Core.Model.Message;

/// <summary>
/// 一个消息段
/// </summary>
/// <typeparam name="T"></typeparam>
public class MessageSegment<T> : IMessageSegment<T> where T : MessageData
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConcurrentDictionary<Type, string> TypeCache = new ConcurrentDictionary<Type, string>();

    public MessageSegment(string type, T data)
    {
        Type = type;
        Data = data;
    }

    public MessageSegment(T data)
    {
        Type = TypeCache.GetOrAdd(data.GetType(), GeneratedType);
        Data = data;
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

    public string Type { get; }

    public T Data { get; }
}
