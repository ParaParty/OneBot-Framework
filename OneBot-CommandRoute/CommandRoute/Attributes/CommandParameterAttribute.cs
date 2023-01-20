using System;

namespace OneBot.CommandRoute.Attributes;

/// <summary>
/// 指令参数绑定
/// </summary>
public class CommandParameterAttribute : Attribute
{
    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 指令参数绑定
    /// </summary>
    /// <param name="name">参数名</param>
    public CommandParameterAttribute(string name)
    {
        Name = name;
    }
}