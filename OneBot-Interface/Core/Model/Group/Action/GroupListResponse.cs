using System.Collections.Generic;

namespace OneBot.Core.Model.Group.Action;

public interface GroupListResponse
{
    List<GroupInfoResponse> GroupInfos { get; }

}
