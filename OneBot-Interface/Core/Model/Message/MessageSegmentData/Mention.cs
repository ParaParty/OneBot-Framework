using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("mention")]
public interface Mention: MessageData
{
    string UserId { get; }
}
