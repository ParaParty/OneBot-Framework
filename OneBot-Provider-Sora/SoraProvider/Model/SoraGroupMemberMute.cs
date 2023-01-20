using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupMemberMute : GroupMemberMute, UnderlaySoraEvent<GroupMuteEventArgs>
{
    public SoraGroupMemberMute(GroupMuteEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId
    {
        get
        {
            var t = WrappedModel.User.Id;
            return t > 0 ? t.ToString() : "all";
        }
    }

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string OperatorId => WrappedModel.Operator.Id.ToString();

    public long Duration => WrappedModel.Duration;

    public GroupMuteEventArgs WrappedModel { get; }
}