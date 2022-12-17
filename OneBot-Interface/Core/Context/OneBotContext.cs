using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace OneBot.Core.Context;

public abstract class OneBotContext 
{
    /// <summary>
    /// 底层事件对象
    /// </summary>
    public abstract object UnderlayEvent { get; protected set; }

    /// <summary>
    /// Sora 基本事件参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public abstract R GetUnderlayEvent<R>();

    /// <summary>
    /// IOC Service Scope
    /// </summary>
    public abstract IServiceScope ServiceScope { get; protected set; }

    /// <summary>
    /// 上下文传递的内容
    /// </summary>
    public abstract IDictionary<object, object?> Items { get; protected set; }
}
