using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks.Dataflow;

namespace OneBot.Core.Services;

public static class HandlerDictionary
{
    private static Dictionary<string, List<(object, ParameterInfo)>> HashSet = new();

    public static void Join(string hashKey, List<(object, ParameterInfo)> list)
    {
        HashSet.Add(hashKey,list);
    }

    public static object GetObject(string hashkey, int index) 
        => HashSet[hashkey][index].Item1;
    
    public static object GetParameterInfo(string hashkey, int index) 
        => HashSet[hashkey][index].Item2;
}
