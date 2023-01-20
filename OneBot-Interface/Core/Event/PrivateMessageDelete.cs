using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "private_message_delete")]
public interface PrivateMessageDelete: OneBotEvent
{
    string MessageId { get; }

    string UserId { get; }
}
