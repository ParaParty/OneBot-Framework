using System.Collections.Generic;

namespace OneBot.Core.Model.Files;

public interface UploadFileRequest : IOneBotActionRequestParams
{
    string Type { get; }

    string Name { get; }

    string? Url { get; }

    IReadOnlyDictionary<string, string>? Headers { get; }

    string? Paths { get; }

    string? Data { get; }

    string? Sha256 { get; }
}
