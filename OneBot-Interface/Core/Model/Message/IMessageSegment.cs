namespace OneBot.Core.Model.Message;

/// <summary>
/// 一个消息，可以使用普通实现 <see cref="MessageSegment{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMessageSegment<T> : MessageSegmentRef where T : MessageData
{
    string Type { get; }

    T Data { get; }
}
