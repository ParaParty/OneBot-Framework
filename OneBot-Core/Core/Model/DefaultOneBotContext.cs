﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Event;

namespace OneBot.Core.Model;

public class DefaultOneBotContext : OneBotContext
{
    public DefaultOneBotContext(IServiceScope serviceScope, OneBotEvent @event)
    {
        Event = @event;
        ServiceScope = serviceScope;
    }

    public override OneBotEvent Event { get; }

    public override IServiceScope ServiceScope { get; }

    public override IDictionary<object, object?> Items { get; } = new ConcurrentDictionary<object, object?>();
}
