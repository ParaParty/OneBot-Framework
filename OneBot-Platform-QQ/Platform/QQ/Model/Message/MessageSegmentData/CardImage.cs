using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("card_image")]
public class CardImage : Dictionary<string, object?>
{
    public CardImage(string fileId)
    {
        Add("file_id", fileId);
    }
}
