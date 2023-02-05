using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraRpsSegment : Rps, UnderlayModel<BaseSegment>
{
    public SoraRpsSegment(BaseSegment data)
    {
        WrappedModel = data;
    }

    public BaseSegment WrappedModel { get; }

    public static IMessageSegment Build(BaseSegment tData)
    {
        return new MessageSegment(new SoraRpsSegment(tData));
    }
}
