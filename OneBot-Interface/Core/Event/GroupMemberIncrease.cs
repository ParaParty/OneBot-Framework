using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "group_member_increase")]
public interface GroupMemberIncrease: OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Join = "join";

        public const string Invite = "invite";
    }

    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
