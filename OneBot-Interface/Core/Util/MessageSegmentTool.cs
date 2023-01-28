using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using OneBot.Core.Model.Message;

namespace OneBot.Core.Util;

public static class MessageSegmentTool
{
    private static readonly Type MessageSegmentType = typeof(IMessageSegment<>);

    private static readonly Lazy<MethodInfo> ExtractMessageFromMessageSegmentMethod = new Lazy<MethodInfo>(() =>
        typeof(MessageSegmentTool).GetMethods().First(s => s.ToString() == "T Get[T,R](OneBot.Core.Model.Message.MessageSegment`1[R], System.String)")
    );

    private static readonly Lazy<MethodInfo> GetSegmentTypeMethod = new Lazy<MethodInfo>(() =>
        typeof(MessageSegmentTool).GetMethods().First(s => s.ToString() == "System.String GetSegmentType[T](OneBot.Core.Model.Message.IMessageSegment`1[T])")
    );

    public static Type? GetSegmentDataType(this MessageSegmentRef it)
    {
        var type = it.GetType();
        var interfaces = type.GetInterfaces();

        var msgSegTypeList = interfaces.Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == MessageSegmentType).ToList();
        if (msgSegTypeList.Count != 1)
        {
            return null;
        }

        return msgSegTypeList[0];
    }

    public static string? GetSegmentType(this MessageSegmentRef it)
    {
        var msgSegType = it.GetSegmentDataType();
        if (msgSegType == null)
        {
            return default;
        }

        var r = msgSegType.GetGenericArguments()[0];
        var val = GetSegmentTypeMethod.Value.MakeGenericMethod(r).Invoke(null, new object[] { it });
        return (string?)val;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    public static string GetSegmentType<T>(this IMessageSegment<T> it) where T : MessageData
    {
        return it.Type;
    }

    public static T? Get<T>(this MessageSegmentRef it, string key)
    {
        var msgSegType = it.GetSegmentDataType();
        if (msgSegType == null)
        {
            return default;
        }
        var r = msgSegType.GetGenericArguments()[0];
        var val = ExtractMessageFromMessageSegmentMethod.Value.MakeGenericMethod(typeof(T), r).Invoke(null, new object[] { it, key });
        return (T?)val;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
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
