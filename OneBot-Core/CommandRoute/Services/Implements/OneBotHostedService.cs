using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace OneBot.CommandRoute.Services.Implements;

public class OneBotHostedService: IHostedService
{
    private readonly ICommandService _commandService;

    private readonly IEventService _eventService;

    private readonly IBotService _botService;

    public OneBotHostedService(ICommandService commandService, IEventService eventService, IBotService botService)
    {
        _commandService = commandService;
        _eventService = eventService;
        _botService = botService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
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
}
