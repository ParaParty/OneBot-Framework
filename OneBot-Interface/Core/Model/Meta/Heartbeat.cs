using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Meta;

[OneBotTypeProperty("meta", "heartbeat")]
public interface Heartbeat : OneBotEvent
{
    long Interval { get; }
}
