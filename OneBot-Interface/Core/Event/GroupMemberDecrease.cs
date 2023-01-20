using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "group_member_decrease")]
public interface GroupMemberDecrease: OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Leave = "leave";

        public const string Kick = "kick";
    }
    
    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
