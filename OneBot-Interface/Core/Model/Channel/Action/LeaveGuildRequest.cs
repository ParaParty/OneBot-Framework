namespace OneBot.Core.Model.Channel.Action;

public class LeaveGuildRequest : IOneBotActionRequestParams
{
    public LeaveGuildRequest(string guildId)
    {
        GuildId = guildId;
    }

    string GuildId { get; }
}
