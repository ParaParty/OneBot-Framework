using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Configuration;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Util;

namespace OneBot.Core.Services;

public class PipelineManager : IPipelineManager
{
    private readonly ActivitySource _middlewareActivitySource = new ActivitySource("OneBot.Middleware", Assembly.GetExecutingAssembly().GetName().Version!.ToString());

    private readonly IHandlerManager _handlerManager;

    private readonly IExceptionHandlerManager _exceptionHandlerManager;

    private readonly OneBotConfiguration _cfg;

    public PipelineManager(IHandlerManager handlerManager, IExceptionHandlerManager exceptionHandlerManager, OneBotConfiguration cfg)
    {
        _handlerManager = handlerManager;
        _exceptionHandlerManager = exceptionHandlerManager;
        _cfg = cfg;
    }

    public async ValueTask Handle(OneBotContext ctx)
    {
        var scope = ctx.ServiceScope;

        OneBotEventDelegate entry = _ => ValueTask.FromResult<object?>(null);

        var middleware = _cfg.Pipeline.Select(s => (scope.ServiceProvider.GetRequiredService(s) as IOneBotMiddleware)!).ToImmutableArray();
        var count = middleware.Length;

        for (int i = count - 1; i >= 0; i--)
        {
            var idx = i;
            var realEntry = entry;
            entry = async context =>
            {
                var middleOperationName = middleware[idx].GetName();
                var middlewareActivity = _middlewareActivitySource.CreateActivity(middleOperationName, ActivityKind.Server);
                using (middlewareActivity?.Start())
                {
                    return await middleware[idx].Invoke(context, realEntry);
                }
            };
        }

        try
        {
            await entry(ctx);
        }
        catch (Exception ex)
        {
            await _exceptionHandlerManager.Handle(ctx, ex);
        }
    }
}
