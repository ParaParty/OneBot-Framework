using OneBot.Core.Attributes;
using OneBot.Core.Model.Message;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("card_image")]
public interface CardImage : MessageData
{
    string File { get; }
}