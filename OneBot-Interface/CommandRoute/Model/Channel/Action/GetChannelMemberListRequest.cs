using System.Collections.Generic;

namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelMemberListRequest
{
    List<GetChannelMemberInfoResponse> ChannelMemberInfos { get; }
}
