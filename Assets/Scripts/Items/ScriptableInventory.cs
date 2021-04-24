using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;
using System.Linq;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Inventory", order = 2)]
public class ScriptableInventory : ScriptableObject {
    public List<ScriptableItem> items;
    public ItemDelegate onItemsUpdate;

    public int HowMany(string type) { 
        return items.Count((x) => x.type == type);
    }

    public void AddItem(ScriptableItem item) {
        items.Add(item);
        onItemsUpdate(items);
    }

    public void RemoveItems(string type, int amount) {
        var remaining = items.Where((x) => x.type == type).Skip(amount);
        items = items.Where((x) => x.type != type).Concat(remaining).ToList();
        onItemsUpdate(items);
    }
}
