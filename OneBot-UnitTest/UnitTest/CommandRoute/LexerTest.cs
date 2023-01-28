using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Configuration;
using OneBot.CommandRoute.Parser;
using OneBot.Core.Model;
using OneBot.Core.Model.CommandRoute;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
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
        Assert.AreEqual("kick", t1.Token[0].Get<string>("Text"));

        var t2 = lexer.NextToken();
        Assert.AreEqual(0, t2.Start.Segment);
        Assert.AreEqual(5, t2.Start.Position);
        Assert.AreEqual(1, t2.End.Segment);
        Assert.AreEqual(0, t2.End.Position);
        Assert.AreEqual(TokenType.WhiteSpace, t2.TokenType);
        Assert.AreEqual(1, t2.Token.Count);
        Assert.AreEqual(" ", t2.Token[0].Get<string>("Text"));

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
        Assert.AreEqual("// 123456", t4.Token[0].Get<string>("Text"));
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
                SimpleTextSegment.Build("--duration=123456hr -F"),
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
        Assert.AreEqual("ban", t1.Token[0].Get<string>("Text"));

        var t2 = lexer.NextToken();
        Assert.AreEqual(0, t2.Start.Segment);
        Assert.AreEqual(4, t2.Start.Position);
        Assert.AreEqual(1, t2.End.Segment);
        Assert.AreEqual(0, t2.End.Position);
        Assert.AreEqual(TokenType.WhiteSpace, t2.TokenType);
        Assert.AreEqual(1, t2.Token.Count);
        Assert.AreEqual(" ", t2.Token[0].Get<string>("Text"));

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
        Assert.AreEqual("'-*- Let the bass \"kick, =-='", t4.Token[0].Get<string>("Text"));
        
        var t5 = lexer.NextToken();
        Assert.AreEqual(3, t5.Start.Segment);
        Assert.AreEqual(0, t5.Start.Position);
        Assert.AreEqual(4, t5.End.Segment);
        Assert.AreEqual(0, t5.End.Position);
        Assert.AreEqual(TokenType.Value, t5.TokenType);
        Assert.AreEqual(1, t5.Token.Count);
        Assert.AreEqual("\"Bass bass kick kick bass kick kick\"\"\"", t5.Token[0].Get<string>("Text"));
    }
}

public class SimpleMentionSegment : Mention, UnderlayModel<string>
{
    public SimpleMentionSegment(string userId)
    {
        WrappedModel = userId;
    }

    public string UserId => WrappedModel;

    public string WrappedModel { get; }

    public static MessageSegmentRef Build(char tData)
    {
        return new MessageSegment<Mention>(new SimpleMentionSegment("" + tData));
    }

    public static MessageSegmentRef Build(string tData)
    {
        return new MessageSegment<Mention>(new SimpleMentionSegment(tData));
    }

    public static MessageSegmentRef Build()
    {
        return new MessageSegment<Mention>(new SimpleMentionSegment(String.Empty));
    }
}
