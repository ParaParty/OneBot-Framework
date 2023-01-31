using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Channel.Action;

public class GetGuildMemberListResponse : IOneBotActionResponseData, IReadOnlyList<GetGuildMemberListResponse>
{
    public IEnumerator<GetGuildMemberListResponse> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public GetGuildMemberListResponse this[int index] => throw new System.NotImplementedException();
}
