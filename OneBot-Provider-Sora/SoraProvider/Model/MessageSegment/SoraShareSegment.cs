using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraShareSegment : Share, UnderlayModel<ShareSegment>
{
    public SoraShareSegment(ShareSegment data)
    {
        WrappedModel = data;
    }

    public string Url => WrappedModel.Url;

    public string Title => WrappedModel.Title;

    public string Content => WrappedModel.Content;

    public string ImageUrl => WrappedModel.ImageUrl;

    public ShareSegment WrappedModel { get; }

    public static MessageSegmentRef Build(ShareSegment tData)
    {
        return new MessageSegment<Share>(new SoraShareSegment(tData));
    }

}