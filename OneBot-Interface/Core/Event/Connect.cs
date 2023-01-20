using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("meta", "connect")]
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
