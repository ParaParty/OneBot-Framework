using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraFaceSegment : Face, UnderlayModel<FaceSegment>
{
    public SoraFaceSegment(FaceSegment data)
    {
        WrappedModel = data;
    }

    public string FaceId => WrappedModel.Id.ToString();

    public FaceSegment WrappedModel { get; }

    public static MessageSegmentRef Build(FaceSegment tData)
    {
        return new MessageSegment<Face>(new SoraFaceSegment(tData));
    }
}