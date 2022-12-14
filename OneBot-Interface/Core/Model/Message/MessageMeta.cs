using System.Collections.Generic;

namespace OneBot.Core.Model.Message;

public interface MessageMeta
{
    string Type { get; }

    Dictionary<string, object> Data { get; }
}
