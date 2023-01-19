using OneBot.Core.Model.Private.Notice;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraFriendRecall : PrivateMessageDelete, UnderlaySoraEvent<FriendRecallEventArgs>
{

    public SoraFriendRecall(FriendRecallEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string MessageId => WrappedModel.MessageId.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public FriendRecallEventArgs WrappedModel { get; }
}
