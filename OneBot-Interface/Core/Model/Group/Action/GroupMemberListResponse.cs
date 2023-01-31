using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Group.Action;

public class GroupMemberListResponse : IOneBotActionResponseData, IReadOnlyList<GroupMemberInfoResponse>
{
    public IEnumerator<GroupMemberInfoResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GroupMemberInfoResponse this[int index] => throw new System.NotImplementedException();
}
