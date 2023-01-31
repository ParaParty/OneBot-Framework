namespace OneBot.Core.Model.Channel.Action;

public class GetGuildInfoRequest : IOneBotActionRequestParams
{
    public GetGuildInfoRequest(string guildId)
    {
        GuildId = guildId;
    }

    string GuildId { get; }
}
