using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraImageSegment : Image, UnderlayModel<ImageSegment>
{
    public SoraImageSegment(ImageSegment data) : base(data.ImgFile)
    {
        WrappedModel = data;
    }

    public ImageSegment WrappedModel { get; }

    public static IMessageSegment Build(ImageSegment tData)
    {
        return new MessageSegment(new SoraImageSegment(tData));
    }
}
