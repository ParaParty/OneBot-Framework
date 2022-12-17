namespace OneBot.Core.Model.Message.MessageSegmentData;

public interface Reply: MessageData
{
    string MessageId { get; }

    string UserId { get; }
}
