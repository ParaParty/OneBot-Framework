using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraReplySegment : Reply, UnderlayModel<ReplySegment>
{
    public SoraReplySegment(ReplySegment data) : base(messageId: data.Target.ToString(), "")
    {
        WrappedModel = data;
    }

    public ReplySegment WrappedModel { get; }

    public static IMessageSegment Build(ReplySegment tData)
    {
        return new MessageSegment(new SoraReplySegment(tData));
    }
}
