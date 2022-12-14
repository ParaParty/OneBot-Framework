using System;

namespace OneBot.Core.Model.Files;

public interface UploadFileFragmentedPreRequest
{
    string Stage { get; }

    string Name { get; }

    Int64 TotalSize { get; }
}
