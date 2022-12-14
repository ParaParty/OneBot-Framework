namespace OneBot.Core.Model.Channel.Action;

public interface GetGuildMemberInfoResponse
{
    string UserId { get; }

    string UserName { get; }

    string UserDisplayName { get; }
}
