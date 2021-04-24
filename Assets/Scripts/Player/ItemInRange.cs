using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInRange : MonoBehaviour {
    public ScriptableInventory inventory;

    List<ItemContainer> inside = new List<ItemContainer>();
    ItemContainer target {
        get {
            return inside.Count > 0 ? inside[0] : null;
        }
    }

    void Update() {
        var pressed = Input.GetButtonDown("Attack");
        if(pressed && target) {
            var localTarget = target;
            inside.Remove(localTarget);

            inventory.AddItem(localTarget.item);
            Destroy(localTarget.gameObject);

            if (target) {
                target.SetButton(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var container = other.GetComponentInParent<ItemContainer>();
        if (container) {
            this.inside.Add(container);
            target.SetButton(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        var container = other.GetComponentInParent<ItemContainer>();
        if (container && this.inside.Contains(container)) {
            container.SetButton(false);
            this.inside.Remove(container);
        }
    }
}
