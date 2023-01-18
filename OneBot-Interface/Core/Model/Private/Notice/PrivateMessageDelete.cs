using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Private.Notice;

[OneBotTypeProperty("notice", "private_message_delete")]
public interface PrivateMessageDelete: OneBotEvent
{
    string GroupId { get; }

    string MessageId { get; }

    string UserId { get; }
}
