using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "friend_decrease")]
public interface FriendDecrease: OneBotEvent
{
    string UserId { get; }
}
