namespace OneBot.Core.Model.Files;

public interface UploadFileFragmentedResponse : IOneBotActionResponseData
{
    string? FileId { get; }
}
