using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Util;

namespace OneBot.Core.Services.Implements;

public class PipelineManager : IPipelineManager
{
    private readonly ActivitySource _middlewareActivitySource = new ActivitySource("OneBot.Middleware", Assembly.GetExecutingAssembly().GetName().Version!.ToString());

    private readonly IHandlerManager _handlerManager;
    
    private readonly IExceptionHandlerManager _exceptionHandlerManager;

    public PipelineManager(IHandlerManager handlerManager, IExceptionHandlerManager exceptionHandlerManager)
    {
        _handlerManager = handlerManager;
        _exceptionHandlerManager = exceptionHandlerManager;
    }

    public async ValueTask Handle(OneBotContext ctx)
    {
        var scope = ctx.ServiceScope;

        OneBotEventDelegate entry = async context =>
        {
            const string middleOperationName = "OneBot.HandlerManager";
            var middlewareActivity = _middlewareActivitySource.CreateActivity(middleOperationName, ActivityKind.Server);
            using (middlewareActivity?.Start())
            {
                await _handlerManager.Handle(context);
            }
        };

        var middleware = scope.ServiceProvider.GetServices<IOneBotMiddleware>().ToImmutableArray();
        var count = middleware.Length;

        for (int i = count - 1; i >= 0; i--)
        {
            var idx = i;
            var realEntry = entry;
            entry = async context =>
            {
                var middleOperationName = middleware[idx].ToDiagnosisName();
                var middlewareActivity = _middlewareActivitySource.CreateActivity(middleOperationName, ActivityKind.Server);
                using (middlewareActivity?.Start())
                {
                    await middleware[idx].Invoke(context, realEntry);
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
