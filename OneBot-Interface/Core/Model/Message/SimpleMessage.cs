using System.Collections.Generic;

namespace OneBot.Core.Model.Message;

public class SimpleMessage : List<MessageSegmentRef>, Message
{
    public SimpleMessage()
    {

    }

    public SimpleMessage(List<MessageSegmentRef> t) : base(t)
    {

    }
    
    public SimpleMessage(MessageSegmentRef t) : base(new List<MessageSegmentRef>() { t })
    {

    }
}
