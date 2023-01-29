using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Configuration;
using OneBot.CommandRoute.Parser;
using OneBot.Core.Model.CommandRoute;
using OneBot.Core.Model.Message;
using OneBot.Core.Util;

namespace OneBot.UnitTest.CommandRoute;

[TestClass]
public class ParserTest
{
    private RouteInfo TestMsg1
    {
        get
        {
            var b = new Message.Builder();

            b.Add("/kick ");
            b.Add(SimpleMentionSegment.Build("123456"));
            b.Add("// 123456");

            return new RouteInfo(
                b.ToMessage(),
                EventType.DiscussMessage,
                0,
                1
            );
        }
    }

    private readonly LexerConfiguration _lexerConfig = new LexerConfiguration();


    [TestMethod]
    public void SimpleCommandTest()
    {
        var lexer = new CommandLexer(_lexerConfig, TestMsg1);
        var parser = new CommandParser(lexer);
        var tree = parser.ParseCommand();
        
        Assert.AreEqual(2, tree.CommandSegments.CommandSegment.Count);
        Assert.AreEqual(0, tree.Flags.Flag.Count);
        
        Assert.AreEqual(1, tree.CommandSegments.CommandSegment[0].CommandToken.Token.Count);
        Assert.AreEqual("kick", tree.CommandSegments.CommandSegment[0].CommandToken.Token[0].Get<string>("Text"));
        
        Assert.AreEqual(1, tree.CommandSegments.CommandSegment[1].CommandToken.Token.Count);
        Assert.AreEqual("123456", tree.CommandSegments.CommandSegment[1].CommandToken.Token[0].Get<string>("UserId"));
    }
}
