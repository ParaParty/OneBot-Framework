using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraVideoSegment : Video, UnderlayModel<VideoSegment>
{
    public SoraVideoSegment(VideoSegment data)
    {
        WrappedModel = data;
    }

    public string FileId => WrappedModel.VideoFile;

    public VideoSegment WrappedModel { get; }

    public static MessageSegmentRef Build(VideoSegment tData)
    {
        return new MessageSegment<Video>(new SoraVideoSegment(tData));
    }
}
