using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneBot.Core.Model;
using Sora.Entities.Base;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public class SoraActionManager
{
    private readonly IReadOnlyDictionary<string, SoraActionHandler> _actionProcessor = new Dictionary<string, SoraActionHandler>();

    public async ValueTask<OneBotActionResponse> DoAction(IServiceProvider sp, SoraApi soraApi, OneBotActionRequest req)
    {
        var processor = _actionProcessor[req.Action];
        return await processor(sp, soraApi, req);
    }
}
