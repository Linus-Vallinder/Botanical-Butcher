using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable<T>
{
    [SerializeField] private List<DropItem<T>> items;
    
    private float totalWeight;

    public DropTable()
    {
        items = new List<DropItem<T>>();
        totalWeight = 0;
    }

    public void AddDrop(DropItem<T> item)
    {
        items.Add(item);
        totalWeight += item.weight;
    }

    public T GetDrop()
    {
        float randomValue = Random.Range(0, totalWeight);

        for (int i = 0; i < items.Count; i++)
        {
            if (randomValue < items[i].weight)
            {
                return items[i].item;
            }

            randomValue -= items[i].weight;
        }

        return default;
    }
}

[System.Serializable]
public class DropItem<T>
{
    public T item;
    public float weight;

    public DropItem(T item, float weight)
    {
        this.item = item;
        this.weight = weight;
    }
}