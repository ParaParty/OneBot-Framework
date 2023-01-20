using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraFriendRequest : FriendRequest, UnderlaySoraEvent<FriendRequestEventArgs>
{
    public SoraFriendRequest(FriendRequestEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string Comment => WrappedModel.Comment;

    public string RequestFlag => WrappedModel.RequestFlag;

    public FriendRequestEventArgs WrappedModel { get; }
}