using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.poke", "group")]
public interface GroupPoke : OneBotEvent
{
    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}