using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "channel_member_decrease")]
public interface ChannelMemberDecrease: OneBotEvent, OneBotEvent.SubType
{
    string GuildId { get; }

    string ChannelId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
