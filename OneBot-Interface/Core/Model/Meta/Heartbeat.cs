namespace OneBot.Core.Model.Meta;

public interface Heartbeat : OneBotEvent
{
    long Interval { get; }
}
