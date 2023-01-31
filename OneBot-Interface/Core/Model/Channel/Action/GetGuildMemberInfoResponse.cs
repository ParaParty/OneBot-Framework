namespace OneBot.Core.Model.Channel.Action;

public class GetGuildMemberInfoResponse : IOneBotActionResponseData
{
    public GetGuildMemberInfoResponse(string userId, string userName, string userDisplayName)
    {
        UserId = userId;
        UserName = userName;
        UserDisplayName = userDisplayName;
    }

    string UserId { get; }

    string UserName { get; }

    string UserDisplayName { get; }
}
