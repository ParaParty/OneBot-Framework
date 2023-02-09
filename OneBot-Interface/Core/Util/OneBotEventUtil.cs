using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Core.Util;

public static class OneBotEventUtil
{

    internal class NameCacheEntry
    {
        internal enum State
        {
            None,

            InAttr,

            InCode,
        }

        internal State Type = State.None;

        internal State DetailType = State.None;

        internal State SubType = State.None;

        internal string? TypeValue;

        internal string? DetailTypeValue;

        internal string? SubTypeValue;
    }

    private static readonly ConcurrentDictionary<Type, NameCacheEntry> TypeCache = new ConcurrentDictionary<Type, NameCacheEntry>();

    public static Dictionary<string, string?> GetEventType(this OneBotEvent e)
    {
        var entry = TypeCache.GetOrAdd(e.GetType(), s =>
        {
            var ret = new NameCacheEntry();

            if (s.IsAssignableTo(typeof(OneBotEvent.Type)))
            {
                ret.Type = NameCacheEntry.State.InCode;
            }

            if (s.IsAssignableTo(typeof(OneBotEvent.DetailType)))
            {
                ret.DetailType = NameCacheEntry.State.InCode;
            }

            if (s.IsAssignableTo(typeof(OneBotEvent.SubType)))
            {
                ret.SubType = NameCacheEntry.State.InCode;
            }

            var attrWalker = new AttributeWalker(s);
            var attr = attrWalker.FindNext<OneBotExtraPropertiesAttribute>(i => i.GetType().IsAssignableTo(typeof(OneBotExtraPropertiesAttribute)));
            while (attr != null)
            {
                var prop = attr.GetProperties();
                {
                    if (prop.TryGetValue(OneBotEvent.Type.PropertyName, out var t) && t is string str)
                    {
                        ret.Type = NameCacheEntry.State.InAttr;
                        ret.TypeValue = str;
                    }
                }

                {
                    if (prop.TryGetValue(OneBotEvent.DetailType.PropertyName, out var t) && t is string str)
                    {
                        ret.DetailType = NameCacheEntry.State.InAttr;
                        ret.DetailTypeValue = str;
                    }
                }

                {
                    if (prop.TryGetValue(OneBotEvent.SubType.PropertyName, out var t) && t is string str)
                    {
                        ret.SubType = NameCacheEntry.State.InAttr;
                        ret.SubTypeValue = str;
                    }
                }

                attr = attrWalker.FindNext<OneBotExtraPropertiesAttribute>(i => i.GetType().IsAssignableTo(typeof(OneBotExtraPropertiesAttribute)));
            }

            return ret;
        });


        var ret = new Dictionary<string, string?>();

        switch (entry.Type)
        {
            case NameCacheEntry.State.InAttr:
                ret[OneBotEvent.Type.PropertyName] = entry.TypeValue;
                break;
            case NameCacheEntry.State.InCode:
                ret[OneBotEvent.Type.PropertyName] = (e as OneBotEvent.Type)!.Type;
                break;
        }

        switch (entry.DetailType)
        {
            case NameCacheEntry.State.InAttr:
                ret[OneBotEvent.DetailType.PropertyName] = entry.DetailTypeValue;
                break;
            case NameCacheEntry.State.InCode:
                ret[OneBotEvent.DetailType.PropertyName] = (e as OneBotEvent.DetailType)!.DetailType;
                break;
        }

        switch (entry.SubType)
        {
            case NameCacheEntry.State.InAttr:
                ret[OneBotEvent.SubType.PropertyName] = entry.SubTypeValue;
                break;
            case NameCacheEntry.State.InCode:
                ret[OneBotEvent.SubType.PropertyName] = (e as OneBotEvent.SubType)!.SubType;
                break;
        }
        return ret;
    }
}
