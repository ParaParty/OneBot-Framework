using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraPokeSegment : Poke, UnderlayModel<PokeSegment>
{
    public SoraPokeSegment(PokeSegment data) : base(data.Uid.ToString())
    {
        WrappedModel = data;
    }

    public PokeSegment WrappedModel { get; }

    public static IMessageSegment Build(PokeSegment tData)
    {
        return new MessageSegment(new SoraPokeSegment(tData));
    }
}
