using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Group.Notice;

[OneBotTypeProperty("notice", "group_member_decrease")]
public interface GroupMemberDecrease: OneBotEvent, OneBotEvent.SubType
{
    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
