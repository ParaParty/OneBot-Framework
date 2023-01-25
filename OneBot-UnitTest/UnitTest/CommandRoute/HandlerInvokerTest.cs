using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Configuration;
using OneBot.Core.Context;

namespace OneBot.UnitTest.CommandRoute;

[TestClass]
public class HandlerInvokerTest
{
    [TestMethod]
    public void CommandRouteConfigFlatTest()
    {
        var root = new CommandRouteNodeBuilder();
        root.Command("shop page [page]", TestFun);
        root.Command("shop [page]", TestFun);
        root.Command("shop buy <name> [amount]", TestFun);
        root.Command("shop sell <name> [amount]", TestFun);
    }


    [TestMethod]
    public void CommandRouteConfigFlatErrorTest()
    {
        var root = new CommandRouteNodeBuilder();
        root.Command("shop page [page]", TestFun);
        root.Command("shop [page]", TestFun);
        root.Command("shop buy <name> [amount]", TestFun);
        root.Command("shop sell <name> [amount]", TestFun);
        Assert.ThrowsException<ArgumentException>(() => root.Command("shop [error]", TestFun));
    }

    [TestMethod]
    public void CommandRouteConfigTiredTest()
    {
        var root = new CommandRouteNodeBuilder();
        root.Group("shop", sr =>
        {
            sr.Command("page [page]", TestFun);
            sr.Command("[page]", TestFun);
            root.Command("shop buy <name> [amount]", TestFun);
            root.Command("shop sell <name> [amount]", TestFun);
        });
    }


    [TestMethod]
    public void CommandRouteConfigTiredErrorTest()
    {
        var root = new CommandRouteNodeBuilder();
        root.Group("shop", sr =>
        {
            sr.Command("page [page]", TestFun);
            sr.Command("[page]", TestFun);
            root.Command("shop buy <name> [amount]", TestFun);
            root.Command("shop sell <name> [amount]", TestFun);
            Assert.ThrowsException<ArgumentException>(() => sr.Command("[error]", TestFun));
        });
    }

    private void TestFun(OneBotContext ctx)
    {
    }
}
