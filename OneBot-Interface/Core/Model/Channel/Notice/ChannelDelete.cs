using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Channel.Notice;

[OneBotTypeProperty("notice", "channel_delete")]
public interface ChannelDelete
{
    string GuildId { get; }

    string ChannelId { get; }

    string OperatorId { get; }
}
