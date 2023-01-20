using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("meta", "status_update")]
public interface StatusUpdate : OneBotEvent
{
    StatusModel Status { get; }

    public interface StatusModel
    {
        bool Good { get; }

        List<Bot> Bots { get; }
    }

    public interface Bot
    {
        object Self { get; }

        bool Online { get; }
    }
}
