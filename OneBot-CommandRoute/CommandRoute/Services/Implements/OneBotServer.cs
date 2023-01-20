using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OneBot.CommandRoute.Services.Implements;

public class OneBotServer : IServer
{
    private readonly ICommandService _commandService;

    private readonly IEventService _eventService;

    private readonly IBotService _botService;

    private readonly IServiceProvider _serviceProvider;

    public OneBotServer(ICommandService commandService, IEventService eventService, IBotService botService, IServiceProvider serviceProvider)
    {
        _commandService = commandService;
        _eventService = eventService;
        _botService = botService;
        _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {

    }

    public async Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken) where TContext : notnull
    {
        var hostedServices = _serviceProvider.GetServices<IHostedService>();
        if (hostedServices.Any(s => s.GetType() == typeof(OneBotHostedService)))
        {
            return;
        }

        // 初始化指令系统
        _commandService.RegisterCommand();

        // 初始化事件系统
        _eventService.RegisterEventHandler();

        // 启动 CQHTTP
        await _botService.SoraService.StartService();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public IFeatureCollection Features { get; } = new FeatureCollection();
}
