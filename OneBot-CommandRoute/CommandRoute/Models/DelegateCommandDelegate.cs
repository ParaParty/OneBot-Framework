using System;

namespace OneBot.CommandRoute.Models;

public class DelegateCommandDelegate : CommandDelegate
{
    private readonly Delegate _action;

    public DelegateCommandDelegate(Delegate action)
    {
        _action = action;
    }
}
