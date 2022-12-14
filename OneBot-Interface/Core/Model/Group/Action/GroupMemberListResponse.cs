using System.Collections.Generic;

namespace OneBot.Core.Model.Group.Action;

public interface GroupMemberListResponse
{
    List<GroupMemberInfoResponse> GroupMemberInfos { get; }
}
