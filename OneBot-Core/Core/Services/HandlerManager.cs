using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OneBot.Core.Attributes;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Util;

namespace OneBot.Core.Services;

public class HandlerManager : IHandlerManager
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IHandlerInvoker _handlerInvoker;

    private readonly IEnumerable<Type> _listenerResolvers;

    private readonly IEnumerable<KeyValuePair<Type, MethodInfo>> _handlerList;

    public HandlerManager(IServiceProvider serviceProvider, IHandlerInvoker handlerInvoker)
    {
        _serviceProvider = serviceProvider;
        _handlerInvoker = handlerInvoker;

        _listenerResolvers = ClassScanner.ScanCurrentDomain(s => s.IsAssignableTo(typeof(IHandlerResolver)));

        var typeList = ClassScanner.ScanCurrentDomain(s => s.IsAssignableTo(typeof(IEventHandler)));
        var handlerList = new List<KeyValuePair<Type, MethodInfo>>();
        foreach (var type in typeList)
        {
            foreach (var method in type.GetMethods())
            {
                if (method.GetCustomAttributes(false).Any(a => a.GetType().IsAssignableTo(typeof(OneBotEventHandlerAttribute))))
                {
                    handlerList.Add(new KeyValuePair<Type, MethodInfo>(type, method));
                }
            }
        }
        _handlerList = handlerList;
    }

    public async ValueTask<object?> Handle(OneBotContext ctx)
    {
        foreach (var rType in _listenerResolvers)
        {
            foreach (var hType in _handlerList)
            {
                var resolver = _serviceProvider.GetService(rType) as IHandlerResolver;
                if (resolver!.Supports(ctx, hType.Key, hType.Value))
                {
                    await _handlerInvoker.Invoke(ctx, hType.Key, hType.Value);
                }
            }
        }
        return ValueTask.FromResult<object?>(null);
    }
}
