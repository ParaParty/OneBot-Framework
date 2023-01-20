using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "friend_increase")]
public interface FriendIncrease: OneBotEvent
{
    string UserId { get; }
}
