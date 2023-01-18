using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Private;

[OneBotTypeProperty("message", "private")]
public interface PrivateMessage : OneBotEvent
{
    string MessageId { get; }

    Message.Message Message { get; }

    string AltMessage { get; }

    string GroupId { get; }

    string UserId { get; }
}
