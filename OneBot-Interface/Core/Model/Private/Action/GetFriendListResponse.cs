using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Private.Action;

public class GetFriendListResponse : IOneBotActionResponseData, IReadOnlyList<GetUserInfoResponse>
{
    public IEnumerator<GetUserInfoResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GetUserInfoResponse this[int index] => throw new System.NotImplementedException();
}
