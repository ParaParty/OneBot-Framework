using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraVideoSegment : Video, UnderlayModel<VideoSegment>
{
    public SoraVideoSegment(VideoSegment data) : base(data.VideoFile)
    {
        WrappedModel = data;
    }

    public VideoSegment WrappedModel { get; }

    public static IMessageSegment Build(VideoSegment tData)
    {
        return new MessageSegment(new SoraVideoSegment(tData));
    }
}
