namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelMemberInfoResponse
{
    string UserId { get; }
    string UserName { get; }
    string UserDisplayName { get; }
}
