using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Configuration;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class InitializationManager : IInitializationManager
{
    private readonly OneBotConfiguration _obCfg;

    private readonly IServiceProvider _sp;

    public InitializationManager(OneBotConfiguration obCfg, IServiceProvider sp)
    {
        _obCfg = obCfg;
        _sp = sp;
    }

    public void Initialize()
    {
        var list = new List<object>();

        foreach (var item in _obCfg.PreparationList)
        {
            list.Add(_sp.GetRequiredService(item));
        }

        foreach (var item in _sp.GetServices<INeedInitialization>())
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        foreach (var o in list)
        {
            if (o is INeedInitialization i)
            {
                i.Initialize();
            }
        }
    }
}
