using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.Core.Context;
using OneBot.Core.Event;
using OneBot.Core.Interface;
using OneBot.Core.Resolvers.Arguments;
using OneBot.UnitTest.Event;
using OneBot.UnitTest.ServiceProvider;

namespace OneBot.UnitTest.HandlerInvoker;

[TestClass]
public class HandlerInvokerTest
{
    [TestMethod]
    public async Task TestNormal()
    {
        var sp = new MockServiceProvider();
        var scope = new MockServiceScope(sp);

        var ctx = new MockOneBotContext(scope);

        var handler = new TestHandlerType(ctx);
        sp.AddService(handler);

        var resolver = new List<IArgumentResolver>();
        resolver.Add(new OneBotCtxResolver());
        resolver.Add(new Arg2Resolver());
        var invoker = new Core.Services.HandlerInvoker(resolver);

        var handlerType = typeof(TestHandlerType);
        await invoker.Invoke(ctx, handlerType, handlerType.GetMethod("TestHandler")!);
        await invoker.Invoke(ctx, handlerType, handlerType.GetMethod("TestHandler2")!);
        await invoker.Invoke(ctx, (OneBotContext actualCtx) => { Assert.AreEqual(ctx, actualCtx); });
        await invoker.Invoke(ctx, (OneBotContext actualCtx, int arg2) =>
        {
            Assert.AreEqual(ctx, actualCtx);
            Assert.AreEqual(2, arg2);
        });
    }
}

public class Arg2Resolver : IArgumentResolver
{
    public bool SupportsParameter(Type? handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo)
    {
        return parameterInfo.Name == "arg2" && parameterInfo.ParameterType.IsAssignableTo(typeof(int));
    }

    public object? ResolveArgument(OneBotContext ctx, Type? handlerType, MethodInfo methodInfo,
        ParameterInfo parameterInfo)
    {
        return 2;
    }
}

public class MockOneBotContext : OneBotContext
{
    public MockOneBotContext(IServiceScope serviceScope)
    {
        Event = new OneBot.UnitTest.Event.MinimalEvent();
        ServiceScope = serviceScope;
    }

    public override OneBotEvent Event { get; }

    public override IServiceScope ServiceScope { get; }

    public override IDictionary<object, object?> Items { get; } = new ConcurrentDictionary<object, object?>();
}

public class TestHandlerType
{
    private readonly MockOneBotContext _expectedContext;

    public TestHandlerType(MockOneBotContext ctx)
    {
        this._expectedContext = ctx;
    }

    public void TestHandler(OneBotContext ctx)
    {
        Assert.AreEqual(_expectedContext, ctx);
    }

    public void TestHandler2(OneBotContext ctx, int arg2)
    {
        Assert.AreEqual(_expectedContext, ctx);
        Assert.AreEqual(2, arg2);
    }
}
