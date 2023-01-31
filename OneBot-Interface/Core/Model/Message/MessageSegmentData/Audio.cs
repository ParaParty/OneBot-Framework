using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("audio")]
public interface Audio : MessageData
{
    string FileId { get; }
}
