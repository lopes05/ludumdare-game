using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resources : MonoBehaviour {
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public ScriptableInventory inventory;

    void Awake() {
        inventory.items = new List<ScriptableItem>();
        inventory.onItemAdd += UpdateTexts;
    }


    void UpdateTexts(ScriptableItem item) {
        woodText.text = "Wood: " + inventory.HowMany("Wood");
        stoneText.text = "Stone: " + inventory.HowMany("Stone");
    }
}
