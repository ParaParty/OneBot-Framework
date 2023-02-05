using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraTtsSegment : Tts, UnderlayModel<TtsSegment>
{
    public SoraTtsSegment(TtsSegment data) : base(data.Content)
    {
        WrappedModel = data;
    }

    public TtsSegment WrappedModel { get; }

    public static IMessageSegment Build(TtsSegment tData)
    {
        return new MessageSegment(new SoraTtsSegment(tData));
    }
}
