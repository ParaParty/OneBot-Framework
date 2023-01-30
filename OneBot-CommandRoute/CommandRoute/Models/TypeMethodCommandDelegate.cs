using System;
using System.Reflection;

namespace OneBot.CommandRoute.Models;

public class TypeMethodCommandDelegate : CommandDelegate
{
    private readonly Type _type;

    private readonly MethodInfo _method;

    public TypeMethodCommandDelegate(Type type, MethodInfo method)
    {
        _type = type;
        _method = method;
    }
}
