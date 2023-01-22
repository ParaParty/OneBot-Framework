using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.FrameworkDemo.Middleware;

public class TestMiddleware: IOneBotMiddleware
{
    private ILogger<TestMiddleware> _logger;

    public TestMiddleware(ILogger<TestMiddleware> logger)
    {
        _logger = logger;
    }
    
    public ValueTask Invoke(OneBotContext oneBotContext, OneBotEventDelegate next)
    {
        using (_logger.BeginScope("机器人开始处理东西了2")) {
            return next(oneBotContext);
        }
    }
}

public class TestMiddleware2 : IOneBotMiddleware
{    private ILogger<TestMiddleware2> _logger;

    public TestMiddleware2(ILogger<TestMiddleware2> logger)
    {
        _logger = logger;
    }
    
    public ValueTask Invoke(OneBotContext oneBotContext, OneBotEventDelegate next)
    {
        using (_logger.BeginScope("机器人开始处理东西了 2")) {
            return next(oneBotContext);
        }
    }
}

public class TestNeedAdminMiddleware : IOneBotMiddleware
{
    public ValueTask Invoke(OneBotContext ctx, OneBotEventDelegate next)
    {
        // 自己判断 ctx 里的动作发起者是否为管理员
        return next(ctx);
    }
}

