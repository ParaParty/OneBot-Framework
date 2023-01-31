namespace OneBot.Core.Model.Group.Action;

public class GroupListRequest : IOneBotActionRequestParams
{
    public GroupListRequest(string groupId)
    {
        GroupId = groupId;
    }

    string GroupId { get; }
}
