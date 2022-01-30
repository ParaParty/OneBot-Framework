namespace OneBot.CommandRoute.Configuration;

public class DefaultOneBotCommandRouteConfiguration : IOneBotCommandRouteConfiguration
{
    private static readonly string[] CommandPrefixConst = { "！", "!", "/" };

    /// <summary>
    /// 默认英文指令前缀
    /// </summary>
    public string[] CommandPrefix => CommandPrefixConst;

    /// <summary>
    /// 默认指令匹配大小写不敏感
    /// </summary>
    public bool IsCaseSensitive => false;
}