namespace OneBot.Core.Model.Files;

public interface GetFileFragmentedResponse : IOneBotActionResponseData
{
    string? Name { get; }

    long? TotalSize { get; }

    string? Sha256 { get; }

    string? Data { get; }
}
