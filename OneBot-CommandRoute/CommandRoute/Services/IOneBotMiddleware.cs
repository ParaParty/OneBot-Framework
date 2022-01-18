using System.Threading.Tasks;
using OneBot.CommandRoute.Models;

namespace OneBot.CommandRoute.Services;

public delegate ValueTask OneBotRequestDelegate(OneBotContext context);

public interface IOneBotMiddleware
{
    public ValueTask Invoke(OneBotContext oneBotContext, OneBotRequestDelegate step);
}
