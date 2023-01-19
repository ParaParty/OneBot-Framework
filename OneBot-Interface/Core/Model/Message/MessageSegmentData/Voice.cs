using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("voice")]
public interface Voice : MessageData
{
    string FileId { get; }
}
