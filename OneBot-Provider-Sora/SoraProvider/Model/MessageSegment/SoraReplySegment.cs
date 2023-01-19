using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraReplySegment : Reply, UnderlayModel<ReplySegment>
{
    public SoraReplySegment(ReplySegment data)
    {
        WrappedModel = data;
    }

    public ReplySegment WrappedModel { get; }

    public static MessageSegmentRef Build(ReplySegment tData)
    {
        return new MessageSegment<Reply>(new SoraReplySegment(tData));
    }

    public string MessageId => WrappedModel.Target.ToString();

    public string UserId => "";
}
