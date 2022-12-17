using System.Collections;
using System.Collections.Generic;

namespace OneBot.Core.Model.Message;

public interface Message: IList<MessageSegmentRef>, IList, IReadOnlyList<MessageSegmentRef>  
{
    
}