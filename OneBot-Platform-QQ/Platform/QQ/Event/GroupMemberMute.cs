using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.notice", "group_member_mute")]
public interface GroupMemberMute : OneBotEvent
{
    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }

    long Duration { get; }
}