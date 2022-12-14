using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public interface GetGuildMemberListResponse
{
    List<GetGuildMemberListResponse> MemberLists { get; }
}
