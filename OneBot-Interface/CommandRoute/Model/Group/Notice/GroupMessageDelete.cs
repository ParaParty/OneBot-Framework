namespace OneBot.CommandRoute.Model.Notice;

public interface GroupMessageDelete
{
    string DetailType { get; }
    string SubType { get; }
    string GroupId { get; }
    string MessageId { get; }
    string UserId { get; }
    string Operator { get; }
}
