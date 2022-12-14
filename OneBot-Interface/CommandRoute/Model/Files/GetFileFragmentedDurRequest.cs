using System;

namespace OneBot.CommandRoute.Model.Files;

public interface GetFileFragmentedDurRequest
{
    string Stage { get; }
    string FileId { get; }
    Int64 Offset { get; }
    Int64 Size { get; }
}
