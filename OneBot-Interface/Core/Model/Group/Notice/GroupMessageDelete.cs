namespace OneBot.Core.Model.Group.Notice;

public interface GroupMessageDelete
{
    string DetailType { get; }

    string SubType { get; }

    string GroupId { get; }

    string MessageId { get; }

    string UserId { get; }

    string Operator { get; }
}
