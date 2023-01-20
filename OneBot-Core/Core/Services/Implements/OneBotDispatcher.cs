using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using OneBot.Core.Attributes;
using OneBot.Core.Event;
using OneBot.Core.Interface;
using OneBot.Core.Util;

namespace OneBot.Core.Services.Implements;

public class OneBotDispatcher : IOneBotEventDispatcher
{
    private readonly IListenerManager _listenerManager;

    internal class NameCacheEntry
    {
        internal enum State
        {
            None,

            InAttr,

            InCode
        }

        internal State Type = State.None;

        internal State DetailType = State.None;

        internal State SubType = State.None;

        internal string? TypeValue;

        internal string? DetailTypeValue;

        internal string? SubTypeValue;
    }

    private readonly ActivitySource _eventActivitySource = new ActivitySource("OneBot.Event", Common.Version);

    private static readonly ConcurrentDictionary<Type, NameCacheEntry> typeCache = new ConcurrentDictionary<Type, NameCacheEntry>();

    public OneBotDispatcher(IListenerManager listenerManager)
    {
        _listenerManager = listenerManager;
    }

    public async ValueTask Dispatch(OneBotEvent e)
    {
        var act = _eventActivitySource.CreateActivity("onebot-event", ActivityKind.Server);
        using (act?.Start())
        {
            if (act != null)
            {
                GenerateTag(act, e);
            }
            await _listenerManager.Dispatch(e);
        }
    }

    private void GenerateTag(Activity act, OneBotEvent e)
    {
        var entry = typeCache.GetOrAdd(e.GetType(), s =>
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


        switch (entry.Type)
        {
            case NameCacheEntry.State.InAttr:
                act.AddTag("onebot.event.type", entry.TypeValue);
                break;
            case NameCacheEntry.State.InCode:
                act.AddTag("onebot.event.type", (e as OneBotEvent.Type)!.Type);
                break;
        }

        switch (entry.DetailType)
        {
            case NameCacheEntry.State.InAttr:
                act.AddTag("onebot.event.detail_type", entry.DetailTypeValue);
                break;
            case NameCacheEntry.State.InCode:
                act.AddTag("onebot.event.detail_type", (e as OneBotEvent.DetailType)!.DetailType);
                break;
        }

        switch (entry.SubType)
        {
            case NameCacheEntry.State.InAttr:
                act.AddTag("onebot.event.sub_type", entry.SubTypeValue);
                break;
            case NameCacheEntry.State.InCode:
                act.AddTag("onebot.event.sub_type", (e as OneBotEvent.SubType)!.SubType);
                break;
        }
    }
}
