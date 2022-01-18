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
    public override BaseSoraEventArgs SoraEventArgs { get; protected set; }

    /// <summary>
    /// IOC Service Scope
    /// </summary>
    public override IServiceScope ServiceScope { get; protected set; }

    /// <summary>
    /// 上下文传递的内容
    /// </summary>
    public override IDictionary<object, object?> Items { get; protected set; } =
        new ConcurrentDictionary<object, object?>();


    public void SetSoraEventArgs(BaseSoraEventArgs e)
    {
        SoraEventArgs = e;
    }

    public void SoraServiceScope(IServiceScope scope)
    {
        ServiceScope = scope;
    }
}
