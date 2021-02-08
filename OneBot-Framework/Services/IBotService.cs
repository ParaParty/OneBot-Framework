using Sora.Server;

namespace QQRobot.Services
{
    public interface IBotService
    {
        //初始化服务器实例
        SoraWSServer Server { get; set; }
        void Start();
    }
}
