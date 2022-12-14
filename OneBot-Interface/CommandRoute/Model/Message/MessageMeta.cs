using System.Collections.Generic;

namespace OneBot.CommandRoute.Model;

public interface MessageMeta
{
    string Type { get; }
    Dictionary<string,object> Data { get; }
}
