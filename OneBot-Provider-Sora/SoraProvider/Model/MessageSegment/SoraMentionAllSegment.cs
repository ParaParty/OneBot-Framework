using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraMentionAllSegment : MentionAll, UnderlayModel<AtSegment>
{
    public SoraMentionAllSegment(AtSegment data)
    {
        WrappedModel = data;
    }

    public AtSegment WrappedModel { get; }

    public static MessageSegmentRef Build(AtSegment tData)
    {
        return new MessageSegment<MentionAll>(new SoraMentionAllSegment(tData));
    }
}
