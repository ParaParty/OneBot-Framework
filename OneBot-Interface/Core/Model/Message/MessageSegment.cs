namespace OneBot.Core.Model.Message;

/// <summary>
/// 一个消息段
/// </summary>
/// <typeparam name="T"></typeparam>
public class MessageSegment<T> : IMessageSegment<T> where T : MessageData
{
    public MessageSegment(string type, T data)
    {
        Type = type;
        Data = data;
    }

    public string Type { get; }

    public T Data { get; }
}
