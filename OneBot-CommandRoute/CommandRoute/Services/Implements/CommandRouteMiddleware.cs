using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.CommandRoute.Services.Implements;

public class CommandRouteMiddleware: IOneBotMiddleware
{
    public async ValueTask Invoke(OneBotContext ctx, OneBotEventDelegate next)
    {

        await next(ctx);
    }
}
