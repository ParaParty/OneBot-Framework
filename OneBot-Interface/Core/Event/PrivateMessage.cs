using OneBot.Core.Attributes;
using OneBot.Core.Interface;

namespace OneBot.Core.Event;

[OneBotTypeProperty("message", "private")]
public interface PrivateMessage : OneBotEvent
{
    string MessageId { get; }

    Model.Message.Message Message { get; }

    string AltMessage { get; }

    string UserId { get; }
}
