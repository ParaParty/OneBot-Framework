using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.lucky_king", "group")]
public interface LuckyKing : OneBotEvent
{
    string SendUser { get; }

    string UserId { get; }

    string GroupId { get; }
}