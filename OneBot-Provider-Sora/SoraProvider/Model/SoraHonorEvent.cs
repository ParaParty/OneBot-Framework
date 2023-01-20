using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraHonorEvent : GroupHonor, UnderlaySoraEvent<HonorEventArgs>
{
    public SoraHonorEvent(HonorEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string SubType => WrappedModel.Honor.ToString();

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.TargetUser.Id.ToString();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string Honor => WrappedModel.Honor.ToString();

    public HonorEventArgs WrappedModel { get; }
}