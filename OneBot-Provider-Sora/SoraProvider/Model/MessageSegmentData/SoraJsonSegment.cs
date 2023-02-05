using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraJsonSegment : Xml, UnderlayModel<CodeSegment>
{
    public SoraJsonSegment(CodeSegment data) : base(data.Content)
    {
        WrappedModel = data;
    }

    public CodeSegment WrappedModel { get; }

    public static IMessageSegment Build(CodeSegment tData)
    {
        return new MessageSegment(new SoraJsonSegment(tData));
    }
}
