using System.Collections.Generic;

namespace OneBot.Core.Model.Files;

public interface UploadFileRequest
{
    string Type { get; }

    string Name { get; }

    string Url { get; }

    Dictionary<string, string> headers { get; }

    string Paths { get; }

    byte[] Data { get; }

    string Sha256 { get; }
}
