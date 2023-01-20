using OneBot.Core.Attributes;

namespace OneBot.Core.Event;

[OneBotTypeProperty("notice", "group")]
public interface GroupMessageDelete : OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Recall = "recall";

        public const string Delete = "delete";
    }
    
    string GroupId { get; }

    string MessageId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
