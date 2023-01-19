using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using Sora.Entities.Segment.DataModel;
using OneBot.Core.Model.Message.MessageSegmentData;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraTextSegment : Text, UnderlayModel<TextSegment>
{
    public SoraTextSegment(TextSegment data)
    {
        WrappedModel = data;
    }

    public string Text => WrappedModel.Content;

    public TextSegment WrappedModel { get; }

    public static MessageSegmentRef Build(TextSegment tData)
    {
        return new MessageSegment<Text>(new SoraTextSegment(tData));
    }
}
