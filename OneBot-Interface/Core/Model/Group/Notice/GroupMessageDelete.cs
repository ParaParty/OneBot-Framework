using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Group.Notice;

[OneBotTypeProperty("notice", "group")]
public interface GroupMessageDelete : OneBotEvent, OneBotEvent.SubType
{
    string GroupId { get; }

    string MessageId { get; }

    string UserId { get; }

    string Operator { get; }
}
