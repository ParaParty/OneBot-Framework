namespace OneBot.Core.Model.Group.Action;

public class GroupMemberInfoResponse : IOneBotActionResponseData
{
    public GroupMemberInfoResponse(string userId, string username, string userDisplayName)
    {
        UserId = userId;
        Username = username;
        UserDisplayName = userDisplayName;
    }

    string UserId { get; }

    string Username { get; }

    string UserDisplayName { get; }
}
