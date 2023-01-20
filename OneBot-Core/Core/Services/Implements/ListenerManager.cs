using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Event;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using OneBot.Core.Util;

namespace OneBot.Core.Services.Implements;

public class ListenerManager : IListenerManager
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IHandlerInvokeTool _handlerInvokeTool;

    private readonly IEnumerable<Type> _listenerResolvers;

    private readonly IEnumerable<Type> _handlerList;

    public ListenerManager(IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory, IHandlerInvokeTool handlerInvokeTool)
    {
        _serviceProvider = serviceProvider;
        _scopeFactory = scopeFactory;
        _handlerInvokeTool = handlerInvokeTool;

        _listenerResolvers = ClassScanner.ScanCurrentDomain(s => s.IsAssignableTo(typeof(IListenerResolver)));
        _handlerList = ClassScanner.ScanCurrentDomain(s => s.IsAssignableTo(typeof(IOneBotEventHandler)));
    }

    public async ValueTask Dispatch(OneBotEvent e)
    {
        using var scope = _scopeFactory.CreateScope();
        var ctx = new DefaultOneBotContext(scope, e);

        foreach (var rType in _listenerResolvers)
        {
            foreach (var hType in _handlerList)
            {
                var resolver = _serviceProvider.GetService(rType) as IListenerResolver;
                if (resolver!.Supports(ctx, hType))
                {
                    await _handlerInvokeTool.Invoke(ctx, hType);
                }
            }
        }
    }
}
