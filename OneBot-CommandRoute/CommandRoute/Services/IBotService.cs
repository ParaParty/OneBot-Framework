using Sora.Server;

namespace OneBot.CommandRoute.Services
{
    /// <summary>
    /// OneBot 客户端（Sora）
    /// </summary>
    public interface IBotService
    {
        SoraWSServer Server { get; set; }
        void Start();
    }
}
