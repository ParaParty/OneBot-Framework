namespace OneBot.Core.Model.Files;

public interface GetFileRequest : IOneBotActionRequestParams
{
    string FileId { get; }

    string Type { get; }
}
