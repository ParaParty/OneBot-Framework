using OneBot.Core.Attributes;
using OneBot.Core.Model.Message;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("music")]
public interface Music : MessageData
{
    string MusicType { get; }

    string MusicId { get; }
}