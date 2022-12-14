using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public interface GetChannelMemberListRequest
{
    List<GetChannelMemberInfoResponse> ChannelMemberInfos { get; }
}
