using System.Collections.Generic;
using UnityEngine;

public static class PrefabRegistry
{
    private static Dictionary<string, GameObject> map = new();

    public static void Register(string itemName, GameObject prefab)
    {
        if (!map.ContainsKey(itemName) && prefab != null)
            map[itemName] = prefab;
    }

    public static GameObject Get(string itemName)
    {
        map.TryGetValue(itemName, out var prefab);
        return prefab;
    }
}
