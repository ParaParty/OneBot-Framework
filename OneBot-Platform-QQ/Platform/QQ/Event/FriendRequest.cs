using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.notice", "friend_request")]
public interface FriendRequest : OneBotEvent
{
    string UserId { get; }

    string Comment { get; }

    string RequestFlag { get; }
}