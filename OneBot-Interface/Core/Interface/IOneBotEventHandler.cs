using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Event;

namespace OneBot.Core.Interface;

public interface IOneBotEventHandler<T> where T : OneBotEvent
{
    ValueTask Invoke(OneBotContext ctx);
}
