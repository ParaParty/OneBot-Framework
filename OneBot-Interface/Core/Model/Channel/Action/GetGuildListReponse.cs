using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public interface GetGuildListReponse
{
    List<GetGuildInfoResponse> GuildInfos { get; }
}
