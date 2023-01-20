using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace OneBot.Core.Context;

public class DefaultOneBotContext<T> : OneBotContext
{
    public DefaultOneBotContext(T underlayEvent, IServiceScope serviceScope)
    {
        UnderlayEvent = underlayEvent ?? throw new ArgumentNullException(nameof(underlayEvent));
        ServiceScope = serviceScope;
    }

    public override object UnderlayEvent { get; protected set; }

    public override TR GetUnderlayEvent<TR>()
    {
        if (UnderlayEvent is not TR ret)
        {
            throw new InvalidCastException($"UnderlayEvent is not an instance of {typeof(T).Name}");
        }
        return ret;
    }

    /// <summary>
    /// IOC Service Scope
    /// </summary>
    public override IServiceScope ServiceScope { get; protected set; } = null!;

    /// <summary>
    /// 上下文传递的内容
    /// </summary>
    public override IDictionary<object, object?> Items { get; protected set; } =
        new ConcurrentDictionary<object, object?>();
}
