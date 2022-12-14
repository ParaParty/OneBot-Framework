using System.Collections.Generic;

namespace OneBot.CommandRoute.Model.Action;

public interface GroupMemberListResponse
{
    List<GroupMemberInfoResponse> GroupMemberInfos { get; }
}
