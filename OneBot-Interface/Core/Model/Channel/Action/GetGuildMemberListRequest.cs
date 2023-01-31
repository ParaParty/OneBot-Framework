namespace OneBot.Core.Model.Channel.Action;

public class GetGuildMemberListRequest : IOneBotActionRequestParams
{
    public GetGuildMemberListRequest(string guildId)
    {
        GuildId = guildId;
    }

    string GuildId { get; }
}
