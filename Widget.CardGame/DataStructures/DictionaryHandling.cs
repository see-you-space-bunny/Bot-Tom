using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Widget.CardGame.Commands;
using Widget.CardGame.Interfaces;

namespace Widget.CardGame.DataStructures;

public static class DictionaryHandling
{
    public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,TKey fromKey,TKey toKey)
    {
        TValue value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }

    internal static Dictionary<TKey, TValue> ToOutgoing<TKey, TValue>(this IDictionary<TKey, TValue> dic) where TValue : ICommandIO<TKey>
    {
        if (typeof(TKey) is null)
            throw new ArgumentNullException(nameof(dic),$"Dictionary key TKey cannot be null.");

        Dictionary<TKey, TValue> outgoingDict = [];
        
        foreach(TKey key in dic.Keys)
            outgoingDict.Add(dic[key].AlternateKey,dic[key]);

        return outgoingDict;
    }
}