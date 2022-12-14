using System.Collections.Generic;

namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetGuildMemberListResponse
{
    List<GetGuildMemberListResponse> MemberLists { get; }
}
