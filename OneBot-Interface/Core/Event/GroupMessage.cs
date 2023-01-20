using OneBot.Core.Attributes;
using OneBot.Core.Interface;

namespace OneBot.Core.Event;

[OneBotTypeProperty("message", "group")]
public interface GroupMessage : OneBotEvent, IMessageEvent
{
    string MessageId { get; }

    Model.Message.Message Message { get; }

    string AltMessage { get; }

    string GroupId { get; }

    string UserId { get; }
}
