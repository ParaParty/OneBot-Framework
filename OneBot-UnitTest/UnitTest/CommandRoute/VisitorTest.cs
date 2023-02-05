using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Configuration;
using OneBot.CommandRoute.Parser;
using OneBot.Core.Model.CommandRoute;
using OneBot.Core.Model.Message;
using OneBot.Core.Util;

namespace OneBot.UnitTest.CommandRoute;

[TestClass]
public class VisitorTest
{
    private RouteInfo TestMsg1
    {
        get
        {
            var b = new Message.Builder();

            b.Add("/kick ");
            b.Add(SimpleMentionSegment.Build("123456"));
            b.Add("--duration=30d --all-group -FTB --operator=\"Any question, contact");
            b.Add(SimpleMentionSegment.Build("654321"));
            b.Add(" \"\"\"\" ");
            b.Add(SimpleMentionSegment.Build("654321"));
            b.Add("\"\"-123 \"\"\"\" \" --description = '\"Let the bass kick\"");
            b.Add(SimpleMentionSegment.Build("654321"));
            b.Add("' -T -NSFW");

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
        var visitor = new CommandVisitor();
        var command = visitor.VisitCommand(tree);

        Assert.AreEqual(2, command.Command.Count);
        Assert.AreEqual(4, command.FullNameFlags.Count);
        Assert.AreEqual(3, command.ShortenFlags.Count);

        Assert.AreEqual(1, command.Command[0].Count);
        Assert.AreEqual("kick", command.Command[0][0].GetText());
        Assert.AreEqual(1, command.Command[1].Count);
        Assert.AreEqual("123456", command.Command[1][0].Get<string>("user_id"));

        Assert.IsTrue(command.FullNameFlags.ContainsKey("duration"));
        Assert.IsTrue(command.FullNameFlags.TryGetValue("duration", out var tryDuration));
        Assert.IsNotNull(tryDuration);
        Assert.AreEqual(1, tryDuration.Count);
        Assert.AreEqual("30d", tryDuration[0].GetText());

        Assert.IsTrue(command.FullNameFlags.ContainsKey("all-group"));
        Assert.IsTrue(command.FullNameFlags.TryGetValue("all-group", out var tryAllGroup));
        Assert.IsNull(tryAllGroup);
        
        Assert.IsTrue(command.FullNameFlags.ContainsKey("operator"));
        Assert.IsTrue(command.FullNameFlags.TryGetValue("operator", out var tryOperator));
        Assert.IsNotNull(tryOperator);
        Assert.AreEqual(5, tryOperator.Count);
        
        Assert.IsFalse(command.FullNameFlags.ContainsKey("1111111111"));

        Assert.IsTrue(command.ShortenFlags.Contains("FTB"));
        Assert.IsTrue(command.ShortenFlags.Contains("T"));
        Assert.IsTrue(command.ShortenFlags.Contains("NSFW"));
        Assert.IsFalse(command.ShortenFlags.Contains("MO"));
    }
}
