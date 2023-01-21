using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Services;

public interface IHandlerManager
{
    ValueTask Handle(OneBotContext e);
}
