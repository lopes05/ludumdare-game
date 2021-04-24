using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class ItemContainer : MonoBehaviour {
    public PickUpObject item;
    public CircleCollider2D areaOfEffect;
    public LayerMask layerMask;
    public GameObject floatingButtons;

    List<Collider2D> insideArea = new List<Collider2D>();
    List<Collider2D> results = new List<Collider2D>();
    SpriteRenderer sr;

    [OnInspectorGUI]
    void Awake() {
        this.sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = item.sprite;
    }

    void Update() {
        var filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);

        areaOfEffect.OverlapCollider(filter, results);
        if(results.Count != insideArea.Count) {
            var added = results.FindAll((x) => !insideArea.Contains(x));
            var removed = insideArea.FindAll((x) => !results.Contains(x));

            // added.ForEach((x) => Debug.Log(x));
            // removed.ForEach((x) => Debug.Log(x));

            insideArea = new List<Collider2D>(results);
            floatingButtons.SetActive(insideArea.Count > 0);
        }
    }
}