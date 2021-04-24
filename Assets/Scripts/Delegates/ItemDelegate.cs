using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDelegate : MonoBehaviour{

    public delegate void MultiDelegate();
    public MultiDelegate multiDelegate;
    // Start is called before the first frame update
    void Start(){
        multiDelegate += AddToInventory;
        multiDelegate += RemoveFromScenario;

        if(multiDelegate != null){
            multiDelegate();
        }
    }

    void AddToInventory(){
        print("ADICIONADO AO INVENTário");
    }

    void RemoveFromScenario(){
        print("REMOVIDO do inventário");
    }
}
