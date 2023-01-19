using OneBot.Core.Attributes;
using OneBot.Core.Model.Message;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("poke")]
public interface Poke : MessageData
{
    string UserId { get; }
}