using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraForwardSegment : Forward, UnderlayModel<ForwardSegment>
{
    public SoraForwardSegment(ForwardSegment data) : base(data.MessageId)
    {
        WrappedModel = data;
    }

    public ForwardSegment WrappedModel { get; }

    public static IMessageSegment Build(ForwardSegment tData)
    {
        return new MessageSegment(new SoraForwardSegment(tData));
    }
}
