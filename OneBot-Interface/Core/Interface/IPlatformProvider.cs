using System.Threading.Tasks;

namespace OneBot.Core.Interface;

public interface IPlatformProvider
{
    public ValueTask Start();

    public ValueTask Stop();
}
