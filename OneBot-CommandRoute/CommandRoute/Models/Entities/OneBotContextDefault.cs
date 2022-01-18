using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Models.Entities;

public class OneBotContextDefault : OneBotContext
{
    /// <summary>
    /// Sora 基本事件参数
    /// </summary>
    public override BaseSoraEventArgs SoraEventArgs { get; protected set; } = null!;

    /// <summary>
    /// Sora 基本事件参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public override T WrapSoraEventArgs<T>()
    {
        var ret = SoraEventArgs as T;
        if (ret == null)
        {
            throw new ArgumentException($"SoraEventArgs is not an instance of {typeof(T).Name}", $"SoraEventArgs");
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


    internal void SetSoraEventArgs(BaseSoraEventArgs e)
    {
        SoraEventArgs = e;
    }

    internal void SoraServiceScope(IServiceScope scope)
    {
        ServiceScope = scope;
    }

    /// <summary>
    /// Sora Sender
    /// </summary>
    public object Sender { get; private set; } = "";

    internal void SetSoraEventSender(object sender)
    {
        Sender = sender;
    }
}
