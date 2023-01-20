using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "channel_delete")]
public interface ChannelDelete
{
    string GuildId { get; }

    string ChannelId { get; }

    string OperatorId { get; }
}
