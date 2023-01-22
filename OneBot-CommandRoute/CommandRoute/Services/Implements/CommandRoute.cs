using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.CommandRoute.Services.Implements;

public class CommandRoute: IOneBotMiddleware
{
    public ValueTask Invoke(OneBotContext ctx, OneBotEventDelegate next)
    {
        throw new System.NotImplementedException();
    }
}
