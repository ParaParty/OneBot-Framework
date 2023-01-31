using System.Collections.Generic;

namespace OneBot.Core.Model.Files;

public interface GetFileResponse : IOneBotActionResponseData
{
    string Name { get; }

    string? Url { get; }

    IReadOnlyDictionary<string, string>? Headers { get; }

    string? Path { get; }

    string? Data { get; }

    string? Sha256 { get; }
}
