namespace OneBot.Core.Model.Group.Action;

public class GroupMemberListInfoRequest : IOneBotActionRequestParams
{
    public GroupMemberListInfoRequest(string groupId)
    {
        GroupId = groupId;
    }

    string GroupId { get; }
}
