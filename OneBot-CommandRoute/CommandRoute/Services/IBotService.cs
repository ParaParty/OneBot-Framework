using Sora.Interfaces;

namespace OneBot.CommandRoute.Services
{
    /// <summary>
    /// OneBot 客户端（Sora）
    /// </summary>
    public interface IBotService
    {
        ISoraService SoraService { get; set; }
        
        void Start();
    }
}