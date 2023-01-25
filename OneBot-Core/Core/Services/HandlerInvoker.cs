using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
    private readonly ConcurrentDictionary<KeyValuePair<Type, MethodInfo>, Func<OneBotContext, ValueTask<object?>>> _invoker =
        new ConcurrentDictionary<KeyValuePair<Type, MethodInfo>, Func<OneBotContext, ValueTask<object?>>>();

    private readonly ConcurrentDictionary<Delegate, Func<OneBotContext, ValueTask<object?>>> _delegateInvoker =
        new ConcurrentDictionary<Delegate, Func<OneBotContext, ValueTask<object?>>>();

    private readonly ImmutableArray<IArgumentResolver> _resolvers;

    public HandlerInvoker(IEnumerable<IArgumentResolver> resolvers)
    {
        _resolvers = resolvers.ToImmutableArray();
    }

    public HandlerInvoker(IServiceProvider serviceProvider)
    {
        _resolvers = serviceProvider.GetServices<IArgumentResolver>().ToImmutableArray();
    }

    public async ValueTask<object?> Invoke(OneBotContext ctx, Type handlerType, MethodInfo handlerMethod)
    {
        var fun = _invoker.GetOrAdd(new KeyValuePair<Type, MethodInfo>(handlerType, handlerMethod),
            _ => GenerateInvokeDelegate(handlerType, handlerMethod)
        );
        return await fun(ctx);
    }

    public async ValueTask<object?> Invoke(OneBotContext ctx, Delegate action)
    {
        var fun = _delegateInvoker.GetOrAdd(action, _ => GenerateInvokeDelegate(action));
        return await fun(ctx);
    }

    private ImmutableArray<KeyValuePair<ParameterInfo, IArgumentResolver>> ResolveArgumentResolvers(Type? handlerType,
        MethodInfo handlerMethod)
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

    private Func<OneBotContext, ValueTask<object?>> GenerateInvokeDelegate(Delegate action)
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

            return ValueTask.FromResult(action.DynamicInvoke(t));
        };
    }

    private Func<OneBotContext, ValueTask<object?>> GenerateInvokeDelegate(Type handlerType, MethodInfo handlerMethod)
    {
        var parameters = handlerMethod.GetParameters();
        var dic = new List<(object, ParameterInfo)>();
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

            dic.Add((acceptableResolvers[0], parameter));
        }

        var hash = handlerType + "|" + handlerMethod;

        HandlerDictionary.Join(hash, dic);

        DynamicMethod dynamicMethod
            = new DynamicMethod(handlerMethod.Name, null, new[] { typeof(OneBotContext) });
        var il = dynamicMethod.GetILGenerator();

        il.Emit(OpCodes.Ldarg, 0); //OneBotContext -> ctx

        il.Emit(OpCodes.Callvirt, typeof(OneBotContext).GetProperty("ServiceScope")!.GetMethod!);

        LocalBuilder ilServiceProvider = il.DeclareLocal(typeof(ServiceProvider));
        il.Emit(OpCodes.Callvirt, typeof(IServiceScope).GetProperty("ServiceProvider")!.GetMethod!);
        il.Emit(OpCodes.Stloc, ilServiceProvider);

        LocalBuilder ilHanderType = il.DeclareLocal(handlerType); //表示的是HandlerType
        il.Emit(OpCodes.Ldtoken, handlerType);
        il.Emit(OpCodes.Stloc, ilHanderType);
        LocalBuilder ilMethodInfo = il.DeclareLocal(typeof(MethodInfo));
        il.Emit(OpCodes.Ldtoken, handlerMethod);
        il.Emit(OpCodes.Stloc, ilMethodInfo);
        il.Emit(OpCodes.Ldloc, ilServiceProvider);
        il.Emit(OpCodes.Ldloc, ilHanderType);
        il.Emit(OpCodes.Call, typeof(ServiceProviderServiceExtensions).GetMethods()[1]);
        LocalBuilder ilHandler = il.DeclareLocal(typeof(object));
        il.Emit(OpCodes.Stloc, ilHandler);
        il.Emit(OpCodes.Ldloc, ilHandler);
        for (int i = 0; i < dic.Count; i++)
        {
            il.Emit(OpCodes.Ldstr, hash);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Call, typeof(HandlerDictionary).GetMethod("GetObject")!);

            il.Emit(OpCodes.Ldarg, 0);
            il.Emit(OpCodes.Ldloc, ilHanderType);
            il.Emit(OpCodes.Ldloc, ilMethodInfo);

            il.Emit(OpCodes.Ldstr, hash);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Call, typeof(HandlerDictionary).GetMethod("GetParameterInfo")!);

            il.Emit(OpCodes.Callvirt, typeof(IArgumentResolver).GetMethod("ResolveArgument")!);

            if (dic[i].Item2.ParameterType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, dic[i].Item2.ParameterType);
            }
        }

        il.Emit(OpCodes.Callvirt, handlerMethod);
        il.Emit(OpCodes.Ret);

        return ctx => ValueTask.FromResult(dynamicMethod.CreateDelegate(typeof(Action<OneBotContext>)).DynamicInvoke(ctx));
    }
}
