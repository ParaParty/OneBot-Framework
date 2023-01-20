using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraFileUpload : GroupFile, UnderlaySoraEvent<FileUploadEventArgs>
{
    public SoraFileUpload(FileUploadEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string FileId => WrappedModel.FileInfo.FileId;

    public string FileName => WrappedModel.FileInfo.Name;

    public long FileSize => WrappedModel.FileInfo.Size;

    public string Busid => WrappedModel.FileInfo.Busid.ToString();

    public FileUploadEventArgs WrappedModel { get; }
}