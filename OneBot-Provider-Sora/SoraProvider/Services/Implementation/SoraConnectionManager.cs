using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sora.Entities.Base;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public class SoraConnectionManager
{
    private readonly IDictionary<string, SoraApi> _map = new ConcurrentDictionary<string, SoraApi>();

    public void DeleteClient(Guid connId)
    {
        _map.Remove(connId.ToString());
    }

    public void AddClient(Guid connId, SoraApi soraApi)
    {
        _map[connId.ToString()] = soraApi;
    }

    public SoraApi? GetClient(Guid connId)
    {
        return _map[connId.ToString()];
    }

    public SoraApi? GetClient(string connId)
    {
        return _map[connId];
    }
}
