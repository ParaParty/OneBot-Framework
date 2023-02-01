using System;
using System.Threading.Tasks;
using OneBot.Core.Model;
using Sora.Entities.Base;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public delegate ValueTask<OneBotActionResponse> SoraActionHandler(IServiceProvider sp, SoraApi soraApi, OneBotActionRequest req);
