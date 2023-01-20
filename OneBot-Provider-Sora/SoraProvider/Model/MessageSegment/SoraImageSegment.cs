using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraImageSegment : Image, UnderlayModel<ImageSegment>
{
    public SoraImageSegment(ImageSegment data)
    {
        WrappedModel = data;
    }

    public string FileId => WrappedModel.ImgFile;

    public ImageSegment WrappedModel { get; }

    public static MessageSegmentRef Build(ImageSegment tData)
    {
        return new MessageSegment<Image>(new SoraImageSegment(tData));
    }
}
