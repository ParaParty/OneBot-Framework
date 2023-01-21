using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneBot.Core.Attributes;
using OneBot.Core.Event;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using OneBot.Core.Util;

namespace OneBot.Core.Services.Implements;

public class EventDispatcher : IEventDispatcher
{
    private readonly IPipelineManager _pipelineManager;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<EventDispatcher> _logger;

    private readonly ActivitySource _eventActivitySource = new ActivitySource("OneBot.Event", Common.Version);

    public EventDispatcher(IPipelineManager pipelineManager, IServiceScopeFactory scopeFactory, ILogger<EventDispatcher> logger)
    {
        _pipelineManager = pipelineManager;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async ValueTask Dispatch(OneBotEvent e)
    {
        using var scope = _scopeFactory.CreateScope();
        var ctx = new DefaultOneBotContext(scope, e);

        var act = _eventActivitySource.CreateActivity("onebot-event", ActivityKind.Server);
        using (act?.Start())
        {
            var tags = OneBotEventUtil.GetType(e);
            if (act != null)
            {
                foreach (var (k, v) in tags)
                {
                    act.AddTag($"onebot.event.{k}", v);
                }
            }

            _logger.LogTrace("Receive an event, {tags}", tags);
            await _pipelineManager.Handle(ctx);
            _logger.LogTrace("Process finished");
        }
    }
}
