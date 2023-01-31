namespace OneBot.Core.Model.Private.Action;

public class GetUserInfoRequest : IOneBotActionRequestParams
{
    public GetUserInfoRequest(string userId)
    {
        UserId = userId;
    }

    string UserId { get; }
}
