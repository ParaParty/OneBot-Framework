namespace OneBot.CommandRoute.Configuration;

public interface IOneBotCommandRouteConfiguration
{
    /// <summary>
    /// 英文指令时使用的前缀，请保证数组每一个长度都等于 1。
    /// 若返回空数组，则表示英文指令也不使用前缀。 
    /// </summary>
    string[] CommandPrefix { get; }

    /// <summary>
    /// 指令匹配是否大小写敏感
    /// </summary>
    bool IsCaseSensitive { get; }
}