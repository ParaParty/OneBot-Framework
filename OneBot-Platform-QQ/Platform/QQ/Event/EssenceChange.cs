using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.essence", "group")]
public interface EssenceChange : OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Add = "add";

        public const string Delete = "delete";
    }

    string MessageId { get; }

    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
