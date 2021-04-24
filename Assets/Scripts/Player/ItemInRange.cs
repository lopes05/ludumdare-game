using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using utils;

public class ItemInRange : MonoBehaviour {
    public ScriptableInventory inventory;

    List<Interactable> inside = new List<Interactable>();
    Interactable target {
        get {
            return inside.Count > 0 ? inside[0] : null;
        }
    }

    void Update() {
        var pressed = Input.GetButtonDown("Attack");
        if(pressed && target) {
            var localTarget = target;

            bool successful = localTarget.HandleClick(gameObject);
            if(successful) {
                inside.Remove(localTarget);
            }
        }
    }

    void Remove(Interactable toRemove) {
        toRemove.SetButton(false);
        inside.Remove(toRemove);

        if (target) {
            target.SetButton(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var container = other.GetComponentInParent<Interactable>();
        if (container) {
            this.inside.Add(container);
            target.SetButton(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        var container = other.GetComponentInParent<Interactable>();
        if (container && this.inside.Contains(container)) {
            Remove(container);
        }
    }
}
