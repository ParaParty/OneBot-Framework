using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("text")]
public interface Text: MessageData
{
    string Text { get; }
}
