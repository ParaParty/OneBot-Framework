using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraTextSegment : Text, UnderlayModel<TextSegment>
{
    public SoraTextSegment(TextSegment data) : base(data.Content)
    {
        WrappedModel = data;
    }

    public TextSegment WrappedModel { get; }

    public static IMessageSegment Build(TextSegment tData)
    {
        return new MessageSegment(new SoraTextSegment(tData));
    }
}
