namespace OneBot.Core.Model.Group.Action;

public class GroupLeaveRequest : IOneBotActionRequestParams
{
    public GroupLeaveRequest(string groupId)
    {
        GroupId = groupId;
    }

    string GroupId { get; }
}
