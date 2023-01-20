using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupCardUpdate : GroupCardUpdate, UnderlaySoraEvent<GroupCardUpdateEventArgs>
{
    public SoraGroupCardUpdate(GroupCardUpdateEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;

    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.User.Id.ToString();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string NewCard => WrappedModel.NewCard;

    public string OldCard => WrappedModel.OldCard;

    public GroupCardUpdateEventArgs WrappedModel { get; }
}