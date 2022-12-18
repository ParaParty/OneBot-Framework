using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using OneBot.Core.Model.Message;

namespace OneBot.Core.Util;

public static class MessageSegmentAccessor
{
    private static readonly Type _messageSegmentType = typeof(IMessageSegment<>);

    private static readonly Lazy<MethodInfo> _extractMessageFromMessageSegmentMethod = new Lazy<MethodInfo>(() =>
        typeof(MessageSegmentAccessor).GetMethods().First(s => s.ToString() == "T Get[T,R](OneBot.Core.Model.Message.MessageSegment`1[R], System.String)")
    );

    public static T? Get<T>(this MessageSegmentRef it, string key)
    {
        var type = it.GetType();
        var interfaces = type.GetInterfaces();

        var msgSegTypeList = interfaces.Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == _messageSegmentType).ToList();
        if (msgSegTypeList.Count != 1)
        {
            return default;
        }

        var msgSegType = msgSegTypeList[0];
        var R = msgSegType.GetGenericArguments()[0];
        var val = _extractMessageFromMessageSegmentMethod.Value.MakeGenericMethod(typeof(T), R).Invoke(null, new object[] { it, key });
        return (T?)val;
    }

    public static T? Get<T, R>(this MessageSegment<R> it, string key) where R : MessageData
    {
        R data = it.Data;
        var type = data.GetType();
        var properties = type.GetProperties();
        var prop = properties.FirstOrDefault(s => s?.Name == key && s.CanRead, null);
        if (prop != null)
        {
            return (T?)prop.GetValue(data);
        }

        var dictionaryInterface = type.GetInterfaces().Where(s => s == typeof(IDictionary)).ToList();
        if (dictionaryInterface.Count > 0)
        {
            return (T?)((IDictionary)data)[key];
        }
        return default;
    }
}
