namespace OneBot.Core.Model.Channel.Action;

public class GetChannelInfoResponse : IOneBotActionResponseData
{
    public GetChannelInfoResponse(string channelId, string channelName)
    {
        ChannelId = channelId;
        ChannelName = channelName;
    }

    string ChannelId { get; }

    string ChannelName { get; }
}
