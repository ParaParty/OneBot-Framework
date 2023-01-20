using System.Collections.Generic;
using System.Linq;

namespace OneBot.CommandRoute.Lexer;

/// <summary>
/// 指令解析器
/// </summary>
public class CommandLexer
{
    /// <summary>
    /// 空白字符
    /// </summary>
    private const string BlankCharacter = "\r\n\t ";

    /// <summary>
    /// 引号
    /// </summary>
    private const string QuoteCharacter = "\"\'";

    /// <summary>
    /// 源信息
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public IList<SoraSegment> SourceCommand { get; }

    /// <summary>
    /// 已经解析了的参数信息
    /// </summary>
    public List<object> ParsedArguments { get; private init; } = new List<object>();

    /// <summary>
    /// 当前扫描到 <code>SourceCommand</code> 的哪一个位置。
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public int ScanObjectPointer { get; private set; }

    /// <summary>
    /// 如果当前扫描到的 <code>SourceCommand</code> 是 <code>CQFunction.Text</code> 的话当前字符串扫描到哪一个位置。
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public int ScanStringPointer { get; private set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="s">被解析的字符串</param>
    public CommandLexer(IList<SoraSegment> s)
    {
        SourceCommand = s.Where(c =>
                c.MessageType != SegmentType.Text
                || ((TextSegment)c.Data).Content.Length > 0)
            .ToList();
    }

    /// <summary>
    /// 检查被解析的字符串是否有效
    /// </summary>
    /// <returns>真: 有效/假: 无效</returns>
    public bool IsValid()
    {
        // 如果已经开始扫描了那必然合法
        if (ScanObjectPointer != 0 || ScanStringPointer != 0) return true;

        var scanObjectPointer = 0;
        var scanStringPointer = 0;

        // 空消息不合法
        if (SourceCommand.Count == 0) return false;

        // 如果第一个消息段为回复消息
        if (SourceCommand[0].MessageType == SegmentType.Reply)
        {
            // 如果消息段长度只有1则不合法
            if (SourceCommand.Count < 2) return false;
            scanObjectPointer = 1;
        }

        // ReSharper disable once JoinDeclarationAndInitializer
        string s;
        var flag = false;

        // 兼容 回复+At+(空格+正文) 的奇怪设计
        if (scanObjectPointer == 1 && SourceCommand[scanObjectPointer].MessageType == SegmentType.At)
        {
            if (SourceCommand[scanObjectPointer].MessageType == SegmentType.At)
            {
                // 如果消息段长度只有2则不合法
                if (SourceCommand.Count < 3) return false;
                scanObjectPointer = 2;
            }

            flag = true;
        }

        // 如果扫描起始消息段不为文本则不合法
        if (SourceCommand[scanObjectPointer].MessageType != SegmentType.Text) return false;
        s = ((TextSegment)SourceCommand[scanObjectPointer].Data).Content;

        // 兼容 回复+空格+At+正文 的奇怪设计
        if (scanObjectPointer == 1 && string.IsNullOrWhiteSpace(s))
        {
            if (SourceCommand.Count < 4) return false;
            if (SourceCommand[2].MessageType != SegmentType.At) return false;
            if (SourceCommand[3].MessageType != SegmentType.Text) return false;

            scanObjectPointer = 3;
            s = ((TextSegment)SourceCommand[scanObjectPointer].Data).Content;

            flag = true;
        }

        if (flag)
        {
            // 舍弃空白段
            while (SourceCommand[scanObjectPointer].MessageType == SegmentType.Text)
            {
                if (!string.IsNullOrWhiteSpace(((TextSegment)SourceCommand[scanObjectPointer].Data).Content)) break;
                scanObjectPointer++;
                if (scanObjectPointer == SourceCommand.Count) return false;
            }

            // 如果舍弃空白段后不是文本段则不合法
            if (SourceCommand[scanObjectPointer].MessageType != SegmentType.Text) return false;
            s = ((TextSegment)(SourceCommand[scanObjectPointer].Data)).Content;

            // 舍弃开头空白字符
            while (scanStringPointer < s.Length && BlankCharacter.Contains(s[scanStringPointer]))
            {
                scanStringPointer++;
            }
        }

        // 普通发言支持
        if (scanObjectPointer == 0)
        {
            // 空消息不合法
            if (s.Length == 0) return false;

            // 空白符号开头不合法
            if (BlankCharacter.Contains((s[0]))) return false;
        }

        ScanObjectPointer = scanObjectPointer;
        ScanStringPointer = scanStringPointer;
        return true;
    }

    /// <summary>
    /// 尝试获得下一个字符串
    /// </summary>
    private bool TryGetNextStringObject(ref string str)
    {
        // 如果往后这个字符串已经扫完了
        if (ScanStringPointer >= str.Length)
        {
            // 那就往后取一个对象
            ScanObjectPointer++;
            ScanStringPointer = 0;

            if (ScanObjectPointer < SourceCommand.Count)
            {
                // 往后取一个
                var nextSegment = SourceCommand[ScanObjectPointer];
                if (nextSegment.MessageType == SegmentType.Text)
                {
                    // 接着往后识别
                    str = ((TextSegment)(nextSegment.Data)).Content;
                }
                else
                {
                    // 如果后面那个不是字符串，那么就结束
                    return true;
                }
            }
            else
            {
                // 如果往后取一个对象已经没得再取了，那就直接返回
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 获取下一个 Token
    /// </summary>
    /// <returns>下一个 Token</returns>
    private object GetNext()
    {
        // 检查是否扫描到末尾
        if (ScanObjectPointer >= SourceCommand.Count)
        {
            throw new ParseToTheEndException();
        }

        // 当前扫描到的消息段
        var s = SourceCommand[ScanObjectPointer];

        // 如果当前扫描到的消息段不是文本消息段则返回当前消息段
        if (s.MessageType != SegmentType.Text)
        {
            ScanObjectPointer++;
            ScanStringPointer = 0;
            return s;
        }

        // 获取当前消息段的文本
        var str = ((TextSegment)(s.Data)).Content;
        var token = "";

        // 检查当前元素是否扫描完成
        if (ScanStringPointer >= str.Length)
        {
            ScanObjectPointer++;
            ScanStringPointer = 0;
            if (ScanObjectPointer >= SourceCommand.Count)
            {
                throw new ParseToTheEndException();
            }

            // 获取下一个消息段
            var ret = SourceCommand[ScanObjectPointer];

            // 如果下一个消息段不是文本则返回
            if (ret.MessageType != SegmentType.Text)
            {
                ScanObjectPointer++;
                return ret;
            }

            // 如果下一个消息段是文本则继续扫描
            s = ret;
            str = ((TextSegment)(s.Data)).Content;
        }

        // 文本参数
        if (BlankCharacter.Contains(str[ScanStringPointer]))
        {
            // 空白元素
            while (BlankCharacter.Contains(str[ScanStringPointer]))
            {
                // 往后拼接
                token += str[ScanStringPointer];
                ScanStringPointer++;
                if (TryGetNextStringObject(ref str))
                {
                    return token;
                }
            }
        }
        else
        {
            // 非空白元素
            if (QuoteCharacter.Contains(str[ScanStringPointer]))
            {
                MessageBody multiElementsToken = new MessageBody();
                var terminate = str[ScanStringPointer];

                ScanStringPointer++;
                TryGetNextStringObject(ref str);

                while (true)
                {
                    // 往后拼接
                    if (SourceCommand[ScanObjectPointer].MessageType == SegmentType.Text)
                    {
                        if (str[ScanStringPointer] == terminate)
                        {
                            // 如果遇到了字符串起始符号
                            ScanStringPointer++;
                            if (ScanStringPointer < str.Length)
                            {
                                // 如果当前字符串还没处理完
                                if (str[ScanStringPointer] == terminate)
                                {
                                    // 如果下一个符号还是起始符号
                                    token += terminate;
                                    ScanStringPointer++;
                                    TryGetNextStringObject(ref str);
                                }
                                else
                                {
                                    // 如果不是起始符号
                                    multiElementsToken += token;
                                    // ReSharper disable once RedundantAssignment
                                    token = "";
                                    break;
                                }
                            }
                            else
                            {
                                // 如果当前字符串处理完了
                                ScanStringPointer = 0;
                                ScanObjectPointer++;

                                if (ScanObjectPointer >= SourceCommand.Count)
                                {
                                    // 如果整个消息已经处理完了
                                    multiElementsToken += token;
                                    // ReSharper disable once RedundantAssignment
                                    token = "";
                                    break;
                                }
                                else
                                {
                                    if (SourceCommand[ScanObjectPointer].MessageType != SegmentType.Text)
                                    {
                                        // 如果下一个消息段不是文本，意味着当前扫描到的地方就是一个完整的文本消息
                                        multiElementsToken += token;
                                        // ReSharper disable once RedundantAssignment
                                        token = "";
                                        break;
                                    }
                                    else
                                    {
                                        // 如果下一个消息段是文本，意味着我们需要看一眼
                                        str = ((TextSegment)SourceCommand[ScanObjectPointer].Data).Content;
                                        if (str[ScanStringPointer] == terminate)
                                        {
                                            // 如果下一个符号还是起始符号
                                            token += terminate;
                                            ScanStringPointer++;
                                            TryGetNextStringObject(ref str);
                                        }
                                        else
                                        {
                                            // 如果不是起始符号
                                            multiElementsToken += token;
                                            // ReSharper disable once RedundantAssignment
                                            token = "";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            token += str[ScanStringPointer];
                            ScanStringPointer++;
                            if (TryGetNextStringObject(ref str))
                            {
                                multiElementsToken += token;
                                token = "";
                            }
                        }
                    }
                    else
                    {
                        multiElementsToken += SourceCommand[ScanObjectPointer];
                        ScanObjectPointer++;
                        if (ScanObjectPointer < SourceCommand.Count)
                        {
                            str = ((TextSegment)SourceCommand[ScanObjectPointer].Data).Content;
                        }
                    }

                    if (ScanObjectPointer >= SourceCommand.Count)
                    {
                        break;
                    }
                }

                return multiElementsToken;
            }
            else
            {
                while (!BlankCharacter.Contains(str[ScanStringPointer]))
                {
                    // 往后拼接
                    token += str[ScanStringPointer];
                    ScanStringPointer++;
                    if (TryGetNextStringObject(ref str))
                    {
                        return token;
                    }
                }
            }
        }
        return token;
    }

    /// <summary>
    /// 获取下一个不是空白的 Token
    /// </summary>
    /// <returns>下一个 Token</returns>
    public object GetNextNotBlank()
    {
        var ret = GetNext();
        while (ret is string s && string.IsNullOrWhiteSpace(s))
        {
            ret = GetNext();
        }
        ParsedArguments.Add(ret);
        return ret;
    }

    /// <summary>
    /// 获得现在已经得到的 Token 列表
    /// </summary>
    /// <returns>已获得的 Token 列表</returns>
    public List<object> GetNowParsedToken()
    {
        List<object> ret = new List<object>(ParsedArguments);

        if (ScanStringPointer != 0)
        {
            ret.Add(((TextSegment)(SourceCommand[ScanObjectPointer].Data)).Content.Substring(ScanStringPointer));
        }
        else
        {
            for (int i = ScanObjectPointer; i < SourceCommand.Count; i++)
            {
                ret.Add(SourceCommand[i]);
            }
        }

        return ret;
    }

    /// <summary>
    /// 克隆 Lexer 的当前状态
    /// </summary>
    /// <returns>新的 Lexer</returns>
    public CommandLexer Clone()
    {
        CommandLexer ret = new CommandLexer(SourceCommand)
        {
            ParsedArguments = new List<object>(ParsedArguments),
            ScanObjectPointer = ScanObjectPointer,
            ScanStringPointer = ScanStringPointer
        };
        return ret;
    }
}