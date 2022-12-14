using System;

namespace OneBot.CommandRoute.Model.Files;

public interface GetFileFragmentedPreResponse
{
    string Name { get; }
    Int64 TotalSize { get; }
    string Sha256 { get; }
}
