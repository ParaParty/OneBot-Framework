using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sora.EventArgs.SoraEvent;
namespace OneBot.CommandRoute.Models;

public abstract class OneBotContext
{
    /// <summary>
    /// Sora 基本事件参数
    /// </summary>
    public abstract BaseSoraEventArgs SoraEventArgs { get; protected set; }

    /// <summary>
    /// Sora 基本事件参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public abstract T WrapSoraEventArgs<T>() where T : BaseSoraEventArgs;

    /// <summary>
    /// IOC Service Scope
    /// </summary>
    public abstract IServiceScope ServiceScope { get; protected set; }

    /// <summary>
    /// 上下文传递的内容
    /// </summary>
    public abstract IDictionary<object, object?> Items { get; protected set; }
}
