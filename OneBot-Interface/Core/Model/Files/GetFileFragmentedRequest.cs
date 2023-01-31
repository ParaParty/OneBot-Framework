namespace OneBot.Core.Model.Files;

public interface GetFileFragmentedRequest : IOneBotActionRequestParams
{
    string Stage { get; }

    string FileId { get; }

    long? Offset { get; }

    long? Size { get; }
}
