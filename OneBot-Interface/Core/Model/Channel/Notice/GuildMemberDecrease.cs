using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Channel.Notice;

[OneBotTypeProperty("notice", "guild_member_decrease")]
public interface GuildMemberDecrease:OneBotEvent, OneBotEvent.SubType
{
    string GuildId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
