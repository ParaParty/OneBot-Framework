using System;

namespace OneBot.Core.Model.Files;

public interface UploadFileFragmentedDurRequest
{
    string Stage { get; }

    string FileId { get; }

    Int64 Offset { get; }

    byte[] Data { get; }
}
