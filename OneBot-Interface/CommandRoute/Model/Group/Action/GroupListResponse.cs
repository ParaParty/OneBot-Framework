using System.Collections.Generic;

namespace OneBot.CommandRoute.Model.Action;

public interface GroupListResponse
{
    List<GroupInfoResponse> GroupInfos { get; }
    
}
