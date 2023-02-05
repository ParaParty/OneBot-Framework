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
            b.Add("--duration=30d --all-group -FTB --operator=\"Any question, contact");
            b.Add(SimpleMentionSegment.Build("654321"));
            b.Add(" \"\"\"\" ");
            b.Add(SimpleMentionSegment.Build("654321"));
            b.Add("\"\"-123 \"\"\"\" \" --description = '\"Let the bass kick\"");
            b.Add(SimpleMentionSegment.Build("654321"));
            b.Add("'");

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
        Assert.AreEqual(5, tree.Flags.Flag.Count);

        Assert.AreEqual(1, tree.CommandSegments.CommandSegment[0].Segment.Count);
        Assert.AreEqual("kick", tree.CommandSegments.CommandSegment[0].Segment[0].GetText());

        Assert.AreEqual(1, tree.CommandSegments.CommandSegment[1].Segment.Count);
        Assert.AreEqual("123456", tree.CommandSegments.CommandSegment[1].Segment[0].Get<string>("user_id"));

        Assert.IsInstanceOfType(tree.Flags.Flag[0], typeof(AstNode.FlagFullname));
        Assert.IsInstanceOfType(tree.Flags.Flag[1], typeof(AstNode.FlagFullname));
        Assert.IsInstanceOfType(tree.Flags.Flag[2], typeof(AstNode.FlagShortenName));
        Assert.IsInstanceOfType(tree.Flags.Flag[3], typeof(AstNode.FlagFullname));
        Assert.IsInstanceOfType(tree.Flags.Flag[4], typeof(AstNode.FlagFullname));

        var duration = (tree.Flags.Flag[0] as AstNode.FlagFullname)!;
        Assert.AreEqual("duration", duration.FlagName);
        Assert.AreEqual(true, duration.HasValue);
        Assert.AreEqual(1, duration.Value!.Count);
        Assert.AreEqual("30d", duration.Value[0].GetText());

        var allGroup = (tree.Flags.Flag[1] as AstNode.FlagFullname)!;
        Assert.AreEqual("all-group", allGroup.FlagName);
        Assert.AreEqual(false, allGroup.HasValue);

        var ftb = (tree.Flags.Flag[2] as AstNode.FlagShortenName)!;
        Assert.AreEqual("FTB", ftb.FlagName);

        var op = (tree.Flags.Flag[3] as AstNode.FlagFullname)!;
        Assert.AreEqual("operator", op.FlagName);
        Assert.AreEqual(true, op.HasValue);
        Assert.AreEqual(5, op.Value!.Count);
        Assert.AreEqual("Any question, contact", op.Value![0].GetText());
        Assert.AreEqual("654321", op.Value[1].Get<string>("user_id"));
        Assert.AreEqual(" \"\" ", op.Value[2].GetText());
        Assert.AreEqual("654321", op.Value[3].Get<string>("user_id"));
        Assert.AreEqual("\"-123 \"\" ", op.Value[4].GetText());

        var description = (tree.Flags.Flag[4] as AstNode.FlagFullname)!;
        Assert.AreEqual("description", description.FlagName);
        Assert.AreEqual(true, description.HasValue);
        Assert.AreEqual(2, description.Value!.Count);
        Assert.AreEqual("\"Let the bass kick\"", description.Value![0].GetText());
        Assert.AreEqual("654321", description.Value[1].Get<string>("user_id"));
    }

    private RouteInfo TestMsg2
    {
        get
        {
            var b = new Message.Builder();

            b.Add("/kick ");
            b.Add(SimpleMentionSegment.Build("123456"));
            b.Add("--duration=30d --all-group -FTB --operator=\"Any question, contact admin\"");
            b.Add("--description = '\"Let the bass kick\"'");

            return new RouteInfo(
                b.ToMessage(),
                EventType.DiscussMessage,
                0,
                1
            );
        }
    }


    [TestMethod]
    public void SimpleCommandTest2()
    {
        var lexer = new CommandLexer(_lexerConfig, TestMsg2);
        var parser = new CommandParser(lexer);
        var tree = parser.ParseCommand();

        Assert.AreEqual(2, tree.CommandSegments.CommandSegment.Count);
        Assert.AreEqual(5, tree.Flags.Flag.Count);

        Assert.AreEqual(1, tree.CommandSegments.CommandSegment[0].Segment.Count);
        Assert.AreEqual("kick", tree.CommandSegments.CommandSegment[0].Segment[0].GetText());

        Assert.AreEqual(1, tree.CommandSegments.CommandSegment[1].Segment.Count);
        Assert.AreEqual("123456", tree.CommandSegments.CommandSegment[1].Segment[0].Get<string>("user_id"));

        Assert.IsInstanceOfType(tree.Flags.Flag[0], typeof(AstNode.FlagFullname));
        Assert.IsInstanceOfType(tree.Flags.Flag[1], typeof(AstNode.FlagFullname));
        Assert.IsInstanceOfType(tree.Flags.Flag[2], typeof(AstNode.FlagShortenName));
        Assert.IsInstanceOfType(tree.Flags.Flag[3], typeof(AstNode.FlagFullname));
        Assert.IsInstanceOfType(tree.Flags.Flag[4], typeof(AstNode.FlagFullname));

        var duration = (tree.Flags.Flag[0] as AstNode.FlagFullname)!;
        Assert.AreEqual("duration", duration.FlagName);
        Assert.AreEqual(true, duration.HasValue);
        Assert.AreEqual(1, duration.Value!.Count);
        Assert.AreEqual("30d", duration.Value[0].GetText());

        var allGroup = (tree.Flags.Flag[1] as AstNode.FlagFullname)!;
        Assert.AreEqual("all-group", allGroup.FlagName);
        Assert.AreEqual(false, allGroup.HasValue);

        var ftb = (tree.Flags.Flag[2] as AstNode.FlagShortenName)!;
        Assert.AreEqual("FTB", ftb.FlagName);

        var op = (tree.Flags.Flag[3] as AstNode.FlagFullname)!;
        Assert.AreEqual("operator", op.FlagName);
        Assert.AreEqual(true, op.HasValue);
        Assert.AreEqual(1, op.Value!.Count);
        Assert.AreEqual("Any question, contact admin", op.Value![0].GetText());

        var description = (tree.Flags.Flag[4] as AstNode.FlagFullname)!;
        Assert.AreEqual("description", description.FlagName);
        Assert.AreEqual(true, description.HasValue);
        Assert.AreEqual(1, description.Value!.Count);
        Assert.AreEqual("\"Let the bass kick\"", description.Value![0].GetText());
    }
}
