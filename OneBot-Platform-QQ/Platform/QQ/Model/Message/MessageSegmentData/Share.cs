using OneBot.Core.Attributes;
using OneBot.Core.Model.Message;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("share")]
public interface Share : MessageData
{
    string Url { get; }

    string Title { get; }

    string Content { get; }

    string ImageUrl { get; }
}