using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Platform.QQ.Model.Message.MessageSegmentData;
using Sora.Entities.Segment.DataModel;

namespace OneBot.Provider.SoraProvider.Model.MessageSegment;

public class SoraCardImageSegment : CardImage, UnderlayModel<CardImageSegment>
{
    public SoraCardImageSegment(CardImageSegment data)
    {
        WrappedModel = data;
    }

    public string File => WrappedModel.ImageFile;

    public CardImageSegment WrappedModel { get; }

    public static MessageSegmentRef Build(CardImageSegment tData)
    {
        return new MessageSegment<CardImage>(new SoraCardImageSegment(tData));
    }
}