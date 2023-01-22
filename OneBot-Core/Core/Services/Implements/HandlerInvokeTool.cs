using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class HandlerInvokeTool : IHandlerInvokeTool
{
    private readonly ConcurrentDictionary<KeyValuePair<Type, MethodInfo>, Func<OneBotContext, ValueTask>> _invoker =
        new ConcurrentDictionary<KeyValuePair<Type, MethodInfo>, Func<OneBotContext, ValueTask>>();

    private readonly IServiceProvider _serviceProvider;

    private readonly ImmutableArray<IArgumentResolver> _resolvers;

    public HandlerInvokeTool(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _resolvers = _serviceProvider.GetServices<IArgumentResolver>().ToImmutableArray();
    }

    public async ValueTask Invoke(OneBotContext ctx, Type handlerType, MethodInfo handlerMethod)
    {
        var fun = _invoker.GetOrAdd(new KeyValuePair<Type, MethodInfo>(handlerType, handlerMethod),
            _ => GenerateInvokeDelegate(handlerType, handlerMethod)
        );
        await fun(ctx);
    }

    private Func<OneBotContext, ValueTask> GenerateInvokeDelegate(Type handlerType, MethodInfo handlerMethod)
    {
        // 把所有的 parameter 都过一次 resolvers，看看有没有一个 resolve 能够解析这个 parameter
        // 超过 1 个可以解析这个 parameter 的就报错吧
        var parameters = handlerMethod.GetParameters();
        foreach (var parameter in parameters)
        {
            _resolvers[0].SupportsParameter(handlerType, handlerMethod, parameter);
        }
        
        // 如果确定可以解析所有的参数，就执行这个事件
        // handlerType.handlerMethod(把参数全丢进去)
        return ctx =>
        {
            
        }
    }
}
