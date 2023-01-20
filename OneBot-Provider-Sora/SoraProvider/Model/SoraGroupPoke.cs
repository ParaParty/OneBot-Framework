using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupPoke : GroupPoke, UnderlaySoraEvent<GroupPokeEventArgs>
{
    public SoraGroupPoke(GroupPokeEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.TargetUser.Id.ToString();

    public string OperatorId => WrappedModel.SendUser.Id.ToString();

    public GroupPokeEventArgs WrappedModel { get; }
}