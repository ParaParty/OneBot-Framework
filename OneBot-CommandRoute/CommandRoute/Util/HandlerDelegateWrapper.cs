using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.CommandRoute.Util;

public class HandlerDelegateWrapper<T>
{
    Type HandlerType { get; init; }

    private MethodInfo? _method;

    private readonly Func<T, Delegate> _action;

    public HandlerDelegateWrapper(Func<T, Delegate> action)
    {
        HandlerType = typeof(T);
        _action = action;
    }

    public async Task Invoke(OneBotContext ctx)
    {
        var services = ctx.ServiceScope.ServiceProvider;
        if (_method == null)
        {
            var handlerInstance = (T)services.GetRequiredService(typeof(T));
            var t = _action(handlerInstance);

            _method = t.Method;
        }
        var handlerInvoker = services.GetRequiredService<IHandlerInvoker>();
        await handlerInvoker.Invoke(ctx, HandlerType, _method);
    }
}
