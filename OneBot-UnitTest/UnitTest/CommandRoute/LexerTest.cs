using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Configuration;
using OneBot.CommandRoute.Exceptions;
using OneBot.CommandRoute.Parser;
using OneBot.Core.Model.CommandRoute;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.SimpleMessageSegment;
using OneBot.Core.Util;

namespace OneBot.UnitTest.CommandRoute;

[TestClass]
public class LexerTest
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

        var t1 = lexer.NextToken();
        Assert.AreEqual(0, t1.Start.Segment);
        Assert.AreEqual(1, t1.Start.Position);
        Assert.AreEqual(0, t1.End.Segment);
        Assert.AreEqual(5, t1.End.Position);
        Assert.AreEqual(TokenType.Ident, t1.TokenType);
        Assert.AreEqual(1, t1.Token.Count);
        Assert.AreEqual("kick", t1.Token[0].GetText());

        var t2 = lexer.NextToken();
        Assert.AreEqual(0, t2.Start.Segment);
        Assert.AreEqual(5, t2.Start.Position);
        Assert.AreEqual(1, t2.End.Segment);
        Assert.AreEqual(0, t2.End.Position);
        Assert.AreEqual(TokenType.WhiteSpace, t2.TokenType);
        Assert.AreEqual(1, t2.Token.Count);
        Assert.AreEqual(" ", t2.Token[0].GetText());

        var t3 = lexer.NextToken();
        Assert.AreEqual(1, t3.Start.Segment);
        Assert.AreEqual(0, t3.Start.Position);
        Assert.AreEqual(2, t3.End.Segment);
        Assert.AreEqual(0, t3.End.Position);
        Assert.AreEqual(TokenType.Value, t3.TokenType);
        Assert.AreEqual(1, t3.Token.Count);
        Assert.AreEqual("123456", t3.Token[0].Get<string>("UserId"));

        var t4 = lexer.NextToken();
        Assert.AreEqual(2, t4.Start.Segment);
        Assert.AreEqual(0, t4.Start.Position);
        Assert.AreEqual(3, t4.End.Segment);
        Assert.AreEqual(0, t4.End.Position);
        Assert.AreEqual(TokenType.Comment, t4.TokenType);
        Assert.AreEqual(1, t4.Token.Count);
        Assert.AreEqual("// 123456", t4.Token[0].GetText());

        Assert.ThrowsException<ReachEndException>(() => lexer.NextToken());
    }

    private RouteInfo TestMsg2
    {
        get
        {
            var b = new List<MessageSegmentRef>
            {
                SimpleTextSegment.Build("/ban "),
                SimpleMentionSegment.Build("123456"),
                SimpleTextSegment.Build("'-*- Let the bass \"kick, =-='"),
                SimpleTextSegment.Build("\"Bass bass kick kick bass kick kick\"\"\""),
                SimpleTextSegment.Build("--duration=123456h -FTB"),
            };

            return new RouteInfo(
                new SimpleMessage(b),
                EventType.DiscussMessage,
                0,
                1
            );
        }
    }

    [TestMethod]
    public void ComplexCommandTest()
    {
        var lexer = new CommandLexer(_lexerConfig, TestMsg2);

        var t1 = lexer.NextToken();
        Assert.AreEqual(0, t1.Start.Segment);
        Assert.AreEqual(1, t1.Start.Position);
        Assert.AreEqual(0, t1.End.Segment);
        Assert.AreEqual(4, t1.End.Position);
        Assert.AreEqual(TokenType.Ident, t1.TokenType);
        Assert.AreEqual(1, t1.Token.Count);
        Assert.AreEqual("ban", t1.Token[0].GetText());

        var t2 = lexer.NextToken();
        Assert.AreEqual(0, t2.Start.Segment);
        Assert.AreEqual(4, t2.Start.Position);
        Assert.AreEqual(1, t2.End.Segment);
        Assert.AreEqual(0, t2.End.Position);
        Assert.AreEqual(TokenType.WhiteSpace, t2.TokenType);
        Assert.AreEqual(1, t2.Token.Count);
        Assert.AreEqual(" ", t2.Token[0].GetText());

        var t3 = lexer.NextToken();
        Assert.AreEqual(1, t3.Start.Segment);
        Assert.AreEqual(0, t3.Start.Position);
        Assert.AreEqual(2, t3.End.Segment);
        Assert.AreEqual(0, t3.End.Position);
        Assert.AreEqual(TokenType.Value, t3.TokenType);
        Assert.AreEqual(1, t3.Token.Count);
        Assert.AreEqual("123456", t3.Token[0].Get<string>("UserId"));

        var t4 = lexer.NextToken();
        Assert.AreEqual(2, t4.Start.Segment);
        Assert.AreEqual(0, t4.Start.Position);
        Assert.AreEqual(3, t4.End.Segment);
        Assert.AreEqual(0, t4.End.Position);
        Assert.AreEqual(TokenType.Value, t4.TokenType);
        Assert.AreEqual(1, t4.Token.Count);
        Assert.AreEqual("'-*- Let the bass \"kick, =-='", t4.Token[0].GetText());

        var t5 = lexer.NextToken();
        Assert.AreEqual(3, t5.Start.Segment);
        Assert.AreEqual(0, t5.Start.Position);
        Assert.AreEqual(4, t5.End.Segment);
        Assert.AreEqual(0, t5.End.Position);
        Assert.AreEqual(TokenType.Value, t5.TokenType);
        Assert.AreEqual(1, t5.Token.Count);
        Assert.AreEqual("\"Bass bass kick kick bass kick kick\"\"\"", t5.Token[0].GetText());

        var t6 = lexer.NextToken();
        Assert.AreEqual(4, t6.Start.Segment);
        Assert.AreEqual(0, t6.Start.Position);
        Assert.AreEqual(4, t6.End.Segment);
        Assert.AreEqual(2, t6.End.Position);
        Assert.AreEqual(TokenType.DoubleDash, t6.TokenType);
        Assert.AreEqual(1, t6.Token.Count);
        Assert.AreEqual("--", t6.Token[0].GetText());

        var t7 = lexer.NextToken();
        Assert.AreEqual(4, t7.Start.Segment);
        Assert.AreEqual(2, t7.Start.Position);
        Assert.AreEqual(4, t7.End.Segment);
        Assert.AreEqual(10, t7.End.Position);
        Assert.AreEqual(TokenType.Ident, t7.TokenType);
        Assert.AreEqual(1, t7.Token.Count);
        Assert.AreEqual("duration", t7.Token[0].GetText());

        var t8 = lexer.NextToken();
        Assert.AreEqual(4, t8.Start.Segment);
        Assert.AreEqual(10, t8.Start.Position);
        Assert.AreEqual(4, t8.End.Segment);
        Assert.AreEqual(11, t8.End.Position);
        Assert.AreEqual(TokenType.Equal, t8.TokenType);
        Assert.AreEqual(1, t8.Token.Count);
        Assert.AreEqual("=", t8.Token[0].GetText());

        var t9 = lexer.NextToken();
        Assert.AreEqual(4, t9.Start.Segment);
        Assert.AreEqual(11, t9.Start.Position);
        Assert.AreEqual(4, t9.End.Segment);
        Assert.AreEqual(18, t9.End.Position);
        Assert.AreEqual(TokenType.Value, t9.TokenType);
        Assert.AreEqual(1, t9.Token.Count);
        Assert.AreEqual("123456h", t9.Token[0].GetText());

        var t10 = lexer.NextToken();
        Assert.AreEqual(4, t10.Start.Segment);
        Assert.AreEqual(18, t10.Start.Position);
        Assert.AreEqual(4, t10.End.Segment);
        Assert.AreEqual(19, t10.End.Position);
        Assert.AreEqual(TokenType.WhiteSpace, t10.TokenType);
        Assert.AreEqual(1, t10.Token.Count);
        Assert.AreEqual(" ", t10.Token[0].GetText());

        var t11 = lexer.NextToken();
        Assert.AreEqual(4, t11.Start.Segment);
        Assert.AreEqual(19, t11.Start.Position);
        Assert.AreEqual(4, t11.End.Segment);
        Assert.AreEqual(20, t11.End.Position);
        Assert.AreEqual(TokenType.SingleDash, t11.TokenType);
        Assert.AreEqual(1, t11.Token.Count);
        Assert.AreEqual("-", t11.Token[0].GetText());

        var t12 = lexer.NextToken();
        Assert.AreEqual(4, t12.Start.Segment);
        Assert.AreEqual(20, t12.Start.Position);
        Assert.AreEqual(5, t12.End.Segment);
        Assert.AreEqual(0, t12.End.Position);
        Assert.AreEqual(TokenType.Ident, t12.TokenType);
        Assert.AreEqual(1, t12.Token.Count);
        Assert.AreEqual("FTB", t12.Token[0].GetText());

        Assert.ThrowsException<ReachEndException>(() => lexer.NextToken());
    }

    private RouteInfo TestMsg3
    {
        get
        {
            var b = new Message.Builder();

            for (int i = 0; i < 10; i++)
            {
                b.Add(SimpleMentionSegment.Build("" + (i * 12345 % 54321)));
            }
            b.Add(SimpleTextSegment.Build("\""));
            for (int i = 1; i <= 10; i++)
            {
                b.Add(SimpleMentionSegment.Build("" + (i * 54321 % 12345)));
            }
            b.Add(SimpleTextSegment.Build("\""));

            return new RouteInfo(
                b.ToMessage(),
                EventType.DiscussMessage,
                0,
                1
            );
        }
    }

    [TestMethod]
    public void ComplexCommandTest2()
    {
        var lexer = new CommandLexer(_lexerConfig, TestMsg3);

        for (int i = 0; i < 10; i++)
        {
            var t = lexer.NextToken();
            Assert.AreEqual(i, t.Start.Segment);
            Assert.AreEqual(0, t.Start.Position);
            Assert.AreEqual(i + 1, t.End.Segment);
            Assert.AreEqual(0, t.End.Position);
            Assert.AreEqual(TokenType.Value, t.TokenType);
            Assert.AreEqual(1, t.Token.Count);
            Assert.AreEqual("" + (i * 12345 % 54321), t.Token[0].Get<string>("UserId"));
        }

        var t2 = lexer.NextToken();
        Assert.AreEqual(10, t2.Start.Segment);
        Assert.AreEqual(0, t2.Start.Position);
        Assert.AreEqual(10 + 1 + 10 + 1, t2.End.Segment);
        Assert.AreEqual(0, t2.End.Position);
        Assert.AreEqual(TokenType.Value, t2.TokenType);
        Assert.AreEqual(1 + 10 + 1, t2.Token.Count);

        Assert.AreEqual("\"", t2.Token[0].GetText());
        for (int i = 1; i <= 10; i++)
        {
            Assert.AreEqual("" + (i * 54321 % 12345), t2.Token[i].Get<string>("UserId"));
        }
        Assert.AreEqual("\"", t2.Token[11].GetText());

        Assert.ThrowsException<ReachEndException>(() => lexer.NextToken());
    }
}
