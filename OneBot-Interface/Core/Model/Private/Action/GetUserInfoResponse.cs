namespace OneBot.Core.Model.Private.Action;

public class GetUserInfoResponse : IOneBotActionResponseData
{
    public GetUserInfoResponse(string userId, string userName, string userDisplayname, string userRemark)
    {
        UserId = userId;
        UserName = userName;
        UserDisplayname = userDisplayname;
        UserRemark = userRemark;
    }

    string UserId { get; }

    string UserName { get; }

    string UserDisplayname { get; }

    string UserRemark { get; }
}
