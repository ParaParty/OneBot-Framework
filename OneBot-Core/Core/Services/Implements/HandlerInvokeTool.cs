using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
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
        IEnumerable<IArgumentResolver>? list = null;
        foreach (var parameter in parameters)
        {
            list = _resolvers
                    .ToList()
                    .Where(sp => sp.SupportsParameter(handlerType, handlerMethod, parameter));
        }
    
        if (list == null || list.Count() != 1)
            throw new ArgumentException("Argument not suitable, for null or not equal with one.");

        // 如果确定可以解析所有的参数，就执行这个事件
        
        DynamicMethod dynamicMethod 
            = new DynamicMethod(handlerMethod.Name,typeof(ValueTask),new Type[]{typeof(OneBotContext)});
        var il = dynamicMethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg,1);
        //non-static -> Callvirt 包装覆写
        il.Emit(OpCodes.Callvirt,typeof(OneBotContext).GetProperty("ServiceScope")!.GetMethod!);
        il.Emit(OpCodes.Callvirt,typeof(OneBotContext).GetProperty("ServiceProvider")!.GetMethod!);
        il.Emit(OpCodes.Stloc,0);//表示 sp [Stack的 0号位]
        il.Emit(OpCodes.Stloc,handlerType); //表示 handlerType
        il.Emit(OpCodes.Callvirt,typeof(ServiceProviderServiceExtensions).GetMethod("GetRequiredService")!);
        int count = 2;
        parameters.ToList().ForEach(sp =>
        {
            il.Emit(OpCodes.Ldarg,0);// ctx
            il.Emit(OpCodes.Ldloc,1); //表示 handlerType
            il.Emit(OpCodes.Stloc,handlerMethod); //表示 MethodInfo
            il.Emit(OpCodes.Stloc,sp.ParameterType); //表示 ParameterInfo
            il.Emit(OpCodes.Callvirt,typeof(IArgumentResolver).GetMethod("ResolveArgument")!);
            il.Emit(OpCodes.Stloc,count++); //位 2 [0 -> sp , 1 -> handlerType]
        });
        int matchCount = 2; //减一位，确保参数数目符合
        while (matchCount != count)
        {
            il.Emit(OpCodes.Ldloc,matchCount);
            matchCount++;
        }
        il.Emit(OpCodes.Call,handlerMethod);
        il.Emit(OpCodes.Ret);
        return (Func<OneBotContext, ValueTask>)dynamicMethod.CreateDelegate(typeof(Func<OneBotContext,ValueTask>));
    }
}
