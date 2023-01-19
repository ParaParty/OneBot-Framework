using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraMusicSegment : Music, UnderlayModel<MusicSegment>
{
    public SoraMusicSegment(MusicSegment data)
    {
        WrappedModel = data;
    }

    public string MusicType => WrappedModel.MusicType.ToString();

    public string MusicId => WrappedModel.MusicId.ToString();

    public MusicSegment WrappedModel { get; }

    public static MessageSegmentRef Build(MusicSegment tData)
    {
        return new MessageSegment<Music>(new SoraMusicSegment(tData));
    }
}