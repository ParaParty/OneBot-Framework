namespace OneBot.Core.Model.Message.MessageSegmentData;

public interface Voice : MessageData
{
    string FileId { get; }
}
