namespace OneBot.Core.Model.Message.MessageSegmentData;

public interface Mention: MessageData
{
    string UserId { get; }
}
