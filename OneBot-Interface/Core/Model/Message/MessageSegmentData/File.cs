using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("file")]
public interface File: MessageData
{
    string FileId { get; }
}
