using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraForwardSegment : Forward, UnderlayModel<ForwardSegment>
{
    public SoraForwardSegment(ForwardSegment data)
    {
        WrappedModel = data;
    }

    public string MessageId => WrappedModel.MessageId;

    public ForwardSegment WrappedModel { get; }

    public static MessageSegmentRef Build(ForwardSegment tData)
    {
        return new MessageSegment<Forward>(new SoraForwardSegment(tData));
    }
}