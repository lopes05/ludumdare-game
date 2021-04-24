using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemContainer : MonoBehaviour {
    public ScriptableItem item;
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

    public void SetButton(bool active) {
        floatingButtons.SetActive(active);
    }
}