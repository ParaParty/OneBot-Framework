using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public class GetGuildListResponse : IOneBotActionResponseData, IReadOnlyList<GetGuildInfoResponse>
{
    public IEnumerator<GetGuildInfoResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GetGuildInfoResponse this[int index] => throw new System.NotImplementedException();
}
