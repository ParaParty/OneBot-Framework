using System;

namespace OneBot.CommandRoute.Model.Files;

public interface UploadFileFragmentedPreRequest
{
    string Stage { get; }
    string Name { get; }
    Int64 TotalSize { get; }
}
