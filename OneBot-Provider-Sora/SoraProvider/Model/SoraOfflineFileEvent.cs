using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraOfflineFileEvent : PrivateFile, UnderlaySoraEvent<OfflineFileEventArgs>
{
    public SoraOfflineFileEvent(OfflineFileEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string FileName => WrappedModel.OfflineFileInfo.Name;

    public long FileSize => WrappedModel.OfflineFileInfo.Size;

    public string Url => WrappedModel.OfflineFileInfo.Url;

    public OfflineFileEventArgs WrappedModel { get; }
}