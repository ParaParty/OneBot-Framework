namespace OneBot.Core.Model.Channel.Action;

public class GetChannelMemberInfoResponse : IOneBotActionResponseData
{
    public GetChannelMemberInfoResponse(string userId, string userName, string userDisplayName)
    {
        UserId = userId;
        UserName = userName;
        UserDisplayName = userDisplayName;
    }

    string UserId { get; }

    string UserName { get; }

    string UserDisplayName { get; }
}
