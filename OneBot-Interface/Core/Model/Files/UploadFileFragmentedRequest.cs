namespace OneBot.Core.Model.Files;

public interface UploadFileFragmentedRequest : IOneBotActionRequestParams
{
    string Stage { get; }

    string? Name { get; }

    long? TotalSize { get; }

    string? FileId { get; }

    long? Offset { get; }

    string? Data { get; }

    string? Sha256 { get; }
}
