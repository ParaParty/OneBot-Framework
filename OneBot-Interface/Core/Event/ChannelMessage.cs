using OneBot.Core.Attributes;
using OneBot.Core.Interface;

namespace OneBot.Core.Event;

[OneBotTypeProperty("message", "channel")]
public interface ChannelMessage : OneBotEvent
{
    string MessageId { get; }

    string Message { get; }

    string AltMessage { get; }

    string GuildId { get; }

    string ChannelId { get; }

    string UserId { get; }
}
