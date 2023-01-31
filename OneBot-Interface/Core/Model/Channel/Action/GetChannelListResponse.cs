using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public class GetChannelListResponse : IOneBotActionResponseData, IReadOnlyList<GetChannelInfoResponse>
{
    public IEnumerator<GetChannelInfoResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GetChannelInfoResponse this[int index] => throw new System.NotImplementedException();
}
