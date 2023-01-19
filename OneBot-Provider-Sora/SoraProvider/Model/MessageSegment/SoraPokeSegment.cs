using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraPokeSegment : Poke, UnderlayModel<PokeSegment>
{
    public SoraPokeSegment(PokeSegment data)
    {
        WrappedModel = data;
    }

    public string UserId => WrappedModel.Uid.ToString();

    public PokeSegment WrappedModel { get; }

    public static MessageSegmentRef Build(PokeSegment tData)
    {
        return new MessageSegment<Poke>(new SoraPokeSegment(tData));
    }
}