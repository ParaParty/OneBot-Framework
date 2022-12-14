namespace OneBot.Core.Model.Files;

public interface GetFileRequest
{
    string FileId { get; }

    string Type { get; }
}
