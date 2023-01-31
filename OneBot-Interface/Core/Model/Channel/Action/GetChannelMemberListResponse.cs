using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public class GetChannelMemberListResponse : IOneBotActionResponseData, IReadOnlyList<GetChannelMemberInfoResponse>
{
    public IEnumerator<GetChannelMemberInfoResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GetChannelMemberInfoResponse this[int index] => throw new System.NotImplementedException();
}
