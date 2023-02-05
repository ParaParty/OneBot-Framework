using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegmentData;

public class SoraCardImageSegment : CardImage, UnderlayModel<CardImageSegment>
{
    public SoraCardImageSegment(CardImageSegment data) : base(fileId: data.ImageFile)
    {
        WrappedModel = data;
        Add("min_width", data.MinWidth);
        Add("min_height", data.MinHeight);
        Add("max_width", data.MaxWidth);
        Add("max_height", data.MaxHeight);
        Add("source", data.Source);
        Add("icon", data.Icon);
    }

    public CardImageSegment WrappedModel { get; }

    public static IMessageSegment Build(CardImageSegment tData)
    {
        return new MessageSegment(new SoraCardImageSegment(tData));
    }
}
