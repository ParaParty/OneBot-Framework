namespace OneBot.Core.Model.Group.Action;

public class GroupInfoRequest : IOneBotActionRequestParams
{
    public GroupInfoRequest(string groupId)
    {
        GroupId = groupId;
    }

    string GroupId { get; }
}
