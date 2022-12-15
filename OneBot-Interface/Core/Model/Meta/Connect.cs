using System.Collections.Generic;

namespace OneBot.Core.Model.Meta;

public interface Connect : OneBotEvent
{
    ConnVersion Version { get; }

    public interface ConnVersion
    {
        string Impl { get; }

        string Version { get; }

        string OnebotVersion { get; }
    }
}

public interface Heartbeat : OneBotEvent
{
    long Interval { get; }
}

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
