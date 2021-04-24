using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/Item", order = 1)]
public class ScriptableItem : ScriptableObject {
    public string type;
    public Sprite sprite;
}
