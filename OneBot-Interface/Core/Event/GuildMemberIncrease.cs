using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "guild_member_increase")]
public interface GuildMemberIncrease: OneBotEvent, OneBotEvent.SubType
{
    string GuildId { get; }

    string ChannelId { get; }

    string UserId { get; }

    string OperatorId { get; }

}
