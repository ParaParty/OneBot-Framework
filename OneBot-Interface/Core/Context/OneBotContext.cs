using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Event;

namespace OneBot.Core.Context;

public abstract class OneBotContext
{
    /// <summary>
    /// 底层事件对象
    /// </summary>
    public abstract OneBotEvent Event { get; }

    /// <summary>
    /// IOC Service Scope
    /// </summary>
    public abstract IServiceScope ServiceScope { get; }

    /// <summary>
    /// 上下文传递的内容
    /// </summary>
    public abstract IDictionary<object, object?> Items { get; }
}
