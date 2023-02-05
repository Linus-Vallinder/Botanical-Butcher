using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public Dictionary<Item, int> Contents = new();

    public int GetTotalWorth()
    {
        var total = 0;
        foreach (var item in Contents)
        {
            total += item.Key.value * item.Value;
        }
        return total;
    }

    public void RemoveAll()
    => Contents.Clear();

    public void Add(Item item)
    {
        if (Contents.ContainsKey(item)) Contents[item] += 1;
        else Contents.Add(item, 1);
    }

    public void Remove(Item type)
    {
        if (Contents.ContainsKey(type))
        {
            var amount = Contents[type];
            if (amount > 0)
            {
                Contents[type] = amount--;
            }
            else Debug.LogWarning("The hero is not in possesion of one of this item!");
        }
    }
}