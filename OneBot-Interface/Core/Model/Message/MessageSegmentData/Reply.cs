using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("reply")]
public interface Reply : MessageData
{
    string MessageId { get; }

    string UserId { get; }
}
