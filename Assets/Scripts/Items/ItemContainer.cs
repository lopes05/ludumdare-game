using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using utils;

public class ItemContainer : Interactable {
    public ScriptableItem item;
    public GameObject floatingButtons;

    List<Collider2D> insideArea = new List<Collider2D>();
    List<Collider2D> results = new List<Collider2D>();
    SpriteRenderer sr;

    [OnInspectorGUI]
    void Awake() {
        this.sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = item.sprite;
    }

    public override void SetButton(bool active) {
        floatingButtons.SetActive(active);
    }

    public override bool HandleClick(GameObject obj) {
        var itemInRange = obj.GetComponent<ItemInRange>();

        itemInRange.inventory.AddItem(item);
        Destroy(gameObject);
        return true;
    }
}