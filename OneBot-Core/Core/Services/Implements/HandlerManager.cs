using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Util;

namespace OneBot.Core.Services.Implements;

public class HandlerManager : IHandlerManager
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IHandlerInvokeTool _handlerInvokeTool;

    private readonly IEnumerable<Type> _listenerResolvers;

    private readonly IEnumerable<Type> _handlerList;

    public HandlerManager(IServiceProvider serviceProvider, IHandlerInvokeTool handlerInvokeTool)
    {
        _serviceProvider = serviceProvider;
        _handlerInvokeTool = handlerInvokeTool;

        _listenerResolvers = ClassScanner.ScanCurrentDomain(s => s.IsAssignableTo(typeof(IHandlerResolver)));
        _handlerList = ClassScanner.ScanCurrentDomain(s => s.IsAssignableTo(typeof(IEventHandler)) && s.GetMethods().Any(c => c.Name == "Invoke"));
    }

    public async ValueTask Handle(OneBotContext ctx)
    {
        foreach (var rType in _listenerResolvers)
        {
            foreach (var hType in _handlerList)
            {
                var resolver = _serviceProvider.GetService(rType) as IHandlerResolver;
                if (resolver!.Supports(ctx, hType))
                {
                    await _handlerInvokeTool.Invoke(ctx, hType);
                }
            }
        }
    }
}
