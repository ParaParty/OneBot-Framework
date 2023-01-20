using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "channel_create")]
public interface ChannelCreate
{
    string GuildId { get; }

    string ChannelId { get; }

    string OperatorId { get; }
}
