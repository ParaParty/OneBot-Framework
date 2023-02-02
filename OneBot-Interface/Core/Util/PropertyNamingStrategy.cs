using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace OneBot.Core.Util;

public abstract class PropertyNamingStrategy
{
    public static readonly ImmutableList<string> DefaultLockedWords = new string[] { }.ToImmutableList();

    public static PropertyNamingStrategy UpperCamel { get; } = new UpperCamelNamingStrategy(DefaultLockedWords);

    public static PropertyNamingStrategy LowerCamel { get; } = new LowerCamelNamingStrategy(DefaultLockedWords);

    public static PropertyNamingStrategy UpperSnake { get; } = new UpperSnakeNamingStrategy(DefaultLockedWords);

    public static PropertyNamingStrategy LowerSnake { get; } = new LowerSnakeNamingStrategy(DefaultLockedWords);

    public static PropertyNamingStrategy UpperKebab { get; } = new UpperKebabNamingStrategy(DefaultLockedWords);

    public static PropertyNamingStrategy LowerKebab { get; } = new LowerKebabNamingStrategy(DefaultLockedWords);

    public abstract string Convert(string src);

    public abstract class SimplePropertyNamingStrategy : PropertyNamingStrategy
    {
        protected readonly string[] LockedWords;

        public string[] GetLockedWords()
        {
            return LockedWords.ToArray();
        }

        protected SimplePropertyNamingStrategy(IEnumerable<string> lockedWords)
        {
            LockedWords = lockedWords.ToArray();
        }
    }

    public class UpperCamelNamingStrategy : SimplePropertyNamingStrategy
    {
        public override string Convert(string src)
            => StringUtils.ToCamelCase(src, StringUtils.CamelCaseType.Upper, LockedWords);

        public UpperCamelNamingStrategy(IEnumerable<string> lockedWords) : base(lockedWords)
        {
        }
    }

    public class LowerCamelNamingStrategy : SimplePropertyNamingStrategy
    {
        public override string Convert(string src)
            => StringUtils.ToCamelCase(src, StringUtils.CamelCaseType.Lower, LockedWords);

        public LowerCamelNamingStrategy(IEnumerable<string> lockedWords) : base(lockedWords)
        {
        }
    }

    public class UpperSnakeNamingStrategy : SimplePropertyNamingStrategy
    {
        public override string Convert(string src)
            => StringUtils.ToSeparatedCase(src, StringUtils.Underline, StringUtils.CaseType.Upper, LockedWords);

        public UpperSnakeNamingStrategy(IEnumerable<string> lockedWords) : base(lockedWords)
        {
        }
    }

    public class LowerSnakeNamingStrategy : SimplePropertyNamingStrategy
    {
        public override string Convert(string src)
            => StringUtils.ToSeparatedCase(src, StringUtils.Underline, StringUtils.CaseType.Lower, LockedWords);

        public LowerSnakeNamingStrategy(IEnumerable<string> lockedWords) : base(lockedWords)
        {
        }
    }

    public class UpperKebabNamingStrategy : SimplePropertyNamingStrategy
    {
        public override string Convert(string src)
            => StringUtils.ToSeparatedCase(src, StringUtils.Dash, StringUtils.CaseType.Upper, LockedWords);

        public UpperKebabNamingStrategy(IEnumerable<string> lockedWords) : base(lockedWords)
        {
        }
    }

    public class LowerKebabNamingStrategy : SimplePropertyNamingStrategy
    {
        public override string Convert(string src)
            => StringUtils.ToSeparatedCase(src, StringUtils.Dash, StringUtils.CaseType.Lower, LockedWords);

        public LowerKebabNamingStrategy(IEnumerable<string> lockedWords) : base(lockedWords)
        {
        }
    }
}
