using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraMentionAllSegment : MentionAll, UnderlayModel<AtSegment>
{
    public SoraMentionAllSegment(AtSegment data)
    {
        WrappedModel = data;
    }

    public AtSegment WrappedModel { get; }

    public static IMessageSegment Build(AtSegment tData)
    {
        return new MessageSegment(new SoraMentionAllSegment(tData));
    }
}
