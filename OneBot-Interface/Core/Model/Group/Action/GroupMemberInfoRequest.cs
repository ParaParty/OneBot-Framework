namespace OneBot.Core.Model.Group.Action;

public class GroupMemberInfoRequest : IOneBotActionRequestParams
{
    public GroupMemberInfoRequest(string groupId, string userId)
    {
        GroupId = groupId;
        UserId = userId;
    }

    string GroupId { get; }

    string UserId { get; }
}
