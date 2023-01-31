namespace OneBot.Core.Model.Group.Action;

public class GroupInfoResponse : IOneBotActionResponseData
{
    public GroupInfoResponse(string groupId, string groupName)
    {
        GroupId = groupId;
        GroupName = groupName;
    }

    string GroupId { get; }

    string GroupName { get; }
}
