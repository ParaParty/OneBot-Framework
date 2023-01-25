using System.Threading.Tasks;
using OneBot.Core.Attributes;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Services;

[OneBotComponentName("OneBot.HandlerManager")]
public class EventHandlerMiddleware : IOneBotMiddleware
{
    private readonly IHandlerManager _handlerManager;

    public EventHandlerMiddleware(IHandlerManager handlerManager)
    {
        _handlerManager = handlerManager;
    }

    public async ValueTask<object?> Invoke(OneBotContext ctx, OneBotEventDelegate next)
    {
        await _handlerManager.Handle(ctx);
        return await next(ctx);
    }
}
