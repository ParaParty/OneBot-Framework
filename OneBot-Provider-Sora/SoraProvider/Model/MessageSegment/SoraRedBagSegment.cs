using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraRedBagSegment : RedBag, UnderlayModel<RedbagSegment>
{
    public SoraRedBagSegment(RedbagSegment data)
    {
        WrappedModel = data;
    }

    public string Title => WrappedModel.Title;

    public RedbagSegment WrappedModel { get; }

    public static MessageSegmentRef Build(RedbagSegment tData)
    {
        return new MessageSegment<RedBag>(new SoraRedBagSegment(tData));
    }
}