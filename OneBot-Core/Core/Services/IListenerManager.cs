using System.Threading.Tasks;
using OneBot.Core.Event;

namespace OneBot.Core.Services;

public interface IListenerManager
{
    ValueTask Dispatch(OneBotEvent e);
}
