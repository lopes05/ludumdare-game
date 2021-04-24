using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;

public class Objective : Interactable {
    public int woodAmount;
    public int stoneAmount;
    public ScriptableInventory inventory;
    public GameObject prefabToSpawn;

    public override void SetButton(bool active) {
        if(active) {
            ObjectiveResource.Instance.SetPosition(gameObject, woodAmount, stoneAmount);
            ObjectiveResource.Instance.SetActive(true);
        } else {
            ObjectiveResource.Instance.SetActive(false);
        }
    }

    public override bool HandleClick(GameObject obj) {
        bool canClick = inventory.HowMany("Wood") >= woodAmount && inventory.HowMany("Stone") >= stoneAmount;
        if(canClick) {
            inventory.RemoveItems("Wood", woodAmount);
            inventory.RemoveItems("Stone", stoneAmount);

            var go = Instantiate(prefabToSpawn, transform);
            go.transform.parent = null;
            Destroy(gameObject);
        }

        return canClick;
    }


    void Build() {

    }
}
