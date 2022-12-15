namespace OneBot.Core.Model;

public interface UnderlayModel<T>
{
    T WrappedModel { get; }
}
