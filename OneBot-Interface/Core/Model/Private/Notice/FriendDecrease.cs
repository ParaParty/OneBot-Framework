using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Private.Notice;

[OneBotTypeProperty("notice", "friend_decrease")]
public interface FriendDecrease: OneBotEvent
{
    string UserId { get; }
}
