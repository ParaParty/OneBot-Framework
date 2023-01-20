using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "channel_member_increase")]
public interface ChannelMemberIncrease: OneBotEvent, OneBotEvent.SubType
{
    string GuildId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
