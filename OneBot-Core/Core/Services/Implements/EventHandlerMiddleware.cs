using System.Threading.Tasks;
using OneBot.Core.Attributes;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

[OneBotComponentName("OneBot.HandlerManager")]
public class EventHandlerMiddleware : IOneBotMiddleware
{
    private readonly HandlerManager _handlerManager;

    public EventHandlerMiddleware(HandlerManager handlerManager)
    {
        _handlerManager = handlerManager;
    }

    public async ValueTask Invoke(OneBotContext ctx, OneBotEventDelegate next)
    {
        await _handlerManager.Handle(ctx);
        await next(ctx);
    }
}
