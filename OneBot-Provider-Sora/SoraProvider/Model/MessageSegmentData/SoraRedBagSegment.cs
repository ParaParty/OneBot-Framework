using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraRedBagSegment : RedBag, UnderlayModel<RedbagSegment>
{
    public SoraRedBagSegment(RedbagSegment data) : base(data.Title)
    {
        WrappedModel = data;
    }

    public RedbagSegment WrappedModel { get; }

    public static IMessageSegment Build(RedbagSegment tData)
    {
        return new MessageSegment(new SoraRedBagSegment(tData));
    }
}
