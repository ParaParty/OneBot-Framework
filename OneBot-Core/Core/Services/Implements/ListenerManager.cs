using System.Threading.Tasks;
using OneBot.Core.Event;

namespace OneBot.Core.Services.Implements;

public class ListenerManager: IListenerManager
{
    public ValueTask Dispatch(OneBotEvent e)
    {
        throw new System.NotImplementedException();
    }
}
