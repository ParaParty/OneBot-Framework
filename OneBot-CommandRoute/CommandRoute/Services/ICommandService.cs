using OneBot.CommandRoute.Events;

namespace OneBot.CommandRoute.Services
{
    /// <summary>
    /// 指令路由服务
    /// </summary>
    public interface ICommandService
    {
        public EventManager Event { get; set; }

        void RegisterCommand();
    }
}