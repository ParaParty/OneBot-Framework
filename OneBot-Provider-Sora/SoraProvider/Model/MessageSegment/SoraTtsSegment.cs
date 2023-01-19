using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraTtsSegment : Tts, UnderlayModel<TtsSegment>
{
    public SoraTtsSegment(TtsSegment data)
    {
        WrappedModel = data;
    }

    public string Content => WrappedModel.Content;

    public TtsSegment WrappedModel { get; }

    public static MessageSegmentRef Build(TtsSegment tData)
    {
        return new MessageSegment<Tts>(new SoraTtsSegment(tData));
    }
}