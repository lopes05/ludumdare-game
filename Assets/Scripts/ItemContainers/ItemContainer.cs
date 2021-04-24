using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class ItemContainer : MonoBehaviour {
    public PickUpObject item;

    SpriteRenderer sr;

    [OnInspectorGUI]
    void Awake() {
        this.sr = GetComponent<SpriteRenderer>();
        sr.sprite = item.sprite;
    }
}