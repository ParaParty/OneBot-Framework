using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraMentionSegment : Mention, UnderlayModel<AtSegment>
{
    public SoraMentionSegment(AtSegment data)
    {
        WrappedModel = data;
    }

    public string UserId => WrappedModel.Target;

    public AtSegment WrappedModel { get; }

    public static MessageSegmentRef Build(AtSegment tData)
    {
        return new MessageSegment<Mention>(new SoraMentionSegment(tData));
    }
}
