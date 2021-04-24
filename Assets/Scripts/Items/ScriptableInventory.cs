using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;
using System.Linq;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Inventory", order = 2)]
public class ScriptableInventory : ScriptableObject {
    public List<ScriptableItem> items;
    public ItemDelegate onItemAdd;

    public int HowMany(string type) { 
        return items.Count((x) => x.type == type);
    }

    public void AddItem(ScriptableItem item) {
        items.Add(item);
        onItemAdd(item);
    }
}
