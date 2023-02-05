using System.Collections.Generic;

namespace OneBot.Core.Model.Message;

/// <summary>
/// 一个消息段
/// </summary>
public interface IMessageSegment
{
    public string Type { get; }

    public IReadOnlyDictionary<string, object?> Data { get; }
}
