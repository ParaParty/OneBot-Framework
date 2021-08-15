namespace OneBot.CommandRoute.Services.Implements
{
    public class DefaultOneBotCommandRouteConfiguration : IOneBotCommandRouteConfiguration
    {
        /// <summary>
        /// 默认英文指令前缀
        /// </summary>
        public string[] CommandPrefix => new[] {"！", "!", "/"};
    }
}
