using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    PickUp
}
public class ItemObject : ScriptableObject {
    
    public GameObject prefab;
    public ItemType type;
    public Sprite sprite;
}
