namespace OneBot.Core.Model.Private.Action;

public class GetSelfInfoResponse : IOneBotActionResponseData
{
    public GetSelfInfoResponse(string userId, string userName, string userDisplayname)
    {
        UserId = userId;
        UserName = userName;
        UserDisplayname = userDisplayname;
    }

    string UserId { get; }

    string UserName { get; }

    string UserDisplayname { get; }
}
