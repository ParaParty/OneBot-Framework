using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OneBot.Core.Attributes;

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
        var pending = new Queue<Type>();
        var mark = new Collection<Type>();
        pending.Enqueue(start);
        mark.Add(start);

        while (pending.TryPeek(out var type))
        {
            pending.Dequeue();

            var attributes = type.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                if (!attribute.GetType().IsAssignableTo(typeof(OneBotExtraPropertiesAttribute)))
                {
                    continue;
                }

                var prop = ((OneBotExtraPropertiesAttribute)attribute).GetProperties();
                var s = prop["type"];
                if (s is string ret)
                {
                    return ret;
                }
            }

            foreach (var item in type.GetInterfaces())
            {
                if (mark.Contains(item))
                {
                    continue;
                }
                pending.Enqueue(item);
                mark.Add(item);
            }
        }
        throw new ArgumentException("message segment type not acceptable");
    }

    public string Type { get; }

    public T Data { get; }
}
