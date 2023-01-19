using OneBot.Core.Model.Group.Notice;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupRecall : GroupMessageDelete, UnderlaySoraEvent<GroupRecallEventArgs>
{
    public SoraGroupRecall(GroupRecallEventArgs t)
    {
        SubType = t.Operator == t.MessageSender ? "recall" : "delete";
        WrappedModel = t;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string SubType { get; init; }

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string MessageId => WrappedModel.MessageId.ToString();

    public string UserId => WrappedModel.MessageSender.Id.ToString();

    public string OperatorId => WrappedModel.Operator.Id.ToString();

    public GroupRecallEventArgs WrappedModel { get; init; }
}
