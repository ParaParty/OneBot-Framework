using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraRpsSegment : Rps, UnderlayModel<BaseSegment>
{
    public SoraRpsSegment(BaseSegment data)
    {
        WrappedModel = data;
    }
    public BaseSegment WrappedModel { get; }

    public static MessageSegmentRef Build(BaseSegment tData)
    {
        return new MessageSegment<Rps>(new SoraRpsSegment(tData));
    }
}