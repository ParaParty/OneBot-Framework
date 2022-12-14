namespace OneBot.Provider.SoraProvider;

using Sora.EventArgs.SoraEvent;

public class GroupMessage : Core.Model.Group.GroupMessage, OneBot.Core.Model.UnderlayModel<GroupMessageEventArgs>
{
    public GroupMessage(GroupMessageEventArgs t)
    {
        UnderlayModel = t;
    }

    public GroupMessageEventArgs UnderlayModel { get; init; }

    public string DetailType => UnderlayModel.SourceType.ToString();

    public string MessageId => UnderlayModel.Message.MessageId.ToString();

    public string Message => UnderlayModel.Message.RawText;

    public string AltMessage => UnderlayModel.Message.RawText;

    public string GroupId => UnderlayModel.SourceGroup.Id.ToString();

    public string UserId => UnderlayModel.Sender.Id.ToString();
}
