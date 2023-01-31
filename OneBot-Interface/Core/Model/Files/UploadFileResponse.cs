namespace OneBot.Core.Model.Files;

public interface UploadFileResponse : IOneBotActionResponseData
{
    string FileId { get; }
}
