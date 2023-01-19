using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("video")]
public interface Video: MessageData
{
    string FileId { get; }
}
