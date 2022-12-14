using System.Collections.Generic;

namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelListResponse
{
    List<GetChannelInfoResponse> ChannelInfos { get; }
}
