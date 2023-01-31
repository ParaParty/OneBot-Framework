namespace OneBot.Core.Model.Group.Action;

public class SetGroupNameRequest : IOneBotActionRequestParams
{
    public SetGroupNameRequest(string groupId, string groupName)
    {
        GroupId = groupId;
        GroupName = groupName;
    }

    string GroupId { get; }

    string GroupName { get; }
}
