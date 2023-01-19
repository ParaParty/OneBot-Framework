using OneBot.Core.Attributes;
using OneBot.Core.Interface;

namespace OneBot.Core.Model.Private;

[OneBotTypeProperty("message", "private")]
public interface PrivateMessage : OneBotEvent, IMessageEvent
{
    string MessageId { get; }

    Message.Message Message { get; }

    string AltMessage { get; }

    string UserId { get; }
}
