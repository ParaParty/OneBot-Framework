using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraRecordSegment : Audio, UnderlayModel<RecordSegment>
{
    public SoraRecordSegment(RecordSegment data) : base(data.RecordFile)
    {
        WrappedModel = data;
    }

    public RecordSegment WrappedModel { get; }

    public static IMessageSegment Build(RecordSegment tData)
    {
        return new Core.Model.Message.MessageSegment(new SoraRecordSegment(tData));
    }
}
