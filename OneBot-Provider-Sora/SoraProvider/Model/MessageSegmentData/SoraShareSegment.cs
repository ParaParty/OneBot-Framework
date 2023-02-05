using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraShareSegment : Share, UnderlayModel<ShareSegment>
{
    public SoraShareSegment(ShareSegment data) : base(url: data.Url, title: data.Title, content: data.Content, imageUrl: data.ImageUrl)
    {
        WrappedModel = data;
    }

    public ShareSegment WrappedModel { get; }

    public static IMessageSegment Build(ShareSegment tData)
    {
        return new MessageSegment(new SoraShareSegment(tData));
    }

}
