using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace OneBot.Core.Services;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public static class HandlerDictionary
{
    private static readonly IDictionary<string, List<(object, ParameterInfo)>> HashSet = new ConcurrentDictionary<string, List<(object, ParameterInfo)>>();

    public static void Add(string hashKey, List<(object, ParameterInfo)> list)
    {
        HashSet.Add(hashKey, list);
    }

    public static object GetObject(string key, int index)
        => HashSet[key][index].Item1;

    public static object GetParameterInfo(string key, int index)
        => HashSet[key][index].Item2;
}
