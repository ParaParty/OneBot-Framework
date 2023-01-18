using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Private.Notice;

[OneBotTypeProperty("notice", "friend_increase")]
public interface FriendIncrease: OneBotEvent
{
    string UserId { get; }
}
