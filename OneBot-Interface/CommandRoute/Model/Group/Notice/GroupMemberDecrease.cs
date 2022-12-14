namespace OneBot.CommandRoute.Model.Notice;

public interface GroupMemberDecrease
{
    string DetailType { get; }
    string SubType { get; }
    string GroupId { get; }
    string UserId { get; }
    string OperatorId { get; }
}
