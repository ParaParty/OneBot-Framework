using Sora.Server;

namespace OneBot.CommandRoute.Services
{
    public interface IBotService
    {
        //初始化服务器实例
        SoraWSServer Server { get; set; }
        void Start();
    }
}
