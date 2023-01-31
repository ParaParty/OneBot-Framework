using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("image")]
public interface Image : MessageData
{
    string FileId { get; }
}
