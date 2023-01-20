using OneBot.Core.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraFriendAdd : FriendIncrease, UnderlaySoraEvent<FriendAddEventArgs>
{

    public SoraFriendAdd(FriendAddEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.NewFriend.Id.ToString();

    public FriendAddEventArgs WrappedModel { get; }
}
