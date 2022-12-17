using System.Collections.Generic;
using System.Linq;
using OneBot.Core.Model.Group;
using OneBot.Core.Model.Message;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupMessageEventArgs : GroupMessage, UnderlaySoraEvent<GroupMessageEventArgs>
{
    public SoraGroupMessageEventArgs(GroupMessageEventArgs t)
    {
        WrappedModel = t;
        Message = t.Message.ConvertToOneBotMessage();
    }

    public GroupMessageEventArgs WrappedModel { get; init; }

    public string DetailType => WrappedModel.SourceType.ToString();

    public string MessageId => WrappedModel.Message.MessageId.ToString();

    public Message Message { get; }

    public string AltMessage => WrappedModel.Message.RawText;

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();
}