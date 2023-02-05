using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraMusicSegment : Music, UnderlayModel<MusicSegment>
{
    public SoraMusicSegment(MusicSegment data) : base(data.MusicType.ToString(), data.MusicId.ToString())
    {
        WrappedModel = data;
    }

    public MusicSegment WrappedModel { get; }

    public static IMessageSegment Build(MusicSegment tData)
    {
        return new MessageSegment(new SoraMusicSegment(tData));
    }
}
