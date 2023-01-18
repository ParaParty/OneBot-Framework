using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Channel.Notice;

[OneBotTypeProperty("notice", "channel_create")]
public interface ChannelCreate
{
    string GuildId { get; }

    string ChannelId { get; }

    string OperatorId { get; }
}
