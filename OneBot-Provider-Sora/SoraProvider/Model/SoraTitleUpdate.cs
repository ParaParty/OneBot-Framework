using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraTitleUpdate : TitleUpdate, UnderlaySoraEvent<TitleUpdateEventArgs>
{
    public SoraTitleUpdate(TitleUpdateEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.TargetUser.Id.ToString();

    public string Title => WrappedModel.NewTitle;

    public TitleUpdateEventArgs WrappedModel { get; }
}