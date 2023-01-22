using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class OneBotHostedService : IHostedService
{
    private readonly IOneBotService _service;

    public OneBotHostedService(IOneBotService service)
    {
        _service = service;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _service.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _service.Stop();
        return Task.CompletedTask;
    }
}
