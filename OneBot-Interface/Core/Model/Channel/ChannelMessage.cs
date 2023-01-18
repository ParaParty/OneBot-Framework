using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Channel;

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
