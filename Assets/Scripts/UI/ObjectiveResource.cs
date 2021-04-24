using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;
using TMPro;

public class ObjectiveResource : Singleton<ObjectiveResource> {
    public Vector3 margin = Vector3.zero;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;

    Canvas canvas;
    GameObject go = null;
    Vector3 Offset = Vector3.zero;


    void Awake() {
        canvas = FindObjectOfType<Canvas>();
        // Offset = transform.position - worldToUISpace(canvas, Player.transform.position);
    }

    public void SetActive(bool active) {
        transform.GetChild(0).gameObject.SetActive(active);
    }

    public void SetPosition(GameObject go, int woodAmount, int stoneAmount){
        this.go = go;

        woodText.text = woodAmount.ToString();
        stoneText.text = stoneAmount.ToString();
    }

    void Update() {
        //Convert the player's position to the UI space then apply the offset
        if(go != null) {
            transform.position = worldToUISpace(canvas, go.transform.position + margin) + Offset;
        }
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos) {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
