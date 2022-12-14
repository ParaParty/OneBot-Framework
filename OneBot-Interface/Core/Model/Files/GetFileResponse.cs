using System.Collections.Generic;

namespace OneBot.Core.Model.Files;

public interface GetFileResponse
{
    string Name { get; }

    string Url { get; }

    Dictionary<string, string> Headers { get; }

    string Path { get; }

    byte Data { get; }

    string Sha256 { get; }
}
