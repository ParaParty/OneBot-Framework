﻿using System;

namespace OneBot.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class OneBotComponentNameAttribute : Attribute
{
    public OneBotComponentNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}