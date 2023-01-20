using System;

namespace OneBot.Core.Attributes;

/// <summary>
/// 获取全部的参数列表
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class ParsedArgumentsAttribute : Attribute
{
}