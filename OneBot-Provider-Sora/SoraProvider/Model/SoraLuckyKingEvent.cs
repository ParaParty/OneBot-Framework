using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraLuckyKingEvent : LuckyKing, UnderlaySoraEvent<LuckyKingEventArgs>
{
    public SoraLuckyKingEvent(LuckyKingEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string SendUser => WrappedModel.SendUser.Id.ToString();

    public string UserId => WrappedModel.TargetUser.Id.ToString();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public LuckyKingEventArgs WrappedModel { get; }
}