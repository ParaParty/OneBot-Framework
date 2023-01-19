using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraRecordSegment : Audio, UnderlayModel<RecordSegment>
{
    public SoraRecordSegment(RecordSegment data)
    {
        WrappedModel = data;
    }

    public string FileId => WrappedModel.RecordFile;

    public RecordSegment WrappedModel { get; }

    public static MessageSegmentRef Build(RecordSegment tData)
    {
        return new MessageSegment<Audio>(new SoraRecordSegment(tData));
    }
}
