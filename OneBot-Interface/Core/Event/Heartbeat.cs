using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("meta", "heartbeat")]
public interface Heartbeat : OneBotEvent
{
    long Interval { get; }
}
