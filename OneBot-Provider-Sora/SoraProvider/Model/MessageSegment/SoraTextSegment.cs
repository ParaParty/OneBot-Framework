using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using Sora.Entities.Segment.DataModel;
using OneBot.Core.Model.Message.MessageSegmentData;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraTextSegment : Text, UnderlayModel<TextSegment>
{
    public SoraTextSegment(TextSegment data)
    {
        Text = data.Content;
        WrappedModel = data;
    }

    public string Text { get; }

    public TextSegment WrappedModel { get; }

    public static MessageSegmentRef Build(TextSegment tData)
    {
        return new MessageSegment<Text>("text", new SoraTextSegment(tData));
    }
}
