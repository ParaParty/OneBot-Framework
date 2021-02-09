using OneBot.CommandRoute.Events;

namespace OneBot.CommandRoute.Services
{
    public interface ICommandService
    {
        public EventManager Event { get; set; }

        void RegisterCommand();
    }
}