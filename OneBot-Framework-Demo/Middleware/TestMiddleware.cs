using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneBot.CommandRoute.Models;
using OneBot.CommandRoute.Services;

namespace OneBot.FrameworkDemo.Middleware;

public class TestMiddleware: IOneBotMiddleware
{
    private ILogger<TestMiddleware> _logger;

    public TestMiddleware(ILogger<TestMiddleware> logger)
    {
        _logger = logger;
    }
    
    public ValueTask Invoke(OneBotContext oneBotContext, OneBotRequestDelegate next)
    {
        using (_logger.BeginScope("机器人开始处理东西了")) {
            return next(oneBotContext);
        }
    }
}
