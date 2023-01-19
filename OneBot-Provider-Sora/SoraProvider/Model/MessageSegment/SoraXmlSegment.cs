using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraXmlSegment : Xml, UnderlayModel<CodeSegment>
{
    public SoraXmlSegment(CodeSegment data)
    {
        WrappedModel = data;
    }

    public string Content => WrappedModel.Content;

    public CodeSegment WrappedModel { get; }

    public static MessageSegmentRef Build(CodeSegment tData)
    {
        return new MessageSegment<Xml>(new SoraXmlSegment(tData));
    }
}