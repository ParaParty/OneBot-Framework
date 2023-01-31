namespace OneBot.Core.Model.Channel.Action;

public class GetGuildMemberInfoRequest : IOneBotActionRequestParams
{
    public GetGuildMemberInfoRequest(string guildId, string userId)
    {
        GuildId = guildId;
        UserId = userId;
    }

    string GuildId { get; }

    string UserId { get; }
}
