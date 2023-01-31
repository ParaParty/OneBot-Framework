namespace OneBot.Core.Model.Channel.Action;

public class LeaveChannelRequest : IOneBotActionRequestParams
{
    public LeaveChannelRequest(string channelId)
    {
        ChannelId = channelId;
    }

    string ChannelId { get; }
}
