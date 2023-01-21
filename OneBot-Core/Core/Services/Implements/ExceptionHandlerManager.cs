using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneBot.Core.Context;

namespace OneBot.Core.Services.Implements;

public class ExceptionHandlerManager : IExceptionHandlerManager
{
    private readonly ILogger<ExceptionHandlerManager> _logger;

    public ExceptionHandlerManager(ILogger<ExceptionHandlerManager> logger)
    {
        _logger = logger;
    }
    public async ValueTask Handle(OneBotContext ctx, Exception ex)
    {
        _logger.LogError(ex, "exception happened, {Msg}", ex.Message);
        throw new NotImplementedException();
    }
}
