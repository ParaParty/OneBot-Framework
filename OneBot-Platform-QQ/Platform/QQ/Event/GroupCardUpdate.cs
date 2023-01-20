using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.notice", "group_card_update")]
public interface GroupCardUpdate : OneBotEvent
{
    string GroupId { get; }

    string UserId { get; }

    string NewCard { get; }
}