using System.Collections.Generic;

namespace OneBot.Core.Model.Message;

public class DefaultMessage : List<MessageSegmentRef>, Message
{
    public DefaultMessage()
    {
        
    }

    public DefaultMessage(List<MessageSegmentRef> t) : base(t)
    {
        
    }
}
