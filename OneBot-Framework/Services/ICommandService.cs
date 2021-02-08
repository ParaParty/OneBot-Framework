using QQRobot.CommandRoute.Events;

namespace QQRobot.Services
{
    public interface ICommandService
    {
        public EventManager Event { get; set; }

        void RegisterCommand();
    }
}