using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour {
    public TextMeshProUGUI loadingText;

    private void Awake() {
        StartCoroutine(Dots());
    }

    IEnumerator Dots() {
        int dots = 0;
        while(true) {
            dots = (dots+1)%4;
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));

            this.loadingText.text = "Loading";
            for(int i=0; i<dots; i++) {
                this.loadingText.text += ".";
            }
        }
    }
}
