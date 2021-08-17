using System;

namespace OneBot.CommandRoute.Services.Implements
{
    public class DefaultOneBotCommandRouteConfiguration : IOneBotCommandRouteConfiguration
    {
        /// <summary>
        /// 默认英文指令前缀
        /// </summary>
        public string[] CommandPrefix => Array.Empty<string>();

        /// <summary>
        /// 默认指令匹配大小写不敏感
        /// </summary>
        public bool IsCaseSensitive => false;
    }
}
