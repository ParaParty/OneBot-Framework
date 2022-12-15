namespace OneBot.Provider.SoraProvider;

using Sora.EventArgs.SoraEvent;

public class GroupMessage : Core.Model.Group.GroupMessage, OneBot.Core.Model.UnderlayModel<GroupMessageEventArgs>
{
    public GroupMessage(GroupMessageEventArgs t)
    {
        WrappedModel = t;
    }

    public GroupMessageEventArgs WrappedModel { get; init; }

    public string DetailType => WrappedModel.SourceType.ToString();

    public string MessageId => WrappedModel.Message.MessageId.ToString();

    public string Message => WrappedModel.Message.RawText;

    public string AltMessage => WrappedModel.Message.RawText;

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();
}
