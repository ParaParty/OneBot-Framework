using OneBot.Core.Attributes;
using OneBot.Core.Model.Message;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("face")]
public interface Face : MessageData
{
    string FaceId { get; }
}