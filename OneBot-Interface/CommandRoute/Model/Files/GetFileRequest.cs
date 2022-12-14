namespace OneBot.CommandRoute.Model.Files;

public interface GetFileRequest
{
    string FileId { get; }
    string Type { get; }
}
