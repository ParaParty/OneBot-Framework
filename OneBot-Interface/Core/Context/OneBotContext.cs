using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Event;

namespace OneBot.Core.Context;

public abstract class OneBotContext
{
    /// <summary>
    /// 底层事件对象
    /// </summary>
    public abstract OneBotEvent UnderlayEvent { get; protected set; }

    /// <summary>
    /// IOC Service Scope
    /// </summary>
    public abstract IServiceScope ServiceScope { get; protected set; }

    /// <summary>
    /// 上下文传递的内容
    /// </summary>
    public abstract IDictionary<object, object?> Items { get; protected set; }
}
