using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Group.Action;

public class GroupListResponse : IOneBotActionResponseData, IReadOnlyList<GroupInfoResponse>
{
    public IEnumerator<GroupInfoResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GroupInfoResponse this[int index] => throw new System.NotImplementedException();
}
