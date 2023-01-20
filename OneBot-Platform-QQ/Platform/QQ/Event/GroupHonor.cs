using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.honor", "group")]
public interface GroupHonor : OneBotEvent, OneBotEvent.SubType
{
    string UserId { get; }

    string GroupId { get; }

    string Honor { get; }
}