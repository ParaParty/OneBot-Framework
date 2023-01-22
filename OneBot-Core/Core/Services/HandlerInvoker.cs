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

    private Func<OneBotContext, ValueTask> GenerateInvokeDelegate(Type handlerType, MethodInfo handlerMethod)
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
        var commitResolvers = resolverList.ToImmutableArray();

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

        DynamicMethod dynamicMethod
            = new DynamicMethod(handlerMethod.Name, typeof(ValueTask), new Type[] { typeof(OneBotContext) });
        var il = dynamicMethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg, 1);
        //non-static -> Callvirt 包装覆写
        il.Emit(OpCodes.Callvirt, typeof(OneBotContext).GetProperty("ServiceScope")!.GetMethod!);
        il.Emit(OpCodes.Callvirt, typeof(OneBotContext).GetProperty("ServiceProvider")!.GetMethod!);
        il.Emit(OpCodes.Stloc, 0);           //表示 sp [Stack的 0号位]
        il.Emit(OpCodes.Stloc, handlerType); //表示 handlerType
        il.Emit(OpCodes.Callvirt, typeof(ServiceProviderServiceExtensions).GetMethod("GetRequiredService")!);
        int count = 2;
        parameters.ToList().ForEach(sp =>
        {
            il.Emit(OpCodes.Ldarg, 0);                // ctx
            il.Emit(OpCodes.Ldloc, 1);                //表示 handlerType
            il.Emit(OpCodes.Stloc, handlerMethod);    //表示 MethodInfo
            il.Emit(OpCodes.Stloc, sp.ParameterType); //表示 ParameterInfo
            il.Emit(OpCodes.Callvirt, typeof(IArgumentResolver).GetMethod("ResolveArgument")!);
            il.Emit(OpCodes.Stloc, count++); //位 2 [0 -> sp , 1 -> handlerType]
        });
        int matchCount = 2;
        while (matchCount != count)
        {
            il.Emit(OpCodes.Ldloc, matchCount);
            matchCount++;
        }
        il.Emit(OpCodes.Call, handlerMethod);
        il.Emit(OpCodes.Ret);
        return (Func<OneBotContext, ValueTask>)dynamicMethod.CreateDelegate(typeof(Func<OneBotContext, ValueTask>));
    }
}
