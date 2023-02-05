using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraFaceSegment : Face, UnderlayModel<FaceSegment>
{
    public SoraFaceSegment(FaceSegment data) : base(data.Id.ToString())
    {
        WrappedModel = data;
    }

    public FaceSegment WrappedModel { get; }

    public static IMessageSegment Build(FaceSegment tData)
    {
        return new MessageSegment(new SoraFaceSegment(tData));
    }
}
