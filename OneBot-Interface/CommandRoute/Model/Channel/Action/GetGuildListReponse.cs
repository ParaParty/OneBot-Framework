using System.Collections.Generic;

namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetGuildListReponse
{
    List<GetGuildInfoResponse> GuildInfos { get; }
}
