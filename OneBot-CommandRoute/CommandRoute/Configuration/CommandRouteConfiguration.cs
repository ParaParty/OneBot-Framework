using OneBot.CommandRoute.Models;

namespace OneBot.CommandRoute.Configuration;

public class CommandRouteConfiguration
{
    CommandTreeNode CommandTreeRoot { get; init; }

    string[] CommandPrefix { get; init; }
    
    bool CaseSensitive { get; init; }
    
    public CommandRouteConfiguration(CommandTreeNode commandTreeRoot, string[] commandPrefix, bool caseSensitive)
    {
        CommandTreeRoot = commandTreeRoot;
        CommandPrefix = commandPrefix;
        CaseSensitive = caseSensitive;
    }

}
