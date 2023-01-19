using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Group.Notice;

[OneBotTypeProperty("notice", "group_member_increase")]
public interface GroupMemberIncrease: OneBotEvent, OneBotEvent.SubType
{
    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
