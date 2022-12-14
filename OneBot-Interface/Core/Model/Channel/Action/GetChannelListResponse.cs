using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public interface GetChannelListResponse
{
    List<GetChannelInfoResponse> ChannelInfos { get; }
}
