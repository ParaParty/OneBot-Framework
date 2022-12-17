using System.Collections.Generic;

namespace OneBot.Core.Model.Meta;

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
