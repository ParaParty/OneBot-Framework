namespace OneBot.Core.Model.Files;

public interface UploadFileFragmentedCloRequest
{
    string Stage { get; }

    string FileId { get; }

    string Sha256 { get; }
}
