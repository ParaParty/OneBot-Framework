using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.title_update", "group")]
public interface TitleUpdate : OneBotEvent
{

    string UserId { get; }

    string Title { get; }
}