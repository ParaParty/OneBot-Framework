using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Services;

public class HandlerInvoker : IHandlerInvoker
{
    private readonly ConcurrentDictionary<KeyValuePair<Type, MethodInfo>, Func<OneBotContext, ValueTask>> _invoker =
        new ConcurrentDictionary<KeyValuePair<Type, MethodInfo>, Func<OneBotContext, ValueTask>>();

    private readonly ConcurrentDictionary<Delegate, Func<OneBotContext, ValueTask>> _delegateInvoker =
        new ConcurrentDictionary<Delegate, Func<OneBotContext, ValueTask>>();

    private readonly ImmutableArray<IArgumentResolver> _resolvers;

    public HandlerInvoker(IEnumerable<IArgumentResolver> resolvers)
    {
        _resolvers = resolvers.ToImmutableArray();
    }

    public HandlerInvoker(IServiceProvider serviceProvider)
    {
        _resolvers = serviceProvider.GetServices<IArgumentResolver>().ToImmutableArray();
    }

    public async ValueTask Invoke(OneBotContext ctx, Type handlerType, MethodInfo handlerMethod)
    {
        var fun = _invoker.GetOrAdd(new KeyValuePair<Type, MethodInfo>(handlerType, handlerMethod),
            _ => GenerateInvokeDelegate(handlerType, handlerMethod)
        );
        await fun(ctx);
    }

    public async ValueTask Invoke(OneBotContext ctx, Delegate action)
    {
        var fun = _delegateInvoker.GetOrAdd(action, _ => GenerateInvokeDelegate(action));
        await fun(ctx);
    }

    private ImmutableArray<KeyValuePair<ParameterInfo, IArgumentResolver>> ResolveArgumentResolvers(Type? handlerType, MethodInfo handlerMethod)
    {
        var parameters = handlerMethod.GetParameters();

        var resolverList = new KeyValuePair<ParameterInfo, IArgumentResolver>[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var acceptableResolvers = _resolvers
                .ToList()
                .Where(sp => sp.SupportsParameter(handlerType, handlerMethod, parameter)).ToArray();

            if (acceptableResolvers.Length != 1)
            {
                throw new ArgumentException("every parameter must be exactly have a unique resolver");
            }
            resolverList[i] = new KeyValuePair<ParameterInfo, IArgumentResolver>(parameter, acceptableResolvers[0]);
        }
        return resolverList.ToImmutableArray();
    }

    private Func<OneBotContext, ValueTask> GenerateInvokeDelegate(Delegate action)
    {
        var commitResolvers = ResolveArgumentResolvers(action.Target?.GetType(), action.Method);
        var handlerType = action.Target?.GetType();
        var handlerMethod = action.Method;
        return ctx =>
        {
            var t = commitResolvers.Select(s =>
            {
                var parameter = s.Key;
                var resolver = s.Value;
                return resolver.ResolveArgument(ctx, handlerType, handlerMethod, parameter);
            }).ToArray();

            action.DynamicInvoke(t);
            return ValueTask.CompletedTask;
        };
    }

    private Func<OneBotContext, ValueTask> GenerateInvokeDelegate(Type handlerType, MethodInfo handlerMethod)
    {
        var commitResolvers = ResolveArgumentResolvers(handlerType, handlerMethod);
        return ctx =>
        {
            var t = commitResolvers.Select(s =>
            {
                var parameter = s.Key;
                var resolver = s.Value;
                return resolver.ResolveArgument(ctx, handlerType, handlerMethod, parameter);
            }).ToArray();

            var instance = ctx.ServiceScope.ServiceProvider.GetRequiredService(handlerType);

            handlerMethod.Invoke(instance, t);
            return ValueTask.CompletedTask;
        };
    }
}
